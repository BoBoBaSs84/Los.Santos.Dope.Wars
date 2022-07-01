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
		private static Warehouse? _Warehouse;
		private static int _WarehousePrice;

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
					if (!_Warehouse!.BlipCreated)
					{
						_Warehouse = new Classes.Warehouse(Constants.WarehouseLocationFranklin, Constants.WarehouseEntranceFranklin, Constants.WarehouseMissionStartFranklin);
						_Warehouse.CreateBlip(BlipSprite.WarehouseForSale);

						if (_playerStats.SpecialReward.Warehouse.HasFlag(Enums.WarehouseStates.Bought))
						{
							BlipColor blipColor = Utils.GetCharacterBlipColor(Utils.GetCharacterFromModel());
							_Warehouse.ChangeBlip(BlipSprite.Warehouse, blipColor);
							_Warehouse.WarehouseStash = _playerStats.Warehouse.WarehouseStash;
						}
					}
				}

				//Warehouse exists
				if (_Warehouse!.BlipCreated && World.GetDistance(player.Position, Constants.WarehouseEntranceFranklin) <= 3f)
				{
					//Warehouse is not yours
					if (!_playerStats.SpecialReward.Warehouse.HasFlag(Enums.WarehouseStates.Bought))
					{
						Screen.ShowHelpTextThisFrame($"~b~Press ~INPUT_CONTEXT~ ~w~to buy the warehouse for ~r~${_WarehousePrice}");
						if (Game.IsControlJustPressed(Control.Context))
						{
							Script.Wait(10);
							BuyWareHouse();
						}
					}

					//Warehouse is yours
					if (_playerStats.SpecialReward.Warehouse.HasFlag(Enums.WarehouseStates.Bought))
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
			Initialized = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static void OnAborted(object sender, EventArgs e)
		{
			if (_Warehouse!.BlipCreated)
				_Warehouse.DeleteBlip();
		}

		/// <summary>
		/// Buying the warehouse or just try it
		/// </summary>
		private static void BuyWareHouse()
		{
			_WarehousePrice = _gameSettings!.GamePlaySettings.SpecialRewardSettings.Warehouse.WarehousePrice;

			if ((Game.Player.Money + _playerStats!.Stash.Money) < _WarehousePrice)
			{
				Screen.ShowSubtitle($"You don't have enough money to buy the warehouse.");
				return;
			}

			Ped? player = Game.Player.Character;
			BlipColor blipColor = Utils.GetCharacterBlipColor(Utils.GetCharacterFromModel());

			int moneyToPay = _WarehousePrice;
			if (_playerStats.Stash.Money > 0)
			{
				int moneyToRemoveFromStash = 0;
				if (_playerStats.Stash.Money >= _WarehousePrice)
				{
					moneyToRemoveFromStash += _WarehousePrice;
					_playerStats.Stash.RemoveDrugMoney(moneyToRemoveFromStash);
				}
				else
				{
					moneyToPay -= _playerStats.Stash.Money;
					moneyToRemoveFromStash = _WarehousePrice - moneyToPay;
					_playerStats.Stash.RemoveDrugMoney(moneyToRemoveFromStash);
				}
				Notification.Show($"~r~${moneyToRemoveFromStash} removed from stash and used as payment.");
			}
			player.Money -= moneyToPay;
			_playerStats.SpecialReward.Warehouse |= Enums.WarehouseStates.Bought;
			Notification.Show("You bought a ~g~warehouse~w~, use it to keep your drugs and drug money safe.");
			Notification.Show("But beware! Other ~r~shady ~w~individuals might be interested in it.");
			Audio.PlaySoundFrontend("PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET");
			_Warehouse!.ChangeBlip(BlipSprite.Warehouse, blipColor);
			Utils.SaveGameState(_gameState!);
		}
	}
}
