namespace LSDW.Dealers;

/// <summary>
/// What a dealer charges, and how much he wants, given what he is holding. Both fall out of
/// one number: his share of the cycle's supply measured against the average share. The man
/// with nothing charges the most and buys the most; the man with a glut undercuts everyone and
/// buys nothing.
/// </summary>
/// <remarks>
/// Pure functions of a share and a mean, so the entire price model is testable without the
/// game. Prices are computed once per cycle, when <see cref="DealerMarket"/> distributes the
/// supply, and do not move again — trading shifts a dealer's <i>appetite</i>, not his price.
/// </remarks>
internal static class MarketPricing
{
  /// <summary>
  /// What a dealer holding exactly the average share charges: market value, to the penny.
  /// </summary>
  /// <remarks>
  /// <b>This being exactly 1.0 is the anchor of the whole curve</b>, not a coincidence of the
  /// floor and ceiling. <see cref="ScarcityMultiplier"/> is written as a deviation from this
  /// midpoint precisely so that the average dealer prices at market in exact arithmetic;
  /// computing it as <c>floor + span / 2</c> instead lands on 0.9999999999999999.
  /// </remarks>
  public const double PriceMid = 1.0;

  /// <summary>
  /// How far either side of <see cref="PriceMid"/> the curve reaches, giving a
  /// 0.55x..1.45x band. Close to the +/-50% the old random re-roll used, so the feel carries
  /// over — but earned from stock now rather than rolled.
  /// </summary>
  public const double PriceSwing = 0.45;

  /// <summary>
  /// The dealer's cut, taken off both sides of every trade: he asks
  /// <c>price x 1.15</c> and bids <c>price x 0.85</c>.
  /// </summary>
  /// <remarks>
  /// <para>
  /// 15% is derived, not chosen. Without a spread the level-100 perks alone
  /// (<c>BuyPriceMultiplier</c> 0.85, <c>SellPriceMultiplier</c> 1.15) make buying and selling
  /// at the <i>same</i> dealer profitable. Closing that needs
  /// <c>(1 + s) x 0.85 &gt;= (1 - s) x 1.15</c>, i.e. <c>s &gt;= 0.15</c>.
  /// </para>
  /// <para>
  /// The consequence is intended: trading between two average dealers is a loss, so the player
  /// has to go and find the glut and the drought. It is also what finally gives
  /// <c>DrugMenuItem</c>'s good-deal arrow something true to say.
  /// </para>
  /// </remarks>
  public const double MarketSpread = 0.15;

  /// <summary>
  /// How many times the average share a dealer will hold before he stops buying. Above the
  /// average because a dealer sitting on exactly his portion would otherwise refuse every
  /// sale, which would leave most of the roster unable to trade in either direction.
  /// </summary>
  private const double DemandTarget = 1.5;

  /// <summary>
  /// The cheapest any dealer gets, as a fraction of market value.
  /// </summary>
  public static double PriceFloor => PriceMid - PriceSwing;

  /// <summary>
  /// The dearest any dealer gets, as a fraction of market value. Reached only by a dealer
  /// holding nothing.
  /// </summary>
  public static double PriceCeiling => PriceMid + PriceSwing;

  /// <summary>
  /// What this dealer's stock does to his price: <see cref="PriceCeiling"/> when he is empty,
  /// exactly <see cref="PriceMid"/> when he holds the average, tending to
  /// <see cref="PriceFloor"/> as he floods.
  /// </summary>
  /// <param name="share">What this dealer is holding.</param>
  /// <param name="meanShare">The average holding — see <see cref="MarketSupply.MeanShare"/>.</param>
  public static double ScarcityMultiplier(int share, double meanShare)
  {
    // No supply at all: there is nothing to be relatively short of, and a drug nobody can
    // source is a dear one.
    if (meanShare <= 0.0)
    {
      return PriceCeiling;
    }

    double relative = share / meanShare;

    if (relative < 0.0)
    {
      relative = 0.0;
    }

    // Written as a deviation from the midpoint so that relative == 1 cancels the swing term
    // outright. See PriceMid.
    return PriceMid + (PriceSwing * (1.0 - relative) / (1.0 + relative));
  }

  /// <summary>
  /// This dealer's price for one unit, before the spread and before any perk.
  /// </summary>
  public static int Price(int marketValue, int share, double meanShare)
  {
    int price = (int)Math.Round(marketValue * ScarcityMultiplier(share, meanShare));

    // Weed is worth 10, and 10 x 0.55 rounds to 6 — but a cheaper drug added later must not
    // round its way to free.
    return (marketValue > 0 && price < 1) ? 1 : price;
  }

  /// <summary>
  /// How much of the drug this dealer will still take off the player: what he is short of
  /// <see cref="DemandTarget"/> times the average share, and nothing once he is over it.
  /// </summary>
  /// <remarks>
  /// The mirror of <see cref="ScarcityMultiplier"/>, and what makes "a dealer who urgently
  /// needs a drug only buys it" fall out of the model rather than being a special case: an
  /// empty dealer has no stock to sell and full demand, a flooded one has stock but no
  /// appetite. <see cref="DealerTrading"/> keeps <c>amount + demand</c> constant as the player
  /// trades, so filling a dealer up exhausts him without moving the price he opened with.
  /// </remarks>
  public static int Demand(int share, double meanShare)
  {
    if (meanShare <= 0.0)
    {
      return 0;
    }

    int target = (int)Math.Ceiling(DemandTarget * meanShare);

    return (share >= target) ? 0 : target - share;
  }

  /// <summary>
  /// What the player pays per unit: the dealer's price plus his cut, then any perk discount.
  /// </summary>
  /// <param name="dealerPrice">This dealer's price for the drug.</param>
  /// <param name="perkMultiplier">The player's buy-side perk — <c>Perks.BuyPriceMultiplier</c>.</param>
  public static int Ask(int dealerPrice, double perkMultiplier)
    => (int)Math.Round(dealerPrice * (1.0 + MarketSpread) * perkMultiplier);

  /// <summary>
  /// What the player is paid per unit: the dealer's price less his cut, then any perk bonus.
  /// </summary>
  /// <param name="dealerPrice">This dealer's price for the drug.</param>
  /// <param name="perkMultiplier">The player's sell-side perk — <c>Perks.SellPriceMultiplier</c>.</param>
  public static int Bid(int dealerPrice, double perkMultiplier)
    => (int)Math.Round(dealerPrice * (1.0 - MarketSpread) * perkMultiplier);
}
