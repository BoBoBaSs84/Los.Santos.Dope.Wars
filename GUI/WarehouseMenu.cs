using GTA;
using LemonUI;
using LemonUI.Menus;
using Los.Santos.Dope.Wars.Classes;
using Los.Santos.Dope.Wars.Extension;
using Los.Santos.Dope.Wars.GUI.Elements;
using Los.Santos.Dope.Wars.Persistence.State;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Los.Santos.Dope.Wars.GUI
{
	/// <summary>
	/// The <see cref="WarehouseMenu"/> class serves as the warehouse menu for the player character and his warehouse property
	/// </summary>
	public class WarehouseMenu : Script
	{
		#region fields
		private static readonly ObjectPool _objectPool = new();
		private static SellMenu _playerMenu = null!;
		private static BuyMenu _warehouseMenu = null!;
		private static StatisticsMenu _statisticsMenu = null!;
		private static NativeItem? _toPlayerMenuSwitch;
		private static NativeItem? _toWarehouseMenuSwitch;
		private static GameState? _gameState;
		private static PlayerStats? _playerStats;
		private static PlayerStash? _playerInventory;
		private static PlayerStash? _warehouseInventory;
		private static bool _warehouseMenuLoaded;
		private static Color _menuColor;
		#endregion

		#region properties
		/// <summary>
		/// The <see cref="ShowWarehouseMenu"/> property if set to true the warehouse menu should popup
		/// </summary>
		public static bool ShowWarehouseMenu { get; set; }

		/// <summary>
		/// The <see cref="Initialized"/> property indicates if the <see cref="Init(PlayerStash, PlayerStash, GameState)"/> method was called
		/// </summary>
		public static bool Initialized { get; private set; }
		#endregion

		#region constructor
		/// <summary>
		/// The standard constructor for <see cref="WarehouseMenu"/> class
		/// </summary>
		public WarehouseMenu()
		{
			_toPlayerMenuSwitch = new NativeItem("Go to player stash", "Want to stash something?");
			_toPlayerMenuSwitch.Activated += ToPlayerMenuSwitchActivated;
			_toWarehouseMenuSwitch = new NativeItem("Go to warehouse stash", "Want to take something?");
			_toWarehouseMenuSwitch.Activated += ToWarehouseMenuSwitchActivated;
			_menuColor = Utils.GetCurrentPlayerColor();

			Tick += OnTick;
		}
		#endregion

		#region public methods
		/// <summary>
		/// The <see cref="Init(PlayerStash, PlayerStash, GameState)"/> method should be called from outside with the needed parameters
		/// </summary>
		/// <param name="playerInventory"></param>
		/// <param name="warehouseInventory"></param>
		/// <param name="gameState"></param>
		public static void Init(PlayerStash playerInventory, PlayerStash warehouseInventory, GameState gameState)
		{
			_playerInventory = playerInventory;
			_warehouseInventory = warehouseInventory;
			_gameState = gameState;
			_playerStats = Utils.GetPlayerStatsFromModel(gameState);
			Initialized = true;
		}
		#endregion

		#region private methods
		/// <summary>
		/// The <see cref="OnTick(object, EventArgs)"/> method, run for every tick
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnTick(object sender, EventArgs e)
		{
			if (!Initialized)
				return;

			if (ShowWarehouseMenu)
			{
				if (!_warehouseMenuLoaded)
				{
					LoadWarehouseMenu();
					_warehouseMenuLoaded = _warehouseMenu.Visible = _statisticsMenu.Visible = true;
				}
			}
			else
			{
				if (_warehouseMenuLoaded)
				{
					_warehouseMenu.Visible = _playerMenu.Visible = _statisticsMenu.Visible = false;
					UnloadWarehouseMenu();
				}
			}
			_objectPool.Process();
		}

		private static void UnloadWarehouseMenu()
		{
			_statisticsMenu.Clear();
			_warehouseMenu.Clear();
			_playerMenu.Clear();
			_warehouseMenuLoaded = false;
		}

		private void LoadWarehouseMenu()
		{
			_playerMenu = new SellMenu("Player", $"", _menuColor);
			_warehouseMenu = new BuyMenu("Warehouse", $"", _menuColor);

			_statisticsMenu = new StatisticsMenu($"Statistics - {Utils.GetCharacterFromModel()}", "", _menuColor)
			{
				AcceptsInput = false
			};
			_statisticsMenu.Add(GetStatsMenuItem());

			_objectPool.Add(_playerMenu);
			_objectPool.Add(_warehouseMenu);
			_objectPool.Add(_statisticsMenu);

			SetRefreshWarehouseMenu(_warehouseInventory!);
			SetRefreshPlayerMenu(_playerInventory!);
		}

		/// <summary>
		/// The <see cref="ToWarehouseMenuSwitchActivated(object, EventArgs)"/> method for switching to the "take from" menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToWarehouseMenuSwitchActivated(object sender, EventArgs e)
		{
			_warehouseMenu.Visible = !_warehouseMenu.Visible;
			_playerMenu.Visible = !_playerMenu.Visible;
		}

		/// <summary>
		/// The <see cref="ToPlayerMenuSwitchActivated(object, EventArgs)"/> method for switching to the "move from" menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToPlayerMenuSwitchActivated(object sender, EventArgs e)
		{
			_warehouseMenu.Visible = !_warehouseMenu.Visible;
			_playerMenu.Visible = !_playerMenu.Visible;
		}

		/// <summary>
		/// The <see cref="SetRefreshWarehouseMenu(PlayerStash)"/> method sets or refreshes the "move into" menu
		/// </summary>
		/// <param name="playerInventory"></param>
		/// <exception cref="NotImplementedException"></exception>
		private void SetRefreshPlayerMenu(PlayerStash playerInventory)
		{
			int index = _playerMenu.SelectedIndex;
			_playerMenu.Clear();
			_playerMenu.Add(_toWarehouseMenuSwitch);
			foreach (Drug drug in playerInventory.Drugs)
			{
				DrugListItem playerListItem = new(drug, true);
				playerListItem.Activated += OnPlayerListItemActivated;
				_playerMenu.Add(playerListItem);
			}
			if (index > -1)
				_playerMenu.SelectedIndex = index;
		}

		/// <summary>
		/// The <see cref="OnPlayerListItemActivated(object, EventArgs)"/> method get called when the player tries to move drugs into his warehouse
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnPlayerListItemActivated(object sender, EventArgs e)
		{
			try
			{
				if (sender is not SellMenu menu || menu.SelectedItem is not DrugListItem menuItem || menuItem.SelectedItem.Equals(0) || menuItem.Tag is not Drug drug)
					return;

				string drugName = drug.Name;
				int drugQuantity = menuItem.SelectedItem;
				int drugPurchasePrice = _playerInventory!.Drugs.Where(x => x.Name.Equals(drug.Name)).Select(x => x.PurchasePrice).SingleOrDefault();

				_warehouseInventory!.MoveIntoInventory(drugName, drugQuantity, drugPurchasePrice);
				_playerInventory.TakeFromInventory(drugName, drugQuantity);

				SetRefreshWarehouseMenu(_warehouseInventory!);
				SetRefreshPlayerMenu(_playerInventory!);

				_statisticsMenu.Clear();
				_statisticsMenu.Add(GetStatsMenuItem());

				Utils.SaveGameState(_gameState!);
			}
			catch (Exception ex)
			{
				Logger.Error($"{nameof(OnPlayerListItemActivated)}\n{ex.Message}\n{ex.InnerException}\n{ex.Source}\n{ex.StackTrace}");
			}
		}

		/// <summary>
		/// The <see cref="SetRefreshWarehouseMenu(PlayerStash)"/> method sets or refreshes the "take from" menu
		/// </summary>
		/// <param name="warehouseInventory"></param>
		private void SetRefreshWarehouseMenu(PlayerStash warehouseInventory)
		{
			int index = _warehouseMenu.SelectedIndex;
			_warehouseMenu.Clear();
			_warehouseMenu.Add(_toPlayerMenuSwitch);
			foreach (Drug drug in warehouseInventory.Drugs)
			{
				DrugListItem warehouseListItem = new(drug, true);
				warehouseListItem.Activated += OnWarehouseListItemActivated;
				_warehouseMenu.Add(warehouseListItem);
			}
			if (index > -1)
				_warehouseMenu.SelectedIndex = index;
		}

		/// <summary>
		/// The <see cref="OnWarehouseListItemActivated(object, EventArgs)"/> method get called when the player tries to move drugs from his warehouse
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnWarehouseListItemActivated(object sender, EventArgs e)
		{
			try
			{
				if (sender is not BuyMenu menu || menu.SelectedItem is not DrugListItem menuItem || menuItem.SelectedItem.Equals(0) || menuItem.Tag is not Drug drug)
					return;

				string drugName = drug.Name;
				int drugQuantity = menuItem.SelectedItem;
				int drugPurchasePrice = _warehouseInventory!.Drugs.Where(x => x.Name.Equals(drug.Name)).Select(x => x.PurchasePrice).SingleOrDefault();

				_playerInventory!.MoveIntoInventory(drugName, drugQuantity, drugPurchasePrice);
				_warehouseInventory.TakeFromInventory(drugName, drugQuantity);

				SetRefreshWarehouseMenu(_warehouseInventory!);
				SetRefreshPlayerMenu(_playerInventory!);

				_statisticsMenu.Clear();
				_statisticsMenu.Add(GetStatsMenuItem());

				Utils.SaveGameState(_gameState!);
			}
			catch (Exception ex)
			{
				Logger.Error($"{nameof(OnWarehouseListItemActivated)}\n{ex.Message}\n{ex.InnerException}\n{ex.Source}\n{ex.StackTrace}");
			}
		}

		/// <summary>
		/// The <see cref="GetStatsMenuItem"/> method gets the stats menu item with filled properties, new and update
		/// </summary>
		/// <returns><see cref="NativeItem"/></returns>
		private static NativeItem GetStatsMenuItem()
		{
			string title = $"Current player level:\t\t{_playerStats.CurrentLevel} / {PlayerStats.MaxLevel}";
			string description = $"Total spent money:\t\t\t${_playerStats.SpentMoney}\n" +
					$"Total earned money:\t\t${_playerStats.EarnedMoney}\n\n" +
					$"Profit:\t\t\t\t\t{((_playerStats.EarnedMoney - _playerStats.SpentMoney < 0) ? "~r~" : "~g~")}${_playerStats.EarnedMoney - _playerStats.SpentMoney}";

			int prevExp = _playerStats.CurrentLevel.Equals(1) ? 0 : (int)Math.Pow(_playerStats.CurrentLevel, 2.5) * 1000;

			List<NativeStatsInfo> nativeStatsInfos = new()
			{
				new NativeStatsInfo("Current bag filling level:", _playerStats.CurrentBagSize * 100 / _playerStats.MaxBagSize),
				new NativeStatsInfo("Experience to next level:", (_playerStats.CurrentExperience - prevExp) * 100 / (_playerStats.NextLevelExperience - prevExp))
			};

			NativeItem nativeItem = new(title, description)
			{
				Panel = new NativeStatsPanel(nativeStatsInfos.ToArray())
			};

			return nativeItem;
		}
		#endregion
	}
}
