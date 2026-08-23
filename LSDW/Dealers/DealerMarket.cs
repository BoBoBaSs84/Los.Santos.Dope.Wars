using LSDW.Drugs;
using LSDW.Helpers;
using LSDW.Leveling;
using LSDW.Services;

using GTA;
using GTA.Chrono;

namespace LSDW.Dealers;

/// <summary>
/// The dealers' market. Once per <c>[Economy] RestockIntervalHours</c> a quantity of every
/// drug enters the world and is split unevenly across the roster; each dealer's price and
/// appetite follow from the share he ends up with. Driven from
/// <see cref="DealerWorld.OnTick(Ped)"/>.
/// </summary>
/// <remarks>
/// <para>
/// The three numbers a dealer trades on — stock, price, demand — are set together, in one
/// pass, and none of them is rolled independently. A dealer sitting on a pile is cheap
/// <i>because</i> he is sitting on a pile; an empty one is dear and hungry for the same reason.
/// That is the whole market: <see cref="MarketSupply"/> decides how much exists,
/// <see cref="MarketDistribution"/> decides who has it, <see cref="MarketPricing"/> turns that
/// into prices.
/// </para>
/// <para>
/// Prices are computed here and nowhere else. Trading moves a dealer's stock and appetite but
/// never his price, so what the player sees when they walk up is what the cycle dealt.
/// </para>
/// </remarks>
internal sealed class DealerMarket
{
  private readonly INotificationService _notifications;
  private readonly List<DrugDealer> _dealers;

  /// <summary>
  /// The player's level, read once per distribution to scale the world supply. The
  /// progression itself rather than a number: a level captured at construction would freeze
  /// the market at level 1 for the session.
  /// </summary>
  private readonly Progression _progression;

  /// <summary>
  /// Scratch buffers for one drug's pass, reused across cycles rather than reallocated. Sized
  /// once from the roster, which is fixed at construction.
  /// </summary>
  private readonly double[] _weights;

  /// <inheritdoc cref="_weights"/>
  private readonly int[] _shares;

  /// <summary>
  /// This cycle's world supply per drug, and with it the average share every price and every
  /// offer baseline is measured against. Kept so <see cref="Services.SaveService"/> can persist
  /// the market and so an offer can size itself against the drug it lands on.
  /// </summary>
  public Dictionary<string, int> TotalSupply { get; } = DrugCatalog.NewAmountMap();

  /// <summary>
  /// When the next distribution is due, or <see langword="null"/> for "has never run" — the
  /// null start makes the first tick of a session deal a market, replacing the zero stock
  /// <see cref="DrugCatalog.NewAmountMap"/> hands every dealer.
  /// </summary>
  public GameClockDateTime? NextDistribution { get; set; }

  /// <summary>
  /// Takes the roster the distribution walks — the list itself, not a copy.
  /// </summary>
  /// <param name="notifications">Where the restock and special-offer tip-offs go.</param>
  /// <param name="dealers">The roster.</param>
  /// <param name="progression">The level each distribution scales its supply by.</param>
  public DealerMarket(INotificationService notifications, List<DrugDealer> dealers, Progression progression)
  {
    _notifications = notifications;
    _dealers = dealers;
    _progression = progression;
    _weights = new double[dealers.Count];
    _shares = new int[dealers.Count];
  }

  /// <summary>
  /// Distributes a fresh supply when the interval comes round. Called <b>before</b>
  /// <see cref="DealerWorld.OnTick(Ped)"/>'s per-dealer loop, never during it.
  /// </summary>
  public void Tick()
  {
    // Scheduled from *now*, so sleeping through eight hours of a one-hour interval collapses
    // into one distribution rather than firing a backlog of eight.
    GameClockDateTime now = GameClock.Now;

    if (NextDistribution is not GameClockDateTime dueAt || now >= dueAt)
    {
      NextDistribution = NextBoundary(now, SettingsService.Economy.RestockIntervalHours);
      DistributeSupply();
    }
  }

  /// <summary>
  /// The in-game moment a market pass next runs: the earliest
  /// <c>midnight + k × interval</c> strictly after <paramref name="now"/>.
  /// </summary>
  private static GameClockDateTime NextBoundary(GameClockDateTime now, int intervalHours)
    => now.Date.AndHms(0, 0, 0)
       + GameClockDuration.FromHours(MarketSchedule.HoursToNextBoundary(now.SecondsFromMidnight, intervalHours));

