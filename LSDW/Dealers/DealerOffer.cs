namespace LSDW.Dealers;

/// <summary>
/// Which way a dealer's special offer runs, or <see cref="None"/> when he is trading
/// normally.
/// </summary>
internal enum DealerOfferKind
{
  /// <summary>
  /// No offer. The dealer's stock and prices are the ordinary daily roll.
  /// </summary>
  None,

  /// <summary>
  /// The dealer is offloading one drug: a lot of it, well under market. The player
  /// buys low.
  /// </summary>
  ClearOut,

  /// <summary>
  /// The dealer badly needs one drug and pays well over market for it. The player
  /// sells high.
  /// </summary>
  InDemand
}

/// <summary>
/// The numbers behind a special offer: how far under market a clear-out goes, how far
/// over an in-demand does, and how much of the drug either one moves.
/// </summary>
/// <remarks>
/// The price magnitudes are absolute fractions of market value and stand deliberately outside
/// the supply model in <see cref="MarketPricing"/> — an offer is a dealer acting against his
/// own book, so it is not meant to fall on the scarcity curve. Only the quantity scales, via
/// the average share.
/// </remarks>
internal static class DealerOffer
{
  /// <summary>
  /// The exclusive upper bound of a magnitude roll. Shared with the draw in
  /// <see cref="DealerMarket"/> so the two cannot drift apart.
  /// </summary>
  public const int MaxRoll = 100;

  /// <summary>
  /// A clear-out prices the drug at 40% of market at its cheapest...
  /// </summary>
  private const double ClearOutPriceMin = 0.40;

  /// <summary>
  /// ...and 60% at its dearest, so it always visibly beats the market.
  /// </summary>
  private const double ClearOutPriceMax = 0.60;

  /// <summary>
  /// An in-demand dealer pays at least half again over market...
  /// </summary>
  private const double InDemandPriceMin = 1.50;

  /// <summary>
  /// ...and at most double it.
  /// </summary>
  private const double InDemandPriceMax = 2.00;

  /// <summary>
  /// The least an offer moves, as a multiple of the average share...
  /// </summary>
  /// <remarks>
  /// A multiple rather than a flat count, so an offer stays worth crossing the map for as the
  /// market deepens with the player's level. Four times the average is well clear of anything
  /// the ordinary distribution hands out, which is what makes an offer recognisable from the
  /// quantity alone.
  /// </remarks>
  private const double OfferQuantityMin = 4.0;

  /// <summary>
  /// ...and the most, at eight times the average share.
  /// </summary>
  private const double OfferQuantityMax = 8.0;

  /// <summary>
  /// What one unit costs at a clear-out: 40..60% of <paramref name="marketValue"/>.
  /// </summary>
  public static int ClearOutPrice(int marketValue, int roll)
    => (int)Math.Round(marketValue * Lerp(ClearOutPriceMin, ClearOutPriceMax, roll));

  /// <summary>
  /// What one unit fetches at an in-demand dealer: 150..200% of
  /// <paramref name="marketValue"/>.
  /// </summary>
  public static int InDemandPrice(int marketValue, int roll)
    => (int)Math.Round(marketValue * Lerp(InDemandPriceMin, InDemandPriceMax, roll));

  /// <summary>
  /// How much of the drug an offer moves: 4..8 times <paramref name="baseline"/>. Stock to
  /// shift for a <see cref="DealerOfferKind.ClearOut"/>, appetite to fill for an
  /// <see cref="DealerOfferKind.InDemand"/>.
  /// </summary>
  /// <remarks>
  /// One quantity serves both directions because an offer is the same event either way round —
  /// a dealer badly out of balance on one drug. It also has to be this large in the in-demand
  /// case or the premium would be unreachable: such a dealer holds no stock by design, and an
  /// ordinary dry dealer's appetite is only about one and a half shares, so raising the price
  /// without raising the appetite would be worth barely more than walking up to any empty
  /// dealer on the map.
  /// </remarks>
  /// <param name="roll">A magnitude roll, 0..<see cref="MaxRoll"/>-1.</param>
  /// <param name="baseline">
  /// The average share for this drug this cycle, which is how the player's level reaches the
  /// offer — see <see cref="MarketSupply.MeanShare"/>. Callers pass at least 1.
  /// </param>
  public static int OfferQuantity(int roll, int baseline)
    => (int)Math.Round(baseline * Lerp(OfferQuantityMin, OfferQuantityMax, roll));

  /// <summary>
  /// Maps a roll onto <paramref name="min"/>..<paramref name="max"/> inclusive at both
  /// ends — hence <see cref="MaxRoll"/> - 1 as the divisor, since the highest roll a
  /// caller can pass is one below the bound.
  /// </summary>
  private static double Lerp(double min, double max, int roll)
    => min + ((max - min) * roll / (MaxRoll - 1));
}
