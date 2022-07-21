using GTA;
using GTA.Native;
using GTA.UI;
using System;

namespace Los.Santos.Dope.Wars.Extension
{
	/// <summary>
	/// The <see cref="ScriptHookUtils"/> class contains mostly methods that use <see cref="Function.Call(Hash)"/>
	/// </summary>
	public static class ScriptHookUtils
	{
		/// <summary>
		/// The empty constructor
		/// </summary>
		static ScriptHookUtils()
		{
		}

		/// <summary>
		/// The <see cref="GetGameDateTime"/> method returns the current in game <see cref="DateTime"/>
		/// </summary>
		/// <returns><see cref="DateTime"/></returns>
		public static DateTime GetGameDateTime()
		{
			return new DateTime(
				Function.Call<int>(Hash.GET_CLOCK_YEAR),
				Function.Call<int>(Hash.GET_CLOCK_MONTH) + 1,
				Function.Call<int>(Hash.GET_CLOCK_DAY_OF_MONTH),
				Function.Call<int>(Hash.GET_CLOCK_HOURS),
				Function.Call<int>(Hash.GET_CLOCK_MINUTES),
				Function.Call<int>(Hash.GET_CLOCK_SECONDS)
			);
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
			Function.Call(Hash.BEGIN_TEXT_COMMAND_THEFEED_POST, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, message);
			Function.Call(Hash.END_TEXT_COMMAND_THEFEED_POST_MESSAGETEXT, texture, texture, true, icon, sender, subject);

			if (playSound)
			{
				Enums.Characters currentCharacter = Utils.GetCharacterFromModel();
				Audio.PlaySoundFrontend("Text_Arrive_Tone", $"Phone_SoundSet_{(currentCharacter.Equals(Enums.Characters.Unknown) ? "Default" : $"{currentCharacter}")}");
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
			double currentBustChance = (double)playerLevel / 2;
			double randomDouble = Constants.random.NextDouble() * 100;
			if (randomDouble <= currentBustChance)
			{
				int wantedLevel = Utils.GetWantedLevelByPlayerLevel(playerLevel);
				Game.Player.WantedLevel = wantedLevel;
				Screen.ShowSubtitle("It's a DEA bust! Get the hell out of there!", 5000);
				Function.Call(Hash.SET_PLAYER_WANTED_LEVEL, player, wantedLevel, 1);
				Function.Call(Hash.SET_PLAYER_WANTED_LEVEL_NOW, player, 0);
			}
		}
	}
}
