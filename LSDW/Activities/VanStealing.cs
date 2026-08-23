using LSDW.Drugs;
using LSDW.Events;
using LSDW.Helpers;
using LSDW.Services;

using GTA;
using GTA.Math;
using GTA.Native;

namespace LSDW.Activities;

/// <summary>
/// Unlocked at level 10. Marks a random fraction of parked cars as stashed; steal
/// one, shake the police, and drive it to a safe house to extract the drugs.
/// </summary>
internal sealed class VanStealing
{
  /// <summary>
  /// Van state, keyed by vehicle handle: 0 = empty (or already looted),
  /// 1 = stashed and untouched, 2 = stashed and currently being stolen.
  /// </summary>
  private readonly Dictionary<int, int> _vanStates = [];
  private bool _playerInCar;
  private Blip? _destinationBlip;
  private Vehicle? _currentVan;
  private Vector3 _safePlace = new(0f, 0f, 0f);
  private int _routeRefreshCounter;

  /// <summary>
  /// The bus the delivery's save request goes out on.
  /// </summary>
  private readonly EventService _events;

  /// <summary>
  /// Where the objective subtitles and the delivery's findings are shown.
  /// </summary>
  private readonly INotificationService _notifications;

  /// <summary>
  /// The bag a delivered van fills.
  /// </summary>
  private readonly PlayerStash _stash;

  public bool VanUnlocked { get; set; }

  /// <summary>
  /// Subscribes to the level-10 unlock. Constructed once at startup, before a loaded save's
  /// level-ups are replayed, so a returning player past level 10 is re-unlocked.
  /// </summary>
  /// <param name="events">The event bus to subscribe on and publish to.</param>
  /// <param name="notifications">Where the objective subtitles and findings are shown.</param>
  /// <param name="stash">The bag a delivered van's contents go into.</param>
  public VanStealing(EventService events, INotificationService notifications, PlayerStash stash)
  {
    _events = events;
    _notifications = notifications;
    _stash = stash;
    _events.Subscribe<VanStealingUnlockedEvent>(_ => VanUnlocked = true);
  }

  /// <summary>
  /// Rolls the van's contents into the player's bag: each drug in catalog order gets a
  /// coin flip and, if it wins, 1-14 grams or whatever still fits.
  /// </summary>
  /// <remarks>
  /// The draw sequence is load-bearing: one <c>Next(10)</c> per catalog drug, plus one
  /// <c>Next</c> for the amount when that one wins.
  /// </remarks>
  private void GiveRandomDrugs()
  {
    string found = "You have found:\n";
    int kinds = 0;

    for (int i = 0; i < PlayerStash.DrugNames.Count; i++)
    {
      string drug = PlayerStash.DrugNames[i];
      int space = _stash.BagSize - _stash.Bag;

      if (RandomHelper.Random.Next(10) > 5 && space > 0)
      {
        // Up to 14 grams, or whatever still fits, whichever is smaller.
        int amount = (space < 14) ? RandomHelper.Random.Next(1, space) : RandomHelper.Random.Next(1, 15);
        if (amount > 0)
        {
          _stash.Add(drug, amount);
          found += "~y~" + amount + "~w~ grams of ~b~" + drug + "\n";
          kinds++;
        }
      }
    }

    if (kinds > 0)
    {
      _notifications.Ticker(found, isImportant: false);
    }
    else if (_stash.BagSize - _stash.Bag <= 0)
    {
      _notifications.Ticker("Your bag is full.", isImportant: false);
    }
    else
    {
      _notifications.Ticker("You have found nothing.", isImportant: false);
    }
  }

  private static Vector3 GetClosestSafePoint(Vector3 playerPos)
  {
    float shortest = float.MaxValue;
    Vector3 closest = new(0f, 0f, 0f);

    foreach (Vector3 candidate in VanRoster.SafePlaces)
    {
      float distance = (candidate - playerPos).Length();
      if (distance < shortest)
      {
        closest = candidate;
        shortest = distance;
      }
    }

    return closest;
  }

  /// <summary>
  /// Re-asserts the GPS route every 300 ticks; the game drops it otherwise.
  /// </summary>
  private void RefreshRoute()
  {
    if (_destinationBlip == null)
    {
      return;
    }

    _routeRefreshCounter++;
    if (_routeRefreshCounter > 300)
    {
      _routeRefreshCounter = 0;
      _destinationBlip.ShowRoute = true;
    }
  }

