using LSDW.Events.Base;

namespace LSDW.Events;

/// <summary>
/// Published by <see cref="Core.DrugDeal"/> while the player was just arrested (the arrest native fired
/// within the last few ticks). <see cref="Dealers.DealerPolice"/> subscribes and confiscates the carried
/// stash, gated on <c>[Police] LoseDrugsWhenBusted</c>.
/// </summary>
/// <remarks>
/// Mutually exclusive with <see cref="PlayerDiedEvent"/>: a dead player never triggers the arrest native,
/// so the publisher's <c>if</c>/<c>else if</c> is what keeps the two apart. Fires per tick while the
/// condition holds, absorbed by the subscriber's empty-bag guard.
/// </remarks>
internal sealed record PlayerArrestedEvent : Event;
