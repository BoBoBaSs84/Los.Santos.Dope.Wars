using GTA;
using GTA.Math;
using Los.Santos.Dope.Wars.Classes;
using Los.Santos.Dope.Wars.Extension;
using Los.Santos.Dope.Wars.GUI;
using Los.Santos.Dope.Wars.Persistence.Settings;
using Los.Santos.Dope.Wars.Persistence.State;
using System;
using System.Collections.Generic;

namespace Los.Santos.Dope.Wars.Missions
{
	/// <summary>
	/// The <see cref="DrugDealerMission"/> class is the "free roam trafficking" mission
	/// </summary>
	public class DrugDealerMission
	{
		#region fields
		private static List<DrugDealer>? _drugDealers;
		private static GameSettings? _gameSettings;
		private static GameState? _gameState;
		private static PlayerStats? _playerStats;
		private static Ped? _player;
		private static DrugDealer? _currentDrugDealer;
		#endregion

		#region properties
		/// <summary>
		/// The <see cref="Initialized"/> property indicates if the <see cref="Init(GameSettings, GameState)"/> method was called
		/// </summary>
		public static bool Initialized { get; private set; }
		#endregion

		#region constructor
		/// <summary>
		/// The empty <see cref="DrugDealerMission"/> class constructor
		/// </summary>
		static DrugDealerMission() { }
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
			_drugDealers = GetDrugDealers();
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
			foreach (DrugDealer? dealer in _drugDealers!)
			{
				dealer.DeleteBlip();
				dealer.DeletePed();
			}
			_drugDealers.Clear();
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

				// The dealer drug stash restock (quantity)
				if (ScriptHookUtils.GetGameDateTime() > _gameState!.LastDealerRestock.AddHours(_gameSettings!.Dealer.RestockIntervalHours))
				{
					_gameState.LastDealerRestock = ScriptHookUtils.GetGameDateTime();

					foreach (DrugDealer drugDealer in _drugDealers!)
					{
						drugDealer.Stash.RestockQuantity(_playerStats, _gameSettings);
						drugDealer.Stash.RefreshDrugMoney(_playerStats, _gameSettings);
						drugDealer.Stash.RefreshCurrentPrice(_playerStats, _gameSettings);
					}
					ScriptHookUtils.NotifyWithPicture("Anonymous", "Tip-off", "The drug dealers have been restocked.", 0);
					Utils.SaveGameState(_gameState);
				}
				else
				// The dealer drug stash refresh (money & prices)
				if (ScriptHookUtils.GetGameDateTime() > _gameState.LastDealerRefresh.AddHours(_gameSettings.Dealer.RefreshIntervalHours))
				{
					_gameState.LastDealerRefresh = ScriptHookUtils.GetGameDateTime();
					foreach (DrugDealer drugDealer in _drugDealers!)
					{
						drugDealer.Stash.RefreshDrugMoney(_playerStats, _gameSettings);
						drugDealer.Stash.RefreshCurrentPrice(_playerStats, _gameSettings);
					}
					Utils.SaveGameState(_gameState);
				}

				foreach (DrugDealer drugDealer in _drugDealers!)
				{
					// checking if the dealer opens up again
					if (drugDealer.NextOpenBusinesTime < ScriptHookUtils.GetGameDateTime() && drugDealer.ClosedforBusiness)
					{
						drugDealer.ClosedforBusiness = !drugDealer.ClosedforBusiness;
					}
					// creating the blips if not already created
					if (!drugDealer.BlipCreated && !drugDealer.ClosedforBusiness)
					{
						drugDealer.CreateBlip();
					}
					// if the player is in range of the dealer
					if (_player.IsInRange(drugDealer.Position, Constants.DealerCreateDistance) && drugDealer.BlipCreated)
					{
						// if the ped was not created
						if (!drugDealer.PedCreated)
						{
							drugDealer.CreatePed();

							(float health, float armor) = Utils.GetDealerHealthArmor(_gameSettings.Dealer, _playerStats.CurrentLevel);

							drugDealer.ApplyDealerSettings(
								health: health,
								armor: armor,
								money: drugDealer.Stash.DrugMoney,
								switchWeapons: _gameSettings.Dealer.CanSwitchWeapons,
								blockEvents: _gameSettings.Dealer.BlockPermanentEvents,
								dropWeapons: _gameSettings.Dealer.DropsEquippedWeaponOnDeath
								);
						}
						// now we are real close to the dealer
						if (_player.IsInRange(drugDealer.Position, Constants.DealInteractionDistance) && _currentDrugDealer is null && Game.Player.WantedLevel.Equals(0))
						{
							_currentDrugDealer = drugDealer;
							DealMenu.Init(_gameState, _playerStats.Stash, _currentDrugDealer.Stash, _player);
							if (CheckIfDealerCanTrade(_currentDrugDealer))
							{
								DealMenu.ShowDealMenu = true;
							}
							else
							{
								DealMenu.ShowDealMenu = false;
							}
						}
						// we are leaving the dealer
						if (!_player.IsInRange(drugDealer.Position, Constants.DealInteractionDistance) && _currentDrugDealer == drugDealer)
						{
							_currentDrugDealer = null;
							// dea bust chance triggered
							if (!Game.Player.WantedLevel.Equals(0))
								drugDealer.FleeFromBust();
							DealMenu.ShowDealMenu = false;
						}
					}
					// if we are leaving the dealer area, delete the ped 
					else if (drugDealer.PedCreated && !_player.IsInRange(drugDealer.Position, Constants.DealerCreateDistance))
					{
						drugDealer.DeletePed();
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error($"{nameof(DrugDealerMission)} - {ex.Message} - {ex.InnerException} - {ex.StackTrace}");
			}
		}
		#endregion

		#region private methods
		private static List<DrugDealer> GetDrugDealers()
		{
			List<DrugDealer> drugDealers = new();
			Tuple<Vector3, float>[] locations = Constants.DrugDealerSpawnLocations;
			foreach (Tuple<Vector3, float>? location in locations)
			{
				drugDealers.Add(new DrugDealer(location.Item1, location.Item2));
			}
			return drugDealers;
		}

		private static bool CheckIfDealerCanTrade(DrugDealer drugDealer)
		{
			if (drugDealer.Ped!.IsFleeing || drugDealer.Ped.IsInCombat || !drugDealer.Ped.IsAlive)
				return false;
			return true;
		}
		#endregion
	}
}
