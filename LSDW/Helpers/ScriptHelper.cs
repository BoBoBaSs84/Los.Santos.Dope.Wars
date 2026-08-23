using GTA;

namespace LSDW.Helpers;

/// <summary>
/// Represents a helper class for script-related functionality: model loading and wanted
/// level management.
/// </summary>
/// <remarks>
/// The notifications this used to carry now live on <c>Services.INotificationService</c>,
/// which subsystems take as a constructor argument. What is left is what still has no seam:
/// both members below block on or mutate game state that only exists in-session.
/// </remarks>
internal static class ScriptHelper
{
  /// <summary>
  /// Gets or set the player's wanted level.
  /// </summary>
  internal static int WantedLevel
  {
    get => Game.Player.Wanted.WantedLevel;
    set
    {
      Game.Player.Wanted.SetWantedLevel(value, delayLawResponse: false);
      Game.Player.Wanted.ApplyWantedLevelChangeNow(delayLawResponse: false);
    }
  }

  /// <summary>
  /// Returns a <c>Model</c> object for the specified <c>PedHash</c> and requests it to be loaded.
  /// </summary>
  /// <param name="pedHash">The <c>PedHash</c> representing the pedestrian model to load.</param>
  /// <returns>
  /// The requested <c>Model</c> object, which may not be loaded if the timeout was reached before it could be loaded.
  /// </returns>
  internal static Model GetPedModel(PedHash pedHash)
    => new Model(pedHash).RequestModel();

  /// <summary>
  /// Returns a <c>Model</c> object for the specified <c>VehicleHash</c> and requests it to be loaded.
  /// </summary>
  /// <param name="vehicleHash">The <c>VehicleHash</c> representing the vehicle model to load.</param>
  /// <returns>
  /// The requested <c>Model</c> object, which may not be loaded if the timeout was reached before it could be loaded.
  /// </returns>
  internal static Model GetVehicleModel(VehicleHash vehicleHash)
    => new Model(vehicleHash).RequestModel();

  /// <summary>
  /// Requests the specified model and waits for it to load if it is a CD image.
  /// This method will block until the model is loaded or the timeout is reached.
  /// </summary>
  /// <param name="model">The model to request and load.</param>
  /// <param name="timeout">The maximum time to wait for the model to load, in milliseconds.</param>
  /// <returns>
  /// The requested model, which may not be loaded if the timeout was reached before it could be loaded.
  /// </returns>
  private static Model RequestModel(this Model model, int timeout = 250)
  {
    _ = model.Request(timeout);
    if (model.IsInCdImage && model.IsValid)
    {
      while (model.IsLoaded.Equals(false))
        Script.Wait(50);
    }
    return model;
  }
}
