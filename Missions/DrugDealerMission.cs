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
				if (ScriptHookUtils.GetGameDate() > _gameState!.LastDealerRestock.AddHours(_gameSettings!.Dealer.RestockIntervalHours))
				{
					_gameState.LastDealerRestock = ScriptHookUtils.GetGameDate();

					foreach (DrugDealer dealer in _drugDealers!)
					{
						dealer.Stash.RestockQuantity(_playerStats, _gameSettings);
						dealer.Stash.RefreshDrugMoney(_playerStats, _gameSettings);
						dealer.Stash.RefreshCurrentPrice(_playerStats, _gameSettings);
					}
					ScriptHookUtils.NotifyWithPicture("Anonymous", "Tip-off", "The drug dealers have been restocked.", 0);
					Utils.SaveGameState(_gameState);
				}
				else
				// The dealer drug stash refresh (money & prices)
				if (ScriptHookUtils.GetGameDate() > _gameState.LastDealerRefresh.AddHours(_gameSettings.Dealer.RefreshIntervalHours))
				{
					_gameState.LastDealerRefresh = ScriptHookUtils.GetGameDate();
					foreach (DrugDealer dealer in _drugDealers!)
					{
						dealer.Stash.RefreshDrugMoney(_playerStats, _gameSettings);
						dealer.Stash.RefreshCurrentPrice(_playerStats, _gameSettings);
					}
					Utils.SaveGameState(_gameState);
				}

				foreach (DrugDealer dealer in _drugDealers!)
				{
					// checking if the dealer opens up again
					if (dealer.NextOpenBusinesTime < ScriptHookUtils.GetGameDate() && dealer.ClosedforBusiness)
					{
						dealer.ClosedforBusiness = !dealer.ClosedforBusiness;
					}

					// creating the blips if not already created
					if (!dealer.BlipCreated && !dealer.ClosedforBusiness)
					{
						dealer.CreateBlip();
					}
					// if the player is in range of the dealer
					if (_player.IsInRange(dealer.Position, Constants.DealerCreateDistance) && dealer.BlipCreated)
					{
						// if the ped was not created
						if (!dealer.PedCreated)
						{
							(float health, float armor) = Utils.GetDealerHealthArmor(_gameSettings.Dealer, _playerStats.CurrentLevel);
							int money = dealer.Stash.DrugMoney;
							dealer.CreatePed(health, armor, money);
						}
						// now we are real close to the dealer
						if (_player.IsInRange(dealer.Position, Constants.InteractionDistance) && _currentDrugDealer is null && Game.Player.WantedLevel == 0)
						{
							_currentDrugDealer = dealer;
							DealMenu.Init(_playerStats.Stash, _player, _currentDrugDealer, _gameState);

							if (CheckIfDealerCanTrade(_currentDrugDealer))
							{
								DealMenu.ShowDealMenu = true;
							}
							else if (!CheckIfDealerCanTrade(_currentDrugDealer))
							{
								DealMenu.ShowDealMenu = false;
							}
						}
						else if ((!_player.IsInRange(dealer.Position, Constants.InteractionDistance) && _currentDrugDealer == dealer) || Game.Player.WantedLevel != 0)
						{
							_currentDrugDealer = null!;
							DealMenu.ShowDealMenu = false;
						}
					}
					// if we are leaving the dealer area, delete the ped 
					else if (dealer.PedCreated)
					{
						dealer.DeletePed();
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
			if (
					drugDealer.Ped!.IsFleeing ||
					drugDealer.Ped.IsInCombat ||
					!drugDealer.Ped.IsAlive ||
					drugDealer.Ped.IsInCombatAgainst(Game.Player.Character) ||
					drugDealer.ClosedforBusiness
					)
				return false;
			return true;
		}
		#endregion
	}
}
