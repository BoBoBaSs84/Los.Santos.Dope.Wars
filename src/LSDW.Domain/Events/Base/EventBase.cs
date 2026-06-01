using LSDW.Domain.Abstractions.Events;

namespace LSDW.Domain.Events.Base;

/// <summary>
/// Represents the base class for all domain events in the system. This class provides common properties
/// such as a unique identifier and a timestamp for when the event occurred. All specific domain events
/// should inherit from this base class to ensure consistency and to leverage shared functionality.
/// </summary>
public abstract class EventBase : IEvent
{
	/// <inheritdoc/>
	public Guid Id { get; } = Guid.NewGuid();

	/// <inheritdoc/>
	public DateTimeOffset OccurredAt { get; } = DateTimeOffset.UtcNow;
}
