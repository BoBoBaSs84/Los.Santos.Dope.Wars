using LSDW.Activities;
using LSDW.Core;
using LSDW.Dealers;
using LSDW.Drugs;
using LSDW.Events;
using LSDW.Leveling;
using LSDW.Persistence;

using GTA.Chrono;

namespace LSDW.Services;

/// <summary>
/// Reads and writes <c>scripts\LSDW.sav</c>, mapping it to and from the live
/// game state. <see cref="SaveFile"/> is the document and owns its own parsing and
/// serialisation; this class only moves values across and touches the disk.
/// </summary>
/// <remarks>
/// It takes every subsystem it persists as a constructor argument, which is why it has more
/// of them than anything else in the mod — it is the one type that touches all of them. It is
/// also the last thing <see cref="Core.DrugDeal"/> builds, because it needs the
/// <see cref="DealerWorld"/>: that ordering is what replaced the pair of mutable
/// <c>Func&lt;&gt;</c> hooks this class used to expose for the dealers and the market. There
/// was never a cycle to break — <see cref="DealerWorld"/> asks for a save by publishing
/// <see cref="SaveRequestedEvent"/> and does not name this class — so nothing had to be
/// deferred; the hooks only existed because a static could not be constructed late.
/// </remarks>
internal sealed class SaveService
{
  private static readonly string SavePath = Path.Combine(AppContext.BaseDirectory, SaveFile.FileName);

  /// <summary>
  /// The bound a persisted distribution hour has to fall inside. A file claiming hour 30 is
  /// hand-edited nonsense, and taking it would put the next cycle on the wrong day.
  /// </summary>
  private const int HoursPerDay = 24;

  /// <summary>Where every read failure below is reported to the player.</summary>
  private readonly INotificationService _notifications;

  /// <summary>The dealers and the market, both persisted in full.</summary>
  private readonly DealerWorld _world;

  /// <summary>The taco property, whose <see cref="TacoProperty.Owned"/> flag is persisted.</summary>
  private readonly TacoProperty _property;

  /// <summary>
  /// The lifetime money figures, and the owner of the
  /// <see cref="PlayerStats.LoadSuccessful"/> flag this class sets.
  /// </summary>
  private readonly PlayerStats _stats;

  /// <summary>The XP, which the level is derived from.</summary>
  private readonly Progression _progression;

  /// <summary>The carried drugs, their cost basis and the shown-tips list.</summary>
  private readonly PlayerStash _stash;

  /// <summary>
  /// Subscribes to <see cref="SaveRequestedEvent"/>, so any subsystem can ask for a
  /// write without depending on this class, and takes everything it persists. Constructed
  /// once at startup, after the dealer world and before <see cref="Load"/>.
  /// </summary>
  /// <param name="events">The event bus to subscribe on.</param>
  /// <param name="notifications">Where a refused or unreadable save is reported.</param>
  /// <param name="world">The dealers and the market to read and restore.</param>
  /// <param name="property">The taco property to read and restore the owned flag on.</param>
  /// <param name="stats">The lifetime figures to read, restore and flag the load on.</param>
  /// <param name="progression">The XP to read and restore.</param>
  /// <param name="stash">The carried drugs and shown tips to read and restore.</param>
  public SaveService(EventService events, INotificationService notifications, DealerWorld world, TacoProperty property, PlayerStats stats, Progression progression, PlayerStash stash)
  {
    _notifications = notifications;
    _world = world;
    _property = property;
    _stats = stats;
    _progression = progression;
    _stash = stash;

    events.Subscribe<SaveRequestedEvent>(_ => Save());
  }

