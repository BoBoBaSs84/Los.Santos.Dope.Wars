using GTA.UI;
using Los.Santos.Dope.Wars.Extension;
using Los.Santos.Dope.Wars.Persistence;
using System;
using System.ComponentModel;

namespace Los.Santos.Dope.Wars.Features
{
	/// <summary>
	/// The <see cref="RewardSystem"/> class servers as the handler for rewarding the currently played character throughout the game progress
	/// </summary>
	public static class RewardSystem
	{
		private static GameState? _gameState;
		private static PlayerStats? _playerStats;

		/// <summary>
		/// The <see cref="Initialized"/> property indicates if the <see cref="Init(GameState)"/> method was called
		/// </summary>
		public static bool Initialized { get; private set; }

		/// <summary>
		/// Standard constructor for the <see cref="RewardSystem"/> class
		/// </summary>
		static RewardSystem() { }

		/// <summary>
		/// The <see cref="OnTick(object, EventArgs)"/> method, run for every tick
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static void OnTick(object sender, EventArgs e)
		{
			if (!Initialized)
				return;

			if (_playerStats != Utils.GetPlayerStatsFromModel(_gameState!))
			{
				_playerStats!.PropertyChanged -= OnCurrentLevelPropertyChanged;
				_playerStats = Utils.GetPlayerStatsFromModel(_gameState!);
				_playerStats.PropertyChanged += OnCurrentLevelPropertyChanged;
			}
		}

		/// <summary>
		/// The <see cref="Init(GameState)"/> method must be called from outside with the needed parameters
		/// </summary>
		/// <param name="gameState"></param>
		public static void Init(GameState gameState)
		{
			_gameState = gameState;
			_playerStats = Utils.GetPlayerStatsFromModel(gameState);
			_playerStats.PropertyChanged += OnCurrentLevelPropertyChanged;
			Initialized = true;
		}

		/// <summary>
		/// The <see cref="OnCurrentLevelPropertyChanged(object, PropertyChangedEventArgs)"/> method is where the magic happens
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void OnCurrentLevelPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (sender is PlayerStats playerStats)
			{
				// this is what happens for every level up
				Notification.Show($"Congratulations you have reached level ~y~{playerStats.CurrentLevel}~w~. Your bag size has been increased to ~y~{playerStats.MaxBagSize}~w~.");

				// this is when the warehouse reward has been granted
				if (playerStats.CurrentLevel.Equals(5))
				{
					playerStats.SpecialReward.Warehouse |= Enums.WarehouseStates.Unlocked;
					Notification.Show($"Congratulations you have unlocked the ~y~warehouse~w~. Buy it to store drugs and drug money safely.");
					// if we have come this far, save
					Utils.SaveGameState(_gameState!);
				}
			}
		}
	}
}