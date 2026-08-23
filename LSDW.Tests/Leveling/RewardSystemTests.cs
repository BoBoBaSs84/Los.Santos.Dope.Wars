using LSDW.Drugs;
using LSDW.Events;
using LSDW.Leveling;
using LSDW.Services;
using LSDW.Tests.Fakes;

namespace LSDW.Tests.Leveling;

/// <summary>
/// The bag curve and the milestone table: which level grants what, and which of those grants
/// reach another subsystem as an event.
/// </summary>
/// <remarks>
/// The notifying paths used to end in a GTA native and were untestable; they now go through
/// <c>INotificationService</c>, and <see cref="FakeNotificationService"/> records what the player
/// would have been shown.
/// <para>
/// The bag and unlock tests below still seed <c>TipsShown</c> with every milestone and replay
/// their level-ups. That is not a workaround any more but the returning player's own state, and
/// it keeps those tests about the table rather than about the messages — which the announcement
/// tests at the end assert on directly.
/// </para>
/// </remarks>
[TestClass]
public sealed class RewardSystemTests
{
  /// <summary>The levels that carry a special reward — every tenth.</summary>
  private static readonly int[] Milestones = [10, 20, 30, 40, 50, 60, 70, 80, 90, 100];

  /// <summary>
  /// The curve <c>RewardSystem.BagForLevel</c> applies, restated rather than referenced: it is
  /// <see langword="private"/> there, and an independent copy is what makes a change to it fail
  /// here instead of agreeing with itself.
  /// </summary>
  private static int ExpectedBag(int level) => 100 + (level - 1) * 10 + Perks.BagBonus(level);

  /// <summary>
  /// Builds a bus, a progression sitting at <paramref name="level"/> and a stash, then the
  /// reward system over them. The level is reached by handing the progression the XP for it and
  /// deriving it — done <i>before</i> the reward system exists, so the replay that derives it
  /// publishes to nobody and construction is what sizes the bag.
  /// </summary>
  private static (EventService Events, FakeNotificationService Notifications, PlayerStash Stash, RewardSystem Rewards) At(int level)
  {
    EventService events = new();
    FakeNotificationService notifications = new();
    Progression progression = new(events);

    if (level > 1)
    {
      progression.XP = CumulativeGoal(level);
      progression.ApplyLoadedState();

      Assert.AreEqual(level, progression.Level, "the test could not reach the level it wanted");
    }

    PlayerStash stash = new();
    RewardSystem rewards = new(events, notifications, progression, stash);

    return (events, notifications, stash, rewards);
  }

  /// <summary>
  /// The XP needed to sit at <paramref name="level"/>: <c>1000 * Σ k²</c> for
  /// <c>k = 1..level-1</c>, the same closed form <c>Progression</c> derives the level with. Copied
  /// in <c>ProgressionTests</c>; change one and the other has to move with it.
  /// </summary>
  private static int CumulativeGoal(int level)
    => (int)((long)1000 * (level - 1) * level * (2L * level - 1) / 6);

  [TestMethod]
  public void Construction_AtLevelOne_SizesTheStartingBag()
  {
    (_, _, PlayerStash stash, _) = At(1);

    Assert.AreEqual(100, stash.BagSize);
  }

  [TestMethod]
  [DataRow(10, 190, DisplayName = "Level 10, curve only")]
  [DataRow(49, 580, DisplayName = "Level 49, just under the first bonus")]
  [DataRow(50, 790, DisplayName = "Level 50, +200")]
  [DataRow(99, 1280, DisplayName = "Level 99, still +200")]
  [DataRow(100, 1590, DisplayName = "The cap, +200 and +300 stacked")]
  public void Construction_AtALoadedLevel_SizesTheBagForIt(int level, int expected)
  {
    // The returning player's case: the bag has to be the size their level says before they
    // touch a dealer, which is why the reward system reads the progression once at startup.
    (_, _, PlayerStash stash, _) = At(level);

    Assert.AreEqual(expected, stash.BagSize);
  }

  [TestMethod]
  public void LeveledUp_ResizesTheBag()
  {
    (EventService events, _, PlayerStash stash, _) = At(1);

    events.Publish(new LeveledUpEvent(2, Replayed: true));

    Assert.AreEqual(110, stash.BagSize);
  }