  public void Load()
  {
    try
    {
      if (!File.Exists(SavePath))
      {
        Save();
        _stats.LoadSuccessful = true;
        return;
      }

      string content = File.ReadAllText(SavePath);

      // Named the same as the INI save it replaced, so an old file would otherwise be
      // reported as unreadable. Say what actually happened and start clean.
      if (SaveFile.IsLegacyIniFormat(content))
      {
        _notifications.Ticker("~y~Save format changed:~w~ the old " + SaveFile.FileName + " was discarded. Starting fresh.");

        Save();
        _stats.LoadSuccessful = true;
        return;
      }

      // TryRead, not a bare Deserialize: a hand-edited save must survive as a notification
      // rather than taking the script down.
      if (!SaveFile.TryRead(content, out SaveFile? saved) || saved is null)
      {
        _notifications.Ticker("Error while loading savedata: " + SaveFile.FileName + " is not readable.");
        return;
      }

      // Refused rather than read: a newer build's file may mean something different by the
      // same element, and guessing loses the player's progress silently.
      if (saved.Version > SaveFile.CurrentVersion)
      {
        _notifications.Ticker("Error while loading savedata: " + SaveFile.FileName + " was written by a newer version.");
        return;
      }

      _progression.XP = saved.Stats.XP;
      _stats.Earned = saved.Stats.Earned;
      _stats.Spent = saved.Stats.Spent;

      _property.Owned = saved.Player.TacoOwned;

      _stash.TipsShown.Clear();
      _stash.TipsShown.AddRange(saved.Player.TipsShown);

      LoadDrugs(saved.Drugs);

      // The market before the dealers: the totals are what a dealer's share means, and
      // restoring a share against last session's totals would misprice every offer for the
      // rest of the cycle.
      LoadMarket(saved.Market);
      LoadDealers(saved.Dealers);

      _stats.LoadSuccessful = true;
    }
    catch (Exception ex)
    {
      _notifications.Ticker("Error while loading savedata: " + ex.Message);
    }
  }

  private void Save()
  {
    SaveFile file = new()
    {
      Stats = new StatsSection
      {
        Earned = _stats.Earned,
        Spent = _stats.Spent,
        XP = _progression.XP
      },
      Player = new PlayerSection
      {
        TacoOwned = _property.Owned,
        TipsShown = [.. _stash.TipsShown]
      },
      Drugs = SaveDrugs(),
      Market = SaveMarket(),
      Dealers = SaveDealers()
    };

    File.WriteAllText(SavePath, SaveFile.Write(file));
  }

  /// <summary>
  /// Writes every dealer in the roster: his discovery, any standing death cooldown, and his
  /// position in each drug.
  /// </summary>
  /// <remarks>
  /// Unlike the version 1 format, undiscovered dealers are written too. The supply is dealt
  /// across the whole roster at once, so leaving some of them out would restore a market whose
  /// shares no longer add up to the totals every price and offer is measured against.
  /// </remarks>
  private List<DealerEntry> SaveDealers()
  {
    List<DealerEntry> entries = [];

    foreach (DrugDealer dealer in _world.Dealers)
    {
      DealerEntry entry = new()
      {
        Id = dealer.Id,
        Discovered = dealer.Discovered,
        Cooldown = dealer.CooldownUntil
      };

      foreach (Drug drug in DrugCatalog.All)
      {
        entry.Stock.Add(new StockEntry
        {
          Name = drug.Name,
          Amount = dealer.Book.Amount[drug.Name],
          Price = dealer.Book.Price[drug.Name],
          Demand = dealer.Book.Demand[drug.Name]
        });
      }

      entries.Add(entry);
    }

    return entries;
  }

  /// <summary>
  /// Restores discovery, death cooldowns and market positions onto the live dealers, matching
  /// on <see cref="DrugDealer.Id"/>. An entry naming a dealer the roster does not have is
  /// ignored, as is a cooldown that will not parse.
  /// </summary>
  private void LoadDealers(List<DealerEntry> entries)
  {
    foreach (DealerEntry entry in entries)
    {
      foreach (DrugDealer dealer in _world.Dealers)
      {
        if (dealer.Id != entry.Id)
        {
          continue;
        }

        dealer.Discovered = entry.Discovered;

        if (entry.Cooldown is GameClockDate until)
        {
          dealer.CooldownUntil = until;
        }

        LoadDealerStock(dealer, entry.Stock);

        dealer.RefreshBlip();
        break;
      }
    }
  }

