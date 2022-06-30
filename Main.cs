using GTA;
using Los.Santos.Dope.Wars.Extension;
using Los.Santos.Dope.Wars.Features;
using Los.Santos.Dope.Wars.Missions;
using Los.Santos.Dope.Wars.Persistence;
using System;

namespace Los.Santos.Dope.Wars
{
	/// <summary>
	/// The <see cref="Main"/> class is the main entry point for the script, inherits from <see cref="Script"/>
	/// </summary>
	public class Main : Script
	{
		private static bool _settingsLoaded;
		private static bool _gameStateLoaded;
		private static bool _traffickingLoaded;
		private static bool _warehouseLoaded;
		private static bool _rewardSystemLoaded;
		private static GameSettings? GameSettings;
		private static GameState? GameState;

		/// <summary>
		/// The <see cref="ScriptDirectory"/> property, this is the main directory for logging and saving the game config and the game state
		/// </summary>
		public static string ScriptDirectory { get; private set; } = string.Empty;

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
			Tick += Trafficking.OnTick;
			Tick += Warehouse.OnTick;
			Tick += RewardSystem.OnTick;

			Aborted += OnAborted;
			Aborted += Trafficking.OnAborted;
			Aborted += Warehouse.OnAborted;			
		}
		#endregion

		private void OnAborted(object sender, EventArgs e)
		{
		}

		private void OnTick(object sender, EventArgs e)
		{
			while (Game.IsLoading && !Game.Player.CanControlCharacter)
				Wait(50);

			if (!_traffickingLoaded)
			{
				Trafficking.Init(GameSettings!, GameState!);
				_traffickingLoaded = Trafficking.Initialized;
				Logger.Status($"{nameof(Trafficking)} loaded: {_traffickingLoaded}");
			}

			if (!_warehouseLoaded)
			{
				Warehouse.Init(GameState!);
				_warehouseLoaded = Warehouse.Initialized;
				Logger.Status($"{nameof(Warehouse)} loaded: {_traffickingLoaded}");
			}

			if (!_rewardSystemLoaded)
			{
				RewardSystem.Init(GameState!);
				_rewardSystemLoaded = RewardSystem.Initialized;
				Logger.Status($"{nameof(RewardSystem)} loaded: {_traffickingLoaded}");
			}
		}

		private static void Init()
		{
			try
			{
				if (!_settingsLoaded || !_gameStateLoaded)
				{
					Logger.Status($"Game: {Constants.AssemblyName} - Vesion: {Constants.AssemblyVersion}");

					if (!_settingsLoaded)
					{
						(bool successs, GameSettings loadedGameSettings) = Utils.LoadGameSettings();
						if (successs)
						{
							GameSettings = loadedGameSettings;
							Logger.Status($"Settings loaded. Version: {GameSettings.Version}");
							_settingsLoaded = successs;
						}
					}
					if (!_gameStateLoaded)
					{
						(bool success, GameState loadedGameState) = Utils.LoadGameState();
						if (success)
						{
							GameState = loadedGameState;
							loadedGameState.LastRestock = ScriptHookUtils.GetGameDate().AddHours(-25);
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
	}
}