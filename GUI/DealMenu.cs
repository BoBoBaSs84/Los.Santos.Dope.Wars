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
	/// The <see cref="DealMenu"/> class serves as the typical deal menu between player character and the dealer
	/// </summary>
	public class DealMenu : Script
	{
		#region fields
		private static readonly ObjectPool _objectPool = new();
		private static SellMenu _sellMenu = null!;
		private static BuyMenu _buyMenu = null!;
		private static StatisticsMenu _statisticsMenu = null!;
		private static NativeItem? _toSellMenuSwitch;
		private static NativeItem? _toBuyMenuSwitch;
		private static GameState? _gameState;
		private static Ped? _player;
		private static PlayerStats? _playerStats;
		private static PlayerStash? _playerStash;
		private static DrugDealer? _drugDealer;
		private static DealerStash? _dealerStash;
		private static bool _dealMenuLoaded;
		private static Color _menuColor;
		#endregion

		#region properties
		/// <summary>
		/// The <see cref="ShowDealMenu"/> property if set to true the deal menu should popup
		/// </summary>
		public static bool ShowDealMenu { get; set; } = false;

		/// <summary>
		/// The <see cref="Initialized"/> property indicates if the <see cref="Init(GameState, PlayerStash, DealerStash, Ped)"/> method was called
		/// </summary>
		public static bool Initialized { get; private set; }
		#endregion

		#region constructor
		/// <summary>
		/// The standard constructor for <see cref="DealMenu"/> class
		/// </summary>
		public DealMenu()
		{
			_toSellMenuSwitch = new NativeItem("Go to sell menu", "Want to sell instead of buying?");
			_toSellMenuSwitch.Activated += ToSellMenuSwitchActivated;
			_toBuyMenuSwitch = new NativeItem("Go to buy menu", "Want to buy instead of selling?");
			_toBuyMenuSwitch.Activated += ToBuyMenuSwitchActivated;
			_menuColor = Utils.GetCurrentPlayerColor();

			Tick += OnTick;
		}
		#endregion

		#region public methods
		/// <summary>
		/// The <see cref="Init(GameState, PlayerStash, DealerStash, Ped)"/> method should be called from outside with the needed parameters
		/// </summary>
		/// <param name="gameState"></param>
		/// <param name="playerStash"></param>
		/// <param name="dealerStash"></param>
		/// <param name="player"></param>
		public static void Init(GameState gameState, PlayerStash playerStash, DealerStash dealerStash, Ped player)
		{
			_playerStash = playerStash;
			_player = player;
			_dealerStash = dealerStash;
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

			if (ShowDealMenu)
			{
				if (!_dealMenuLoaded)
				{
					LoadDealMenu();
					_dealMenuLoaded = _buyMenu.Visible = _statisticsMenu.Visible = true;
				}
			}
			else
			{
				if (_dealMenuLoaded)
				{
					_buyMenu.Visible = _sellMenu.Visible = _statisticsMenu.Visible = false;
					UnloadDealMenu();
				}
			}
			_objectPool.Process();
		}

		private void LoadDealMenu()
		{
			try
			{
				_sellMenu = new SellMenu("Sell", $"", _menuColor);
				_buyMenu = new BuyMenu("Buy", $"Dealer Money: ${_dealerStash!.DrugMoney}", _menuColor);

				_statisticsMenu = new StatisticsMenu($"Statistics - {Utils.GetCharacterFromModel()}", "", _menuColor) { AcceptsInput = false };
				_statisticsMenu.Add(GetStatsMenuItem());

				_objectPool.Add(_buyMenu);
				_objectPool.Add(_sellMenu);
				_objectPool.Add(_statisticsMenu);

				SetRefreshBuyMenu(_dealerStash);
				SetRefreshSellMenu(_playerStash!, _dealerStash);
			}
			catch (Exception ex)
			{
				Logger.Error($"{nameof(LoadDealMenu)}\n{ex.Message}\n{ex.InnerException}\n{ex.Source}\n{ex.StackTrace}");
			}
		}

		private static void UnloadDealMenu()
		{
			_statisticsMenu.Clear();
			_buyMenu.Clear();
			_sellMenu.Clear();
			_dealMenuLoaded = false;
			Initialized = false;
		}

		/// <summary>
		/// The <see cref="ToBuyMenuSwitchActivated(object, EventArgs)"/> method for switching to the buy menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToBuyMenuSwitchActivated(object sender, EventArgs e)
		{
			_buyMenu.Visible = !_buyMenu.Visible;
			_sellMenu.Visible = !_sellMenu.Visible;
		}

		/// <summary>
		/// The <see cref="ToSellMenuSwitchActivated(object, EventArgs)"/> method for switching to the sell menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToSellMenuSwitchActivated(object sender, EventArgs e)
		{
			_buyMenu.Visible = !_buyMenu.Visible;
			_sellMenu.Visible = !_sellMenu.Visible;
		}

		/// <summary>
		/// The <see cref="SetRefreshBuyMenu(DealerStash)"/> method sets or refreshes the buy menu
		/// </summary>
		/// <param name="dealerStash"></param>
		private void SetRefreshBuyMenu(DealerStash dealerStash)
		{
			int index = _buyMenu.SelectedIndex;
			_buyMenu.Clear();
			_buyMenu.Subtitle = $"Dealer Money: ${dealerStash.DrugMoney}";
			_buyMenu.Add(_toSellMenuSwitch);
			foreach (Drug drug in dealerStash.Drugs)
			{
				DrugListItem buyListItem = new(drug);
				buyListItem.Activated += BuyListItemActivated;
				_buyMenu.Add(buyListItem);
			}
			if (index > -1)
				_buyMenu.SelectedIndex = index;
		}

		/// <summary>
		/// The <see cref="BuyListItemActivated(object, EventArgs)"/> method is called when the player tries to buy drugs from a dealer type of person
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BuyListItemActivated(object sender, EventArgs e)
		{
			try
			{
				if (sender is not BuyMenu menu || menu.SelectedItem is not DrugListItem menuItem || menuItem.SelectedItem.Equals(0) || menuItem.Tag is not Drug drug)
					return;

				string drugName = drug.Name;
				int drugQuantity = menuItem.SelectedItem;
				int drugPrice = _dealerStash!.Drugs.Where(x => x.Name.Equals(drug.Name)).Select(x => x.CurrentPrice).SingleOrDefault();
				int transactionValue = drugQuantity * drugPrice;

				// early exit
				if (Game.Player.Money < transactionValue)
				{
					GTA.UI.Screen.ShowSubtitle("You don't have enough ~y~money ~w~bitch! ~r~Fuck off~w~!");
					return;
				}

				//player buys drugs from dealer
				_playerStash!.BuyDrug(drugName, drugQuantity, drugPrice);
				_dealerStash.SellDrug(drugName, drugQuantity, drugPrice);

				_playerStats!.SpentMoney += transactionValue;
				GTA.UI.Screen.ShowSubtitle($"You got yourself {drugQuantity} packs of ~y~{drugName} ~w~with a total value of ~r~${transactionValue}.");
				Audio.PlaySoundFrontend("PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET");

				SetRefreshBuyMenu(_dealerStash!);
				SetRefreshSellMenu(_playerStash!, _dealerStash!);

				_statisticsMenu.Clear();
				_statisticsMenu.Add(GetStatsMenuItem());

				Utils.SaveGameState(_gameState!);
				ScriptHookUtils.DrugEnforcementAdministrationBust(_player!, _playerStats.CurrentLevel);
			}
			catch (Exception ex)
			{
				Logger.Error($"{nameof(BuyListItemActivated)}\n{ex.Message}\n{ex.InnerException}\n{ex.Source}\n{ex.StackTrace}");
			}
		}

		/// <summary>
		/// The <see cref="SetRefreshSellMenu(PlayerStash, DealerStash)"/> method sets or refreshes the sell menu
		/// </summary>
		/// <param name="playerStash"></param>
		/// <param name="dealerStash"></param>
		private void SetRefreshSellMenu(PlayerStash playerStash, DealerStash dealerStash)
		{
			int index = _sellMenu.SelectedIndex;
			_sellMenu.Clear();
			_sellMenu.Add(_toBuyMenuSwitch);
			foreach (Drug drug in playerStash.Drugs)
			{
				// we need the current price of the drug, only the opposing dealer can give that...
				int currentDrugPrice = dealerStash.Drugs.Where(x => x.Name.Equals(drug.Name)).Select(x => x.CurrentPrice).FirstOrDefault();
				drug.CurrentPrice = currentDrugPrice;

				DrugListItem sellListItem = new(drug, true);
				sellListItem.Activated += SellListItemActivated;
				_sellMenu.Add(sellListItem);
			}
			if (index > -1)
				_sellMenu.SelectedIndex = index;
		}

		/// <summary>
		/// The <see cref="SellListItemActivated(object, EventArgs)"/> method is called when the player tries to sell drugs to a dealer type of person
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SellListItemActivated(object sender, EventArgs e)
		{
			try
			{
				if (sender is not SellMenu menu || menu.SelectedItem is not DrugListItem menuItem || menuItem.SelectedItem.Equals(0) || menuItem.Tag is not Drug drug)
					return;

				string drugName = drug.Name;
				int drugQuantity = menuItem.SelectedItem;
				int currentDrugPrice = _dealerStash!.Drugs.Where(x => x.Name.Equals(drug.Name)).Select(x => x.CurrentPrice).SingleOrDefault();
				int purchasePrice = _playerStash!.Drugs.Where(x => x.Name.Equals(drug.Name)).Select(x => x.PurchasePrice).SingleOrDefault();
				int transactionValue = drugQuantity * currentDrugPrice;

				// early exit
				if (_dealerStash.DrugMoney < transactionValue)
				{
					GTA.UI.Screen.ShowSubtitle("The dealer does not have enough ~y~money~w~!");
					return;
				}

				//player sells to dealer
				_playerStash.SellDrug(drugName, drugQuantity, currentDrugPrice);
				_dealerStash.BuyDrug(drugName, drugQuantity, currentDrugPrice);

				int profit = (currentDrugPrice - purchasePrice) * drugQuantity;

				if (profit > 0)
					_playerStats!.AddExperiencePoints(profit);

				_playerStats!.EarnedMoney += transactionValue;
				GTA.UI.Screen.ShowSubtitle($"You peddled {drugQuantity} packs of ~y~{drugName} ~w~for a total value of ~g~${transactionValue}.");
				Audio.PlaySoundFrontend("PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET");

				SetRefreshBuyMenu(_dealerStash!);
				SetRefreshSellMenu(_playerStash!, _dealerStash!);

				_statisticsMenu.Clear();
				_statisticsMenu.Add(GetStatsMenuItem());

				Utils.SaveGameState(_gameState!);
				ScriptHookUtils.DrugEnforcementAdministrationBust(_player!, _playerStats.CurrentLevel);
			}
			catch (Exception ex)
			{
				Logger.Error($"{nameof(SellListItemActivated)}\n{ex.Message}\n{ex.InnerException}\n{ex.Source}\n{ex.StackTrace}");
			}
		}

		/// <summary>
		/// Gets the stats menu item with filled properties, new and update
		/// </summary>
		/// <returns><see cref="NativeItem"/></returns>
		private static NativeItem GetStatsMenuItem()
		{
			string title = $"Current player level:\t\t{_playerStats!.CurrentLevel} / {PlayerStats.MaxLevel}";
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
	}
	#endregion
}