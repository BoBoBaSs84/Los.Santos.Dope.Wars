using LSDW.Leveling;

namespace LSDW.Tests.Leveling;

/// <summary>
/// The milestone thresholds: which level turns each passive bonus on, and what it is
/// worth either side of it.
/// </summary>
/// <remarks>
/// Every perk is a pure function of a level, so all eight milestones are checked either side of
/// their threshold without a player who has reached one. Every expected value below predates the
/// conversion from properties that read the level to methods that take it — which is what makes
/// them evidence of the intended behaviour rather than a transcription of the current
/// implementation.
/// </remarks>
[TestClass]
public sealed class PerksTests
{
  private const double Tolerance = 1e-9;

  /// <summary>
  /// Every perk at one level, as one comparable value — what
  /// <see cref="EveryPerk_ChangesOnlyAtItsMilestone"/> walks the whole range with.
  /// </summary>
  private static (double, double, double, int, int, int, bool) Snapshot(int level)
    => (Perks.BuyPriceMultiplier(level), Perks.SellPriceMultiplier(level), Perks.XpMultiplierBonus(level),
        Perks.BagBonus(level), Perks.TacoHeatGapThreshold(level), Perks.TacoInterestOneIn(level), Perks.TacoDrawsGangs(level));

  [TestMethod]
  [DataRow(1, 1.0)]
  [DataRow(29, 1.0)]
  [DataRow(30, 0.90)]
  [DataRow(99, 0.90)]
  [DataRow(100, 0.85)]
  public void BuyPriceMultiplier_CheapensAt30And100(int level, double expected)
  {
    Assert.AreEqual(expected, Perks.BuyPriceMultiplier(level), Tolerance);
  }

  [TestMethod]
  [DataRow(1, 1.0)]
  [DataRow(69, 1.0)]
  [DataRow(70, 1.10)]
  [DataRow(99, 1.10)]
  [DataRow(100, 1.15)]
  public void SellPriceMultiplier_PaysMoreAt70And100(int level, double expected)
  {
    Assert.AreEqual(expected, Perks.SellPriceMultiplier(level), Tolerance);
  }

  /// <remarks>
  /// No tier above 40: at the cap there is nothing left to level, so the capstone grants
  /// price perks rather than more XP.
  /// </remarks>
  [TestMethod]
  [DataRow(1, 1.0)]
  [DataRow(39, 1.0)]
  [DataRow(40, 1.25)]
  [DataRow(100, 1.25)]
  public void XpMultiplierBonus_RisesAt40AndStaysThere(int level, double expected)
  {
    Assert.AreEqual(expected, Perks.XpMultiplierBonus(level), Tolerance);
  }

  /// <remarks>
  /// The two tiers stack: 200 at level 50 plus 300 at the cap is 500, not 300.
  /// </remarks>
  [TestMethod]
  [DataRow(1, 0)]
  [DataRow(49, 0)]
  [DataRow(50, 200)]
  [DataRow(99, 200)]
  [DataRow(100, 500)]
  public void BagBonus_AddsAt50AndAgainAt100(int level, int expected)
  {
    Assert.AreEqual(expected, Perks.BagBonus(level));
  }

  [TestMethod]
  [DataRow(1, 15)]
  [DataRow(59, 15)]
  [DataRow(60, 10)]
  [DataRow(100, 10)]
  public void TacoHeatGapThreshold_TightensAt60(int level, int expected)
  {
    Assert.AreEqual(expected, Perks.TacoHeatGapThreshold(level));
  }

  [TestMethod]
  [DataRow(1, 3)]
  [DataRow(79, 3)]
  [DataRow(80, 2)]
  [DataRow(100, 2)]
  public void TacoInterestOneIn_HalvesTheOddsAt80(int level, int expected)
  {
    Assert.AreEqual(expected, Perks.TacoInterestOneIn(level));
  }

  [TestMethod]
  [DataRow(1, true)]
  [DataRow(89, true)]
  [DataRow(90, false)]
  [DataRow(100, false)]
  public void TacoDrawsGangs_StopsAt90(int level, bool expected)
  {
    Assert.AreEqual(expected, Perks.TacoDrawsGangs(level));
  }

  /// <summary>
  /// The perks are step functions, so every level between two milestones has to agree with
  /// the milestone below it. This walks all hundred rather than trusting the boundaries
  /// above to be the only edges.
  /// </summary>
  [TestMethod]
  public void EveryPerk_ChangesOnlyAtItsMilestone()
  {
    int[] milestones = [30, 40, 50, 60, 70, 80, 90, 100];

    for (int level = 2; level <= Progression.MaxLevel; level++)
    {
      bool changed = Snapshot(level) != Snapshot(level - 1);

      Assert.AreEqual(milestones.Contains(level), changed, "level " + level);
    }
  }
}