  /// <summary>
  /// Deals a whole market: for each drug, how much of it exists this cycle, who ends up with
  /// it, and what that makes every dealer charge and want. Then announces the cycle and rolls
  /// the special offer.
  /// </summary>
  /// <remarks>
  /// <para>
  /// Walks <see cref="PlayerStash.DrugNames"/> — catalog order, which must stay stable: the
  /// draws below come off the shared <see cref="RandomHelper.Random"/> in this order, so
  /// reordering the catalog changes what the market deals.
  /// </para>
  /// <para>
  /// Two draws per dealer per drug, always both, so the sequence does not depend on whether a
  /// dealer came up dry. The level is read once for the whole pass: a player who levels up
  /// mid-cycle sees the deeper market at the next distribution, not immediately.
  /// </para>
  /// </remarks>
  private void DistributeSupply()
  {
    foreach (DrugDealer dealer in _dealers)
    {
      // Ends any standing offer first, so the distribution below writes over ordinary values
      // rather than over a skewed price this pass would then remember as "previous". Takes no
      // draws.
      ClearOffer(dealer);
    }

    int level = _progression.Level;
    double supplyScale = SettingsService.Economy.MarketSupply;

    foreach (string drugName in PlayerStash.DrugNames)
    {
      int total = MarketSupply.TotalSupply(
        DrugCatalog.Get(drugName).SupplyBase,
        _dealers.Count,
        level,
        MarketSupply.Jitter(RandomHelper.Random.NextDouble()),
        supplyScale);

      for (int index = 0; index < _dealers.Count; index++)
      {
        bool dry = RandomHelper.Random.NextPercent() < MarketDistribution.DryChance;

        // 1 - NextDouble() lands in (0,1], keeping the log finite: NextDouble() can return 0
        // but never 1.
        double draw = 1.0 - RandomHelper.Random.NextDouble();

        _weights[index] = dry ? 0.0 : -Math.Log(draw);
      }

      MarketDistribution.Apportion(_weights, total, _shares);

      TotalSupply[drugName] = total;

      double meanShare = MarketSupply.MeanShare(total, _dealers.Count);
      int marketValue = PlayerStash.MarketValue[drugName];

      for (int index = 0; index < _dealers.Count; index++)
      {
        DrugDealer dealer = _dealers[index];
        int share = _shares[index];

        dealer.Book.SetPosition(
          drugName,
          share,
          MarketPricing.Price(marketValue, share, meanShare),
          MarketPricing.Demand(share, meanShare));
      }
    }

    _notifications.Picture("Anonymous", "Tip-off", "The dealers have restocked. Prices have moved.");

    RollSpecialOffer();
  }

  /// <summary>
  /// The average share for one drug this cycle, as an offer baseline — at least 1, so an offer
  /// on a drug the market barely stocked still moves something.
  /// </summary>
  private int OfferBaseline(string drugName)
  {
    double meanShare = MarketSupply.MeanShare(TotalSupply[drugName], _dealers.Count);
    int baseline = (int)Math.Ceiling(meanShare);

    return (baseline < 1) ? 1 : baseline;
  }

  /// <summary>
  /// Rolls a special offer: a small chance that one dealer is either dumping a drug
  /// cheap or paying over the odds for one.
  /// </summary>
  private void RollSpecialOffer()
  {
    int chanceRoll = RandomHelper.Random.NextPercent();
    int dealerIndex = RandomHelper.Random.Next(_dealers.Count);
    int drugIndex = RandomHelper.Random.Next(PlayerStash.DrugNames.Count);
    bool inDemand = RandomHelper.Random.Next(2) == 0;
    int priceRoll = RandomHelper.Random.Next(0, DealerOffer.MaxRoll);
    int quantityRoll = RandomHelper.Random.Next(0, DealerOffer.MaxRoll);

    DrugDealer dealer = _dealers[dealerIndex];

    if (chanceRoll >= SettingsService.Economy.SpecialOfferChance || dealer.CooldownUntil is not null || !dealer.Revealed)
    {
      return;
    }

    string drugName = PlayerStash.DrugNames[drugIndex];
    int marketValue = PlayerStash.MarketValue[drugName];
    int quantity = DealerOffer.OfferQuantity(quantityRoll, OfferBaseline(drugName));

    // Which way the offer runs is decided here; what it does to his book — including
    // remembering the position it overwrites, so ending it is an exact undo — belongs to the
    // book itself.
    if (inDemand)
    {
      dealer.Book.ApplyInDemand(drugName, DealerOffer.InDemandPrice(marketValue, priceRoll), quantity);
    }
    else
    {
      dealer.Book.ApplyClearOut(drugName, DealerOffer.ClearOutPrice(marketValue, priceRoll), quantity);
    }

    dealer.RefreshBlip();

    // The drug and the neighbourhood are named; the dealer is not. Finding which of the
    // roster it is, is what the flagged blip is for.
    string zone = World.GetZoneLocalizedName(dealer.Position);
    string tip = inDemand
      ? "Someone in " + zone + " is paying over the odds for " + drugName + "."
      : "Someone in " + zone + " is dumping " + drugName + " cheap.";

    _notifications.Picture("Anonymous", "Tip-off", tip);
  }

  /// <summary>
  /// Ends a dealer's special offer, putting back the price, stock and demand it skewed. Safe
  /// to call on a dealer who has no offer.
  /// </summary>
  internal static void ClearOffer(DrugDealer dealer)
  {
    dealer.Book.ClearOffer();

    // Unconditional, as it always has been: blip refreshes are idempotent by design, and
    // each site refreshes for the state it just changed rather than trying to work out
    // whether anything moved.
    dealer.RefreshBlip();
  }
}
