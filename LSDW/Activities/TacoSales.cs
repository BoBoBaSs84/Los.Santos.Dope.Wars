using LSDW.Drugs;
using LSDW.Events;
using LSDW.Helpers;
using LSDW.Leveling;
using LSDW.Services;

namespace LSDW.Activities;

/// <summary>
/// What a completed taco sale does: picks a drug out of the player's stash, prices it,
/// pays them, and reports the sale to the rest of the mod.
/// </summary>
internal sealed class TacoSales
{
  private readonly EventService _events;
  private readonly INotificationService _notifications;
  private readonly TacoHeatWindow _heatWindow;
  private readonly Progression _progression;
  private readonly PlayerStash _stash;

  /// <summary>
  /// Gets the takings so far this session. Reset by <see cref="ResetProfit"/> when a
  /// session starts, and reported when one ends.
  /// </summary>
  public int Profit { get; private set; }

  /// <param name="events">The event bus a completed sale reports itself on.</param>
  /// <param name="notifications">Where each sale's ticker line goes.</param>
  /// <param name="heatWindow">The window that decides when selling too fast draws police.</param>
  /// <param name="progression">The level the heat threshold is read from.</param>
  /// <param name="stash">The bag each sale comes out of.</param>
  public TacoSales(EventService events, INotificationService notifications, TacoHeatWindow heatWindow, Progression progression, PlayerStash stash)
  {
    _events = events;
    _notifications = notifications;
    _heatWindow = heatWindow;
    _progression = progression;
    _stash = stash;
  }

  /// <summary>
  /// Zeroes the session takings. Called as a session starts.
  /// </summary>
  public void ResetProfit()
    => Profit = 0;

  /// <summary>
  /// Sells one unit of a random drug the player is actually carrying, at a random
  /// markup of 0-50% over the market price.
  /// </summary>
  public void SellRandomDrug()
  {
    List<string> carried = [.. _stash.Drugs
      .Where(playerDrug => playerDrug.Value > 0)
      .Select(playerDrug => playerDrug.Key)];

    string drug = carried[RandomHelper.Random.Next(carried.Count)];
    _stash.Remove(drug, 1);

    int marketPrice = PlayerStash.MarketValue[drug];
    double markup = RandomHelper.Random.NextDouble() * SettingsService.Economy.TacoMaxMarkup;
    int salePrice = marketPrice + Convert.ToInt32(Math.Floor(marketPrice * markup));

    _notifications.Ticker("Sold ~b~" + drug + "~w~ for ~g~$" + salePrice + "~w~ with a profit of ~g~" + Convert.ToInt32(Math.Floor(markup * 100.0)) + "%", isImportant: false);
    Profit += salePrice;

    // Gross take for the money box, margin over what the player paid for XP (a loss
    // grants none). One event, both reactions.
    _events.Publish(new DrugsSoldEvent(salePrice, salePrice - _stash.BoughtPrice[drug]));

    _events.Publish(new SaveRequestedEvent());

    // The window itself lives in TacoHeatWindow, which is free of GTA types so the
    // gap maths can be checked headlessly. Raising the wanted level stays here: it is
    // the one part of this that touches the game.
    if (_heatWindow.RecordSale(Perks.TacoHeatGapThreshold(_progression.Level)))
    {
      ScriptHelper.WantedLevel++;
    }
  }
}
