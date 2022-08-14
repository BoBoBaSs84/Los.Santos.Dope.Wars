using GTA;
using GTA.Math;
using GTA.Native;
using static Los.Santos.Dope.Wars.Extension.Utils;

namespace Los.Santos.Dope.Wars.Extension;

/// <summary>
/// The <see cref="ScriptHookUtils"/> class contains mostly methods that use <see cref="Function.Call(Hash)"/>
/// </summary>
public static class ScriptHookUtils
{
	/// <summary>
	/// The <see cref="FlashMinimapDisplay"/> methods lets the small minimap on the screen flash (once)
	/// </summary>
	public static void FlashMinimapDisplay()
	{
		Logger.Trace(" called");
		Function.Call(Hash.FLASH_MINIMAP_DISPLAY);
	}

	/// <summary>
	/// The <see cref="RequestModel(PedHash)"/> method returns the model requested by the ped hash parameter
	/// </summary>
	/// <param name="pedHash"></param>
	/// <returns><see cref="Model"/></returns>
	public static Model RequestModel(PedHash pedHash)
	{
		Logger.Trace(" called");
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
	/// The <see cref="RequestModel(VehicleHash)"/> method returns the model requested by the vehicle hash parameter
	/// </summary>
	/// <param name="vehicleHash"></param>
	/// <returns><see cref="Model"/></returns>
	public static Model RequestModel(VehicleHash vehicleHash)
	{
		Logger.Trace(" called");
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
	/// The <see cref="GetCurrentDateTime"/> method returns the current ingame date and time
	/// </summary>
	/// <returns><see cref="DateTime"/></returns>
	public static DateTime GetCurrentDateTime()
	{
		Logger.Trace(" called");
		return World.CurrentDate;
	}

	/// <summary>
	/// The <see cref="GetCurrentPlayerPosition"/> method returns the current player position
	/// </summary>
	/// <returns><see cref="Vector3"/></returns>
	public static Vector3 GetCurrentPlayerPosition()
	{
		Logger.Trace(" called");
		return Game.Player.Character.Position;
	}

	/// <summary>
	/// Sends a message to the player phone
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="subject"></param>
	/// <param name="message"></param>
	/// <param name="icon"></param>
	/// <param name="texture"></param>
	/// <param name="playSound"></param>
	public static void NotifyWithPicture(string sender, string subject, string message, int icon, string texture = "CHAR_DEFAULT", bool playSound = true)
	{
		Logger.Trace(" called");
		Function.Call(Hash.BEGIN_TEXT_COMMAND_THEFEED_POST, "STRING");
		Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, message);
		Function.Call(Hash.END_TEXT_COMMAND_THEFEED_POST_MESSAGETEXT, texture, texture, true, icon, sender, subject);

		if (playSound)
		{
			Enums.Character currentCharacter = GetCharacterFromModel();
			Audio.PlaySoundFrontend("Text_Arrive_Tone", $"Phone_SoundSet_{(currentCharacter.Equals(Enums.Character.Unknown) ? "Default" : $"{currentCharacter}")}");
		}
	}

	/// <summary>
	/// The <see cref="DrugEnforcementAdministrationBust(Ped, int)"/> method calculates if a bust should be started and initiates everything necessary for it.
	/// The current logic is, current player level divided by two. Lvl1 -> 0.5%, Lvl10 -> 5%, Lvl50 -> 25%
	/// </summary>
	/// <param name="player"></param>
	/// <param name="playerLevel"></param>
	public static void DrugEnforcementAdministrationBust(Ped player, int playerLevel)
	{
		Logger.Trace(" called");
		double currentBustChance = (double)playerLevel / 2;
		double randomDouble = GetRandomDouble() * 100;
		if (randomDouble <= currentBustChance)
		{
			int wantedLevel = GetWantedLevelByPlayerLevel(playerLevel);
			Game.Player.WantedLevel = wantedLevel;
			GTA.UI.Screen.ShowSubtitle("It's a DEA bust! Get the hell out of there!", 5000);
			Function.Call(Hash.SET_PLAYER_WANTED_LEVEL, player, wantedLevel, 1);
			Function.Call(Hash.SET_PLAYER_WANTED_LEVEL_NOW, player, 0);
		}
	}
}
