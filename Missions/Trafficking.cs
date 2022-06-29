using GTA;
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
		private static List<DrugDealer> DrugDealers = null!;
		private static GameSettings GameSettings = null!;
		private static GameState GameState = null!;
		private static Script Script = null!;
		private static PlayerStats PlayerStats = null!;

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
			GameSettings = gameSettings;
			GameState = gameState;
			DrugDealers = GetDrugDealers();
		}

		/// <summary>
		/// The <see cref="OnAborted(object, EventArgs)"/> method
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static void OnAborted(object sender, EventArgs e)
		{
			foreach (DrugDealer? dealer in DrugDealers)
			{
				dealer.DeleteBlip();
				dealer.DeletePed();
			}
			DrugDealers.Clear();
		}

		/// <summary>
		/// The <see cref="OnTick(object, EventArgs)"/> method, run for every tick
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static void OnTick(object sender, EventArgs e)
		{
			if (sender is Script script)
				Script = script;

			while (GameState is null && GameState is null)
				return;

			try
			{
				Ped player = Game.Player.Character;
				PlayerStats = Utils.GetPlayerStatsFromModel(GameState);

				// The dealer drug stash restock (quantity)
				if (ScriptHookUtils.GetGameDate() > GameState.LastRestock.AddHours(GameSettings.DealerSettings.RestockIntervalHours))
				{
					GameState.LastRestock = ScriptHookUtils.GetGameDate();

					foreach (DrugDealer dealer in DrugDealers)
					{
						dealer.DrugStash.Init();
						dealer.DrugStash.RestockQuantity(PlayerStats, GameSettings);
						dealer.DrugStash.RefreshDrugMoney(PlayerStats, GameSettings);
						dealer.DrugStash.RefreshCurrentPrice(PlayerStats, GameSettings);
					}
					ScriptHookUtils.NotifyWithPicture("Anonymous", "Tip-off", "The drug dealers have been restocked.", 0);
					Utils.SaveGameState(GameState);
				}
				else
				// The dealer drug stash refresh (money & prices)
				if (ScriptHookUtils.GetGameDate() > GameState.LastRefresh.AddHours(GameSettings.DealerSettings.RefreshIntervalHours))
				{
					GameState.LastRefresh = ScriptHookUtils.GetGameDate();
					foreach (DrugDealer dealer in DrugDealers)
					{
						dealer.DrugStash.RefreshDrugMoney(PlayerStats, GameSettings);
						dealer.DrugStash.RefreshCurrentPrice(PlayerStats, GameSettings);
					}
					Utils.SaveGameState(GameState);
				}

				foreach (DrugDealer dealer in DrugDealers)
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
							(float health, float armor) = Utils.GetDealerHealthArmor(GameSettings.DealerSettings, PlayerStats.CurrentLevel);
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
						DealMenu.Init(PlayerStats.DrugStash, (DrugStash)dealer.DrugStash, GameState);

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
