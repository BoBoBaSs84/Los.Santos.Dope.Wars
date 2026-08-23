using LSDW.Activities;

namespace LSDW.Tests.Activities;

/// <summary>
/// The sliding window that decides when working the taco van too fast draws police
/// attention.
/// </summary>
/// <remarks>
/// <c>DrawsHeat</c> is checked against synthetic timestamps, which is what it was split out of
/// <c>RecordSale</c> for. <c>RecordSale</c> itself is covered as far as the wall clock allows:
/// the window is per-instance now, so a test gets a clean one, and everything that follows from
/// the <i>count</i> of sales rather than their spacing is checkable. What is not is the spacing
/// — the clock is reached for rather than passed in, so the tests below only assert what holds
/// for sales that all land inside one window.
/// </remarks>
[TestClass]
public sealed class TacoHeatWindowTests
{
  /// <summary>The default gap threshold; <c>Perks.TacoHeatGapThreshold</c> tightens it to 10 at L60.</summary>
  private const int DefaultThreshold = 15;

  [TestMethod]
  public void DrawsHeat_SalesBunchedTogether_IsTrue()
  {
    Assert.IsTrue(TacoHeatWindow.DrawsHeat([100, 101, 102, 103], DefaultThreshold));
  }

  [TestMethod]
  public void DrawsHeat_SalesSpreadOut_IsFalse()
  {
    Assert.IsFalse(TacoHeatWindow.DrawsHeat([100, 200, 300, 400], DefaultThreshold));
  }

  [TestMethod]
  public void DrawsHeat_SpanExactlyAtTheThreshold_IsTrue()
  {
    // Inclusive: the comparison is <=.
    Assert.IsTrue(TacoHeatWindow.DrawsHeat([100, 105, 110, 115], DefaultThreshold));
  }

  [TestMethod]
  public void DrawsHeat_SpanOneOverTheThreshold_IsFalse()
  {
    Assert.IsFalse(TacoHeatWindow.DrawsHeat([100, 105, 110, 116], DefaultThreshold));
  }

  [TestMethod]
  [DataRow(new[] { 100, 101, 102, 103 })]
  [DataRow(new[] { 100, 200, 300, 400 })]
  [DataRow(new[] { 0, 1, 1, 9999 })]
  [DataRow(new[] { 5, 5, 5, 5 }, DisplayName = "Identical timestamps")]
  public void DrawsHeat_SumOfGaps_TelescopesToLastMinusFirst(int[] timestamps)
  {
    // The loop is left in its original accumulating form rather than simplified. This
    // pins the equivalence, so a future simplification can be checked rather than argued.
    int span = timestamps[timestamps.Length - 1] - timestamps[0];

    Assert.IsTrue(TacoHeatWindow.DrawsHeat(timestamps, span));
    Assert.IsFalse(TacoHeatWindow.DrawsHeat(timestamps, span - 1));
  }

  [TestMethod]
  public void DrawsHeat_SingleTimestamp_HasZeroSpanAndDrawsHeat()
  {
    // Not reachable from RecordSale, which only calls this with a full window of four.
    Assert.IsTrue(TacoHeatWindow.DrawsHeat([100], 0));
  }

  [TestMethod]
  public void DrawsHeat_EmptyWindow_Throws()
  {
    // Documents the edge rather than endorsing it: the first read is [Count - 1].
    _ = Assert.Throws<ArgumentOutOfRangeException>(() => TacoHeatWindow.DrawsHeat([], DefaultThreshold));
  }

  [TestMethod]
  public void DrawsHeat_TighterPerkThreshold_NeedsFasterSelling()
  {
    // The L60 "cooler head" perk drops the threshold from 15 to 10, so a run that drew
    // heat before no longer does.
    int[] window = [100, 104, 108, 112];

    Assert.IsTrue(TacoHeatWindow.DrawsHeat(window, 15));
    Assert.IsFalse(TacoHeatWindow.DrawsHeat(window, 10));
  }

  [TestMethod]
  public void RecordSale_ForTheFirstFourSales_DrawsNoHeat()
  {
    // The documented off-by-one: the early return covers counts 0..3, so the *fifth* sale is the
    // first that can draw heat, not the fourth. A threshold nothing could beat is passed in to
    // show the count is what decides, not the spacing.
    TacoHeatWindow window = new();

    for (int sale = 1; sale <= 4; sale++)
    {
      Assert.IsFalse(window.RecordSale(int.MaxValue), "sale " + sale + " drew heat");
    }
  }

  [TestMethod]
  public void RecordSale_OnTheFifthQuickSale_DrawsHeat()
  {
    // Five calls in a test are inside one another by a wide margin, so the span is nowhere near
    // the default threshold whatever the machine is doing.
    TacoHeatWindow window = new();

    for (int sale = 1; sale <= 4; sale++)
    {
      _ = window.RecordSale(DefaultThreshold);
    }

    Assert.IsTrue(window.RecordSale(DefaultThreshold));
  }

  [TestMethod]
  public void RecordSale_WithAThresholdNoSpanCanMeet_NeverDrawsHeat()
  {
    // The span is a sum of non-negative gaps, so a negative threshold is unmeetable — which is
    // what a full window looks like from the other side of the comparison.
    TacoHeatWindow window = new();

    for (int sale = 1; sale <= 10; sale++)
    {
      Assert.IsFalse(window.RecordSale(-1), "sale " + sale + " met an impossible threshold");
    }
  }

  [TestMethod]
  public void RecordSale_OnAFreshWindow_StartsTheCountAgain()
  {
    // What owning the window rather than leaving it ambient buys: a script reload builds a new
    // one, so the previous run's sales cannot draw heat for this one.
    TacoHeatWindow first = new();
    for (int sale = 1; sale <= 5; sale++)
    {
      _ = first.RecordSale(DefaultThreshold);
    }

    TacoHeatWindow second = new();

    Assert.IsFalse(second.RecordSale(DefaultThreshold), "a fresh window inherited a full one");
  }

  [TestMethod]
  public void RecordSale_PastTheFifthSale_KeepsDrawingHeat()
  {
    // The window slides rather than resetting, so a player who keeps selling keeps drawing heat
    // instead of getting four free sales back.
    TacoHeatWindow window = new();

    for (int sale = 1; sale <= 4; sale++)
    {
      _ = window.RecordSale(DefaultThreshold);
    }

    for (int sale = 5; sale <= 8; sale++)
    {
      Assert.IsTrue(window.RecordSale(DefaultThreshold), "sale " + sale + " stopped drawing heat");
    }
  }
}
