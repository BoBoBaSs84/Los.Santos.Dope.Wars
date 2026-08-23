using LSDW.Drugs;
using LSDW.Leveling;

namespace LSDW.Dealers;

/// <summary>
/// How much of a drug exists in the world for one market cycle. The total is the quantity
/// <see cref="MarketDistribution"/> splits across the roster and that
/// <see cref="MarketPricing"/> prices every dealer against, so this type sets the scale of
/// the whole economy.
/// </summary>
/// <remarks>
/// Pure arithmetic, deliberately: the draw that feeds <see cref="Jitter"/> is taken in
/// <see cref="DealerMarket"/> and passed in, which is what lets the supply curve be tested
/// without the game running.
/// </remarks>
internal static class MarketSupply
{
  /// <summary>
  /// How much deeper the market gets per level. At <see cref="Progression.MaxLevel"/> this
  /// is roughly three times the level-1 supply — enough that the player is no longer
  /// fighting over scraps, not so much that scarcity stops setting prices.
  /// </summary>
  private const double GrowthPerLevel = 0.02;

  /// <summary>
  /// The thinnest a cycle's supply comes in at, as a fraction of the level's baseline...
  /// </summary>
  private const double JitterMin = 0.75;

  /// <summary>
  /// ...and how far above that it can reach, so a cycle runs 75%..125% of baseline. Without
  /// this every cycle would deal the same hand and prices would only move as the player
  /// levels.
  /// </summary>
  private const double JitterSpan = 0.50;

  /// <summary>
  /// The multiplier the player's level puts on supply: <c>1.0</c> at level 1, rising
  /// linearly.
  /// </summary>
  public static double LevelScale(int level)
    => 1.0 + ((level - 1) * GrowthPerLevel);

  /// <summary>
  /// Maps a <c>[0,1)</c> draw onto this cycle's supply jitter — see <see cref="JitterMin"/>.
  /// </summary>
  public static double Jitter(double roll)
    => JitterMin + (JitterSpan * roll);

  /// <summary>
  /// How many units of one drug enter the world this cycle, across the whole roster.
  /// </summary>
  /// <param name="supplyBase">The drug's <see cref="Drug.SupplyBase"/> — units per dealer at level 1.</param>
  /// <param name="dealerCount">How many dealers share it out.</param>
  /// <param name="level">The player's level, via <see cref="LevelScale"/>.</param>
  /// <param name="jitter">This cycle's variation, from <see cref="Jitter"/>.</param>
  /// <param name="supplyScale">The <c>[Economy] MarketSupplyPercent</c> knob as a fraction.</param>
  public static int TotalSupply(int supplyBase, int dealerCount, int level, double jitter, double supplyScale)
  {
    if (supplyBase <= 0 || dealerCount <= 0)
    {
      return 0;
    }

    double total = (double)supplyBase * dealerCount * LevelScale(level) * jitter * supplyScale;

    return (total <= 0.0) ? 0 : (int)Math.Round(total);
  }

  /// <summary>
  /// The share a dealer holding exactly his fair portion would have. This is the pivot every
  /// price turns on — see <see cref="MarketPricing.ScarcityMultiplier"/>.
  /// </summary>
  public static double MeanShare(int total, int dealerCount)
    => (dealerCount <= 0 || total <= 0) ? 0.0 : (double)total / dealerCount;
}
