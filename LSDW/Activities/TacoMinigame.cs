using LSDW.Abstractions;
using LSDW.Drugs;
using LSDW.Events;
using LSDW.Leveling;
using LSDW.Services;
using LSDW.UserInterface;

using GTA;

namespace LSDW.Activities;

/// <summary>
/// The taco front: the session the player runs out of the van, and the entry points
/// <see cref="Core.DrugDeal"/> drives it through.
/// </summary>
internal sealed class TacoMinigame
{
  private readonly INotificationService _notifications;
  private readonly TacoVanKeeper _vanKeeper;
  private readonly TacoCustomers _customers;
  private readonly TacoSales _sales;
  private readonly TacoHud _hud;
  private readonly Progression _progression;
  private readonly PlayerStash _stash;

  /// <summary>Whether the level-20 unlock has landed.</summary>
  public bool MinigameUnlocked { get; set; }

  /// <summary>Whether a selling session is currently running.</summary>
  public bool OnMission { get; set; }

  /// <summary>
  /// The property, for <see cref="SaveService"/> to read and write its
  /// <see cref="TacoProperty.Owned"/> flag. Exposed for the same reason
  /// <see cref="Dealers.DealerWorld.Dealers"/> is: the save layer needs the live object,
  /// and handing it over beats a second callback hook.
  /// </summary>
  public TacoProperty Property { get; }

  /// <summary>
  /// Builds the cluster and subscribes to the level-20 unlock. Constructed once at startup,
  /// before a loaded save's level-ups are replayed.
  /// </summary>
  /// <param name="events">The event bus to subscribe on and hand down to the cluster.</param>
  /// <param name="notifications">The notification service, handed down to the cluster.</param>
  /// <param name="audio">The audio provider, handed down to the cluster.</param>
  /// <param name="money">The wallet, handed down to the property that spends from it.</param>
  /// <param name="progression">The level the cluster's perks are read from.</param>
  /// <param name="stash">The bag a session sells out of.</param>
  public TacoMinigame(EventService events, INotificationService notifications, IAudioProvider audio, IPlayerMoney money, Progression progression, PlayerStash stash)
  {
    _notifications = notifications;
    _progression = progression;
    _stash = stash;
    Property = new TacoProperty(events, notifications, audio, money);
    _vanKeeper = new TacoVanKeeper(notifications);
    _customers = new TacoCustomers(progression);
    _sales = new TacoSales(events, notifications, new TacoHeatWindow(), progression, stash);
    _hud = new TacoHud();

    events.Subscribe<TacoFrontUnlockedEvent>(_ => Unlock());
  }

  private void Unlock()
  {
    MinigameUnlocked = true;
    Property.ShowBlip();
  }

  /// <summary>
  /// Starts a session: zeroes the takings, flags the player for the local gangs, and
  /// explains the rules.
  /// </summary>
  public void StartMinigame()
  {
    OnMission = true;
    _sales.ResetProfit();
    // L90 "low profile" leaves the player un-flagged so gangs ignore them.
    Game.Player.Character.IsEnemy = Perks.TacoDrawsGangs(_progression.Level);
    _customers.Clear();
    _notifications.Ticker("You receive money for each transaction you make, but you will also get police attention.");
    _notifications.Ticker("To make a transaction, stop near a customer.");
    _notifications.Ticker("Local gangs will not appreciate you doing business on their turf.");
    _notifications.Subtitle("Look for customers.", 20000);
  }

  /// <summary>
  /// Ends the session. Callers that have already reported the takings themselves
  /// pass <paramref name="showResult"/> = false to suppress the summary.
  /// </summary>
  public void StopMinigame(bool showResult = true)
  {
    OnMission = false;
    Game.Player.Character.IsEnemy = false;
    _vanKeeper.CloseCounter();
    _customers.Clear();

    if (showResult)
    {
      _notifications.Ticker("You've made ~g~$" + _sales.Profit + "~w~ in profit this session.");
    }

    _notifications.Subtitle("");
  }

  /// <summary>
  /// Removes the property blip and the taco van, and ends any session in progress.
  /// Called from <see cref="Core.DrugDeal"/>'s <c>Aborted</c> handler.
  /// </summary>
  public void Cleanup()
  {
    Property.Cleanup();
    _vanKeeper.Cleanup();

    OnMission = false;
    Game.Player.Character.IsEnemy = false;
  }

  /// <summary>
  /// Ticked only while the minigame is unlocked. Keeps the property marker and the van up
  /// to date, then runs the mission if one is in progress.
  /// </summary>
  public void OnTick(Ped character)
  {
    UpdateProperty(character);

    if (OnMission)
    {
      UpdateMission(character);
    }
  }

  /// <summary>
  /// The property before it is bought (a marker and a prompt), and the taco van
  /// once it is (spawned in range, deleted out of it, with a one-shot hint on
  /// climbing in).
  /// </summary>
  private void UpdateProperty(Ped character)
  {
    // While it is still for sale there is no van to keep.
    if (Property.DrawForSale(character))
    {
      return;
    }

    _vanKeeper.UpdateForPlayer(character);
  }

  /// <summary>
  /// <c>2</c> starts and stops a session; <c>E</c> buys the property. Both are
  /// no-ops until the minigame is unlocked.
  /// </summary>
  public void ProcessKey(Keys key)
  {
    Ped character = Game.Player.Character;

    if (key == SettingsService.Binds.TacoSession && _vanKeeper.IsPlayerAboard(character) && MinigameUnlocked)
    {
      if (!OnMission && _stash.Drugs.Values.Sum() != 0)
      {
        StartMinigame();
      }
      else if (OnMission)
      {
        StopMinigame();
      }
      else if (!OnMission && _stash.Drugs.Values.Sum() == 0)
      {
        _notifications.Ticker("You don't have any drugs to sell.");
      }
    }
    else if (key == SettingsService.Binds.BuyProperty && MinigameUnlocked)
    {
      Property.TryPurchase(character);
    }
  }

  private void UpdateMission(Ped character)
  {
    if (!OnMission || !_vanKeeper.IsPlayerAboard(character) || _stash.Drugs.Values.Sum() == 0)
    {
      if (_stash.Drugs.Values.Sum() == 0)
      {
        _notifications.Ticker("You ran out of drugs!\n You made ~g~$" + _sales.Profit + "~w~ in profit.");
        StopMinigame(showResult: false);
      }
      return;
    }

    // The trunk is the shop counter: open when stopped, shut when moving.
    _vanKeeper.UpdateCounter();

    _hud.Draw(_customers.TacosSold, _stash.Drugs.Values.Sum(), _sales.Profit);
    _customers.TrackNearbyCustomers(character);
    _customers.ServeCustomers(character, () => _sales.SellRandomDrug());
  }
}
