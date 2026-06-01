namespace LSDW.Domain.Events.Base;

/// <summary>
/// Represents the base event data for pedestrian (ped) related events.
/// </summary>
/// <param name="pedId">The unique identifier of the ped.</param>
public abstract class PedEventBase(int pedId) : EventBase
{
	/// <summary>
	/// Gets the unique identifier of the ped.
	/// </summary>
	public int PedId => pedId;
}
