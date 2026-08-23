using LSDW.Events;
using LSDW.Leveling;
using LSDW.Services;

namespace LSDW.Tests.Leveling;

/// <summary>
/// The XP curve, and what a sale does to a player's place on it.
/// </summary>
/// <remarks>
/// <c>Level</c>, <c>XP</c> and <c>ApplyLoadedState</c> were static mutable state publishing on a
/// static bus, which is why only the curve was covered for so long. They are an instance taking
/// its bus as an argument now, so each test below builds one, drives it and watches what it
/// publishes without leaking into the next.
/// <para>
/// <b>No test here may call <c>SettingsService.Load()</c>.</b> <c>OnDrugsSold</c> scales its gain
/// by <c>SettingsService.Economy.XpMultiplier</c>, which is the one piece of static state the
/// suite still reads. It is 1.0 because those statics are seeded from the <c>SettingsFile</c>
/// defaults, and loading would both write an INI into the test output directory and reach
/// <c>ScriptHelper</c> on its failure paths. The first test asserts the value rather than
/// assuming it, so a changed default fails as itself instead of as arithmetic.
/// </para>
/// </remarks>
[TestClass]
public sealed class ProgressionTests
{
  /// <summary>
  /// The XP required to sit at <paramref name="level"/> — the same closed form the type derives
  /// the level with, restated because it is <see langword="private"/> there. Copied in
  /// <c>RewardSystemTests</c>, which needs to reach a level for a different reason; change one and
  /// the other has to move with it.
  /// </summary>
  private static int CumulativeGoal(int level)
    => (int)((long)1000 * (level - 1) * level * (2L * level - 1) / 6);

  /// <summary>
  /// Builds a bus, a progression over it, and a list that every level-up it publishes lands in.
  /// The bus is handed back because sales arrive on it — nothing calls into the progression
  /// directly except the save loader.
  /// </summary>
  private static (EventService Events, Progression Progression, List<LeveledUpEvent> LevelUps) Build()
  {
    EventService events = new();
    Progression progression = new(events);
    List<LeveledUpEvent> levelUps = [];

    events.Subscribe<LeveledUpEvent>(levelUps.Add);

    return (events, progression, levelUps);
  }

  [TestMethod]
  public void TheSuiteRunsOnTheDefaultSettings()
  {
    // The precondition every XP figure below rests on. Load() is never called, so these are the
    // SettingsFile defaults mapped once at static initialisation.
    Assert.AreEqual(1.0, SettingsService.Economy.XpMultiplier);
  }

  [TestMethod]
  [DataRow(1, 1_000)]
  [DataRow(2, 4_000)]
  [DataRow(3, 9_000)]
  [DataRow(10, 100_000)]
  [DataRow(100, 10_000_000)]
  public void GoalForLevel_IsAThousandTimesTheSquare(int level, int expected)
  {
    Assert.AreEqual(expected, Progression.GoalForLevel(level));
  }

  [TestMethod]
  public void GoalForLevel_RisesWithEveryLevel()
  {
    for (int level = 2; level <= Progression.MaxLevel; level++)
    {
      Assert.IsGreaterThan(Progression.GoalForLevel(level - 1), Progression.GoalForLevel(level));
    }
  }

  [TestMethod]
  public void NewProgression_StartsAtLevelOneWithNoXp()
  {
    (_, Progression progression, _) = Build();

    Assert.AreEqual(1, progression.Level);
    Assert.AreEqual(0, progression.XP);
    Assert.AreEqual(0, progression.XpIntoCurrentLevel);
  }

  [TestMethod]
  public void DrugsSold_WithAProfit_GrantsThatMuchXp()
  {
    // One-for-one at the default multiplier and below level 40, where the reputation perk starts
    // scaling it.
    (EventService events, Progression progression, _) = Build();

    events.Publish(new DrugsSoldEvent(Revenue: 900, Profit: 350));

    Assert.AreEqual(350, progression.XP);
  }

  [TestMethod]
  [DataRow(0, DisplayName = "Broke even")]
  [DataRow(-500, DisplayName = "Sold at a loss")]
  public void DrugsSold_WithoutAProfit_GrantsNothing(int profit)
  {
    // Revenue is ignored on purpose: a player who dumps a bag below cost has not earned
    // anything to level on.
    (EventService events, Progression progression, List<LeveledUpEvent> levelUps) = Build();

    events.Publish(new DrugsSoldEvent(Revenue: 5_000, Profit: profit));

    Assert.AreEqual(0, progression.XP);
    Assert.IsEmpty(levelUps);
  }

  [TestMethod]
  public void DrugsBought_GrantsNothing()
  {
    // Buys publish their own event, which this type does not subscribe to — spending is not
    // progress.
    (EventService events, Progression progression, _) = Build();

    events.Publish(new DrugsBoughtEvent(Cost: 50_000));

    Assert.AreEqual(0, progression.XP);
  }

  [TestMethod]
  public void DrugsSold_ShortOfTheGoal_LevelsNothingUp()
  {
    (EventService events, Progression progression, List<LeveledUpEvent> levelUps) = Build();

    events.Publish(new DrugsSoldEvent(999, 999));

    Assert.AreEqual(1, progression.Level);
    Assert.IsEmpty(levelUps);
    Assert.AreEqual(999, progression.XpIntoCurrentLevel);
  }

