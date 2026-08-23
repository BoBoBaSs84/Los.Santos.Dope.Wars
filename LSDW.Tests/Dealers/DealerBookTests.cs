using LSDW.Dealers;
using LSDW.Drugs;

namespace LSDW.Tests.Dealers;

/// <summary>
/// One dealer's trading position: what a trade does to it, what a trade must <i>not</i> do to
/// it, and the special offer that stands outside the model and then has to be undone exactly.
/// </summary>
/// <remarks>
/// None of this was reachable before. The three maps hung off <c>DrugDealer</c>, whose
/// constructor creates a blip and therefore needs the game, so the rules below existed only as
/// comments over the two call sites in <c>DealerTrading</c> that had to honour them.
/// </remarks>
[TestClass]
public sealed class DealerBookTests
{
  private const string Cocaine = "Cocaine";
  private const string Weed = "Weed";

  /// <summary>
  /// A book with a known position in <see cref="Cocaine"/>, since a fresh one is all zeroes
  /// and a trade against nothing proves nothing.
  /// </summary>
  private static DealerBook Stocked(int amount = 20, int price = 900, int demand = 10)
  {
    DealerBook book = new();
    book.SetPosition(Cocaine, amount, price, demand);

    return book;
  }

  [TestMethod]
  public void NewBook_HoldsNothingAndAsksMarketValue()
  {
    // NewPriceMap, not NewAmountMap: a dealer whose prices have never been set reads as
    // asking market value rather than as giving everything away.
    DealerBook book = new();

    foreach (Drug drug in DrugCatalog.All)
    {
      Assert.AreEqual(0, book.Amount[drug.Name], drug.Name + " started with stock");
      Assert.AreEqual(0, book.Demand[drug.Name], drug.Name + " started with an appetite");
      Assert.AreEqual(drug.MarketValue, book.Price[drug.Name], drug.Name + " started off market value");
    }
  }

  [TestMethod]
  public void NewBook_HasNoOffer()
  {
    DealerBook book = new();

    Assert.IsFalse(book.HasOffer);
    Assert.IsNull(book.OfferDrug);
    Assert.AreEqual(DealerOfferKind.None, book.Offer);
  }

  [TestMethod]
  public void Knows_AnswersForTheCatalogOnly()
  {
    DealerBook book = new();

    Assert.IsTrue(book.Knows(Cocaine));
    Assert.IsFalse(book.Knows("Mandrax"), "a drug this build never shipped");
  }

  [TestMethod]
  public void SetPosition_WritesAllThreeAtOnce()
  {
    DealerBook book = new();

    book.SetPosition(Cocaine, 42, 1234, 7);

    Assert.AreEqual(42, book.Amount[Cocaine]);
    Assert.AreEqual(1234, book.Price[Cocaine]);
    Assert.AreEqual(7, book.Demand[Cocaine]);
  }

  [TestMethod]
  public void SetPosition_LeavesEveryOtherDrugAlone()
  {
    DealerBook book = new();

    book.SetPosition(Cocaine, 42, 1234, 7);

    Assert.AreEqual(0, book.Amount[Weed]);
    Assert.AreEqual(0, book.Demand[Weed]);
  }

  [TestMethod]
  [DataRow(1, DisplayName = "One gram")]
  [DataRow(7, DisplayName = "Part of the stock")]
  [DataRow(20, DisplayName = "The whole stock")]
  public void PlayerBought_ConservesAmountPlusDemand(int amount)
  {
    // The rule the market rests on: taking stock leaves him that much shorter, so his
    // appetite grows by exactly what his shelf lost. It is what stops the player emptying a
    // bag into the one dealer paying the most.
    DealerBook book = Stocked();
    int before = book.Amount[Cocaine] + book.Demand[Cocaine];

    book.PlayerBought(Cocaine, amount);

    Assert.AreEqual(20 - amount, book.Amount[Cocaine]);
    Assert.AreEqual(10 + amount, book.Demand[Cocaine]);
    Assert.AreEqual(before, book.Amount[Cocaine] + book.Demand[Cocaine]);
  }

  [TestMethod]
  [DataRow(1, DisplayName = "One gram")]
  [DataRow(7, DisplayName = "Part of his appetite")]
  [DataRow(10, DisplayName = "All he wanted")]
  public void PlayerSold_ConservesAmountPlusDemand(int amount)
  {
    DealerBook book = Stocked();
    int before = book.Amount[Cocaine] + book.Demand[Cocaine];

    book.PlayerSold(Cocaine, amount);

    Assert.AreEqual(20 + amount, book.Amount[Cocaine]);
    Assert.AreEqual(10 - amount, book.Demand[Cocaine]);
    Assert.AreEqual(before, book.Amount[Cocaine] + book.Demand[Cocaine]);
  }

  [TestMethod]
  public void PlayerBought_ThenSoldBack_RestoresThePosition()
  {
    DealerBook book = Stocked();

    book.PlayerBought(Cocaine, 8);
    book.PlayerSold(Cocaine, 8);

    Assert.AreEqual(20, book.Amount[Cocaine]);
    Assert.AreEqual(10, book.Demand[Cocaine]);
  }

  [TestMethod]
  public void PlayerBought_DoesNotMoveThePrice()
  {
    // Price is written once per cycle and never again. If trading moved it, a dealer's
    // quote would drift as the player worked him, and the round trip the 15% spread is
    // sized to forbid would open up.
    DealerBook book = Stocked();

    book.PlayerBought(Cocaine, 20);

    Assert.AreEqual(900, book.Price[Cocaine]);
  }

