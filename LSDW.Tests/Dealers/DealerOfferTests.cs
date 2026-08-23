using LSDW.Dealers;

namespace LSDW.Tests.Dealers;

/// <summary>
/// The magnitudes behind a dealer's special offer.
/// </summary>
/// <remarks>
/// Rolls come off the shared <c>RandomHelper.Random</c> in <c>DealerMarket</c>, so these
/// take the roll as an argument and the whole range can be swept exhaustively — 100 values
/// is small enough to check every one rather than sample.
/// </remarks>
[TestClass]
public sealed class DealerOfferTests
{
  private const int MarketValue = 1000;

  /// <summary>
  /// A stand-in for the average share a drug reaches the streets at, which is what an offer's
  /// quantity is a multiple of.
  /// </summary>
  private const int Baseline = 10;

  [TestMethod]
  [DataRow(0, 400, DisplayName = "Lowest roll is 40% of market")]
  [DataRow(99, 600, DisplayName = "Highest roll is 60% of market")]
  public void ClearOutPrice_AtTheEnds_HitsTheDocumentedBounds(int roll, int expected)
  {
    Assert.AreEqual(expected, DealerOffer.ClearOutPrice(MarketValue, roll));
  }

  [TestMethod]
  [DataRow(0, 1500, DisplayName = "Lowest roll is 150% of market")]
  [DataRow(99, 2000, DisplayName = "Highest roll is 200% of market")]
  public void InDemandPrice_AtTheEnds_HitsTheDocumentedBounds(int roll, int expected)
  {
    Assert.AreEqual(expected, DealerOffer.InDemandPrice(MarketValue, roll));
  }

  [TestMethod]
  [DataRow(0, 40, DisplayName = "Lowest roll is 4x the baseline")]
  [DataRow(99, 80, DisplayName = "Highest roll is 8x the baseline")]
  public void OfferQuantity_AtTheEnds_HitsTheDocumentedBounds(int roll, int expected)
  {
    Assert.AreEqual(expected, DealerOffer.OfferQuantity(roll, Baseline));
  }

  [TestMethod]
  public void OfferQuantity_ScalesWithTheBaseline()
  {
    // The baseline is the average share, which grows with the player's level — so an offer
    // has to grow with it or it stops being worth crossing the map for.
    Assert.AreEqual(4, DealerOffer.OfferQuantity(0, 1));
    Assert.AreEqual(400, DealerOffer.OfferQuantity(0, 100));
    Assert.AreEqual(8, DealerOffer.OfferQuantity(DealerOffer.MaxRoll - 1, 1));
    Assert.AreEqual(800, DealerOffer.OfferQuantity(DealerOffer.MaxRoll - 1, 100));
  }

  [TestMethod]
  public void ClearOutPrice_AcrossEveryRoll_AlwaysBeatsTheMarket()
  {
    // The point of a clear-out is that it is visibly cheap. If a rounding change ever let
    // one land at or above market, the offer would advertise itself as a bad deal.
    for (int roll = 0; roll < DealerOffer.MaxRoll; roll++)
    {
      Assert.IsLessThan(MarketValue, DealerOffer.ClearOutPrice(MarketValue, roll),
        $"roll {roll} priced at or above market");
    }
  }

  [TestMethod]
  public void InDemandPrice_AcrossEveryRoll_AlwaysPaysOverTheMarket()
  {
    for (int roll = 0; roll < DealerOffer.MaxRoll; roll++)
    {
      Assert.IsGreaterThan(MarketValue, DealerOffer.InDemandPrice(MarketValue, roll),
        $"roll {roll} paid at or below market");
    }
  }

  [TestMethod]
  public void OfferQuantity_AcrossEveryRoll_StaysWellAboveAnOrdinaryShare()
  {
    // Documented rationale: the quantity alone should identify an offer. An ordinary dealer
    // holds the baseline on average, so four times it is comfortably outside what the
    // distribution hands out — and it stays that way at every level, which a flat count
    // would not.
    for (int roll = 0; roll < DealerOffer.MaxRoll; roll++)
    {
      int quantity = DealerOffer.OfferQuantity(roll, Baseline);

      Assert.IsGreaterThanOrEqualTo(4 * Baseline, quantity, $"roll {roll} moved too little");
      Assert.IsLessThanOrEqualTo(8 * Baseline, quantity, $"roll {roll} moved too much");
    }
  }

  [TestMethod]
  public void OfferQuantity_AcrossEveryRoll_RisesWithTheRoll()
  {
    for (int roll = 1; roll < DealerOffer.MaxRoll; roll++)
    {
      Assert.IsGreaterThanOrEqualTo(
        DealerOffer.OfferQuantity(roll - 1, Baseline),
        DealerOffer.OfferQuantity(roll, Baseline));
    }
  }

  [TestMethod]
  public void ClearOutPrice_AcrossEveryRoll_RisesWithTheRoll()
  {
    // Monotonic, so the roll reads as "how good the deal is" in one direction.
    for (int roll = 1; roll < DealerOffer.MaxRoll; roll++)
    {
      Assert.IsGreaterThanOrEqualTo(
        DealerOffer.ClearOutPrice(MarketValue, roll - 1),
        DealerOffer.ClearOutPrice(MarketValue, roll));
    }
  }

  [TestMethod]
  public void Prices_ScaleWithMarketValue()
  {
    Assert.AreEqual(0, DealerOffer.ClearOutPrice(0, 50));
    Assert.AreEqual(0, DealerOffer.InDemandPrice(0, 50));
    Assert.AreEqual(4, DealerOffer.ClearOutPrice(10, 0));
    Assert.AreEqual(15, DealerOffer.InDemandPrice(10, 0));
  }
}
