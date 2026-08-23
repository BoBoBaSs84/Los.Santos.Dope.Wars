using LSDW.Events.Base;

namespace LSDW.Events;

/// <summary>
/// Published by any subsystem that has just mutated persisted state and wants it
/// written to disk. <see cref="SaveService"/> subscribes and does the write, so no
/// subsystem has to depend on the save code to persist.
/// </summary>
internal sealed record SaveRequestedEvent : Event;
