using LSDW.Domain.Events.Base;

namespace LSDW.Domain.Events.Peds;

/// <summary>
/// Represents the event data for when a ped dies.
/// </summary>
/// <param name="pedId">The unique identifier of the ped that died.</param>
public sealed class PedestrianDied(int pedId) : PedEventBase(pedId)
{ }
