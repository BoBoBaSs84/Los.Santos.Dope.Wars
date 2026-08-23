using LSDW.Dealers;

namespace LSDW.Tests.Dealers;

/// <summary>
/// The scarcity curve: what a dealer's share of the supply does to his price and to his
/// appetite.
/// </summary>
/// <remarks>
/// This is the whole economic model, and all of it is a pure function of a share and a mean —
/// the draws live in <c>DealerMarket</c>, which needs the game. So the market's behaviour is
/// checked here rather than only observed in play.
/// </remarks>
[TestClass]
public sealed class MarketPricingTests
{
  private const int MarketValue = 1000;
  private const double MeanShare = 10.0;

  [TestMethod]
  public void ScarcityMultiplier_AtTheAverageShare_IsExactlyMarket()
  {
    // Load-bearing, and load-bearing *exactly*: the average dealer must price at market to the
    // penny, or every price in the game carries a rounding bias. This is why the curve is
    // written as a deviation from PriceMid rather than as floor + span / 2, which lands on
    // 0.9999999999999999.
    Assert.AreEqual(1.0, MarketPricing.ScarcityMultiplier(10, MeanShare));
    Assert.AreEqual(MarketPricing.PriceMid, MarketPricing.ScarcityMultiplier(10, MeanShare));
    Assert.AreEqual(MarketValue, MarketPricing.Price(MarketValue, 10, MeanShare));
  }

  [TestMethod]
  public void ScarcityMultiplier_ForAnEmptyDealer_IsTheCeiling()
  {
    Assert.AreEqual(MarketPricing.PriceCeiling, MarketPricing.ScarcityMultiplier(0, MeanShare));
  }

  [TestMethod]
  public void ScarcityMultiplier_WithNoSupplyAtAll_IsTheCeiling()
  {
    // Nothing to be relatively short of. A drug nobody can source is a dear one.
    Assert.AreEqual(MarketPricing.PriceCeiling, MarketPricing.ScarcityMultiplier(0, 0.0));
  }

  [TestMethod]
  public void ScarcityMultiplier_AcrossEveryShare_NeverRises()
  {
    // The one property the player has to be able to rely on: more stock is never dearer.
    for (int share = 1; share <= 500; share++)
    {
      Assert.IsLessThanOrEqualTo(
        MarketPricing.ScarcityMultiplier(share - 1, MeanShare),
        MarketPricing.ScarcityMultiplier(share, MeanShare),
        $"share {share} priced above share {share - 1}");
    }
  }

  [TestMethod]
  public void ScarcityMultiplier_AcrossEveryShare_StaysInsideTheBand()
  {
    for (int share = 0; share <= 100_000; share += 97)
    {
      double multiplier = MarketPricing.ScarcityMultiplier(share, MeanShare);

      Assert.IsGreaterThan(MarketPricing.PriceFloor, multiplier, $"share {share} undercut the floor");
      Assert.IsLessThanOrEqualTo(MarketPricing.PriceCeiling, multiplier, $"share {share} beat the ceiling");
    }
  }

  [TestMethod]
  public void PriceFloorAndCeiling_SitEitherSideOfMarket()
  {
    Assert.IsLessThan(1.0, MarketPricing.PriceFloor);
    Assert.IsGreaterThan(1.0, MarketPricing.PriceCeiling);
  }

  [TestMethod]
  public void Price_ForTheCheapestDrug_NeverRoundsToFree()
  {
    // Weed is worth 10 and survives the floor on its own, but a cheaper drug added later must
    // not round its way to nothing.
    for (int share = 0; share <= 1000; share++)
    {
      Assert.IsGreaterThanOrEqualTo(1, MarketPricing.Price(1, share, MeanShare), $"share {share} priced at nothing");
    }
  }

  [TestMethod]
  public void Price_ForAWorthlessDrug_StaysWorthless()
  {
    // The guard above must not invent value where the catalog says there is none.
    Assert.AreEqual(0, MarketPricing.Price(0, 0, MeanShare));
    Assert.AreEqual(0, MarketPricing.Price(0, 50, MeanShare));
  }

  [TestMethod]
  public void Demand_ForAnEmptyDealer_IsTheFullTarget()
  {
    // He holds nothing, so he wants the lot: one and a half average shares.
    Assert.AreEqual(15, MarketPricing.Demand(0, MeanShare));
  }

  [TestMethod]
  public void Demand_ForAFloodedDealer_IsNothing()
  {
    // Which is what makes him sell-only. The mirror of an empty dealer being buy-only.
    Assert.AreEqual(0, MarketPricing.Demand(15, MeanShare));
    Assert.AreEqual(0, MarketPricing.Demand(500, MeanShare));
  }

  [TestMethod]
  public void Demand_AcrossEveryShare_KeepsStockPlusAppetiteConstant()
  {
    // The invariant DealerTrading maintains as the player trades: filling a dealer up uses his
    // appetite without moving the price he opened the cycle with.
    int target = MarketPricing.Demand(0, MeanShare);

    for (int share = 0; share <= target; share++)
    {
      Assert.AreEqual(target, share + MarketPricing.Demand(share, MeanShare), $"share {share} broke the balance");
    }
  }

  [TestMethod]
  public void Demand_AcrossEveryShare_NeverRises()
  {
    for (int share = 1; share <= 500; share++)
    {
      Assert.IsLessThanOrEqualTo(
        MarketPricing.Demand(share - 1, MeanShare),
        MarketPricing.Demand(share, MeanShare),
        $"share {share} wanted more than share {share - 1}");
    }
  }

  [TestMethod]
  public void Demand_WithNoSupplyAtAll_IsNothing()
  {
    Assert.AreEqual(0, MarketPricing.Demand(0, 0.0));
  }

  [TestMethod]
  public void AskAndBid_AtTheSameDealer_LeaveNoFreeRoundTrip()
  {
    // The reason the spread is 15% and not a number someone liked: without it the level-100
    // perks alone (buy 0.85, sell 1.15) make buying and selling at one dealer profitable.
    const double buyPerk = 0.85;
    const double sellPerk = 1.15;

    for (int price = 1; price <= 2000; price++)
    {
      Assert.IsGreaterThanOrEqualTo(
        MarketPricing.Bid(price, sellPerk),
        MarketPricing.Ask(price, buyPerk),
        $"price {price} could be round-tripped for a profit");
    }
  }

  [TestMethod]
  public void AskAndBid_WithoutPerks_StraddleTheDealersPrice()
  {
    Assert.AreEqual(1150, MarketPricing.Ask(1000, 1.0));
    Assert.AreEqual(850, MarketPricing.Bid(1000, 1.0));
  }

  [TestMethod]
  public void Ask_IsNeverCheaperThanBid()
  {
    for (int price = 1; price <= 2000; price++)
    {
      Assert.IsGreaterThanOrEqualTo(MarketPricing.Bid(price, 1.0), MarketPricing.Ask(price, 1.0), $"price {price}");
    }
  }

  [TestMethod]
  public void AskAndBid_AtTrivialPrices_CollapseTogetherRatherThanInvert()
  {
    // Below 4 the 15% either way rounds to the same integer, so the spread disappears. That
    // costs nothing: a drug priced at 3 offers no profit to round-trip either. What must never
    // happen is the pair inverting into a free margin, which the assertion above covers.
    for (int price = 1; price <= 3; price++)
    {
      Assert.AreEqual(MarketPricing.Ask(price, 1.0), MarketPricing.Bid(price, 1.0), $"price {price}");
    }

    Assert.IsGreaterThan(MarketPricing.Bid(4, 1.0), MarketPricing.Ask(4, 1.0));
  }
}
