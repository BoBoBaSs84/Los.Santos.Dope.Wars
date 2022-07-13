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
		private static readonly ObjectPool objectPool = new();
		private static SellMenu sellMenu = null!;
		private static BuyMenu buyMenu = null!;
		private static StatisticsMenu statisticsMenu = null!;
		private static NativeItem? toSellMenuSwitch;
		private static NativeItem? toBuyMenuSwitch;
		private static GameState? gameState;
		private static PlayerStats? playerStats;
		private static PlayerStash? playerStash;
		private static DealerStash? dealerStash;
		private static bool _dealMenuLoaded;
		#endregion

		#region properties
		/// <summary>
		/// The <see cref="ShowDealMenu"/> property of type <see cref="bool"/>, if set to true the deal menu should popup
		/// </summary>
		public static bool ShowDealMenu { get; set; } = false;

		/// <summary>
		/// The <see cref="Initialized"/> property indicates if the <see cref="Init(PlayerStash, DealerStash, GameState)"/> method was called
		/// </summary>
		public static bool Initialized { get; private set; }
		#endregion

		#region constructor
		/// <summary>
		/// The standard constructor for <see cref="DealMenu"/> class
		/// </summary>
		public DealMenu()
		{
			toSellMenuSwitch = new NativeItem("Go to sell menu", "Want to sell instead of buying?");
			toSellMenuSwitch.Activated += ToSellMenuSwitchActivated;
			toBuyMenuSwitch = new NativeItem("Go to buy menu", "Want to buy instead of selling?");
			toBuyMenuSwitch.Activated += ToBuyMenuSwitchActivated;

			Tick += OnTick;
		}
		#endregion

		#region public methods
		/// <summary>
		/// The <see cref="Init(PlayerStash, DealerStash, GameState)"/> method should be called from outside with the needed parameters
		/// </summary>
		/// <param name="playerStash"></param>
		/// <param name="dealerStash"></param>
		/// <param name="gameState"></param>
		public static void Init(PlayerStash playerStash, DealerStash dealerStash, GameState gameState)
		{
			DealMenu.playerStash = playerStash;
			DealMenu.dealerStash = dealerStash;
			DealMenu.gameState = gameState;
			playerStats = Utils.GetPlayerStatsFromModel(gameState);
			Initialized = true;
		}
		#endregion

		#region private methods
		private void OnTick(object sender, EventArgs e)
		{
			if (!Initialized)
				return;

			if (ShowDealMenu)
			{
				if (!_dealMenuLoaded)
				{
					LoadDealMenu();
					_dealMenuLoaded = true;
					buyMenu.Visible = true;
					statisticsMenu.Visible = true;
				}
			}
			else
			{
				buyMenu.Visible = false;
				sellMenu.Visible = false;
				statisticsMenu.Visible = false;
				UnloadDealMenu();
			}

			objectPool.Process();
		}

		private void LoadDealMenu()
		{
			try
			{
				sellMenu = new SellMenu("Sell", $"", GetMenuBannerColor());
				buyMenu = new BuyMenu("Buy", $"Dealer Money: ${dealerStash.DrugMoney}", GetMenuBannerColor());
				
				statisticsMenu = new StatisticsMenu($"Statistics - {Utils.GetCharacterFromModel()}", "", GetMenuBannerColor()) { AcceptsInput = false };
				statisticsMenu.Add(GetStatsMenuItem());

				objectPool.Add(buyMenu);
				objectPool.Add(sellMenu);
				objectPool.Add(statisticsMenu);

				SetRefreshBuyMenu(dealerStash);
				SetRefreshSellMenu(playerStash, dealerStash);
			}
			catch (Exception ex)
			{
				Logger.Error($"{nameof(LoadDealMenu)}\n{ex.Message}\n{ex.InnerException}\n{ex.Source}\n{ex.StackTrace}");
			}
		}

		private static void UnloadDealMenu()
		{
			statisticsMenu.Clear();
			buyMenu.Clear();
			sellMenu.Clear();
			_dealMenuLoaded = false;
		}

		/// <summary>
		/// The <see cref="ToBuyMenuSwitchActivated(object, EventArgs)"/> method for switching to the buy menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToBuyMenuSwitchActivated(object sender, EventArgs e)
		{
			buyMenu.Visible = !buyMenu.Visible;
			sellMenu.Visible = !sellMenu.Visible;
		}

		/// <summary>
		/// The <see cref="ToSellMenuSwitchActivated(object, EventArgs)"/> method for switching to the sell menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToSellMenuSwitchActivated(object sender, EventArgs e)
		{
			buyMenu.Visible = !buyMenu.Visible;
			sellMenu.Visible = !sellMenu.Visible;
		}

		/// <summary>
		/// The <see cref="SetRefreshBuyMenu(DealerStash)"/> method sets or refreshes the buy menu
		/// </summary>
		/// <param name="dealerStash"></param>
		private void SetRefreshBuyMenu(DealerStash dealerStash)
		{
			int index = buyMenu.SelectedIndex;
			buyMenu.Clear();
			buyMenu.Subtitle = $"Dealer Money: ${dealerStash.DrugMoney}";
			buyMenu.Add(toSellMenuSwitch);
			foreach (Drug drug in dealerStash.Drugs)
			{
				DrugListItem drugListItem = new(drug);
				drugListItem.Activated += NativeListItem_OnActivated;
				buyMenu.Add(drugListItem);
			}
			if (index > -1)
				buyMenu.SelectedIndex = index;
		}

		/// <summary>
		/// The <see cref="SetRefreshSellMenu(PlayerStash, DealerStash)"/> method sets or refreshes the sell menu
		/// </summary>
		/// <param name="playerStash"></param>
		/// <param name="dealerStash"></param>
		private void SetRefreshSellMenu(PlayerStash playerStash, DealerStash dealerStash)
		{
			int index = sellMenu.SelectedIndex;
			sellMenu.Clear();
			sellMenu.Add(toBuyMenuSwitch);
			foreach (Drug drug in playerStash.Drugs)
			{
				// we need the current price of the drug, only the opposing dealer can give that...
				int currentDrugPrice = dealerStash.Drugs.Where(x => x.Name.Equals(drug.Name)).Select(x => x.CurrentPrice).FirstOrDefault();
				drug.CurrentPrice = currentDrugPrice;

				DrugListItem drugListItem = new(drug, true);
				drugListItem.Activated += NativeListItem_OnActivated;
				sellMenu.Add(drugListItem);
			}
			if (index > -1)
				sellMenu.SelectedIndex = index;
		}

		/// <summary>
		/// Gets the stats menu item with filled properties, new and update
		/// </summary>
		/// <returns></returns>
		private static NativeItem GetStatsMenuItem()
		{
			string title = $"Current player level:\t\t{playerStats.CurrentLevel} / {PlayerStats.MaxLevel}";
			string description = $"Total spent money:\t\t\t${playerStats.SpentMoney}\n" +
					$"Total earned money:\t\t${playerStats.EarnedMoney}\n\n" +
					$"Profit:\t\t\t\t\t{((playerStats.EarnedMoney - playerStats.SpentMoney < 0) ? "~r~" : "~g~")}${playerStats.EarnedMoney - playerStats.SpentMoney}";

			int prevExp = playerStats.CurrentLevel.Equals(1) ? 0 : (int)Math.Pow(playerStats.CurrentLevel, 2.5) * 1000;

			List<NativeStatsInfo> nativeStatsInfos = new()
			{
				new NativeStatsInfo("Current bag filling level:", playerStats.CurrentBagSize * 100 / playerStats.MaxBagSize),
				new NativeStatsInfo("Experience to next level:", (playerStats.CurrentExperience - prevExp) * 100 / (playerStats.NextLevelExperience - prevExp))
			};

			NativeItem nativeItem = new(title, description)
			{
				Panel = new NativeStatsPanel(nativeStatsInfos.ToArray())
			};

			return nativeItem;
		}

		/// <summary>
		/// Get the banner color for the current character
		/// </summary>
		/// <returns></returns>
		private static Color GetMenuBannerColor()
		{
			return (PedHash)Game.Player.Character.Model switch
			{
				PedHash.Franklin => Color.FromArgb(171, 237, 171),
				PedHash.Michael => Color.FromArgb(101, 180, 212),
				PedHash.Trevor => Color.FromArgb(244, 164, 96),
				_ => Color.Black
			};
		}

		private void NativeListItem_OnActivated(object sender, EventArgs e)
		{
			BuyMenu nativeMenuLeft;
			SellMenu nativeMenuRight;

			try
			{
				if (sender is BuyMenu)
				{
					nativeMenuLeft = sender as BuyMenu;
					if (nativeMenuLeft.SelectedItem is NativeListItem<int>)
					{
						NativeListItem<int> nativeListItem = nativeMenuLeft.SelectedItem as NativeListItem<int>;
						// early saftey exit
						if (nativeListItem.SelectedItem.Equals(0))
							return;

						string drugName = nativeListItem.Title;
						int drugQuantity = nativeListItem.SelectedItem;
						int drugPrice = dealerStash.Drugs.Where(x => x.Name.Equals(nativeListItem.Title)).Select(x => x.CurrentPrice).SingleOrDefault();
						int transactionValue = drugQuantity * drugPrice;

						// early exit
						if (Game.Player.Money < transactionValue)
						{
							GTA.UI.Screen.ShowSubtitle("You don't have enough ~y~money ~w~bitch! ~r~Fuck off~w~!");
							return;
						}

						//player buys drugs from dealer
						playerStash.BuyDrug(drugName, drugQuantity, drugPrice);
						dealerStash.SellDrug(drugName, drugQuantity, drugPrice);

						SetRefreshBuyMenu(dealerStash);
						SetRefreshSellMenu(playerStash, dealerStash);

						playerStats.SpentMoney += transactionValue;
						GTA.UI.Screen.ShowSubtitle($"You got yourself {drugQuantity} packs of ~y~{drugName} ~w~with a total value of ~r~${transactionValue}.");
						Audio.PlaySoundFrontend("PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET");
					}
				}
				if (sender is SellMenu)
				{
					nativeMenuRight = sender as SellMenu;
					if (nativeMenuRight.SelectedItem is NativeListItem<int>)
					{
						NativeListItem<int> nativeListItem = nativeMenuRight.SelectedItem as NativeListItem<int>;
						// early saftey exit
						if (nativeListItem.SelectedItem.Equals(0))
							return;

						string drugName = nativeListItem.Title;
						int drugQuantity = nativeListItem.SelectedItem;
						int currentDrugPrice = dealerStash.Drugs.Where(x => x.Name.Equals(nativeListItem.Title)).Select(x => x.CurrentPrice).SingleOrDefault();
						int purchasePrice = playerStash.Drugs.Where(x => x.Name.Equals(nativeListItem.Title)).Select(x => x.PurchasePrice).SingleOrDefault();
						int transactionValue = drugQuantity * currentDrugPrice;

						// early exit
						if (dealerStash.DrugMoney < transactionValue)
						{
							GTA.UI.Screen.ShowSubtitle("The dealer does not have enough ~y~money~w~!");
							return;
						}

						//player sells to dealer
						playerStash.SellDrug(drugName, drugQuantity, currentDrugPrice);
						dealerStash.BuyDrug(drugName, drugQuantity, currentDrugPrice);

						int profit = (currentDrugPrice - purchasePrice) * drugQuantity;

						if (profit > 0)
							playerStats.AddExperiencePoints(profit);

						playerStats.EarnedMoney += transactionValue;
						GTA.UI.Screen.ShowSubtitle($"You peddled {drugQuantity} packs of ~y~{drugName} ~w~for a total value of ~g~${transactionValue}.");
						Audio.PlaySoundFrontend("PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET");

						SetRefreshBuyMenu(dealerStash);
						SetRefreshSellMenu(playerStash, dealerStash);
					}
				}
				statisticsMenu.Clear();
				statisticsMenu.Add(GetStatsMenuItem());
				Utils.SaveGameState(gameState!);
			}
			catch (Exception ex)
			{
				Logger.Error($"{nameof(NativeListItem_OnActivated)}\n{ex.Message}\n{ex.InnerException}\n{ex.Source}\n{ex.StackTrace}");
			}
		}
	}
	#endregion
}