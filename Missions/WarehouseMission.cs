using GTA;
using GTA.UI;
using Los.Santos.Dope.Wars.Classes;
using Los.Santos.Dope.Wars.Extension;
using Los.Santos.Dope.Wars.Persistence;
using System;

namespace Los.Santos.Dope.Wars.Missions
{
	/// <summary>
	/// The <see cref="WarehouseMission"/> class holds the creation an mission related stuff for the "special warehouse reward"
	/// </summary>
	public static class WarehouseMission
	{
		private static GameSettings? _gameSettings;
		private static GameState? _gameState;
		private static PlayerStats? _playerStats;
		private static Warehouse? _warehouse;
		private static int _warehousePrice;

		/// <summary>
		/// The <see cref="Initialized"/> property indicates if the <see cref="Init(GameSettings, GameState)"/> method was called
		/// </summary>
		public static bool Initialized { get; private set; }

		/// <summary>
		/// The <see cref="ShowWarehouseMenu"/> property
		/// </summary>
		public static bool ShowWarehouseMenu { get; set; }

		/// <summary>
		/// The empty <see cref="WarehouseMission"/> class constructor
		/// </summary>
		static WarehouseMission() { }

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
				_playerStats = Utils.GetPlayerStatsFromModel(_gameState!);

			try
			{
				Ped? player = Game.Player.Character;

				//all necessary flags are there
				if (_playerStats!.SpecialReward.Warehouse.HasFlag(Enums.WarehouseStates.Unlocked) || _playerStats.SpecialReward.Warehouse.HasFlag(Enums.WarehouseStates.Bought) || _playerStats.SpecialReward.Warehouse.HasFlag(Enums.WarehouseStates.Upgraded))
				{
					if (!_warehouse!.BlipCreated)
					{
						_warehouse = new Warehouse(Constants.WarehouseLocationFranklin, Constants.WarehouseEntranceFranklin, Constants.WarehouseMissionStartFranklin);
						_warehouse.CreateBlip(BlipSprite.WarehouseForSale);

						if (_playerStats.SpecialReward.Warehouse.HasFlag(Enums.WarehouseStates.Bought))
						{
							BlipColor blipColor = Utils.GetCharacterBlipColor(Utils.GetCharacterFromModel());
							_warehouse.ChangeBlip(BlipSprite.Warehouse, blipColor);
							_warehouse.Stash = _playerStats.Warehouse.Stash;
						}
					}
				}

				//Warehouse exists
				if (_warehouse!.BlipCreated && World.GetDistance(player.Position, Constants.WarehouseEntranceFranklin) <= 3f)
				{
					//Warehouse is not yours
					if (!_playerStats.SpecialReward.Warehouse.HasFlag(Enums.WarehouseStates.Bought))
					{
						Screen.ShowHelpTextThisFrame($"~b~Press ~INPUT_CONTEXT~ ~w~to buy the warehouse for ~r~${_warehousePrice}");
						if (Game.IsControlJustPressed(Control.Context))
						{
							Script.Wait(10);
							BuyWareHouse();
						}
					}

					//Warehouse is yours
					if (_playerStats.SpecialReward.Warehouse.HasFlag(Enums.WarehouseStates.Bought))
					{
						Screen.ShowHelpTextThisFrame($"~b~Press ~INPUT_CONTEXT~ ~w~to transfer drugs to or from your warehouse.");
						if (Game.IsControlJustPressed(Control.Context))
						{
							Script.Wait(10);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error($"{nameof(WarehouseMission)} - {ex.Message} - {ex.InnerException} - {ex.StackTrace}");
			}
		}

		/// <summary>
		/// The <see cref="Init(GameSettings, GameState)"/> method must be called from outside with the needed parameters
		/// </summary>
		/// <param name="gameSettings"></param>
		/// <param name="gameState"></param>
		public static void Init(GameSettings gameSettings, GameState gameState)
		{
			_gameSettings = gameSettings;
			_gameState = gameState;
			_playerStats = Utils.GetPlayerStatsFromModel(gameState);
			_warehouse = new();
			_warehousePrice = gameSettings.GamePlaySettings.SpecialRewardSettings.Warehouse.WarehousePrice;
			Initialized = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static void OnAborted(object sender, EventArgs e)
		{
			if (_warehouse!.BlipCreated)
				_warehouse.DeleteBlip();
		}

		/// <summary>
		/// Buying the warehouse or just try it
		/// </summary>
		private static void BuyWareHouse()
		{
			_warehousePrice = _gameSettings!.GamePlaySettings.SpecialRewardSettings.Warehouse.WarehousePrice;

			if (Game.Player.Money < _warehousePrice)
			{
				Screen.ShowSubtitle($"You don't have enough money to buy the warehouse.");
				return;
			}

			BlipColor blipColor = Utils.GetCharacterBlipColor(Utils.GetCharacterFromModel());
			Game.Player.Money -= _warehousePrice;
			_playerStats!.SpecialReward.Warehouse |= Enums.WarehouseStates.Bought;
			Notification.Show("You bought a ~g~warehouse~w~, use it to keep your drugs safe.");
			Notification.Show("But beware! Other ~r~shady ~w~individuals might be interested in it.");
			Audio.PlaySoundFrontend("PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET");
			_warehouse!.ChangeBlip(BlipSprite.Warehouse, blipColor);
			Utils.SaveGameState(_gameState!);
		}
	}
}
