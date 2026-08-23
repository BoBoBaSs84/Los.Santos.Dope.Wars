namespace LSDW.Leveling;

/// <summary>
/// The passive bonuses the milestone rewards grant, each a pure function of the level.
/// The subsystems that care read these at the point of use; nothing has to be "applied"
/// or persisted.
/// </summary>
/// <remarks>
/// Static while <see cref="Progression"/> is not, and takes the level rather than reading
/// it: there is no state here to own, and a caller that already holds the progression can
/// hand over the number it wants the answer for. That also makes every threshold below
/// checkable without a level to reach first.
/// </remarks>
internal static class Perks
{
  /// <summary>
  /// L30 — dealers charge the player 10% less to buy; the L100 capstone deepens it to 15%.
  /// </summary>
  /// <param name="level">The player's level.</param>
  public static double BuyPriceMultiplier(int level) => level >= 100 ? 0.85 : level >= 30 ? 0.90 : 1.0;

  /// <summary>
  /// L70 — dealers pay the player 10% more to sell; the L100 capstone raises it to 15%.
  /// </summary>
  /// <param name="level">The player's level.</param>
  public static double SellPriceMultiplier(int level) => level >= 100 ? 1.15 : level >= 70 ? 1.10 : 1.0;

  /// <summary>
  /// L40 boosts XP gain to ×1.25. There is no higher tier: at the level-100 cap there
  /// is nothing left to level, so the capstone gives price perks, not more XP.
  /// </summary>
  /// <param name="level">The player's level.</param>
  public static double XpMultiplierBonus(int level) => level >= 40 ? 1.25 : 1.0;

  /// <summary>
  /// L50 adds 200 bag slots on top of the linear curve; L100 adds 300 more.
  /// </summary>
  /// <param name="level">The player's level.</param>
  public static int BagBonus(int level) => (level >= 50 ? 200 : 0) + (level >= 100 ? 300 : 0);

  /// <summary>
  /// L60 "cooler head" — the taco-van sliding-window gap under which selling too fast
  /// draws heat, tightened from 15 to 10 so the player has to sell faster to trigger it.
  /// </summary>
  /// <param name="level">The player's level.</param>
  public static int TacoHeatGapThreshold(int level) => level >= 60 ? 10 : 15;

  /// <summary>
  /// L80 "fuller streets" — the taco interest roll is one-in-<i>this</i>; dropping from
  /// 3 to 2 makes a customer half likely to want a sale instead of a third.
  /// </summary>
  /// <param name="level">The player's level.</param>
  public static int TacoInterestOneIn(int level) => level >= 80 ? 2 : 3;

  /// <summary>
  /// L90 "low profile" — a taco session no longer marks the player as an enemy, so local
  /// gangs leave them alone.
  /// </summary>
  /// <param name="level">The player's level.</param>
  public static bool TacoDrawsGangs(int level) => level < 90;
}
