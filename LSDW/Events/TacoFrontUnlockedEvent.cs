using LSDW.Events.Base;

namespace LSDW.Events;

/// <summary>
/// Published by <see cref="RewardSystem"/> when the level-20 milestone is reached.
/// <see cref="TacoMinigame"/> subscribes, unlocks the minigame and drops the property
/// blip — the reward table stays out of the taco front's internals.
/// </summary>
internal sealed record TacoFrontUnlockedEvent : Event;
