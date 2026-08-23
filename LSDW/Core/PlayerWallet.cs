using LSDW.Abstractions;
using LSDW.Events;
using LSDW.Services;

namespace LSDW.Core;

/// <summary>
/// The single GTA-side home for crediting sale revenue: subscribes to
/// <see cref="DrugsSoldEvent"/> and adds its gross <see cref="DrugsSoldEvent.Revenue"/>
/// to the player's money, so no sell site touches the wallet directly.
/// </summary>
/// <remarks>
/// Holds no state and is never called again after construction. It is an instance anyway,
/// so that its subscription belongs to something with a lifetime rather than living for as
/// long as the assembly does.
/// </remarks>
internal sealed class PlayerWallet
{
  /// <summary>
  /// Subscribes to sales. Constructed once at startup, before any sale can fire.
  /// </summary>
  /// <param name="events">The event bus to subscribe on.</param>
  /// <param name="money">The wallet the revenue lands in.</param>
  public PlayerWallet(EventService events, IPlayerMoney money)
    => events.Subscribe<DrugsSoldEvent>(sale => money.Money += sale.Revenue);
}
