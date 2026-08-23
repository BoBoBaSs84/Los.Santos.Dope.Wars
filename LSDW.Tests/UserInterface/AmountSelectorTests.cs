using LSDW.UserInterface;

namespace LSDW.Tests.UserInterface;

/// <summary>
/// Choosing how much to trade: the quantity against its ceiling, the accept/cancel choice, and
/// which of the two a key press moved.
/// </summary>
/// <remarks>
/// The return value is what decides whether the window clicks. Its asymmetry is the thing worth
/// pinning: the quantity keys stay silent against a limit, the toggle keys never do.
/// </remarks>
[TestClass]
public sealed class AmountSelectorTests
{
  private const Keys Up = Keys.F13;
  private const Keys Down = Keys.F14;
  private const Keys Left = Keys.F15;
  private const Keys Right = Keys.F16;

  private static AmountSelector NewSelector(int maxAmount = 5)
    => new(Up, Down, Left, Right) { MaxAmount = maxAmount };

  [TestMethod]
  public void NewSelector_OffersNothingAndIsNotOnAccept()
  {
    AmountSelector selector = new(Up, Down, Left, Right);

    Assert.AreEqual(0, selector.Amount);
    Assert.AreEqual(0, selector.MaxAmount);
    Assert.IsFalse(selector.Ok);
  }

  [TestMethod]
  public void Up_BelowTheCeiling_RaisesTheAmount()
  {
    AmountSelector selector = NewSelector();

    Assert.AreEqual(AmountChange.Amount, selector.TryProcessKey(Up));
    Assert.AreEqual(1, selector.Amount);
  }

  [TestMethod]
  public void Up_AtTheCeiling_ChangesNothing()
  {
    AmountSelector selector = NewSelector(maxAmount: 2);

    _ = selector.TryProcessKey(Up);
    _ = selector.TryProcessKey(Up);

    Assert.AreEqual(AmountChange.None, selector.TryProcessKey(Up));
    Assert.AreEqual(2, selector.Amount);
  }

  [TestMethod]
  public void Up_WithNothingAvailable_ChangesNothing()
  {
    // A ceiling of zero is a real state and reaches the player in three ways: a full bag, a
    // dealer with no stock, and a dealer who wants none of what they are carrying. The window
    // opens, and holding the key does nothing and says nothing.
    AmountSelector selector = NewSelector(maxAmount: 0);

    Assert.AreEqual(AmountChange.None, selector.TryProcessKey(Up));
    Assert.AreEqual(0, selector.Amount);
  }

  [TestMethod]
  public void Down_AboveZero_LowersTheAmount()
  {
    AmountSelector selector = NewSelector();
    _ = selector.TryProcessKey(Up);
    _ = selector.TryProcessKey(Up);

    Assert.AreEqual(AmountChange.Amount, selector.TryProcessKey(Down));
    Assert.AreEqual(1, selector.Amount);
  }

  [TestMethod]
  public void Down_AtZero_ChangesNothing()
  {
    AmountSelector selector = NewSelector();

    Assert.AreEqual(AmountChange.None, selector.TryProcessKey(Down));
    Assert.AreEqual(0, selector.Amount);
  }

  [TestMethod]
  public void Up_ToTheCeilingAndBackDown_WalksOneAtATime()
  {
    AmountSelector selector = NewSelector(maxAmount: 3);

    for (int expected = 1; expected <= 3; expected++)
    {
      Assert.AreEqual(AmountChange.Amount, selector.TryProcessKey(Up));
      Assert.AreEqual(expected, selector.Amount);
    }

    for (int expected = 2; expected >= 0; expected--)
    {
      Assert.AreEqual(AmountChange.Amount, selector.TryProcessKey(Down));
      Assert.AreEqual(expected, selector.Amount);
    }
  }

  [TestMethod]
  public void Left_FlipsTheChoice()
  {
    AmountSelector selector = NewSelector();
    selector.Ok = true;

    Assert.AreEqual(AmountChange.Ok, selector.TryProcessKey(Left));
    Assert.IsFalse(selector.Ok);
  }

  [TestMethod]
  public void Right_FlipsTheChoiceToo()
  {
    // Both keys do the same thing: the choice is a pair, so either direction crosses it.
    AmountSelector selector = NewSelector();
    selector.Ok = true;

    Assert.AreEqual(AmountChange.Ok, selector.TryProcessKey(Right));
    Assert.IsFalse(selector.Ok);
  }

  [TestMethod]
  public void Toggling_AlwaysReportsAChange()
  {
    // The asymmetry against the quantity keys: there is always something to flip, so the
    // window clicks on every press even though two presses land back where they started.
    AmountSelector selector = NewSelector();

    Assert.AreEqual(AmountChange.Ok, selector.TryProcessKey(Left));
    Assert.AreEqual(AmountChange.Ok, selector.TryProcessKey(Left));
    Assert.IsFalse(selector.Ok);
  }

  [TestMethod]
  public void Toggling_LeavesTheAmountAlone()
  {
    AmountSelector selector = NewSelector();
    _ = selector.TryProcessKey(Up);

    _ = selector.TryProcessKey(Left);

    Assert.AreEqual(1, selector.Amount);
  }

  [TestMethod]
  public void AnUnboundKey_ChangesNothing()
  {
    AmountSelector selector = NewSelector();
    selector.Ok = true;

    Assert.AreEqual(AmountChange.None, selector.TryProcessKey(Keys.Space));
    Assert.AreEqual(0, selector.Amount);
    Assert.IsTrue(selector.Ok);
  }

  [TestMethod]
  public void TheBinds_AreCapturedPerSelector()
  {
    AmountSelector selector = new(Keys.W, Keys.S, Keys.A, Keys.D) { MaxAmount = 5 };

    Assert.AreEqual(AmountChange.None, selector.TryProcessKey(Up));
    Assert.AreEqual(AmountChange.Amount, selector.TryProcessKey(Keys.W));
    Assert.AreEqual(1, selector.Amount);
  }

  [TestMethod]
  public void Up_FromAboveTheCeiling_KeepsGoing()
  {
    // Documenting the comparison, not endorsing it: the check is `!=`, so an amount already
    // past the ceiling is not pulled back. Nothing reaches this state, because every window
    // sets the ceiling and then zeroes the amount — which is the reason it has never
    // mattered, and the reason that order should stay.
    AmountSelector selector = NewSelector(maxAmount: 2);
    selector.Amount = 5;

    Assert.AreEqual(AmountChange.Amount, selector.TryProcessKey(Up));
    Assert.AreEqual(6, selector.Amount);
  }
}
