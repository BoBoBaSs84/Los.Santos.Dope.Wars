using LSDW.Helpers;
using LSDW.Services;

using GTA;
using GTA.Math;

namespace LSDW.Activities;

/// <summary>
/// The taco van: spawning it near its parking spot, deleting it once the player is well
/// away from it, the one-shot hint on climbing in, and the trunk that serves as the shop
/// counter.
/// </summary>
internal sealed class TacoVanKeeper
{
  /// <summary>
  /// Where the van is parked.
  /// </summary>
  public static readonly Vector3 ParkingLocation = new(-654.939f, -716.812f, 28.502f);

  /// <summary>
  /// Which way it faces when it spawns.
  /// </summary>
  public static readonly float ParkingHeading = 270.745f;

  /// <summary>
  /// Whether the player was in a vehicle on the previous tick, so the "press 2"
  /// hint fires once on entering the taco van rather than every tick while sitting
  /// in it.
  /// </summary>
  private bool _lastInVehicle;

  /// <summary>Where the one-shot hint goes.</summary>
  private readonly INotificationService _notifications;

  /// <summary>
  /// Takes the notification service the boarding hint is shown through.
  /// </summary>
  /// <param name="notifications">Where the one-shot hint goes.</param>
  public TacoVanKeeper(INotificationService notifications)
    => _notifications = notifications;

  /// <summary>
  /// Gets the van, or <see langword="null"/> when none is spawned.
  /// </summary>
  public Vehicle? Van { get; private set; }

  /// <summary>
  /// Whether the player is sitting in the taco van.
  /// </summary>
  public bool IsPlayerAboard(Ped character)
    => character.IsInVehicle(Van);

  /// <summary>
  /// Puts a van at the parking spot.
  /// </summary>
  public void Spawn()
  {
    Model model = ScriptHelper.GetVehicleModel(VehicleHash.Taco);
    Van = Vehicle.Create(model, ParkingLocation, ParkingHeading);
    Van.Mods.InstallModKit();
    Van.Mods[VehicleModType.Horns].Index = 27;
    Van.IsPersistent = false;
  }

  /// <summary>
  /// Spawns the van when the player comes near its spot, deletes it once they are well
  /// away from it, and fires the start-a-session hint the first tick they climb in.
  /// </summary>
  public void UpdateForPlayer(Ped character)
  {
    if (character.IsInRange(ParkingLocation, 50f))
    {
      if (Van == null || Van.IsDead)
      {
        Spawn();
      }
    }
    else if (Van != null && !character.IsInRange(Van.Position, 200f))
    {
      Van.Delete();
    }

    // Fire the hint once on entering a vehicle, not every tick while sitting in it.
    if (!_lastInVehicle && character.IsInVehicle())
    {
      _lastInVehicle = true;
      if (character.CurrentVehicle == Van)
      {
        _notifications.Ticker("Press ~b~2~w~ to start the Taco minigame.", isImportant: false);
      }
    }
    else if (!character.IsInVehicle())
    {
      _lastInVehicle = false;
    }
  }

  /// <summary>
  /// Opens the trunk when the van has stopped and shuts it when it moves — the trunk is
  /// the shop counter.
  /// </summary>
  public void UpdateCounter()
  {
    if (Van!.Speed < 1f)
    {
      Van.Doors[VehicleDoorIndex.Trunk].Open(loose: false, instantly: false);
    }
    else
    {
      Van.Doors[VehicleDoorIndex.Trunk].Close(instantly: false);
    }
  }

  /// <summary>
  /// Shuts the counter as a session ends.
  /// </summary>
  public void CloseCounter()
    => Van!.Doors[VehicleDoorIndex.Trunk].Close(instantly: false);

  /// <summary>
  /// Deletes the van. Called from <see cref="TacoMinigame.Cleanup"/> when the script is
  /// unloaded or reloaded.
  /// </summary>
  public void Cleanup()
  {
    Van?.Delete();
    Van = null;
  }
}
