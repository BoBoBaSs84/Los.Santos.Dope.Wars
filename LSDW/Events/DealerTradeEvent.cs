using LSDW.Dealers;
using LSDW.Events.Base;

namespace LSDW.Events;

/// <summary>
/// Published by <see cref="DealerTrading"/> when a buy or sell completes at a dealer.
/// <see cref="DealerWorld"/> subscribes and rolls the chance of a police bust, so the
/// trading flow does not have to reach into the dealer world to trigger it.
/// </summary>
/// <param name="Dealer">The dealer the trade happened with.</param>
internal sealed record DealerTradeEvent(DrugDealer Dealer) : Event;
