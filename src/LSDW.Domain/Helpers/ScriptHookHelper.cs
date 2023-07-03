using GTA;

namespace LSDW.Domain.Helpers;

/// <summary>
/// The script hook helper class.
/// </summary>
internal static class ScriptHookHelper
{
	/// <summary>
	/// Returns the requested <see cref="Ped"/> <see cref="Model"/>.
	/// </summary>
	/// <param name="pedHash">The ped hash parameter.</param>
	internal static Model GetPedModel(PedHash pedHash)
		=> new Model(pedHash).RequestModel();

	/// <summary>
	/// Returns the requested <see cref="Vehicle"/> <see cref="Model"/>.
	/// </summary>
	/// <param name="vehicleHash">The vehicle hash parameter.</param>
	internal static Model GetVehicleModel(VehicleHash vehicleHash)
		=> new Model(vehicleHash).RequestModel();

	/// <summary>
	/// Returns the requested <see cref="Weapon"/> <see cref="Model"/>.
	/// </summary>
	/// <param name="weaponHash">The weapon hash parameter.</param>
	internal static Model GetWeaponModel(WeaponHash weaponHash)
		=> new Model(weaponHash).RequestModel();

	/// <summary>
	/// Attempts to load this <see cref="Model"/> into memory for a given period of time.
	/// </summary>
	/// <param name="model">The model to load.</param>
	/// <param name="timeout">The time in milliseconds before giving up trying to load this <see cref="Model"/>.</param>
	/// <param name="ms">The time in milliseconds to pause for.</param>
	private static Model RequestModel(this Model model, int timeout = 250, int ms = 50)
	{
		_ = model.Request(timeout);
		if (model.IsInCdImage && model.IsValid)
		{
			while (!model.IsLoaded)
				Script.Wait(ms);
			return model;
		}
		model.MarkAsNoLongerNeeded();
		return model;
	}
}
