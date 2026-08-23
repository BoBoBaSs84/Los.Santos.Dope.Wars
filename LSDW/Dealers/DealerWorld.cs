using LSDW.Events;
using LSDW.Helpers;
using LSDW.Leveling;
using LSDW.Services;

using GTA;
using GTA.Chrono;

namespace LSDW.Dealers;

/// <summary>
/// The street dealers as they exist in the world: the roster, which of them has a ped
/// standing at his spot right now, and which of them the player has found. Knows nothing
/// about the menus — <see cref="OnTick"/> reports which dealer the player is standing at
/// and <see cref="DrugDeal"/> decides what to do about it.
/// </summary>
internal sealed class DealerWorld
{
  /// <summary>
  /// Assigned in the constructor rather than initialised inline: building the roster reports
  /// a duplicated spot id, so it needs the notification service to have arrived first.
  /// </summary>
  private readonly List<DrugDealer> _dealers;
  private readonly EventService _events;
  private readonly INotificationService _notifications;

  /// <summary>
  /// The dealers, in roster order, for <see cref="SaveService"/> to read and write their
  /// persisted state. Read-only: the roster is fixed at construction.
  /// </summary>
  public IReadOnlyList<DrugDealer> Dealers => _dealers;

  /// <summary>
  /// The market, for <see cref="SaveService"/> to read and write its persisted state — this
  /// cycle's supply and when the next one is due.
  /// </summary>
  public DealerMarket Market { get; }

  /// <summary>
  /// Builds the roster's dealers — and with them their blips — and the market that
  /// decides what they carry.
  /// </summary>
  /// <param name="events">The event bus to ask for a save on when a dealer is found.</param>
  /// <param name="notifications">Where a found dealer and a roster error are reported.</param>
  /// <param name="progression">
  /// Passed straight through to the market, which scales each cycle's supply by the level.
  /// The progression rather than the level itself: the market re-reads it every cycle, so a
  /// number captured here would freeze the world's depth at whatever the level was on the
  /// session's first tick.
  /// </param>
  public DealerWorld(EventService events, INotificationService notifications, Progression progression)
  {
    _events = events;
    _notifications = notifications;
    _dealers = DealerRoster.CreateDealers(notifications);
    Market = new DealerMarket(notifications, _dealers, progression);
  }

  /// <summary>
  /// Deals a fresh market when the interval comes round, spawns and despawns the dealers'
  /// peds by range, and returns the dealer the player is standing at, or
  /// <see langword="null"/> if they are not at one.
  /// </summary>
  public DrugDealer? OnTick(Ped character)
  {
    // Before the loop below, not inside it: a dealer restocked partway through the walk
    // is not the same as one restocked before it starts.
    Market.Tick();

    DrugDealer? atDealer = null;

    // Read once for the whole roster — the wanted level is a native call.
    bool hot = SettingsService.Dealers.FleeWantedLevel > 0
      && ScriptHelper.WantedLevel >= SettingsService.Dealers.FleeWantedLevel;

    foreach (DrugDealer dealer in _dealers)
    {
      // Checked regardless of range, unlike everything else in this loop: the blip is
      // map-wide, so a lapsing cooldown has to restore it wherever the player is.
      if (dealer.CooldownUntil is GameClockDate until && GameClock.Today >= until)
      {
        dealer.CooldownUntil = null;
        dealer.RefreshBlip();
      }

      // Peds only exist near the player; they are deleted again on the way out.
      if (character.IsInRange(dealer.Position, 100f))
      {
        DiscoverDealer(dealer);

        // The corpse is left to the out-of-range Delete() below; the cooldown only
        // decides whether he is spawned again afterwards.
        if (SettingsService.Dealers.HasCooldown && dealer.CooldownUntil is null && dealer.MainPed?.IsDead == true)
        {
          dealer.CooldownUntil = GameClock.Today + GameClockDuration.FromDays(SettingsService.Dealers.CooldownDays);
          dealer.RefreshBlip();

          // A dead dealer's offer is unreachable, so it is dropped rather than left to
          // expire. ClearOffer refreshes the blip a second time; that is deliberate —
          // each site refreshes for the state it changed, and RefreshBlip is idempotent.
          DealerMarket.ClearOffer(dealer);
        }

        DealerPeds.UpdateHeatReaction(dealer, character, hot);

        // Nobody is spawned while the player is hot: a dealer who has bolted must not be
        // replaced at his spot a tick later.
        if (!hot && dealer.CooldownUntil is null && (dealer.MainPed == null || !dealer.MainPed.Exists()))
        {
          DealerPeds.SpawnDealerPed(dealer);
        }

        // The dealer blocks non-temporary events so he holds his spot, which also swallows
        // the combat event when the player shoots him. The first hit unblocks his events
        // and tasks him by hand. Melee counts as damage, so a punch provokes him too.
        if (!dealer.Hostile && dealer.MainPed?.HasBeenDamagedBy(character) == true)
        {
          dealer.Hostile = true;
          dealer.MainPed.BlockPermanentEvents = false;
          dealer.MainPed.Task.Combat(character);
        }
      }
      else
      {
        dealer.MainPed?.Delete();
      }

      // First match in roster order, and only a dealer able to trade. Range alone is not
      // enough: a corpse stays in range and passes Exists(), and a hostile or fleeing
      // dealer is alive but not selling. IsInjured is also true for a ped that does not
      // exist, so `?.IsInjured == false` reads as "alive and able to trade".
      if (atDealer == null && !dealer.Hostile && !dealer.Fleeing && dealer.MainPed?.IsInjured == false && character.IsInRange(dealer.Position, 3f))
      {
        atDealer = dealer;
      }
    }

    return atDealer;
  }

  /// <summary>
  /// Marks a dealer found the first time the player comes near him, and asks for a save.
  /// </summary>
  private void DiscoverDealer(DrugDealer dealer)
  {
    if (dealer.Discovered)
    {
      return;
    }

    dealer.Discovered = true;
    dealer.RefreshBlip();

    if (SettingsService.Dealers.RequireDiscovery)
    {
      string zone = World.GetZoneLocalizedName(dealer.Position);
      _notifications.Picture("Anonymous", "Tip-off", "You found a dealer in " + zone + ".");
    }

    _events.Publish(new SaveRequestedEvent());
  }

  /// <summary>
  /// Removes every dealer blip and ped from the world. Called from
  /// <see cref="DrugDeal"/>'s <c>Aborted</c> handler when the script is unloaded or
  /// reloaded.
  /// </summary>
  public void Cleanup()
  {
    foreach (DrugDealer dealer in _dealers)
    {
      dealer.MainPed?.Delete();
      dealer.MainPed = null;
      dealer.DealBlip?.Delete();
    }
  }
}
