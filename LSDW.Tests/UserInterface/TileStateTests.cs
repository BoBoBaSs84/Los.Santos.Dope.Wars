using LSDW.UserInterface;

namespace LSDW.Tests.UserInterface;

/// <summary>
/// A drug tile's two decisions: whether the player can act on it, and whether the price on it
/// is a good one. Between them they drive everything about how the tile looks.
/// </summary>
/// <remarks>
/// The availability rule is where <i>an empty dealer only buys and a flooded one only sells</i>
/// becomes visible — the market model produces those two states, and this is what the player
/// sees of them. Neither was covered before: the rule lived on <c>DrugMenuItem</c>, which holds
/// four <c>GTA.UI</c> elements and cannot be constructed headlessly.
/// </remarks>
[TestClass]
public sealed class TileStateTests
{
  [TestMethod]
  public void NewTile_IsNotTradeable()
  {
    // Tiles are built empty and filled by DealerTrading.Open before the menu is drawn.
    Assert.IsFalse(new TileState().Tradeable);
  }

  [TestMethod]
  public void NewTile_HasNoSellLimit()
  {
    // The buy side never sets Demand, so the default has to be a value that cannot constrain
    // it. Were it 0, every buy tile would read as unavailable.
    Assert.AreEqual(TileState.NoLimit, new TileState().Demand);
  }

  [TestMethod]
  public void BuyTile_WithStock_IsTradeable()
  {
    TileState tile = new() { SellingDrugs = false, Amount = 5 };

    Assert.IsTrue(tile.Tradeable);
  }

  [TestMethod]
  public void BuyTile_WithoutStock_IsNotTradeable()
  {
    // The dealer has none of it, so there is nothing to buy — no special case needed.
    TileState tile = new() { SellingDrugs = false, Amount = 0 };

    Assert.IsFalse(tile.Tradeable);
  }

  [TestMethod]
  public void BuyTile_IgnoresDemandEntirely()
  {
    // What limits a purchase is his stock, not his appetite. Even a dealer who wants none of
    // it will still sell what he has, which is why the condition short-circuits on the side.
    TileState tile = new() { SellingDrugs = false, Amount = 5, Demand = 0 };

    Assert.IsTrue(tile.Tradeable);
  }

  [TestMethod]
  public void SellTile_WithStockAndAppetite_IsTradeable()
  {
    TileState tile = new() { SellingDrugs = true, Amount = 5, Demand = 3 };

    Assert.IsTrue(tile.Tradeable);
  }

  [TestMethod]
  public void SellTile_AgainstAFloodedDealer_IsNotTradeable()
  {
    // He is sitting on a glut, so he buys nothing however much the player is carrying. This
    // is the market's "a flooded dealer only sells" arriving on screen.
    TileState tile = new() { SellingDrugs = true, Amount = 500, Demand = 0 };

    Assert.IsFalse(tile.Tradeable);
  }

  [TestMethod]
  public void SellTile_WithNothingCarried_IsNotTradeable()
  {
    TileState tile = new() { SellingDrugs = true, Amount = 0, Demand = 30 };

    Assert.IsFalse(tile.Tradeable);
  }

  [TestMethod]
  public void SellTile_WithAppetiteDrivenNegative_IsNotTradeable()
  {
    // `> 0`, not `!= 0`: the book does not clamp the appetite, so a negative one has to read
    // the same as none. See DealerBook.PlayerSold.
    TileState tile = new() { SellingDrugs = true, Amount = 5, Demand = -2 };

    Assert.IsFalse(tile.Tradeable);
  }

  [TestMethod]
  public void BuyTile_BelowTheMarket_IsAGoodDeal()
  {
    TileState tile = new() { SellingDrugs = false, Price = 700, MarketPrice = 850 };

    Assert.AreEqual(TileDeal.Good, tile.Deal);
  }

  [TestMethod]
  public void BuyTile_AboveTheMarket_IsABadDeal()
  {
    TileState tile = new() { SellingDrugs = false, Price = 1000, MarketPrice = 850 };

    Assert.AreEqual(TileDeal.Bad, tile.Deal);
  }

  [TestMethod]
  public void SellTile_AboveWhatWasPaid_IsAGoodDeal()
  {
    // The reference on the sell side is the player's own cost basis, not the market.
    TileState tile = new() { SellingDrugs = true, Price = 900, MarketPrice = 700 };

    Assert.AreEqual(TileDeal.Good, tile.Deal);
  }

  [TestMethod]
  public void SellTile_BelowWhatWasPaid_IsABadDeal()
  {
    TileState tile = new() { SellingDrugs = true, Price = 600, MarketPrice = 700 };

    Assert.AreEqual(TileDeal.Bad, tile.Deal);
  }

  [TestMethod]
  public void EitherSide_AtTheSamePrice_IsNeutral()
  {
    // No arrow at all rather than a green one: the old code reached this by both its
    // good-deal and bad-deal tests coming back false, which is easy to break by rewriting
    // either comparison as an inclusive one.
    Assert.AreEqual(TileDeal.Neutral, new TileState { SellingDrugs = false, Price = 850, MarketPrice = 850 }.Deal);
    Assert.AreEqual(TileDeal.Neutral, new TileState { SellingDrugs = true, Price = 850, MarketPrice = 850 }.Deal);
  }

  [TestMethod]
  public void TheSamePrices_ReadOppositeWaysOnTheTwoSides()
  {
    // 900 against a reference of 700 is a bad buy and a good sale. The direction flag is what
    // makes the arrow mean anything.
    Assert.AreEqual(TileDeal.Bad, new TileState { SellingDrugs = false, Price = 900, MarketPrice = 700 }.Deal);
    Assert.AreEqual(TileDeal.Good, new TileState { SellingDrugs = true, Price = 900, MarketPrice = 700 }.Deal);
  }

  [TestMethod]
  public void Deal_IsIndependentOfTradeability()
  {
    // A tile can hold a good price and still be unavailable — an empty dealer with a
    // tempting quote. The tile draws no arrow in that case, but because it is not tradeable,
    // not because the price stopped being good.
    TileState tile = new() { SellingDrugs = false, Amount = 0, Price = 700, MarketPrice = 850 };

    Assert.IsFalse(tile.Tradeable);
    Assert.AreEqual(TileDeal.Good, tile.Deal);
  }
}
