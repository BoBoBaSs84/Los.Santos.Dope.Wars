using LSDW.Domain.Events.Base;

namespace LSDW.Domain.Events.Peds;

/// <summary>
/// Represents the event data for when a ped is added.
/// </summary>
/// <param name="pedId">The unique identifier of the ped that was added.</param>
public sealed class PedestrianAdded(int pedId) : PedEventBase(pedId)
{ }
