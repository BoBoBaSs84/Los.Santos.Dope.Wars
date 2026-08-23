using LSDW.UserInterface;

namespace LSDW.Tests.UserInterface;

/// <summary>
/// The strings a transaction window puts in front of the player: the price comparison, the
/// running total, and the profit-or-loss verdict on the trade they are about to confirm.
/// </summary>
/// <remarks>
/// First coverage of anything in <c>UserInterface</c>. It is reachable because the arithmetic
/// moved off <c>TransactionWindow</c>, whose every setter writes into a
/// <c>GTA.UI.TextElement</c>; the window itself still needs the game and still is not covered.
/// <para>
/// The expected strings are written out in full rather than assembled from the same pieces the
/// production code uses. A test that rebuilt them would agree with any change made to them.
/// </para>
/// </remarks>
[TestClass]
public sealed class TransactionSummaryTests
{
  [TestMethod]
  public void Delta_AtZero_ReadsGreen()
  {
    // The boundary is `>= 0`, so breaking even shows green rather than red. A player who
    // buys at exactly market value has not lost anything.
    Assert.AreEqual("~g~$0", TransactionSummary.Delta(0));
  }

  [TestMethod]
  [DataRow(250, "~g~$250", DisplayName = "In the player's favour")]
  [DataRow(-250, "~r~$-250", DisplayName = "Against them — the sign stays in the figure")]
  public void Delta_TakesItsColourFromTheSign(int delta, string expected)
  {
    Assert.AreEqual(expected, TransactionSummary.Delta(delta));
  }

  [TestMethod]
  public void MoneyText_WhenBuying_MeasuresTheMarketAgainstThePrice()
  {
    // Buying under the market is the win, so the delta is market minus price.
    Assert.AreEqual("$850\n$700\n~g~$150", TransactionSummary.MoneyText(sellingDrugs: false, value: 700, normalValue: 850));
  }

  [TestMethod]
  public void MoneyText_WhenBuyingOverTheMarket_ReadsRed()
  {
    Assert.AreEqual("$850\n$1000\n~r~$-150", TransactionSummary.MoneyText(sellingDrugs: false, value: 1000, normalValue: 850));
  }

  [TestMethod]
  public void MoneyText_WhenSelling_MeasuresThePriceAgainstWhatWasPaid()
  {
    // The other way round: being paid above cost is the win.
    Assert.AreEqual("$700\n$850\n~g~$150", TransactionSummary.MoneyText(sellingDrugs: true, value: 850, normalValue: 700));
  }

  [TestMethod]
  public void MoneyText_WhenSellingUnderCost_ReadsRed()
  {
    Assert.AreEqual("$700\n$600\n~r~$-100", TransactionSummary.MoneyText(sellingDrugs: true, value: 600, normalValue: 700));
  }

  [TestMethod]
  public void MoneyText_AtTheSamePrice_ReadsGreenBothWays()
  {
    // The one input where the two directions agree, and they must: the delta is zero, and
    // zero is green by Delta's boundary.
    Assert.AreEqual("$850\n$850\n~g~$0", TransactionSummary.MoneyText(sellingDrugs: false, value: 850, normalValue: 850));
    Assert.AreEqual("$850\n$850\n~g~$0", TransactionSummary.MoneyText(sellingDrugs: true, value: 850, normalValue: 850));
  }

  [TestMethod]
  public void MoneyTextWithMarket_LaysOutFourLines()
  {
    // Sell side only: market, what was paid, what this dealer offers, and the margin over
    // cost — which is the third line against the second, not against the first.
    Assert.AreEqual(
      "$850\n$700\n$900\n~g~$200",
      TransactionSummary.MoneyTextWithMarket(globalValue: 850, normalValue: 700, value: 900));
  }

  [TestMethod]
  public void MoneyTextWithMarket_BelowCost_ReadsRed()
  {
    Assert.AreEqual(
      "$850\n$700\n$650\n~r~$-50",
      TransactionSummary.MoneyTextWithMarket(globalValue: 850, normalValue: 700, value: 650));
  }

