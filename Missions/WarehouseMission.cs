using GTA;
using GTA.UI;
using Los.Santos.Dope.Wars.Classes;
using Los.Santos.Dope.Wars.Extension;
using Los.Santos.Dope.Wars.GUI;
using Los.Santos.Dope.Wars.Persistence.Settings;
using Los.Santos.Dope.Wars.Persistence.State;
using System;

namespace Los.Santos.Dope.Wars.Missions
{
	/// <summary>
	/// The <see cref="WarehouseMission"/> class holds the creation an mission related stuff for the "special warehouse reward"
	/// </summary>
	public static class WarehouseMission
	{
		#region fields
		private static Ped? _player;
		private static GameSettings? _gameSettings;
		private static GameState? _gameState;
		private static PlayerStats? _playerStats;
		private static Warehouse? _warehouse;
		private static int _warehousePrice;
		#endregion

		#region properties
		/// <summary>
		/// The <see cref="Initialized"/> property indicates if the <see cref="Init(GameSettings, GameState)"/> method was called
		/// </summary>
		public static bool Initialized { get; private set; }

		/// <summary>
		/// The <see cref="ShowWarehouseMenu"/> property
		/// </summary>
		public static bool ShowWarehouseMenu { get; set; }
		#endregion

		#region constructor
		/// <summary>
		/// The empty <see cref="WarehouseMission"/> class constructor
		/// </summary>
		static WarehouseMission() { }
		#endregion

		#region public methods
		/// <summary>
		/// The <see cref="OnTick(object, EventArgs)"/> method, run for every tick
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static void OnTick(object sender, EventArgs e)
		{
			if (!Initialized)
				return;

			if (_player != Game.Player.Character)
				_player = Game.Player.Character;

			if (_playerStats != Utils.GetPlayerStatsFromModel(_gameState!))
				_playerStats = Utils.GetPlayerStatsFromModel(_gameState!);

			try
			{
				//all necessary flags are there
				if (_playerStats!.Reward.Warehouse.HasFlag(Enums.WarehouseStates.Unlocked) || _playerStats.Reward.Warehouse.HasFlag(Enums.WarehouseStates.Bought) || _playerStats.Reward.Warehouse.HasFlag(Enums.WarehouseStates.Upgraded))
				{
					if (!_warehouse!.BlipCreated)
					{
						_warehouse = new Warehouse(Constants.WarehouseLocationFranklin, Constants.WarehouseEntranceFranklin, Constants.WarehouseMissionStartFranklin);
						_warehouse.CreateBlip(BlipSprite.WarehouseForSale);

						if (_playerStats.Reward.Warehouse.HasFlag(Enums.WarehouseStates.Bought))
						{
							BlipColor blipColor = Utils.GetCharacterBlipColor(Utils.GetCharacterFromModel());
							_warehouse.ChangeBlip(BlipSprite.Warehouse, blipColor);
							_warehouse.Stash = _playerStats.Warehouse.Stash;
						}
					}
				}

				//Warehouse exists
				if (_warehouse!.BlipCreated)
				{
					// now we are real close to the warehouse entrance
					if (_player.IsInRange(_warehouse.Entrance, 2f) && Game.Player.WantedLevel == 0)
					{
						//Warehouse is not yours
						if (!_playerStats.Reward.Warehouse.HasFlag(Enums.WarehouseStates.Bought))
						{
							Screen.ShowHelpTextThisFrame($"~b~Press ~INPUT_CONTEXT~ ~w~to buy the warehouse for ~r~${_warehousePrice}");
							if (Game.IsControlJustPressed(Control.Context))
							{
								Script.Wait(10);
								BuyWareHouse();
							}
						}
						//Warehouse is yours
						else if (_playerStats.Reward.Warehouse.HasFlag(Enums.WarehouseStates.Bought))
						{
							WarehouseMenu.Init(_playerStats.Stash, _warehouse.Stash, _gameState!);
							Script.Wait(10);
							WarehouseMenu.ShowWarehouseMenu = true;
						}
					}
					// now we are not close to the warehouse entrance or we are wanted by the cops
					else if (!_player.IsInRange(_warehouse.Entrance, 2f) || Game.Player.WantedLevel != 0)
					{
						WarehouseMenu.ShowWarehouseMenu = false;
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
			_player = Game.Player.Character;
			_playerStats = Utils.GetPlayerStatsFromModel(gameState);
			_warehouse = new();
			_warehousePrice = gameSettings.GamePlay.Reward.Warehouse.WarehousePrice;
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
		#endregion

		#region private methods
		/// <summary>
		/// Buying the warehouse or just try it
		/// </summary>
		private static void BuyWareHouse()
		{
			_warehousePrice = _gameSettings!.GamePlay.Reward.Warehouse.WarehousePrice;

			if (Game.Player.Money < _warehousePrice)
			{
				Screen.ShowSubtitle($"You don't have enough money to buy the warehouse.");
				return;
			}

			BlipColor blipColor = Utils.GetCharacterBlipColor(Utils.GetCharacterFromModel());
			Game.Player.Money -= _warehousePrice;
			_playerStats!.Reward.Warehouse |= Enums.WarehouseStates.Bought;
			Notification.Show("You bought a ~g~warehouse~w~, use it to keep your drugs safe.");
			Notification.Show("But beware! Other ~r~shady ~w~individuals might be interested in it.");
			Audio.PlaySoundFrontend("PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET");
			_warehouse!.ChangeBlip(BlipSprite.Warehouse, blipColor);
			Utils.SaveGameState(_gameState!);
		}
		#endregion
	}
}
