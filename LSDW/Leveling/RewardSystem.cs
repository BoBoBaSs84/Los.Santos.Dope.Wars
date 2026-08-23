using LSDW.Drugs;
using LSDW.Events;
using LSDW.Services;

namespace LSDW.Leveling;

/// <summary>
/// What a level grants. Reacts to <see cref="LeveledUpEvent"/>: bag size on every level,
/// a special reward on every tenth. It decides <i>which</i> level unlocks a feature but
/// never touches the feature itself — the van and taco unlocks are published as events
/// (<see cref="VanStealingUnlockedEvent"/>, <see cref="TacoFrontUnlockedEvent"/>) that the
/// owning subsystems subscribe to.
/// </summary>
internal sealed class RewardSystem
{
  /// <summary>
  /// A milestone reward: its one-time announcement, plus an optional one-shot effect.
  /// Most specials are passive bonuses living in <see cref="Perks"/> as functions of the
  /// level, so they have no <see cref="Apply"/>; only the two stateful unlocks do.
  /// </summary>
  /// <param name="Message">The unlock notification, shown once ever.</param>
  /// <param name="FollowUp">An optional second ticker line.</param>
  /// <param name="Apply">The one-shot effect, for the two stateful unlocks.</param>
  private sealed record Special(string Message, string? FollowUp = null, Action? Apply = null);

  /// <summary>
  /// The bus the unlocks and the save request go out on.
  /// </summary>
  private readonly EventService _events;
  private readonly INotificationService _notifications;
  private readonly PlayerStash _stash;

  /// <summary>
  /// The special rewards, keyed by the level that grants them (every tenth).
  /// </summary>
  /// <remarks>
  /// An instance field built in the constructor, not a static initialiser, because the two
  /// unlock lambdas publish on this instance's bus.
  /// </remarks>
  private readonly Dictionary<int, Special> _specials;

  /// <summary>
  /// Builds the milestone table. An instance method for the sake of the two lambdas below,
  /// which close over <see cref="_events"/>.
  /// </summary>
  private Dictionary<int, Special> BuildSpecials() => new()
  {
    [10] = new Special(
      "You have gained the street smarts to know where to look in basic vehicles. You may encounter vehicles with a marker on top.",
      "You can steal those vehicles and extract drugs after you lose the police.",
      () => _events.Publish(new VanStealingUnlockedEvent())),
    [20] = new Special(
      "You have unlocked the Taco Bomb Drug Front.",
      "You can use it as a front to distribute drugs. It's been marked on your map as an Armored Van.",
      () => _events.Publish(new TacoFrontUnlockedEvent())),
    [30] = new Special(
      "Connections. Your dealers now charge you 10% less to buy."),
    [40] = new Special(
      "Reputation. You earn 25% more XP from every sale."),
    [50] = new Special(
      "Bigger operation. Your bag gains 200 extra slots."),
    [60] = new Special(
      "Cooler head. You can work the taco van faster before it draws police attention."),
    [70] = new Special(
      "Better fences. Your dealers now pay you 10% more to sell."),
    [80] = new Special(
      "Fuller streets. More customers come looking when you run the taco van."),
    [90] = new Special(
      "Low profile. Running the taco van no longer turns the local gangs against you."),
    [100] = new Special(
      "~y~Kingpin.~w~ You've reached the top: +300 more bag slots, and your dealers buy 15% cheaper and sell 15% higher. Thank you for playing!")
  };

  /// <summary>
  /// Subscribes to level-ups and seeds the bag for the starting level. Called once at
  /// startup, before <see cref="Progression.ApplyLoadedState"/> replays a loaded save.
  /// </summary>
  /// <param name="events">The event bus to subscribe on and publish to.</param>
  /// <param name="notifications">Where the level-up and unlock messages go.</param>
  /// <param name="progression">The level the starting bag is sized from.</param>
  /// <param name="stash">The bag this sizes, and the record of which tips have shown.</param>
  public RewardSystem(EventService events, INotificationService notifications, Progression progression, PlayerStash stash)
  {
    _events = events;
    _notifications = notifications;
    _stash = stash;
    _specials = BuildSpecials();

    _events.Subscribe<LeveledUpEvent>(OnLeveledUp);

    // The progression is read here and never held: every later level arrives on the event,
    // which carries its own level.
    _stash.BagSize = BagForLevel(progression.Level);
  }

  /// <summary>
  /// The linear curve plus any milestone bag bonuses (<see cref="Perks.BagBonus"/>).
  /// </summary>
  private static int BagForLevel(int level) => 100 + (level - 1) * 10 + Perks.BagBonus(level);

  private void OnLeveledUp(LeveledUpEvent levelUp)
  {
    _stash.BagSize = BagForLevel(levelUp.Level);

    if (levelUp.Level % 10 == 0 && _specials.TryGetValue(levelUp.Level, out Special? special))
    {
      // Milestone: the special's own notification stands in for the plain one, so the
      // player gets a single unlock message rather than "Level Up" plus the unlock.
      special.Apply?.Invoke();
      Announce(levelUp.Level, special.Message, special.FollowUp);
      return;
    }

    // Every other level gets a plain ticker — but not while ApplyLoadedState is replaying
    // a loaded save, or a returning player would be buried under one per level reached.
    if (!levelUp.Replayed)
    {
      _notifications.Ticker("~b~Level Up!~w~ You are now level " + levelUp.Level + ".");
    }
  }

  /// <summary>
  /// Shows a milestone notification exactly once, ever. <see cref="PlayerStash.TipsShown"/>
  /// is persisted, which is what stops this re-firing on every replay and across sessions.
  /// </summary>
  private void Announce(int level, string message, string? followUp)
  {
    if (_stash.TipsShown.Contains(level))
    {
      return;
    }

    _notifications.Ticker("~b~Level Unlocked!~w~\n" + message);
    if (followUp != null)
    {
      _notifications.Ticker(followUp);
    }

    _stash.TipsShown.Add(level);
    _events.Publish(new SaveRequestedEvent());
  }
}
