using GTA;

namespace LSDW.Domain.Helpers;

/// <summary>
/// The script hook helper class.
/// </summary>
internal static class ScriptHookHelper
{
	/// <summary>
	/// Returns the model requested.
	/// </summary>
	/// <param name="pedHash">The ped hash parameter.</param>
	internal static Model RequestModel(PedHash pedHash)
		=> RequestModel((int)pedHash);

	/// <summary>
	/// Returns the model requested.
	/// </summary>
	/// <param name="vehicleHash">The vehicle hash parameter.</param>
	internal static Model RequestModel(VehicleHash vehicleHash)
		=> RequestModel((int)vehicleHash);

	/// <summary>
	/// Returns the model requested.
	/// </summary>
	/// <param name="weaponHash">The weapon hash parameter.</param>
	internal static Model RequestModel(WeaponHash weaponHash)
		=> RequestModel((int)weaponHash);

	private static Model RequestModel(int value)
	{
		Model model = new(value);
		_ = model.Request(250);
		if (model.IsInCdImage && model.IsValid)
		{
			while (!model.IsLoaded)
				Script.Wait(50);
			return model;
		}
		model.MarkAsNoLongerNeeded();
		return model;
	}
}
