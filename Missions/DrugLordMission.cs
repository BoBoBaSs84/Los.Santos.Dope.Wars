using GTA;
using GTA.Math;
using Los.Santos.Dope.Wars.Classes;
using Los.Santos.Dope.Wars.Extension;
using Los.Santos.Dope.Wars.Persistence.Settings;
using Los.Santos.Dope.Wars.Persistence.State;
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
		private static DateTime LastAppearance;
		private static DateTime NextCheckForAppearance;
		private static bool ShouldAppear;
		private static bool IsAppeared;

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
			foreach (DrugLord? drugLord in _drugLords!)
			{
				foreach (Bodyguard? bodyguard in drugLord.Bodyguards)
				{
					bodyguard.DeleteBlip();
					bodyguard.DeletePed();
				}
				drugLord.DeleteBlip();
				drugLord.DeletePed();
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

				// if the reward is not yet unlocked, early exit
				if (!_playerStats.Reward.DrugLords.HasFlag(Enums.DrugLordStates.Unlocked))
					return;

				if (ScriptHookUtils.GetGameDateTime() > NextCheckForAppearance)
					CheckForAppearance();

				// drug lord appeared 12 hours ago...
				if (ScriptHookUtils.GetGameDateTime() > LastAppearance.AddHours(12) && IsAppeared && _drugLord is not null)
					CheckForDisappearance();

				if (_drugLord is null && ShouldAppear)
					SummonDrugLord();

			}
			catch (Exception ex)
			{
				Logger.Error($"{nameof(DrugLordMission)} - {ex.Message} - {ex.InnerException} - {ex.StackTrace}");
			}
		}

		private static List<DrugLord>? GetDrugLords()
		{
			List<DrugLord> drugLords = new();
			Tuple<Vector3, float>[] locations = Constants.DrugDealerSpawnLocations;
			foreach (Tuple<Vector3, float> location in locations)
				drugLords.Add(new DrugLord(location.Item1, location.Item2));
			return drugLords;
		}

		private static void CheckForAppearance()
		{
			LastAppearance = ScriptHookUtils.GetGameDateTime();
			NextCheckForAppearance = LastAppearance.AddDays(24);
			double chanceForAppearance = 30;
			double randomDouble = Constants.random.NextDouble() * 100;
			if (randomDouble <= chanceForAppearance)
				ShouldAppear = true;
		}

		private static void CheckForDisappearance()
		{
			foreach (Bodyguard? bodyguard in _drugLord!.Bodyguards)
			{
				bodyguard.DeleteBlip();
				bodyguard.DeletePed();
			}

			_drugLord.DeleteBlip();
			_drugLord.DeletePed();
			_drugLord = null!;

			ShouldAppear = !ShouldAppear;
			IsAppeared = !IsAppeared;
		}

		private static void SummonDrugLord()
		{
			if (_gameSettings is null && _playerStats is null)
				return;

			int randomPick = Utils.GetRandomInt(0, _drugLords!.Count);
			_drugLord = _drugLords[randomPick];

			if (!_drugLord.BlipCreated)
				_drugLord.CreateBlip("Drug Lord", true, false);

			_drugLord.Stash.RestockQuantity(_playerStats, _gameSettings, true);
			_drugLord.Stash.RefreshDrugMoney(_playerStats, _gameSettings, true);
			_drugLord.Stash.RefreshCurrentPrice(_playerStats, _gameSettings, true);

			(float health, float armor) = Utils.GetDealerHealthArmor(_gameSettings.Dealer, _playerStats.CurrentLevel);

			if (!_drugLord.PedCreated)
				_drugLord.CreatePed(
					health: health,
					armor: armor,
					money: _drugLord.Stash.DrugMoney,
					switchWeapons: _gameSettings.Dealer.CanSwitchWeapons,
					blockEvents: _gameSettings.Dealer.BlockPermanentEvents,
					dropWeapons: _gameSettings.Dealer.DropsEquippedWeaponOnDeath
					);

			foreach (Bodyguard bodyguard in _drugLord.Bodyguards)
				bodyguard.CreatePed(_drugLord.Ped!);

			IsAppeared = true;
		}
	}
}