  [TestMethod]
  public void DrugsSold_ReachingTheGoal_PublishesOneLevelUp()
  {
    (EventService events, Progression progression, List<LeveledUpEvent> levelUps) = Build();

    events.Publish(new DrugsSoldEvent(1_000, 1_000));

    Assert.AreEqual(2, progression.Level);
    LeveledUpEvent levelUp = Assert.ContainsSingle(levelUps);
    Assert.AreEqual(2, levelUp.Level);
    Assert.IsFalse(levelUp.Replayed, "a live level-up is not a replay");
  }

  [TestMethod]
  public void DrugsSold_ClearingSeveralGoalsAtOnce_PublishesOnePerLevel()
  {
    // A single big sale can cross several levels, and RewardSystem has to see each of them or a
    // milestone in the middle is skipped.
    (EventService events, Progression progression, List<LeveledUpEvent> levelUps) = Build();

    events.Publish(new DrugsSoldEvent(30_000, 30_000));

    Assert.AreEqual(5, progression.Level);
    Assert.AreSequenceEqual([2, 3, 4, 5], levelUps.Select(levelUp => levelUp.Level));
  }

  [TestMethod]
  public void DrugsSold_AcrossManySales_LevelsOnTheRunningTotal()
  {
    // The XP is cumulative, so ten sales worth 100 each level the player exactly as one worth
    // 1000 does.
    (EventService events, Progression progression, List<LeveledUpEvent> levelUps) = Build();

    for (int sale = 0; sale < 10; sale++)
    {
      events.Publish(new DrugsSoldEvent(100, 100));
    }

    Assert.AreEqual(1_000, progression.XP);
    Assert.AreEqual(2, progression.Level);
    _ = Assert.ContainsSingle(levelUps);
  }

  [TestMethod]
  public void XpIntoCurrentLevel_OnALevelBoundary_IsZero()
  {
    (EventService events, Progression progression, _) = Build();

    events.Publish(new DrugsSoldEvent(5_000, 5_000));

    Assert.AreEqual(3, progression.Level);
    Assert.AreEqual(0, progression.XpIntoCurrentLevel);
  }

  [TestMethod]
  public void XpIntoCurrentLevel_PartWayThrough_IsTheRemainder()
  {
    // The HUD bar's numerator; GoalForLevel is its denominator.
    (EventService events, Progression progression, _) = Build();

    events.Publish(new DrugsSoldEvent(1_500, 1_500));

    Assert.AreEqual(2, progression.Level);
    Assert.AreEqual(500, progression.XpIntoCurrentLevel);
  }

  [TestMethod]
  public void DrugsSold_AtTheLevelCap_AccruesNothing()
  {
    // Not merely "does not level": the XP itself stops. It is an int, and accruing forever would
    // overflow and derive a level below the cap.
    (EventService events, Progression progression, List<LeveledUpEvent> levelUps) = Build();
    progression.XP = CumulativeGoal(Progression.MaxLevel);
    progression.ApplyLoadedState();
    levelUps.Clear();

    events.Publish(new DrugsSoldEvent(int.MaxValue / 2, int.MaxValue / 2));

    Assert.AreEqual(Progression.MaxLevel, progression.Level);
    Assert.AreEqual(CumulativeGoal(Progression.MaxLevel), progression.XP);
    Assert.IsEmpty(levelUps);
  }

  [TestMethod]
  public void ApplyLoadedState_DerivesTheLevelFromTheLoadedXp()
  {
    (EventService events, Progression progression, _) = Build();
    progression.XP = 30_000;

    progression.ApplyLoadedState();

    Assert.AreEqual(5, progression.Level);
  }

  [TestMethod]
  public void ApplyLoadedState_ReplaysEveryLevelAsAReplay()
  {
    // This is what re-applies bag size and the two stateful unlocks for a returning player, and
    // the flag is what keeps them from getting one ticker per level they ever reached.
    (EventService events, Progression progression, List<LeveledUpEvent> levelUps) = Build();
    progression.XP = 30_000;

    progression.ApplyLoadedState();

    Assert.AreSequenceEqual([2, 3, 4, 5], levelUps.Select(levelUp => levelUp.Level));
    Assert.IsTrue(levelUps.TrueForAll(levelUp => levelUp.Replayed));
  }

  [TestMethod]
  public void ApplyLoadedState_ForANewPlayer_PublishesNothing()
  {
    (EventService events, Progression progression, List<LeveledUpEvent> levelUps) = Build();

    progression.ApplyLoadedState();

    Assert.AreEqual(1, progression.Level);
    Assert.IsEmpty(levelUps);
  }

  [TestMethod]
  public void ApplyLoadedState_AtTheCap_ReplaysEveryLevelExactlyOnce()
  {
    // The heaviest replay there is: ninety-nine level-ups for a capped player, in order and with
    // no gaps, because RewardSystem walks them to rebuild the bag.
    (EventService events, Progression progression, List<LeveledUpEvent> levelUps) = Build();
    progression.XP = CumulativeGoal(Progression.MaxLevel);

    progression.ApplyLoadedState();

    Assert.AreEqual(Progression.MaxLevel, progression.Level);
    Assert.AreSequenceEqual(
      [.. Enumerable.Range(2, Progression.MaxLevel - 1)],
      levelUps.Select(levelUp => levelUp.Level));
  }
}