  [TestMethod]
  public void LeveledUp_AcrossEveryLevel_TracksTheCurve()
  {
    (EventService events, _, PlayerStash stash, _) = At(1);
    stash.TipsShown.AddRange(Milestones);

    for (int level = 2; level <= Progression.MaxLevel; level++)
    {
      events.Publish(new LeveledUpEvent(level, Replayed: true));

      Assert.AreEqual(ExpectedBag(level), stash.BagSize, "level " + level);
    }
  }

  [TestMethod]
  public void LeveledUp_AcrossEveryLevel_NeverShrinksTheBag()
  {
    // The one thing a player would notice going wrong: a level that costs them slots.
    (EventService events, _, PlayerStash stash, _) = At(1);
    stash.TipsShown.AddRange(Milestones);

    int previous = stash.BagSize;

    for (int level = 2; level <= Progression.MaxLevel; level++)
    {
      events.Publish(new LeveledUpEvent(level, Replayed: true));

      Assert.IsGreaterThan(previous, stash.BagSize, "level " + level + " shrank the bag");
      previous = stash.BagSize;
    }
  }

  [TestMethod]
  public void LeveledUp_ToTen_UnlocksVanStealing()
  {
    (EventService events, _, PlayerStash stash, _) = At(1);
    stash.TipsShown.AddRange(Milestones);
    bool unlocked = false;
    events.Subscribe<VanStealingUnlockedEvent>(_ => unlocked = true);

    events.Publish(new LeveledUpEvent(10, Replayed: true));

    Assert.IsTrue(unlocked);
  }

  [TestMethod]
  public void LeveledUp_ToTwenty_UnlocksTheTacoFront()
  {
    (EventService events, _, PlayerStash stash, _) = At(1);
    stash.TipsShown.AddRange(Milestones);
    bool unlocked = false;
    events.Subscribe<TacoFrontUnlockedEvent>(_ => unlocked = true);

    events.Publish(new LeveledUpEvent(20, Replayed: true));

    Assert.IsTrue(unlocked);
  }

  [TestMethod]
  public void LeveledUp_AcrossEveryLevel_UnlocksEachFeatureExactlyOnce()
  {
    // Both unlocks re-apply on a replay by design — the features are stateful and a returning
    // player has to get them back — so what is checked is that they fire at their own level and
    // no other. The remaining eight milestones are passive perks and publish nothing.
    (EventService events, _, PlayerStash stash, _) = At(1);
    stash.TipsShown.AddRange(Milestones);
    List<int> vanLevels = [];
    List<int> tacoLevels = [];
    int current = 0;

    events.Subscribe<VanStealingUnlockedEvent>(_ => vanLevels.Add(current));
    events.Subscribe<TacoFrontUnlockedEvent>(_ => tacoLevels.Add(current));

    for (current = 1; current <= Progression.MaxLevel; current++)
    {
      events.Publish(new LeveledUpEvent(current, Replayed: true));
    }

    Assert.AreSequenceEqual([10], vanLevels);
    Assert.AreSequenceEqual([20], tacoLevels);
  }

  [TestMethod]
  public void LeveledUp_OnAnOrdinaryLevel_PublishesNothing()
  {
    // Nine levels in ten grant the bag and nothing else. Anything published here would reach
    // the van, the taco front or the save file for a level that unlocked none of them.
    (EventService events, _, PlayerStash stash, _) = At(1);
    stash.TipsShown.AddRange(Milestones);
    bool anything = false;

    events.Subscribe<VanStealingUnlockedEvent>(_ => anything = true);
    events.Subscribe<TacoFrontUnlockedEvent>(_ => anything = true);
    events.Subscribe<SaveRequestedEvent>(_ => anything = true);

    events.Publish(new LeveledUpEvent(5, Replayed: true));

    Assert.IsFalse(anything);
    Assert.AreEqual(ExpectedBag(5), stash.BagSize);
  }

  [TestMethod]
  public void LeveledUp_OnAMilestoneAlreadyShown_AsksForNoSave()
  {
    // What stops a returning player's replay rewriting the save once per milestone they ever
    // passed: the save request sits after the tip, on the far side of the TipsShown guard.
    (EventService events, _, PlayerStash stash, _) = At(1);
    stash.TipsShown.AddRange(Milestones);
    bool saveRequested = false;
    events.Subscribe<SaveRequestedEvent>(_ => saveRequested = true);

    foreach (int milestone in Milestones)
    {
      events.Publish(new LeveledUpEvent(milestone, Replayed: true));
    }

    Assert.IsFalse(saveRequested);
    Assert.AreSequenceEqual(Milestones, stash.TipsShown);
  }

