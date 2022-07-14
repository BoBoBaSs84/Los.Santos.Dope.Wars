using GTA;
using Los.Santos.Dope.Wars.Extension;
using Los.Santos.Dope.Wars.Features;
using Los.Santos.Dope.Wars.Missions;
using Los.Santos.Dope.Wars.Persistence.Settings;
using Los.Santos.Dope.Wars.Persistence.State;
using System;

namespace Los.Santos.Dope.Wars
{
	/// <summary>
	/// The <see cref="Main"/> class is the main entry point for the script, inherits from <see cref="Script"/>
	/// </summary>
	public class Main : Script
	{
		#region fields
		private static bool _gameSettingsLoaded;
		private static bool _gameStateLoaded;
		private static bool _drugDealerMissionLoaded;
		private static bool _drugLordMissionLoaded;
		private static bool _warehouseMissionLoaded;
		private static bool _rewardSystemLoaded;
		private static GameSettings? GameSettings;
		private static GameState? GameState;
		#endregion

		#region properties
		/// <summary>
		/// The <see cref="ScriptDirectory"/> property, this is the main directory for logging and saving the game config and the game state
		/// </summary>
		public static string ScriptDirectory { get; private set; } = string.Empty;
		#endregion

		#region ctor
		/// <summary>
		/// Empty constructor for <see cref="Main"/>
		/// </summary>
		public Main()
		{
			Interval = 10;
			ScriptDirectory = BaseDirectory;

			Init();

			Tick += OnTick;
			Aborted += OnAborted;
		}
		#endregion

		#region private methods
		private void OnAborted(object sender, EventArgs e)
		{
			_drugDealerMissionLoaded = _drugLordMissionLoaded = _rewardSystemLoaded = _warehouseMissionLoaded = false;
		}

		private void OnTick(object sender, EventArgs e)
		{
			while (Game.IsLoading && !Game.Player.CanControlCharacter && !_gameSettingsLoaded && !_gameStateLoaded)
				Wait(50);

			if (!_drugDealerMissionLoaded)
			{
				DrugDealerMission.Init(GameSettings!, GameState!);
				_drugDealerMissionLoaded = DrugDealerMission.Initialized;
				Tick += DrugDealerMission.OnTick;
				Aborted += DrugDealerMission.OnAborted;
				Logger.Status($"{nameof(DrugDealerMission)} loaded: {_drugDealerMissionLoaded}");
			}

			if (!_drugLordMissionLoaded)
			{
				DrugLordMission.Init(GameSettings!, GameState!);
				_drugLordMissionLoaded = DrugLordMission.Initialized;
				Tick += DrugLordMission.OnTick;
				Aborted += DrugLordMission.OnAborted;
				Logger.Status($"{nameof(DrugLordMission)} loaded: {_drugLordMissionLoaded}");
			}

			if (!_warehouseMissionLoaded)
			{
				WarehouseMission.Init(GameSettings!, GameState!);
				_warehouseMissionLoaded = WarehouseMission.Initialized;
				Tick += WarehouseMission.OnTick;
				Aborted += WarehouseMission.OnAborted;
				Logger.Status($"{nameof(WarehouseMission)} loaded: {_drugDealerMissionLoaded}");
			}

			if (!_rewardSystemLoaded)
			{
				RewardSystem.Init(GameState!);
				_rewardSystemLoaded = RewardSystem.Initialized;
				Tick += RewardSystem.OnTick;
				Logger.Status($"{nameof(RewardSystem)} loaded: {_drugDealerMissionLoaded}");
			}
		}

		private static void Init()
		{
			try
			{
				if (!_gameSettingsLoaded || !_gameStateLoaded)
				{
					Logger.Status($"Game: {Constants.AssemblyName} - Vesion: {Constants.AssemblyVersion}");

					if (!_gameSettingsLoaded)
					{
						(bool successs, GameSettings loadedGameSettings) = Utils.LoadGameSettings();
						if (successs)
						{
							GameSettings = loadedGameSettings;
							Logger.Status($"Settings loaded. Version: {GameSettings.Version}");
							_gameSettingsLoaded = successs;
						}
					}
					if (!_gameStateLoaded)
					{
						(bool success, GameState loadedGameState) = Utils.LoadGameState();
						if (success)
						{
							GameState = loadedGameState;
							loadedGameState.LastDealerRestock = ScriptHookUtils.GetGameDate().AddHours(-25);
							Logger.Status($"Last game state loaded. Version: {GameSettings!.Version}");
							_gameStateLoaded = success;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error($"{ex.Message} - {ex.InnerException} - {ex.StackTrace}");
			}
		}
		#endregion
	}
}