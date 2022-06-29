using GTA;
using GTA.UI;
using Los.Santos.Dope.Wars.Classes;
using Los.Santos.Dope.Wars.Contracts;
using Los.Santos.Dope.Wars.Extension;
using Los.Santos.Dope.Wars.Persistence;
using System;

namespace Los.Santos.Dope.Wars.Missions
{
	/// <summary>
	/// The <see cref="Warehouse"/> class holds the creation an mission related stuff for the "special warehouse reward"
	/// </summary>
	public static class Warehouse
	{
		private static Script Script = null!;
		private static GameState GameState = null!;
		private static PlayerStats PlayerStats = null!;
		private static DrugWarehouse DrugWarehouse = null!;

		/// <summary>
		/// The <see cref="ShowWarehouseMenu"/> property
		/// </summary>
		public static bool ShowWarehouseMenu { get; set; }

		/// <summary>
		/// The empty <see cref="Warehouse"/> class constructor
		/// </summary>
		static Warehouse() { }

		/// <summary>
		/// The <see cref="OnTick(object, EventArgs)"/> method, run for every tick
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static void OnTick(object sender, EventArgs e)
		{
			if (sender is Script script)
				Script = script;

			while (GameState is null && PlayerStats is null && DrugWarehouse is null)
				Script.Wait(50);

			try
			{
				Ped? player = Game.Player.Character;

				if (PlayerStats!.CurrentLevel >= 5 && !PlayerStats.SpecialReward.Warehouse.HasFlag(Enums.WarehouseStates.Unlocked))
					PlayerStats.SpecialReward.Warehouse |= Enums.WarehouseStates.Unlocked;

				//all necessary flags are there
				if (PlayerStats.SpecialReward.Warehouse.HasFlag(Enums.WarehouseStates.Unlocked) || PlayerStats.SpecialReward.Warehouse.HasFlag(Enums.WarehouseStates.Bought) || PlayerStats.SpecialReward.Warehouse.HasFlag(Enums.WarehouseStates.Upgraded))
				{
					BlipColor blipColor = Utils.GetCharacterBlipColor(Utils.GetCharacterFromModel());
					if (!DrugWarehouse!.BlipCreated)
					{
						DrugWarehouse = new DrugWarehouse(Constants.WarehouseLocationFranklin, Constants.WarehouseEntranceFranklin, Constants.WarehouseMissionStartFranklin);
						DrugWarehouse.CreateBlip();

						if (!PlayerStats.SpecialReward.Warehouse.HasFlag(Enums.WarehouseStates.Bought))
							DrugWarehouse.ChangeBlip("Property: Drug Warehouse", BlipSprite.WarehouseForSale, blipColor);
						else
							DrugWarehouse.ChangeBlip("Drug Warehouse", BlipSprite.Warehouse, blipColor);
					}
				}

				//Warehouse exists
				if (DrugWarehouse!.BlipCreated && World.GetDistance(player.Position, Constants.WarehouseEntranceFranklin) <= 3f)
				{
					//Warehouse is not yours
					if (!PlayerStats.SpecialReward.Warehouse.HasFlag(Enums.WarehouseStates.Bought))
					{
						Screen.ShowHelpTextThisFrame($"~b~Press ~INPUT_CONTEXT~ ~w~to buy the warehouse for ~r~${Constants.DrugWarehousePrice}");
						if (Game.IsControlJustPressed(Control.Context))
						{
							Script.Wait(10);
							BuyDrugWareHouse();
						}
					}

					//Warehouse is yours
					if (PlayerStats.SpecialReward.Warehouse.HasFlag(Enums.WarehouseStates.Bought))
					{
						Screen.ShowHelpTextThisFrame($"~b~Press ~INPUT_CONTEXT~ ~w~to transfer drugs or drug money to your warehouse.");
						if (Game.IsControlJustPressed(Control.Context))
						{
							Script.Wait(10);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error($"{nameof(Warehouse)} - {ex.Message} - {ex.InnerException} - {ex.StackTrace}");
			}
		}

		/// <summary>
		/// The <see cref="Init(GameState)"/> method must be called from outside with the needed parameters
		/// </summary>
		/// <param name="gameState"></param>
		public static void Init(GameState gameState)
		{
			GameState = gameState;
			PlayerStats = Utils.GetPlayerStatsFromModel(gameState);
            DrugWarehouse = new()
            {
                DrugStash = PlayerStats.Warehouse.DrugStash
            };
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static void OnAborted(object sender, EventArgs e)
		{
			if (DrugWarehouse.BlipCreated)
				DrugWarehouse.DeleteBlip();
		}

		/// <summary>
		/// Buying the warehouse or just try it
		/// </summary>
		private static void BuyDrugWareHouse()
		{
			if ((Game.Player.Money + PlayerStats.DrugStash.Money) < Constants.DrugWarehousePrice)
			{
				Screen.ShowSubtitle($"You don't have enough money to buy the warehouse.");
				return;
			}

			Ped? player = Game.Player.Character;
			BlipColor blipColor = Utils.GetCharacterBlipColor(Utils.GetCharacterFromModel());

			int moneyToPay = Constants.DrugWarehousePrice;
			if (PlayerStats.DrugStash.Money > 0)
			{
				int moneyToRemoveFromStash = 0;
				if (PlayerStats.DrugStash.Money >= Constants.DrugWarehousePrice)
				{
					moneyToRemoveFromStash += Constants.DrugWarehousePrice;
					PlayerStats.DrugStash.Money -= moneyToRemoveFromStash;
				}
				else
				{
					moneyToPay -= PlayerStats.DrugStash.Money;
					moneyToRemoveFromStash = Constants.DrugWarehousePrice - moneyToPay;
					PlayerStats.DrugStash.Money -= moneyToRemoveFromStash;
				}
				Notification.Show($"~r~${moneyToRemoveFromStash} removed from stash and used as payment.");
			}
			player.Money -= moneyToPay;
			PlayerStats.SpecialReward.Warehouse |= Enums.WarehouseStates.Bought;
			Screen.ShowSubtitle("You bought a warehouse, use it to keep your drugs and drug money safe.");
			Screen.ShowSubtitle("But beware! Other ~r~shady ~w~individuals might be interested in it.");
			Audio.PlaySoundFrontend("PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET");
			DrugWarehouse.ChangeBlip("Drug Warehouse", BlipSprite.Warehouse, blipColor);
			Utils.SaveGameState(GameState);
		}
	}
}