  /// <summary>
  /// Restores one dealer's stock, prices and demand. A drug the file does not mention keeps
  /// what it has, and a name the catalog does not know is skipped — <see cref="LoadDrugs"/>
  /// is the one place that reports an unknown drug, so this stays quiet rather than saying it
  /// again per dealer.
  /// </summary>
  private static void LoadDealerStock(DrugDealer dealer, List<StockEntry> stock)
  {
    foreach (StockEntry entry in stock)
    {
      if (!dealer.Book.Knows(entry.Name))
      {
        continue;
      }

      dealer.Book.SetPosition(entry.Name, entry.Amount, entry.Price, entry.Demand);
    }
  }

  /// <summary>
  /// Writes this cycle's supply totals and when the next distribution is due.
  /// </summary>
  private MarketSection SaveMarket()
  {
    MarketSection section = new();
    DealerMarket market = _world.Market;

    // Absent when no market has been dealt yet, which cannot normally happen — Load runs
    // before the first tick, so the very first save of a new game writes no schedule and the
    // first tick then deals one.
    if (market.NextDistribution is GameClockDateTime due)
    {
      section.Next = due.Date;
      section.NextHour = due.Hour;
    }

    foreach (Drug drug in DrugCatalog.All)
    {
      section.Supply.Add(new SupplyEntry
      {
        Name = drug.Name,
        Total = market.TotalSupply[drug.Name]
      });
    }

    return section;
  }

  /// <summary>
  /// Restores the supply totals and the distribution schedule. Anything missing or unreadable
  /// leaves <see cref="DealerMarket.NextDistribution"/> null, and a null schedule deals a fresh
  /// market on the next tick — which is exactly how a version 1 save, and a new game, start.
  /// </summary>
  private void LoadMarket(MarketSection section)
  {
    DealerMarket market = _world.Market;

    foreach (SupplyEntry entry in section.Supply)
    {
      if (market.TotalSupply.ContainsKey(entry.Name))
      {
        market.TotalSupply[entry.Name] = entry.Total;
      }
    }

    if (section.Next is GameClockDate date && section.NextHour is >= 0 and < HoursPerDay)
    {
      market.NextDistribution = date.AndHms(section.NextHour, 0, 0);
    }
  }

  /// <summary>
  /// The player's bag count is not persisted — it is the sum of what they carry, and
  /// <see cref="PlayerStash.Bag"/> derives it, so there is nothing to restore here.
  /// </summary>
  /// <remarks>
  /// A catalog drug the file does not mention keeps its defaults; a name the catalog does
  /// not know is reported once and skipped, which is what a renamed or removed drug looks
  /// like from here.
  /// </remarks>
  private void LoadDrugs(List<DrugEntry> entries)
  {
    List<string> unknown = [];

    foreach (DrugEntry entry in entries)
    {
      if (!_stash.Drugs.ContainsKey(entry.Name))
      {
        unknown.Add(entry.Name);
        continue;
      }

      _stash.Restore(entry.Name, entry.Amount, entry.BoughtPrice);
    }

    if (unknown.Count > 0)
    {
      _notifications.Ticker("~y~Save warning:~w~ " + SaveFile.FileName + " holds drugs this build does not know: " + string.Join(", ", [.. unknown]) + ".");
    }
  }

  private List<DrugEntry> SaveDrugs()
  {
    List<DrugEntry> entries = [];

    foreach (Drug drug in DrugCatalog.All)
    {
      entries.Add(new DrugEntry
      {
        Name = drug.Name,
        Amount = _stash.Drugs[drug.Name],
        BoughtPrice = _stash.BoughtPrice[drug.Name]
      });
    }

    return entries;
  }
}
