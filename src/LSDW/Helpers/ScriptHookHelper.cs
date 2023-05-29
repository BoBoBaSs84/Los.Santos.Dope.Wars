using GTA;

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

	/// <summary>
	/// Returns the model requested.
	/// </summary>
	/// <param name="vehicleHash">The vehicle hash parameter.</param>
	internal static Model RequestModel(VehicleHash vehicleHash)
	{
		Model model = new(vehicleHash);
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

	/// <summary>
	/// Returns the current ingame date and time.
	/// </summary>
	internal static DateTime GetCurrentDateTime()
		=> World.CurrentDate;

	/// <summary>
	/// Returns the associated player color of the current character.
	/// </summary>
	public static Color GetCharacterColor()
		=> (PedHash)Game.Player.Character.Model switch
		{
			PedHash.Franklin => Color.LimeGreen,
			PedHash.Michael => Color.SkyBlue,
			PedHash.Trevor => Color.SandyBrown,
			_ => Color.Aqua
		};
}
