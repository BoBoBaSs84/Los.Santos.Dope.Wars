using LSDW.Events.Base;

namespace LSDW.Events;

/// <summary>
/// Published by <see cref="RewardSystem"/> when the level-10 milestone is reached.
/// <see cref="VanStealing"/> subscribes and unlocks itself, so the reward table never
/// reaches into the feature it grants.
/// </summary>
internal sealed record VanStealingUnlockedEvent : Event;
