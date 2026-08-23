using LSDW.Core;
using LSDW.Events;
using LSDW.Services;

namespace LSDW.Tests.Core;

/// <summary>
/// The lifetime money figures, accumulated off the transaction events.
/// </summary>
/// <remarks>
/// The one sibling of <c>PlayerWallet</c> that can be tested: both subscribe to
/// <c>DrugsSoldEvent</c>, but the wallet credits <c>Game.Player.Money</c> and needs the game,
/// while this only adds to two <see langword="int"/>s.
/// </remarks>
[TestClass]
public sealed class PlayerStatsTests
{
  [TestMethod]
  public void Earned_AfterASale_IsTheGrossRevenue()
  {
    // Revenue, not profit: the money box counts what came in, and the margin is what the XP
    // curve reads off the same event.
    EventService events = new();
    PlayerStats stats = new(events);

    events.Publish(new DrugsSoldEvent(Revenue: 1200, Profit: 350));

    Assert.AreEqual(1200, stats.Earned);
    Assert.AreEqual(0, stats.Spent);
  }

  [TestMethod]
  public void Spent_AfterAPurchase_IsTheCost()
  {
    EventService events = new();
    PlayerStats stats = new(events);

    events.Publish(new DrugsBoughtEvent(Cost: 850));

    Assert.AreEqual(850, stats.Spent);
    Assert.AreEqual(0, stats.Earned);
  }

  [TestMethod]
  public void EarnedAndSpent_AcrossManyTrades_AccumulateIndependently()
  {
    EventService events = new();
    PlayerStats stats = new(events);

    events.Publish(new DrugsBoughtEvent(200));
    events.Publish(new DrugsSoldEvent(500, 300));
    events.Publish(new DrugsBoughtEvent(100));
    events.Publish(new DrugsSoldEvent(250, -50));

    Assert.AreEqual(750, stats.Earned);
    Assert.AreEqual(300, stats.Spent);
  }

  [TestMethod]
  public void Earned_AfterALossMakingSale_StillRises()
  {
    // A sale below what the player paid is a negative profit and a positive revenue. Only the
    // XP curve cares about the sign.
    EventService events = new();
    PlayerStats stats = new(events);

    events.Publish(new DrugsSoldEvent(Revenue: 400, Profit: -450));

    Assert.AreEqual(400, stats.Earned);
  }

  [TestMethod]
  public void LoadSuccessful_BeforeALoad_IsFalse()
  {
    // The flag the stats panel and the XP bar stay hidden on, so a player whose save would
    // not read is never shown figures that are not theirs. SaveService sets it.
    PlayerStats stats = new(new EventService());

    Assert.IsFalse(stats.LoadSuccessful);
  }
}
