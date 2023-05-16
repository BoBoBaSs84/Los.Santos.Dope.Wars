using GTA;
using GTA.Math;
using GTA.Native;

namespace LSDW.Helpers;

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
	{
		Model model = new(pedHash);
		model.Request(250);
		if (model.IsInCdImage && model.IsValid)
		{
			while (!model.IsLoaded)
				Script.Wait(50);
			return model;
		}
		model.MarkAsNoLongerNeeded();
		return model;
	}

	/// <summary>
	/// Returns the model requested.
	/// </summary>
	/// <param name="vehicleHash">The vehicle hash parameter.</param>
	internal static Model RequestModel(VehicleHash vehicleHash)
	{
		Model model = new(vehicleHash);
		model.Request(250);
		if (model.IsInCdImage && model.IsValid)
		{
			while (!model.IsLoaded)
				Script.Wait(50);
			return model;
		}
		model.MarkAsNoLongerNeeded();
		return model;
	}
	/// <summary>
	/// Returns the current ingame date and time.
	/// </summary>
	internal static DateTime GetCurrentDateTime()
		=> World.CurrentDate;
}
