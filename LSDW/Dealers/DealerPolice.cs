using LSDW.Drugs;
using LSDW.Events;
using LSDW.Helpers;
using LSDW.Services;

namespace LSDW.Dealers;

/// <summary>
/// The police side of dealing: the chance that a completed trade turns out to be a
/// setup, and the heat it lands the player in.
/// </summary>
/// <remarks>
/// Holds no state of its own beyond the bus. It is an instance anyway, so that its three
/// subscriptions belong to something with a lifetime rather than living for as long as the
/// assembly does.
/// </remarks>
internal sealed class DealerPolice
{
  /// <summary>
  /// The bus a confiscation's save request goes out on.
  /// </summary>
  private readonly EventService _events;

  /// <summary>
  /// Where a bust and a confiscation are announced.
  /// </summary>
  private readonly INotificationService _notifications;

  /// <summary>
  /// The bag a bust empties.
  /// </summary>
  private readonly PlayerStash _stash;

  /// <summary>
  /// Subscribes to the police-reaction events: a completed trade rolls a bust, and a
  /// death or arrest confiscates the carried stash. Constructed once at startup.
  /// </summary>
  /// <param name="events">The event bus to subscribe on and publish to.</param>
  /// <param name="notifications">Where a bust and a confiscation are announced.</param>
  /// <param name="stash">The carried stash a death or arrest confiscates.</param>
  public DealerPolice(EventService events, INotificationService notifications, PlayerStash stash)
  {
    _events = events;
    _notifications = notifications;
    _stash = stash;
    _events.Subscribe<DealerTradeEvent>(HandleDealerTrade);
    _events.Subscribe<PlayerDiedEvent>(HandlePlayerDied);
    _events.Subscribe<PlayerArrestedEvent>(HandlePlayerArrested);
  }

  private void HandlePlayerArrested(PlayerArrestedEvent @event)
  {
    if (SettingsService.Police.LoseDrugsWhenBusted)
      Confiscate();
  }

  private void HandlePlayerDied(PlayerDiedEvent @event)
  {
    if (SettingsService.Police.LoseDrugsOnDeath)
      Confiscate();
  }

  /// <summary>
  /// Empties the carried stash and persists the loss. The empty-bag guard makes it idempotent, which is
  /// what lets the death/arrest events fire every tick their condition holds without confiscating twice.
  /// </summary>
  private void Confiscate()
  {
    if (_stash.Bag <= 0)
      return;

    _notifications.Ticker("Your drugs got revoked.", isImportant: false);
    _stash.ClearCarried();

    _events.Publish(new SaveRequestedEvent());
  }

  /// <summary>
  /// Some fraction of transactions are a setup: the dealer bolts and the police arrive.
  /// How often, and how hard, is <c>[Police] BustChancePercent</c> and
  /// <c>[Police] BustWantedLevel</c>.
  /// </summary>
  private void HandleDealerTrade(DealerTradeEvent @event)
  {
    if (RandomHelper.Random.NextPercent() >= SettingsService.Police.BustChance || @event.Dealer is null)
      return;

    @event.Dealer.MainPed?.Task.FleeFrom(@event.Dealer.MainPed.Position);
    _notifications.Subtitle("It's a police bust! Get out of there!");

    if (SettingsService.Police.BustWantedLevel > 0)
      ScriptHelper.WantedLevel = SettingsService.Police.BustWantedLevel;
  }
}
