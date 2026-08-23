using LSDW.Events.Base;

namespace LSDW.Events;

/// <summary>
/// Raised when the player buys from a dealer. <see cref="PlayerStats"/> subscribes and
/// adds the <see cref="Cost"/> to lifetime spend. Buys grant no XP, so
/// <see cref="Progression"/> ignores them.
/// </summary>
/// <param name="Cost">What the player paid — <c>buyPrice * amount</c>.</param>
internal sealed record DrugsBoughtEvent(int Cost) : Event;
