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
		#region fields
		private static List<DrugLord>? _drugLords;
		private static DrugLord? _drugLord;
		private static GameSettings? _gameSettings;
		private static GameState? _gameState;
		private static PlayerStats? _playerStats;
		private static Ped? _player;
		private static DateTime LastCheckForAppearance;
		private static DateTime NextCheckForAppearance;
		private static bool ShouldAppear;
		private static bool IsAppeared;
		#endregion

		#region properties
		/// <summary>
		/// The <see cref="Initialized"/> property indicates if the <see cref="Init(GameSettings, GameState)"/> method was called
		/// </summary>
		public static bool Initialized { get; private set; }
		#endregion

		#region constructor
		/// <summary>
		/// The empty <see cref="DrugLordMission"/> class constructor
		/// </summary>
		static DrugLordMission() { }
		#endregion

		#region public methods
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

				if (ScriptHookUtils.GetGameDateTime() >= NextCheckForAppearance)
					CheckForAppearance();

				// drug lord appeared 12 hours ago...
				if (ScriptHookUtils.GetGameDateTime() >= LastCheckForAppearance.AddHours(12) && IsAppeared && _drugLord is not null)
					CheckForDisappearance();

				if (_drugLord is null && ShouldAppear)
					SummonDrugLord();

			}
			catch (Exception ex)
			{
				Logger.Error($"{nameof(DrugLordMission)} - {ex.Message} - {ex.InnerException} - {ex.StackTrace}");
			}
		}
		#endregion

		#region private methods
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
			LastCheckForAppearance = ScriptHookUtils.GetGameDateTime();
			NextCheckForAppearance = LastCheckForAppearance.AddDays(24);
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
			if (_gameSettings is not null && _playerStats is not null)
			{
				int randomPick = Utils.GetRandomInt(0, _drugLords!.Count);
				_drugLord = _drugLords[randomPick];

				if (!_drugLord.BlipCreated)
					_drugLord.CreateBlip(
						blipSprite: BlipSprite.DrugPackage,
						blipName: "Drug Lord",
						isShortRange: false
						);

				if (!_drugLord.PedCreated)
					_drugLord.CreatePed(
						pedHash: Utils.GetRandomPedHash(Enums.PedType.DrugLord),
						weaponHash: Utils.GetRandomWeaponHash(Enums.PedType.DrugLord)
						);

				_drugLord.Restock(_gameSettings, _playerStats);

				foreach (Bodyguard bodyguard in _drugLord.Bodyguards)
				{
					var pedHash = Utils.GetRandomPedHash(Enums.PedType.Bodyguard);
					var weaponHash = Utils.GetRandomWeaponHash(Enums.PedType.Bodyguard);
					bodyguard.CreatePed(pedHash, weaponHash, _drugLord.Ped!);
				}
				IsAppeared = true;
			}
		}
		#endregion
	}
}