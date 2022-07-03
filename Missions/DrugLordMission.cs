using GTA;
using Los.Santos.Dope.Wars.Classes;
using Los.Santos.Dope.Wars.Extension;
using Los.Santos.Dope.Wars.Persistence;
using System;
using System.Collections.Generic;

namespace Los.Santos.Dope.Wars.Missions
{
	/// <summary>
	/// The <see cref="DrugLordMission"/> class is the "drug lords trafficking" mission
	/// </summary>
	public static class DrugLordMission
	{
		private static List<DrugLord>? _drugLords;
		private static DrugLord? _drugLord;
		private static GameSettings? _gameSettings;
		private static GameState? _gameState;
		private static PlayerStats? _playerStats;
		private static Ped? _player;		

		/// <summary>
		/// The <see cref="Initialized"/> property indicates if the <see cref="Init(GameSettings, GameState)"/> method was called
		/// </summary>
		public static bool Initialized { get; private set; }

		/// <summary>
		/// The empty <see cref="DrugLordMission"/> class constructor
		/// </summary>
		static DrugLordMission() { }

		/// <summary>
		/// The <see cref="Init(GameSettings, GameState)"/> must be called from outside with the needed parameters
		/// </summary>
		/// <param name="gameSettings"></param>
		/// <param name="gameState"></param>
		public static void Init(GameSettings gameSettings, GameState gameState)
		{
			_gameSettings = gameSettings;
			_gameState = gameState;
			_drugLords = GetDrugLords();
			_player = Game.Player.Character;
			Initialized = true;
		}

		/// <summary>
		/// The <see cref="OnAborted(object, EventArgs)"/> method
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static void OnAborted(object sender, EventArgs e)
		{
			foreach (DrugLord? dealer in _drugLords!)
			{
				dealer.DeleteBlip();
				dealer.DeletePed();
			}
			_drugLords.Clear();
		}

		/// <summary>
		/// The <see cref="OnTick(object, EventArgs)"/> method, run for every tick
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static void OnTick(object sender, EventArgs e)
		{
			if (!Initialized)
				return;

			try
			{
				if (_player != Game.Player.Character)
					_player = Game.Player.Character;

				if (_playerStats != Utils.GetPlayerStatsFromModel(_gameState!))
					_playerStats = Utils.GetPlayerStatsFromModel(_gameState!);

				// if the reward is not unlocked, early exit
				if (!_playerStats.SpecialReward.DrugLords.HasFlag(Enums.DrugLordStates.Unlocked))
					return;
			}
			catch (Exception ex)
			{
				Logger.Error($"{nameof(DrugLordMission)} - {ex.Message} - {ex.InnerException} - {ex.StackTrace}");
			}
		}

		private static List<DrugLord>? GetDrugLords()
		{
			return _drugLords ??= new List<DrugLord>();
		}
	}
}