  [TestMethod]
  public void PlayerSold_DoesNotMoveThePrice()
  {
    DealerBook book = Stocked();

    book.PlayerSold(Cocaine, 10);

    Assert.AreEqual(900, book.Price[Cocaine]);
  }

  [TestMethod]
  public void PlayerSold_PastHisAppetite_GoesNegative()
  {
    // Documenting what the book does, not endorsing it. Nothing here clamps the appetite;
    // what stops it in the game is the sell window, which caps the amount at Demand before
    // it ever reaches this method. Read this test as the reason that cap has to stay.
    DealerBook book = Stocked();

    book.PlayerSold(Cocaine, 15);

    Assert.AreEqual(-5, book.Demand[Cocaine]);
  }

  [TestMethod]
  public void ApplyInDemand_LeavesHimBuyOnly()
  {
    // He needs it, so he has none — which is what makes him buy-only, exactly as an
    // ordinary dry dealer is.
    DealerBook book = Stocked();

    book.ApplyInDemand(Cocaine, 1500, 30);

    Assert.AreEqual(0, book.Amount[Cocaine]);
    Assert.AreEqual(1500, book.Price[Cocaine]);
    Assert.AreEqual(30, book.Demand[Cocaine]);
    Assert.AreEqual(DealerOfferKind.InDemand, book.Offer);
    Assert.AreEqual(Cocaine, book.OfferDrug);
    Assert.IsTrue(book.HasOffer);
  }

  [TestMethod]
  public void ApplyClearOut_LeavesHimSellOnly()
  {
    // Dumping it, so he is not in the market to buy any: a clear-out that also paid over
    // the odds would let the player sell it straight back.
    DealerBook book = Stocked();

    book.ApplyClearOut(Cocaine, 400, 30);

    Assert.AreEqual(30, book.Amount[Cocaine]);
    Assert.AreEqual(400, book.Price[Cocaine]);
    Assert.AreEqual(0, book.Demand[Cocaine]);
    Assert.AreEqual(DealerOfferKind.ClearOut, book.Offer);
    Assert.AreEqual(Cocaine, book.OfferDrug);
  }

  [TestMethod]
  public void ClearOffer_AfterInDemand_IsAnExactUndo()
  {
    // The claim the old code made in a comment and never checked. An offer skews all three
    // numbers, so ending it has to put all three back.
    DealerBook book = Stocked();

    book.ApplyInDemand(Cocaine, 1500, 30);
    book.ClearOffer();

    Assert.AreEqual(20, book.Amount[Cocaine]);
    Assert.AreEqual(900, book.Price[Cocaine]);
    Assert.AreEqual(10, book.Demand[Cocaine]);
  }

  [TestMethod]
  public void ClearOffer_AfterClearOut_IsAnExactUndo()
  {
    DealerBook book = Stocked();

    book.ApplyClearOut(Cocaine, 400, 30);
    book.ClearOffer();

    Assert.AreEqual(20, book.Amount[Cocaine]);
    Assert.AreEqual(900, book.Price[Cocaine]);
    Assert.AreEqual(10, book.Demand[Cocaine]);
  }

  [TestMethod]
  public void ClearOffer_ForgetsTheOffer()
  {
    DealerBook book = Stocked();

    book.ApplyInDemand(Cocaine, 1500, 30);
    book.ClearOffer();

    Assert.IsFalse(book.HasOffer);
    Assert.IsNull(book.OfferDrug);
    Assert.AreEqual(DealerOfferKind.None, book.Offer);
  }

  [TestMethod]
  public void ClearOffer_WithNoOfferStanding_ChangesNothing()
  {
    // How the distribution can clear every dealer unconditionally before dealing a fresh
    // market, which is what it does on every cycle.
    DealerBook book = Stocked();

    book.ClearOffer();

    Assert.AreEqual(20, book.Amount[Cocaine]);
    Assert.AreEqual(900, book.Price[Cocaine]);
    Assert.AreEqual(10, book.Demand[Cocaine]);
    Assert.IsFalse(book.HasOffer);
  }

  [TestMethod]
  public void ClearOffer_Twice_ChangesNothingTheSecondTime()
  {
    // The second cycle's clear lands on a book already cleared by the first. If the undo
    // were not forgotten with the offer, this would write the pre-offer position back over
    // whatever the new distribution had just dealt.
    DealerBook book = Stocked();

    book.ApplyInDemand(Cocaine, 1500, 30);
    book.ClearOffer();
    book.SetPosition(Cocaine, 5, 1100, 2);
    book.ClearOffer();

    Assert.AreEqual(5, book.Amount[Cocaine]);
    Assert.AreEqual(1100, book.Price[Cocaine]);
    Assert.AreEqual(2, book.Demand[Cocaine]);
  }

  [TestMethod]
  public void ApplyOffer_ThenTrade_ThenClear_UndoesToBeforeTheOffer()
  {
    // An offer is a dealer acting against his own book, and the player trading on it moves
    // the same numbers the undo restores. The undo wins: the position it puts back is the
    // one the cycle dealt, not the one the trade left.
    DealerBook book = Stocked();

    book.ApplyClearOut(Cocaine, 400, 30);
    book.PlayerBought(Cocaine, 30);
    book.ClearOffer();

    Assert.AreEqual(20, book.Amount[Cocaine]);
    Assert.AreEqual(10, book.Demand[Cocaine]);
  }
}
