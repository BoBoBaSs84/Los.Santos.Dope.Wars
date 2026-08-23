using LSDW.Dealers;

namespace LSDW.Tests.Dealers;

/// <summary>
/// How a dealer's blip should look, given everything currently true about him.
/// </summary>
/// <remarks>
/// Total over three booleans, so all eight combinations are swept rather than sampled —
/// including the ones no caller can currently produce, which is what "total" is worth. The
/// blip itself is a GTA type and the application of this state is not covered; the decision
/// is all that lives here.
/// </remarks>
[TestClass]
public sealed class DealerBlipStateTests
{
  [TestMethod]
  [DataRow(false, false, false, DealerBlipState.HiddenAlpha, DealerBlipState.RestingScale, DisplayName = "Undiscovered")]
  [DataRow(false, false, true, DealerBlipState.HiddenAlpha, DealerBlipState.OfferScale, DisplayName = "Undiscovered, with an offer")]
  [DataRow(false, true, false, DealerBlipState.HiddenAlpha, DealerBlipState.RestingScale, DisplayName = "Undiscovered and dead")]
  [DataRow(false, true, true, DealerBlipState.HiddenAlpha, DealerBlipState.OfferScale, DisplayName = "Undiscovered and dead, with an offer")]
  [DataRow(true, false, false, DealerBlipState.VisibleAlpha, DealerBlipState.RestingScale, DisplayName = "Trading normally")]
  [DataRow(true, false, true, DealerBlipState.VisibleAlpha, DealerBlipState.OfferScale, DisplayName = "Trading, with an offer")]
  [DataRow(true, true, false, DealerBlipState.HiddenAlpha, DealerBlipState.RestingScale, DisplayName = "Discovered but on cooldown")]
  [DataRow(true, true, true, DealerBlipState.HiddenAlpha, DealerBlipState.OfferScale, DisplayName = "On cooldown, with an offer")]
  public void For_AcrossEveryCombination_MatchesTheDocumentedState(
    bool discovered, bool onCooldown, bool hasOffer, int expectedAlpha, float expectedScale)
  {
    DealerBlipState state = DealerBlipState.For(discovered, onCooldown, hasOffer);

    Assert.AreEqual(expectedAlpha, state.Alpha);
    Assert.AreEqual(expectedScale, state.Scale);
  }

  [TestMethod]
  public void For_TreatsAlphaAndScaleAsSeparateSignals()
  {
    // The scale follows the offer alone and the alpha follows visibility alone: a dead dealer
    // holding an offer keeps the larger blip at zero opacity rather than losing the offer
    // scale, which is what keeps the two decisions from having to be made together.
    DealerBlipState hiddenWithOffer = DealerBlipState.For(discovered: false, onCooldown: true, hasOffer: true);

    Assert.AreEqual(DealerBlipState.HiddenAlpha, hiddenWithOffer.Alpha);
    Assert.AreEqual(DealerBlipState.OfferScale, hiddenWithOffer.Scale);
  }

  [TestMethod]
  public void For_WithTheSameInputs_IsEquatable()
  {
    // A record struct, and the dealer code compares one against the last to decide whether a
    // blip refresh has anything to do.
    Assert.AreEqual(
      DealerBlipState.For(discovered: true, onCooldown: false, hasOffer: false),
      DealerBlipState.For(discovered: true, onCooldown: false, hasOffer: false));

    Assert.AreNotEqual(
      DealerBlipState.For(discovered: true, onCooldown: false, hasOffer: false),
      DealerBlipState.For(discovered: true, onCooldown: false, hasOffer: true));
  }
}
