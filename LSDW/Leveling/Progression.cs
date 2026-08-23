using LSDW.Events;
using LSDW.Services;

namespace LSDW.Leveling;

/// <summary>
/// XP and levels. Event-driven: it subscribes to <see cref="DrugsSoldEvent"/>, turns
/// each profitable sale into XP, and publishes a <see cref="LeveledUpEvent"/> per level
/// crossed. The rewards those levels grant live in <see cref="RewardSystem"/>, not here.
/// </summary>
internal sealed class Progression
{
  public const int MaxLevel = 100;

  private const int BaseGoal = 1000;

  /// <summary>
  /// The bus level-ups are published on.
  /// </summary>
  private readonly EventService _events;

  public int Level { get; private set; } = 1;

  /// <summary>
  /// Lifetime XP. Persisted by <see cref="SaveService"/>; the level follows from it.
  /// </summary>
  public int XP { get; set; }

  /// <summary>
  /// The XP cost of leaving <paramref name="level"/> — the HUD bar's denominator.
  /// </summary>
  /// <remarks>
  /// Static, unlike the rest of the type: it is the curve, not a player's place on it.
  /// </remarks>
  public static int GoalForLevel(int level) => BaseGoal * level * level;

  /// <summary>
  /// How far into the current level the player is, for the HUD bar.
  /// </summary>
  public int XpIntoCurrentLevel => (int)(XP - CumulativeGoal(Level));

  /// <summary>
  /// Subscribes to sales. Constructed once at startup, before any sale can fire.
  /// </summary>
  /// <param name="events">The event bus to subscribe on and publish to.</param>
  public Progression(EventService events)
  {
    _events = events;
    _events.Subscribe<DrugsSoldEvent>(OnDrugsSold);
  }

  /// <summary>
  /// Rebuilds <see cref="Level"/> from the loaded <see cref="XP"/> and replays the
  /// level-ups so <see cref="RewardSystem"/> re-applies bag size and unlocks.
  /// Notifications stay suppressed by the persisted <see cref="PlayerStash.TipsShown"/>.
  /// </summary>
  public void ApplyLoadedState()
  {
    Level = 1;
    RaiseLevels(replayed: true);
  }

  private void OnDrugsSold(DrugsSoldEvent sale)
  {
    // At the level cap there is nothing left to earn XP toward, and XP is an int:
    // accruing forever would eventually overflow and derive a level below the cap.
    if (Level >= MaxLevel)
    {
      return;
    }

    if (sale.Profit <= 0)
    {
      return;
    }

    int gain = (int)(sale.Profit * SettingsService.Economy.XpMultiplier * Perks.XpMultiplierBonus(Level));
    if (gain <= 0)
    {
      return;
    }

    XP += gain;
    RaiseLevels();
  }

  private void RaiseLevels(bool replayed = false)
  {
    while (Level < MaxLevel && XP >= CumulativeGoal(Level + 1))
    {
      Level++;
      _events.Publish(new LeveledUpEvent(Level, replayed));
    }
  }

  /// <summary>
  /// The XP required to be at <paramref name="level"/>: the sum of every lower level's
  /// goal, <c>1000 * Σ k²</c> for <c>k = 1..level-1</c>, in closed form. <see langword="long"/>
  /// so the multiplication does not overflow before the divide.
  /// </summary>
  private static long CumulativeGoal(int level)
    => (long)BaseGoal * (level - 1) * level * (2L * level - 1) / 6;
}
