using GTA;
using GTA.Math;
using GTA.UI;
using Los.Santos.Dope.Wars.Classes;
using Los.Santos.Dope.Wars.Extension;
using Los.Santos.Dope.Wars.GUI;
using Los.Santos.Dope.Wars.Persistence.Settings;
using Los.Santos.Dope.Wars.Persistence.State;
using System;
using System.Linq;

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
		private static DateTime _nextDrugVanMissionStart;
		private static Enums.WarehouseMissionStates _missionState;
		private static bool _drugVanSetupDone;
		private static Vehicle? _drugVan;
		private static Ped? _drugVanDriver;
		private static DealerStash? _vanStash;
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
			{
				_playerStats = Utils.GetPlayerStatsFromModel(_gameState!);
				if (_warehouse is not null && _warehouse.BlipCreated)
				{
					_warehouse.DeleteBlip();
					_warehouse = null;
				}
			}

			try
			{
				#region warehouse
				//all necessary flags are there
				if (_playerStats.Reward.Warehouse.HasFlag(Enums.WarehouseStates.Unlocked))
				{
					if (_warehouse is null)
					{
						var (location, entrance, mission) = Utils.GetWarehousePositions();
						_warehouse = new Warehouse(location, entrance, mission);
					}
					//if the blip has not been created
					if (!_warehouse.BlipCreated)
					{
						_warehouse.CreateBlip(BlipSprite.WarehouseForSale);

						// player has bought the warehouse
						if (_playerStats.Reward.Warehouse.HasFlag(Enums.WarehouseStates.Bought))
						{
							BlipColor blipColor = Utils.GetCharacterBlipColor(Utils.GetCharacterFromModel());
							_warehouse.UpdateBlip(BlipSprite.Warehouse, blipColor);
							_warehouse.Stash = _playerStats.Warehouse.Stash;
						}
					}
				}
				//Warehouse exists
				if (_warehouse is not null && _warehouse.BlipCreated)
				{
					// player has bought the warehouse
					if (_playerStats.Reward.Warehouse.HasFlag(Enums.WarehouseStates.Bought))
					{
						// draw the entrance
						if (_player.IsInRange(_warehouse.EntranceMarker, Constants.MarkerDrawDistance))
							_warehouse.DrawEntranceMarker(_warehouse.EntranceMarker, Utils.GetCurrentPlayerColor());
						// draw mission marker
						if (_player.IsInRange(_warehouse.MissionMarker, Constants.MarkerDrawDistance))
							if (_nextDrugVanMissionStart <= ScriptHookUtils.GetGameDateTime() && _missionState.Equals(Enums.WarehouseMissionStates.NotStarted))
								_warehouse.DrawMissionMarker(_warehouse.MissionMarker, Utils.GetCurrentPlayerColor());
						// start mission
						if (_player.IsInRange(_warehouse.MissionMarker, Constants.MarkerInteractionDistance))
							if (_missionState.Equals(Enums.WarehouseMissionStates.NotStarted) && _nextDrugVanMissionStart <= ScriptHookUtils.GetGameDateTime())
							{
								Screen.ShowHelpTextThisFrame($"~b~Press ~INPUT_CONTEXT~ ~w~to start a warehouse mission.");
								if (Game.IsControlJustPressed(Control.Context))
								{
									Script.Wait(10);
									_missionState = Enums.WarehouseMissionStates.Started;
									_nextDrugVanMissionStart = ScriptHookUtils.GetGameDateTime().AddHours(12);
								}
							}
					}
					// now we are real close to the warehouse entrance
					if (_player.IsInRange(_warehouse.EntranceMarker, Constants.MarkerInteractionDistance) && Game.Player.WantedLevel.Equals(0))
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
							WarehouseMenu.ShowWarehouseMenu = true;
						}
					}
					// now we are not close to the warehouse entrance or we are wanted by the cops
					else if (!_player.IsInRange(_warehouse.EntranceMarker, Constants.MarkerInteractionDistance) || !Game.Player.WantedLevel.Equals(0))
						WarehouseMenu.ShowWarehouseMenu = false;
				}
				#endregion

				#region mission
				// the mission has been started
				if (_missionState.Equals(Enums.WarehouseMissionStates.Started))
				{
					Game.IsMissionActive = true;

					if (!_drugVanSetupDone)
					{
						Model vehicleModel = ScriptHookUtils.RequestModel(Statics.WarehouseMissionVehicles[Utils.GetRandomInt(Statics.WarehouseMissionVehicles.Count)]);
						Model driverModel = Utils.GetRandomPedHash(Enums.PedType.DrugDealer);
						// some random position around the player position, at least 1000f plus ...
						float random = (float)(Utils.GetRandomDouble() * 1000) + 1000;
						Vector3 location = World.GetNextPositionOnStreet(_player.Position.Around(random), true);
						_drugVan = World.CreateVehicle(vehicleModel, location);

						_drugVan.IsCollisionProof = _drugVan.IsBulletProof = _drugVan.IsMeleeProof = _drugVan.IsFireProof = _drugVan.IsExplosionProof = false;
						Blip blip = _drugVan.AddBlip();
						blip.Sprite = BlipSprite.DrugPackage;
						blip.IsShortRange = false;
						blip.Name = "Drug Van";

						_drugVanDriver = World.CreatePed(driverModel, location.Around(5f));
						_drugVanDriver.Weapons.Give(Utils.GetRandomWeaponHash(Enums.PedType.DrugDealer), 500, true, true);
						_drugVanDriver.RelationshipGroup.SetRelationshipBetweenGroups(_player.RelationshipGroup, Relationship.Hate, true);
						_drugVanDriver.Task.EnterVehicle(_drugVan, VehicleSeat.Driver, -1, 1, EnterVehicleFlags.WarpIn);

						_drugVanSetupDone = true;
					}

					if (Game.Player.WantedLevel.Equals(0))
					{
						if (_drugVan is not null)
						{
							Screen.ShowSubtitle($"~b~Steal ~w~the drugs that are in the ~y~car ~w~of the model type {_drugVan.DisplayName}!");

							if (_drugVan.IsConsideredDestroyed || _player.IsDead || _player.IsCuffed)
								_missionState = Enums.WarehouseMissionStates.Aborted;

							if (_drugVan.Driver.Equals(_player) && _player.CurrentVehicle is not null)
								_missionState = Enums.WarehouseMissionStates.VanStolen;
						}
					}
					else
					{
						Screen.ShowSubtitle($"~b~Lose ~w~the ~r~cops!");
					}
				}
				// the vehicle has been stolen
				if (_missionState.Equals(Enums.WarehouseMissionStates.VanStolen))
				{
					if (_player.IsInRange(_warehouse!.MissionMarker, Constants.MarkerDrawDistance))
						_warehouse!.DrawMissionMarker(_warehouse.MissionMarker, Utils.GetCurrentPlayerColor());

					if (_vanStash is null)
					{
						_vanStash = new();
						_vanStash.RestockQuantity(_playerStats, _gameSettings!);
						_vanStash.RefreshDrugMoney(_playerStats, _gameSettings!);
					}

					if (Game.Player.WantedLevel.Equals(0))
					{
						if (_drugVan is not null)
						{
							Screen.ShowSubtitle($"~b~Bring ~w~the car back to your ~y~warehouse.");

							if (_drugVan.IsConsideredDestroyed || _player.IsDead || _player.IsCuffed)
								_missionState = Enums.WarehouseMissionStates.Aborted;

							if (World.GetDistance(_drugVan.Position, _warehouse!.MissionMarker) < Constants.MarkerInteractionDistance)
							{
								_drugVan.Speed = 0f;
								_player.Task.LeaveVehicle();
								_missionState = Enums.WarehouseMissionStates.VanDelivered;
							}
						}
					}
					else
					{
						Screen.ShowSubtitle($"~b~Lose ~w~the ~r~cops!");
					}
				}
				// the vehicle has been delivered
				if (_missionState.Equals(Enums.WarehouseMissionStates.VanDelivered))
				{
					if (_drugVan is not null)
					{
						_drugVan.LockStatus = VehicleLockStatus.PlayerCannotEnter;
						_drugVan.AttachedBlip.Delete();
						_drugVan.IsPersistent = true;
						_drugVan.MarkAsNoLongerNeeded();
						_drugVan = null;
					}
					if (_drugVanDriver is not null)
					{
						_drugVanDriver.MarkAsNoLongerNeeded();
						_drugVanDriver = null;
					}
					if (_vanStash is not null)
					{
						foreach (Drug drug in _vanStash.Drugs.Where(x => x.Quantity > 0))
						{
							_warehouse!.Stash.MoveIntoInventory(drug.Name, drug.Quantity, 0);
							Notification.Show($"{drug.Quantity}x~y~{drug.Name} ~w~worth ~g~${drug.Quantity * drug.AveragePrice} ~w~added to your warehouse.");
						}

						if (_vanStash.DrugMoney > 0)
						{
							_player.Money += _vanStash.DrugMoney;
							Notification.Show($"You've found ~g~${_vanStash.DrugMoney} ~w~in the car.");
						}
						_vanStash = null;
					}

					int earnedXP = (int)(_playerStats.NextLevelExperience * 0.10);
					_playerStats.AddExperiencePoints(earnedXP);

					Screen.ShowSubtitle($"You've gained {earnedXP} experience points.");
					_missionState = Enums.WarehouseMissionStates.NotStarted;
					_drugVanSetupDone = false;
					Game.IsMissionActive = false;
				}
				// the mission has been aborted (killed, arrested...)
				if (_missionState.Equals(Enums.WarehouseMissionStates.Aborted))
				{
					CleanUpMission();
					Game.IsMissionActive = false;
					_missionState = Enums.WarehouseMissionStates.NotStarted;
				}
				#endregion
			}
			catch (Exception ex)
			{
				Logger.Error($"{nameof(WarehouseMission)}\nMessage:\t{ex.Message}\nInnerException:\t{ex.InnerException}\nStackTrace:\t{ex.StackTrace}\n" +
					$"Source:\t{ex.Source}\nTargetSite:\t{ex.TargetSite.Name}\nData.Values:\t{ex.Data.Values}");
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
			_warehousePrice = gameSettings.GamePlay.Reward.Warehouse.WarehousePrice;
			_nextDrugVanMissionStart = ScriptHookUtils.GetGameDateTime();
			_missionState = Enums.WarehouseMissionStates.NotStarted;
			Initialized = true;
		}

		/// <summary>
		/// The <see cref="OnAborted(object, EventArgs)"/> method
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static void OnAborted(object sender, EventArgs e)
		{
			if (_warehouse!.BlipCreated)
			{
				_warehouse.DeleteBlip();
				_warehouse = null;
			}
			CleanUpMission();
		}
		#endregion

		#region private methods
		/// <summary>
		/// The <see cref="BuyWareHouse"/> method takes care of everything that is necessary and must be done to buy the warehouse
		/// </summary>
		private static void BuyWareHouse()
		{
			if (Game.Player.Money < _warehousePrice)
			{
				Screen.ShowSubtitle($"You don't have enough ~r~money ~w~to buy the ~y~warehouse~w~.");
				return;
			}

			BlipColor blipColor = Utils.GetCharacterBlipColor(Utils.GetCharacterFromModel());
			Game.Player.Money -= _warehousePrice;
			_playerStats!.Reward.Warehouse |= Enums.WarehouseStates.Bought;
			Notification.Show("You bought a ~g~warehouse~w~, use it to keep your drugs safe.");
			Notification.Show("But beware! Other ~r~shady ~w~individuals might be interested in it.");
			Audio.PlaySoundFrontend("PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET");
			_warehouse!.UpdateBlip(BlipSprite.Warehouse, blipColor);
			Utils.SaveGameState(_gameState!);
		}

		/// <summary>
		/// The <see cref="CleanUpMission"/> methods cleans the mission related things
		/// </summary>
		private static void CleanUpMission()
		{
			if (_drugVan is not null)
			{
				_drugVan.AttachedBlip.Delete();
				_drugVan.MarkAsNoLongerNeeded();
				_drugVan = null;
			}
			if (_vanStash is not null)
			{
				_vanStash = null;
			}
			if (_drugVanDriver is not null)
			{
				_drugVanDriver.MarkAsNoLongerNeeded();
				_drugVanDriver.Delete();
			}
			_drugVanSetupDone = false;
			_missionState = Enums.WarehouseMissionStates.NotStarted;
		}
		#endregion
	}
}