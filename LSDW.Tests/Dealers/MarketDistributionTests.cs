using LSDW.Dealers;

namespace LSDW.Tests.Dealers;

/// <summary>
/// The apportionment that splits a cycle's supply across the roster.
/// </summary>
/// <remarks>
/// The weights are drawn in <c>DealerMarket</c> and passed in, so these use hand-written
/// vectors. What matters is that no unit is ever lost or invented: the total is also what every
/// price is measured against, so an apportionment that does not add up misprices the whole map.
/// </remarks>
[TestClass]
public sealed class MarketDistributionTests
{
  /// <summary>
  /// Deals <paramref name="total"/> units across <paramref name="weights"/> and hands back the
  /// shares. The buffer is the caller's responsibility in the real signature, because
  /// <c>DealerMarket</c> reuses one across every drug in the catalog; nothing here needs that, so
  /// it is allocated per call and the tests read as weights in, shares out.
  /// </summary>
  private static int[] Apportion(double[] weights, int total)
  {
    int[] shares = new int[weights.Length];

    MarketDistribution.Apportion(weights, total, shares);

    return shares;
  }

  [TestMethod]
  public void Apportion_AcrossManyTotals_HandsOutEveryUnit()
  {
    // The one test that keeps its own buffer rather than going through the helper: reusing it
    // across every total is also what the real caller does, so a share left behind from the
    // previous deal would show up here.
    double[] weights = [0.4, 1.9, 0.0, 3.2, 0.7, 0.0, 5.1, 0.02, 1.0, 2.5];
    int[] shares = new int[weights.Length];

    for (int total = 0; total <= 2000; total++)
    {
      MarketDistribution.Apportion(weights, total, shares);

      Assert.AreEqual(total, shares.Sum(), $"total {total} did not add up");
    }
  }

  [TestMethod]
  public void Apportion_GivesNothingToAZeroWeight()
  {
    int[] shares = Apportion([1.0, 0.0, 2.0, 0.0, 3.0], 600);

    Assert.AreEqual(0, shares[1]);
    Assert.AreEqual(0, shares[3]);
    Assert.AreEqual(600, shares.Sum());
  }

  [TestMethod]
  public void Apportion_WithEveryDealerDry_LeavesTheSupplyOnTheDock()
  {
    // The documented exception to "the shares sum to the total": nobody can be given anything,
    // so nothing reaches the street. Unreachable with the shipped roster size, but it must not
    // hand the whole supply to dealer zero either.
    int[] shares = Apportion([0.0, 0.0, 0.0], 500);

    Assert.AreEqual(0, shares.Sum());
  }

  [TestMethod]
  public void Apportion_WithOneWeightedDealer_GivesHimTheLot()
  {
    int[] shares = Apportion([0.0, 0.0, 0.75, 0.0], 137);

    Assert.AreEqual(137, shares[2]);
    Assert.AreEqual(137, shares.Sum());
  }

  [TestMethod]
  public void Apportion_NeverGivesAHeavierWeightLess()
  {
    // Ordering is the whole point: the dealer with the biggest weight is the glut, and he is
    // the one the player buys from.
    int[] shares = Apportion([0.5, 1.0, 2.0, 4.0, 8.0], 3100);

    for (int index = 1; index < shares.Length; index++)
    {
      Assert.IsGreaterThanOrEqualTo(shares[index - 1], shares[index], $"dealer {index} carried less weight but more stock");
    }
  }

  [TestMethod]
  public void Apportion_WithEqualWeights_SplitsEvenly()
  {
    int[] shares = Apportion([1.0, 1.0, 1.0, 1.0], 100);

    Assert.AreSequenceEqual([25, 25, 25, 25], shares);
  }

  [TestMethod]
  public void Apportion_WithATotalBelowTheDealerCount_StillAddsUp()
  {
    // More dealers than units. Somebody has to get nothing, and no unit may be conjured to
    // round everyone up to one.
    int[] shares = Apportion([1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0], 3);

    Assert.AreEqual(3, shares.Sum());
  }

  [TestMethod]
  public void Apportion_TreatsANegativeWeightAsDry()
  {
    int[] shares = Apportion([-1.0, 2.0], 50);

    Assert.AreEqual(0, shares[0]);
    Assert.AreEqual(50, shares[1]);
  }

  [TestMethod]
  public void Apportion_WithABufferTooShort_Throws()
  {
    double[] weights = [1.0, 1.0];

    _ = Assert.ThrowsExactly<ArgumentException>(() => MarketDistribution.Apportion(weights, 10, new int[1]));
  }

  [TestMethod]
  public void Apportion_WithANegativeTotal_Throws()
  {
    double[] weights = [1.0];

    _ = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => MarketDistribution.Apportion(weights, -1, new int[1]));
  }
}
