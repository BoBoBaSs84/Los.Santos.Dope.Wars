﻿using GTA;
using GTA.Math;
using Los.Santos.Dope.Wars.Classes;
using Los.Santos.Dope.Wars.Extension;
using Los.Santos.Dope.Wars.GUI;
using Los.Santos.Dope.Wars.Persistence;
using System;
using System.Collections.Generic;

namespace Los.Santos.Dope.Wars.Missions
{
	/// <summary>
	/// The <see cref="Trafficking"/> class is the "free roam trafficking" mission
	/// </summary>
	public class Trafficking
	{
		private static List<DrugDealer>? _drugDealers;
		private static GameSettings? _gameSettings;
		private static GameState? _gameState;
		private static Script? _script;
		private static PlayerStats? _playerStats;

		private static DrugDealer? CurrentDrugDealer { get; set; }

		/// <summary>
		/// The empty <see cref="Trafficking"/> class constructor
		/// </summary>
		static Trafficking() { }

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
			if (sender is Script script)
				_script = script;

			while (_gameSettings is null && _gameState is null)
				return;

			try
			{
				Ped player = Game.Player.Character;
				_playerStats = Utils.GetPlayerStatsFromModel(_gameState!);

				// The dealer drug stash restock (quantity)
				if (ScriptHookUtils.GetGameDate() > _gameState!.LastRestock.AddHours(_gameSettings!.DealerSettings.RestockIntervalHours))
				{
					_gameState.LastRestock = ScriptHookUtils.GetGameDate();

					foreach (DrugDealer dealer in _drugDealers!)
					{
						dealer.DrugStash.Init();
						dealer.DrugStash.RestockQuantity(_playerStats, _gameSettings);
						dealer.DrugStash.RefreshDrugMoney(_playerStats, _gameSettings);
						dealer.DrugStash.RefreshCurrentPrice(_playerStats, _gameSettings);
					}
					ScriptHookUtils.NotifyWithPicture("Anonymous", "Tip-off", "The drug dealers have been restocked.", 0);
					Utils.SaveGameState(_gameState);
				}
				else
				// The dealer drug stash refresh (money & prices)
				if (ScriptHookUtils.GetGameDate() > _gameState.LastRefresh.AddHours(_gameSettings.DealerSettings.RefreshIntervalHours))
				{
					_gameState.LastRefresh = ScriptHookUtils.GetGameDate();
					foreach (DrugDealer dealer in _drugDealers!)
					{
						dealer.DrugStash.RefreshDrugMoney(_playerStats, _gameSettings);
						dealer.DrugStash.RefreshCurrentPrice(_playerStats, _gameSettings);
					}
					Utils.SaveGameState(_gameState);
				}

				foreach (DrugDealer dealer in _drugDealers!)
				{
					// creating the blips if not already created
					if (!dealer.BlipCreated)
					{
						dealer.CreateBlip();
					}
					// if the player is in range of the dealer
					if (player.IsInRange(dealer.Position, 100f))
					{
						// if the ped was not created
						if (!dealer.PedCreated)
						{
							(float health, float armor) = Utils.GetDealerHealthArmor(_gameSettings.DealerSettings, _playerStats.CurrentLevel);
							int money = dealer.DrugStash.Money;
							dealer.CreatePed(health, armor, money);
						}
					}
					// if we are leaving the dealer area, delete the ped 
					else if (dealer.PedCreated)
					{
						dealer.Ped!.Delete();
						dealer.PedCreated = false;
					}

					// now we are real close to the dealer
					if (player.IsInRange(dealer.Position, 3f) && CurrentDrugDealer is null && Game.Player.WantedLevel == 0)
					{
						CurrentDrugDealer = dealer;
						DealMenu.Init(_playerStats.DrugStash, (DrugStash)dealer.DrugStash, _gameState);

						if (CheckIfDealerCanTrade(dealer))
							DealMenu.ShowDealMenu = true;

						else if (!CheckIfDealerCanTrade(dealer))
							DealMenu.ShowDealMenu = false;
					}

					else if ((!player.IsInRange(dealer.Position, 3f) && CurrentDrugDealer == dealer) || Game.Player.WantedLevel != 0)
					{
						CurrentDrugDealer = null!;
						DealMenu.ShowDealMenu = false;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error($"{nameof(Trafficking)} - {ex.Message} - {ex.InnerException} - {ex.StackTrace}");
			}
		}

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
	}
}