  /// <summary>The prefix every plain level-up ticker carries.</summary>
  private const string LevelUpPrefix = "~b~Level Up!~w~";

  /// <summary>The prefix every milestone announcement carries.</summary>
  private const string UnlockPrefix = "~b~Level Unlocked!~w~\n";

  [TestMethod]
  public void LeveledUp_OnALiveOrdinaryLevel_ShowsTheLevelTicker()
  {
    (EventService events, FakeNotificationService notifications, _, _) = At(1);

    events.Publish(new LeveledUpEvent(5, Replayed: false));

    Assert.HasCount(1, notifications.Tickers);
    Assert.AreEqual(LevelUpPrefix + " You are now level 5.", notifications.Tickers[0]);
  }

  [TestMethod]
  public void LeveledUp_OnAReplayedOrdinaryLevel_ShowsNothing()
  {
    // The returning player's case, and the reason LeveledUpEvent carries Replayed at all:
    // ApplyLoadedState walks every level they ever reached, and one ticker each would bury
    // them at startup.
    (EventService events, FakeNotificationService notifications, _, _) = At(1);

    for (int level = 2; level < 10; level++)
    {
      events.Publish(new LeveledUpEvent(level, Replayed: true));
    }

    Assert.IsEmpty(notifications.Tickers);
  }

  [TestMethod]
  public void LeveledUp_OnAFirstMilestone_AnnouncesItAndItsFollowUp()
  {
    // Level 10 is one of the two milestones with a second line. Both go out, in order, and
    // the announcement replaces the plain ticker rather than joining it.
    (EventService events, FakeNotificationService notifications, PlayerStash stash, _) = At(1);
    bool saveRequested = false;
    events.Subscribe<SaveRequestedEvent>(_ => saveRequested = true);

    events.Publish(new LeveledUpEvent(10, Replayed: false));

    Assert.HasCount(2, notifications.Tickers);
    Assert.IsTrue(notifications.Tickers[0].StartsWith(UnlockPrefix, StringComparison.Ordinal), notifications.Tickers[0]);
    Assert.IsFalse(notifications.Tickers[1].StartsWith(UnlockPrefix, StringComparison.Ordinal), "the follow-up is a plain second line");
    Assert.DoesNotContain(LevelUpPrefix, notifications.Tickers[0]);
    Assert.Contains(10, stash.TipsShown);
    Assert.IsTrue(saveRequested, "showing a tip for the first time has to persist it");
  }

  [TestMethod]
  public void LeveledUp_OnAFirstMilestoneWithoutAFollowUp_AnnouncesOneLine()
  {
    // Level 30 is a passive perk: one line, no second, and nothing published but the save.
    (EventService events, FakeNotificationService notifications, _, _) = At(1);

    events.Publish(new LeveledUpEvent(30, Replayed: false));

    Assert.HasCount(1, notifications.Tickers);
    Assert.IsTrue(notifications.Tickers[0].StartsWith(UnlockPrefix, StringComparison.Ordinal), notifications.Tickers[0]);
  }

  [TestMethod]
  public void LeveledUp_OnAMilestoneAlreadyShown_AnnouncesNothing()
  {
    // The far side of the TipsShown guard: the unlock still applies on a replay, but the
    // player is not told about it a second time.
    (EventService events, FakeNotificationService notifications, PlayerStash stash, _) = At(1);
    stash.TipsShown.AddRange(Milestones);

    foreach (int milestone in Milestones)
    {
      events.Publish(new LeveledUpEvent(milestone, Replayed: false));
    }

    Assert.IsEmpty(notifications.Tickers);
  }

  [TestMethod]
  public void LeveledUp_AcrossEveryMilestone_AnnouncesEachExactlyOnce()
  {
    // Every tenth level has an entry in the table, and each is shown once ever — including
    // across the replay a second session brings, which is what TipsShown is persisted for.
    (EventService events, FakeNotificationService notifications, _, _) = At(1);

    for (int pass = 0; pass < 2; pass++)
    {
      foreach (int milestone in Milestones)
      {
        events.Publish(new LeveledUpEvent(milestone, Replayed: false));
      }
    }

    // Ten milestones, two of which carry a follow-up line.
    Assert.HasCount(12, notifications.Tickers);
    Assert.HasCount(10, notifications.Tickers.FindAll(ticker => ticker.StartsWith(UnlockPrefix, StringComparison.Ordinal)));
  }
}