  [TestMethod]
  public void SummaryText_HeadsWithBagsTimesPrice()
  {
    string summary = TransactionSummary.SummaryText(sellingDrugs: false, amount: 4, value: 700, normalValue: 850);

    Assert.StartsWith("4 bags X $700 = $2800", summary);
  }

  [TestMethod]
  public void SummaryText_WhenBuyingUnderTheMarket_PromisesProfit()
  {
    Assert.AreEqual(
      "4 bags X $700 = $2800\n\n~g~Potential Profit of $600",
      TransactionSummary.SummaryText(sellingDrugs: false, amount: 4, value: 700, normalValue: 850));
  }

  [TestMethod]
  public void SummaryText_WhenBuyingOverTheMarket_WarnsOfLoss()
  {
    Assert.AreEqual(
      "4 bags X $1000 = $4000\n\n~r~Potential Loss of $600",
      TransactionSummary.SummaryText(sellingDrugs: false, amount: 4, value: 1000, normalValue: 850));
  }

  [TestMethod]
  public void SummaryText_WhenSellingAboveCost_PromisesProfit()
  {
    Assert.AreEqual(
      "4 bags X $850 = $3400\n\n~g~Potential Profit of $600",
      TransactionSummary.SummaryText(sellingDrugs: true, amount: 4, value: 850, normalValue: 700));
  }

  [TestMethod]
  public void SummaryText_WhenSellingBelowCost_WarnsOfLoss()
  {
    Assert.AreEqual(
      "4 bags X $600 = $2400\n\n~r~Potential Loss of $400",
      TransactionSummary.SummaryText(sellingDrugs: true, amount: 4, value: 600, normalValue: 700));
  }

  [TestMethod]
  public void SummaryText_AtZeroBuyingAGoodDeal_StillReadsAsProfit()
  {
    // The rule the whole type exists to protect. A window opens at Amount 0, where the total
    // is 0 whichever way the deal runs. The verdict comes off the per-unit delta, so a good
    // deal opens green — branching on the total would open every window red.
    Assert.AreEqual(
      "0 bags X $700 = $0\n\n~g~Potential Profit of $0",
      TransactionSummary.SummaryText(sellingDrugs: false, amount: 0, value: 700, normalValue: 850));
  }

  [TestMethod]
  public void SummaryText_AtZeroBuyingABadDeal_ReadsAsLoss()
  {
    // The same zero total, the opposite verdict, from the same per-unit rule.
    Assert.AreEqual(
      "0 bags X $1000 = $0\n\n~r~Potential Loss of $0",
      TransactionSummary.SummaryText(sellingDrugs: false, amount: 0, value: 1000, normalValue: 850));
  }

  [TestMethod]
  public void SummaryText_AtZeroSelling_TakesTheOppositeVerdictFromTheSameNumbers()
  {
    // value 850 against a cost of 700: a profit to a seller, a bad buy to a buyer. Same
    // three numbers, same zero amount, opposite colours — which is what makes the direction
    // flag load-bearing rather than cosmetic.
    Assert.StartsWith("0 bags X $850 = $0\n\n~g~", TransactionSummary.SummaryText(sellingDrugs: true, amount: 0, value: 850, normalValue: 700));
    Assert.StartsWith("0 bags X $850 = $0\n\n~r~", TransactionSummary.SummaryText(sellingDrugs: false, amount: 0, value: 850, normalValue: 700));
  }

  [TestMethod]
  public void SummaryText_AtBreakEven_ReadsProfitBuyingAndLossSelling()
  {
    // The one asymmetry in the branch: buying is `>= 0` and selling is `< 0`, so paying
    // exactly the reference price is a profit to a buyer and a loss to a seller. Pinned
    // because it is the only input where the two directions disagree about zero.
    Assert.StartsWith("2 bags X $850 = $1700\n\n~g~", TransactionSummary.SummaryText(sellingDrugs: false, amount: 2, value: 850, normalValue: 850));
    Assert.StartsWith("2 bags X $850 = $1700\n\n~r~", TransactionSummary.SummaryText(sellingDrugs: true, amount: 2, value: 850, normalValue: 850));
  }
}
