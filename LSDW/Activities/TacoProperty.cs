using LSDW.Abstractions;
using LSDW.Events;
using LSDW.Services;

using GTA;
using GTA.Math;

namespace LSDW.Activities;

/// <summary>
/// The taco factory itself: its map blip, the marker over it while it is still for sale,
/// the purchase prompt, and the purchase.
/// </summary>
/// <remarks>
/// Owns one world object — the blip — so it has a <see cref="Cleanup"/> that
/// <see cref="TacoMinigame.Cleanup"/> calls. Nothing else here outlives a tick.
/// </remarks>
internal sealed class TacoProperty
{
  /// <summary>Where the factory is.</summary>
  public static readonly Vector3 Location = new(-655.363f, -676.696f, 31.569f);

  private readonly EventService _events;
  private readonly INotificationService _notifications;
  private readonly IAudioProvider _audio;
  private readonly IPlayerMoney _money;

  private Blip? _tacoBlip;

  /// <param name="events">The event bus a purchase asks for its save on.</param>
  /// <param name="notifications">Where the purchase prompt and its confirmation go.</param>
  /// <param name="audio">The purchase sound.</param>
  /// <param name="money">The wallet the price comes out of.</param>
  public TacoProperty(EventService events, INotificationService notifications, IAudioProvider audio, IPlayerMoney money)
  {
    _events = events;
    _notifications = notifications;
    _audio = audio;
    _money = money;
  }

  /// <summary>
  /// Gets or sets whether the player has purchased the property. Persisted as
  /// <c>[Player] TacoOwned</c>.
  /// </summary>
  public bool Owned { get; set; }

  /// <summary>
  /// Puts the property on the map. Called when the level-20 unlock lands, including the
  /// replayed one a returning player's save produces, so the blip comes back with them.
  /// </summary>
  /// <remarks>
  /// Guarded against a second call: the unlock is republished on every load, and a second
  /// blip would leave the first orphaned underneath it — invisible, since the two match.
  /// </remarks>
  public void ShowBlip()
  {
    if (_tacoBlip != null)
    {
      return;
    }

    _tacoBlip = Blip.Create(Location);
    _tacoBlip.Sprite = BlipSprite.ArmoredTruck;
    _tacoBlip.IsShortRange = true;
    _tacoBlip.Color = BlipColor.Yellow;
  }

  /// <summary>
  /// Draws the for-sale marker and the purchase prompt, and reports whether the property
  /// is still unowned — in which case the caller has nothing further to do this tick.
  /// </summary>
  /// <returns>
  /// <see langword="true"/> while the property is still for sale; <see langword="false"/>
  /// once it has been bought.
  /// </returns>
  public bool DrawForSale(Ped character)
  {
    if (Owned)
    {
      return false;
    }

    // Marker on the building while it is still for sale.
    World.DrawMarker(MarkerType.Cylinder, Location + new Vector3(0f, 0f, -1f), Vector3.Zero, Vector3.Zero, new Vector3(1f, 1f, 1f), Color.FromArgb(100, 255, 20, 0), false, true, false);

    if (character.IsInRange(Location, 2f))
    {
      _notifications.Subtitle("Press ~b~E~w~ to purchase this property for ~g~$" + SettingsService.Economy.TacoPropertyPrice);
    }

    return true;
  }

  /// <summary>
  /// Buys the property if the player is standing at it with the money to hand.
  /// </summary>
  /// <remarks>
  /// The affordability check is part of the condition rather than a message: pressing the
  /// key without the money does nothing at all, as it always has.
  /// </remarks>
  /// <param name="character">The player.</param>
  public void TryPurchase(Ped character)
  {
    if (Owned || !character.IsInRange(Location, 2f) || _money.Money < SettingsService.Economy.TacoPropertyPrice)
    {
      return;
    }

    Owned = true;
    _notifications.Ticker("You can use the Taco Factory as a front for drug distribution.");
    _notifications.Ticker("You will find a Taco Van around the building.");
    _money.Money -= SettingsService.Economy.TacoPropertyPrice;
    _notifications.Subtitle("Property purchased!", 7000);
    _audio.PlaySoundFrontendAndForget("PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET", false);
    _events.Publish(new SaveRequestedEvent());
  }

  /// <summary>
  /// Removes the map blip. Called from <see cref="TacoMinigame.Cleanup"/> when the script
  /// is unloaded or reloaded.
  /// </summary>
  /// <remarks>
  /// <see cref="Owned"/> is deliberately not reset: a reload builds a fresh instance
  /// anyway, and it is restored from the save. Only the game-side blip needs undoing by
  /// hand.
  /// </remarks>
  public void Cleanup()
  {
    _tacoBlip?.Delete();
    _tacoBlip = null;
  }
}
