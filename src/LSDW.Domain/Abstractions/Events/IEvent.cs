namespace LSDW.Domain.Abstractions.Events;

/// <summary>
/// Represents a domain event that has occurred within the system. Domain events are used to capture
/// significant occurrences or changes in the state of the domain model, allowing for decoupled
/// communication between different parts of the system. Each event should contain relevant information
/// about what happened and when it happened, enabling other components to react accordingly.
/// </summary>
public interface IEvent
{
	/// <summary>
	/// Gets the timestamp when the event occurred.
	/// </summary>
	DateTimeOffset OccurredAt { get; }

	/// <summary>
	/// Gets the unique identifier of the event.
	/// </summary>
	Guid Id { get; }
}
