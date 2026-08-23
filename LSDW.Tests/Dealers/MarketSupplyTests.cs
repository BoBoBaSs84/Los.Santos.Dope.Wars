using LSDW.Dealers;
using LSDW.Leveling;

namespace LSDW.Tests.Dealers;

/// <summary>
/// How much of a drug reaches the streets in one cycle, and how that grows with the player.
/// </summary>
/// <remarks>
/// The curve takes a level as an argument rather than reading one, which is what lets every level
/// be checked without a player who has reached it — <see cref="Progression.MaxLevel"/> is read
/// here as the constant it is. The levelling itself is driven in <c>ProgressionTests</c>; nothing
/// on this path needs it.
/// </remarks>
[TestClass]
public sealed class MarketSupplyTests
{
  private const int DealerCount = 50;
  private const int SupplyBase = 5;
  private const double NoJitter = 1.0;
  private const double NoScale = 1.0;

  [TestMethod]
  public void LevelScale_AtLevelOne_IsUnscaled()
  {
    Assert.AreEqual(1.0, MarketSupply.LevelScale(1));
  }

  [TestMethod]
  public void LevelScale_AcrossEveryLevel_Rises()
  {
    for (int level = 2; level <= Progression.MaxLevel; level++)
    {
      Assert.IsGreaterThan(MarketSupply.LevelScale(level - 1), MarketSupply.LevelScale(level), $"level {level}");
    }
  }

  [TestMethod]
  public void LevelScale_AtTheCap_DeepensTheMarketWithoutDrowningIt()
  {
    // A market that grows too fast stops rewarding the search for a glut, because everything is
    // a glut. Roughly threefold across a hundred levels is the intended shape.
    double atCap = MarketSupply.LevelScale(Progression.MaxLevel);

    Assert.IsGreaterThan(2.0, atCap);
    Assert.IsLessThan(4.0, atCap);
  }

  [TestMethod]
  public void Jitter_AcrossItsRange_StaysWithinThreeQuartersAndFiveQuarters()
  {
    // NextDouble() is [0,1), so the top of the band is approached but never reached.
    Assert.AreEqual(0.75, MarketSupply.Jitter(0.0));
    Assert.IsLessThan(1.25, MarketSupply.Jitter(0.999999));
    Assert.IsGreaterThan(1.0, MarketSupply.Jitter(0.999999));
  }

  [TestMethod]
  public void TotalSupply_ScalesWithTheDealerCount()
  {
    int fifty = MarketSupply.TotalSupply(SupplyBase, DealerCount, 1, NoJitter, NoScale);
    int hundred = MarketSupply.TotalSupply(SupplyBase, DealerCount * 2, 1, NoJitter, NoScale);

    Assert.AreEqual(250, fifty);
    Assert.AreEqual(fifty * 2, hundred);
  }

  [TestMethod]
  public void TotalSupply_ScalesWithTheDrugsBaseline()
  {
    Assert.AreEqual(
      MarketSupply.TotalSupply(SupplyBase, DealerCount, 1, NoJitter, NoScale) * 2,
      MarketSupply.TotalSupply(SupplyBase * 2, DealerCount, 1, NoJitter, NoScale));
  }

  [TestMethod]
  public void TotalSupply_ScalesWithTheLevel()
  {
    Assert.IsGreaterThan(
      MarketSupply.TotalSupply(SupplyBase, DealerCount, 1, NoJitter, NoScale),
      MarketSupply.TotalSupply(SupplyBase, DealerCount, Progression.MaxLevel, NoJitter, NoScale));
  }

  [TestMethod]
  public void TotalSupply_ScalesWithTheSettingsKnob()
  {
    int baseline = MarketSupply.TotalSupply(SupplyBase, DealerCount, 1, NoJitter, NoScale);

    Assert.AreEqual(baseline * 2, MarketSupply.TotalSupply(SupplyBase, DealerCount, 1, NoJitter, 2.0));
    Assert.AreEqual(baseline / 2, MarketSupply.TotalSupply(SupplyBase, DealerCount, 1, NoJitter, 0.5));
  }

  [TestMethod]
  public void TotalSupply_WithNothingToGiveOut_IsNothing()
  {
    Assert.AreEqual(0, MarketSupply.TotalSupply(0, DealerCount, 1, NoJitter, NoScale));
    Assert.AreEqual(0, MarketSupply.TotalSupply(SupplyBase, 0, 1, NoJitter, NoScale));
  }

  [TestMethod]
  public void MeanShare_IsTheTotalSpreadEvenly()
  {
    Assert.AreEqual(5.0, MarketSupply.MeanShare(250, DealerCount));
  }

  [TestMethod]
  public void MeanShare_WithNothingToSpread_IsZero()
  {
    // Guards the division, and is the value MarketPricing reads as "this drug is not on the
    // market" — see its ceiling behaviour.
    Assert.AreEqual(0.0, MarketSupply.MeanShare(0, DealerCount));
    Assert.AreEqual(0.0, MarketSupply.MeanShare(250, 0));
  }
}
