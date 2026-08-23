using LSDW.Events.Base;

namespace LSDW.Events;

/// <summary>
/// Raised by a sell site the moment a sale completes. <see cref="Progression"/> reads
/// <see cref="Profit"/> for XP and <see cref="PlayerStats"/> reads <see cref="Revenue"/>
/// for the money box; buys publish <see cref="DrugsBoughtEvent"/> instead.
/// </summary>
/// <param name="Revenue">The gross take of the sale — <c>sellPrice * amount</c>. Always positive.</param>
/// <param name="Profit">The sell margin, revenue minus what the player paid. May be negative.</param>
internal sealed record DrugsSoldEvent(int Revenue, int Profit) : Event;