  public void OnTick()
  {
    Ped character = Game.Player.Character;

    MarkNearbyVans(character);
    UpdateVanMarkers(character);
    UpdateHeist(character);

    bool parkedAtSafeHouse = character.IsInRange(_safePlace, 5f)
      && character.IsInVehicle()
      && character.CurrentVehicle.Speed < 1f
      && character.CurrentVehicle == _currentVan
      && (_currentVan == null || _vanStates[_currentVan.Handle] != 0);

    if (parkedAtSafeHouse)
    {
      _notifications.Subtitle("");
      _destinationBlip?.Delete();
      _events.Publish(new SaveRequestedEvent());

      if (_currentVan != null)
      {
        _vanStates[_currentVan.Handle] = 0;
        _currentVan.AttachedBlip.Delete();
      }

      GiveRandomDrugs();
      _currentVan = null;
      _playerInCar = false;
    }
  }

  /// <summary>
  /// Removes the destination blip and every blip this activity hung on a van. Called
  /// from <see cref="Core.DrugDeal"/>'s <c>Aborted</c> handler.
  /// </summary>
  public void Cleanup()
  {
    _destinationBlip?.Delete();
    _destinationBlip = null;

    foreach (int handle in _vanStates.Keys)
    {
      if (Entity.FromHandle(handle) is Vehicle van)
      {
        van.AttachedBlip?.Delete();
      }
    }

    _currentVan = null;
    _playerInCar = false;
  }

  /// <summary>
  /// Rolls once per newly-seen car: 1 in 50 is carrying a stash.
  /// </summary>
  private void MarkNearbyVans(Ped character)
  {
    foreach (Vehicle vehicle in World.GetNearbyVehicles(character, 50f))
    {
      if (!VanRoster.WantedVans.Contains(vehicle.Model))
        continue;

      if (_vanStates.ContainsKey(vehicle.Handle))
        continue;

      int state = (RandomHelper.Random.Next(50) == 0) ? 1 : 0;
      if (state == 1)
      {
        Function.Call(Hash.FLASH_MINIMAP_DISPLAY);
        Blip blip = vehicle.AddBlip();
        blip.Color = BlipColor.Blue;
        blip.IsShortRange = true;
        blip.Scale = 0.8f;
      }

      _vanStates.Add(vehicle.Handle, state);
    }
  }

  private void UpdateVanMarkers(Ped character)
  {
    // Indexed rather than foreach on purpose: the body writes back into _vanStates,
    // which would invalidate an enumerator.
    for (int i = 0; i < _vanStates.Count; i++)
    {
      if (_vanStates.ElementAt(i).Value == 0)
      {
        continue;
      }

      // v3 keeps Vehicle's handle constructor internal. Entity.FromHandle returns
      // null once the car is gone; v2's `new Vehicle(handle)` returned an object
      // whose every check here came back false, so skipping it is the same thing.
      if (Entity.FromHandle(_vanStates.ElementAt(i).Key) is not Vehicle van)
      {
        continue;
      }

      if (!_playerInCar && character.IsInVehicle(van))
      {
        // Stealing it: pick the drop-off now, and roll for a police response.
        _safePlace = GetClosestSafePoint(character.Position);

        if (_vanStates[van.Handle] != 2)
          ScriptHelper.WantedLevel = RandomHelper.Random.Next(0, 4);

        _vanStates[van.Handle] = 2;
        _playerInCar = true;
        _currentVan = van;
      }
      else if (!character.IsInVehicle(van))
      {
        if (van.IsInRange(character.Position, 80f))
        {
          World.DrawMarker(MarkerType.Cone, van.Position + new Vector3(0f, 0f, 3f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.FromArgb(100, 230, 230, 21), true, true, false);
          if (van.AttachedBlip == null)
          {
            Blip blip = van.AddBlip();
            blip.Color = BlipColor.Blue;
            blip.IsShortRange = true;
            blip.Scale = 0.8f;
          }
        }
        else
        {
          van.AttachedBlip?.Delete();
        }
      }
    }
  }

  /// <summary>
  /// Drives the "lose the cops, then reach the safe house" objective.
  /// </summary>
  private void UpdateHeist(Ped character)
  {
    if (_currentVan == null || _vanStates[_currentVan.Handle] <= 0)
    {
      return;
    }

    if (ScriptHelper.WantedLevel > 0)
    {
      _notifications.Subtitle("Lose the cops.");
    }
    else
    {
      _notifications.Subtitle("Take the vehicle to a ~y~safe place.");
      if (_destinationBlip == null || !_destinationBlip.Exists())
      {
        _destinationBlip = Blip.Create(_safePlace);
        _destinationBlip.Color = BlipColor.Yellow;
        _destinationBlip.ShowRoute = true;
      }

      RefreshRoute();
      World.DrawMarker(MarkerType.Cylinder, _safePlace + new Vector3(0f, 0f, -1f), Vector3.Zero, Vector3.Zero, new Vector3(5f, 5f, 5f), Color.FromArgb(60, 230, 230, 21), false, true, false);
    }

    // Bailed out of the van: abandon the run.
    if (!character.IsInVehicle(_currentVan))
    {
      _destinationBlip?.Delete();
      _playerInCar = false;
      _currentVan = null;
      _notifications.Subtitle("");
    }
  }

}
