﻿using GTA;
using Los.Santos.Dope.Wars.Extension;
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
		private static GameSettings GameSettings = null!;
		private static GameState GameState = null!;

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

			Tick += OnTick;
			Tick += Trafficking.OnTick;
			Tick += Warehouse.OnTick;

			Aborted += OnAborted;
			Aborted += Trafficking.OnAborted;
			Aborted += Warehouse.OnAborted;

			Init();
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
				Trafficking.Init(GameSettings, GameState);
				_traffickingLoaded = true;
				Logger.Status($"Trafficking loaded: {_traffickingLoaded}");
			}

			if (!_warehouseLoaded)
			{
				Warehouse.Init(GameState);
				_warehouseLoaded = true;
				Logger.Status($"Warehouse loaded: {_traffickingLoaded}");
			}
		}

		private static void Init()
		{
			try
			{
				if (!_settingsLoaded || !_gameStateLoaded)
				{
					Logger.Status($"Mod: {Constants.AssemblyName} - Vesion: {Constants.AssemblyVersion}");

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
							Logger.Status($"Last game state loaded. Version: {GameSettings.Version}");
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