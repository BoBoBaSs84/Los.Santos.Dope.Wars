using LSDW.Events.Base;

namespace LSDW.Events;

/// <summary>
/// Raised by <see cref="Progression"/> once per level crossed. <see cref="RewardSystem"/>
/// reacts to it: bag size every level, a special reward every tenth.
/// </summary>
/// <param name="Level">The level just reached.</param>
/// <param name="Replayed">
/// <see langword="true"/> when this is <see cref="Progression.ApplyLoadedState"/> replaying
/// a loaded save rather than a live level-up, so the plain per-level notification stays
/// suppressed. Milestone rewards ignore it — their notifications are gated by
/// <see cref="PlayerStash.TipsShown"/> instead, and their effects must re-apply on replay.
/// </param>
internal sealed record LeveledUpEvent(int Level, bool Replayed = false) : Event;
