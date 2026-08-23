using LSDW.Events.Base;

namespace LSDW.Events;

/// <summary>
/// Published by <see cref="Core.DrugDeal"/> every tick the player is dead. <see cref="Dealers.DealerPolice"/>
/// subscribes and confiscates the carried stash, gated on <c>[Police] LoseDrugsOnDeath</c>, so the entry
/// point never reaches into the stash or the police settings itself.
/// </summary>
/// <remarks>
/// Fires per tick rather than once per death — faithful to the old polled confiscation, whose repeated
/// runs were absorbed by an idempotent empty-bag guard, which now lives in the subscriber.
/// </remarks>
internal sealed record PlayerDiedEvent : Event;
