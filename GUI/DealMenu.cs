using GTA;
using LemonUI;
using LemonUI.Menus;
using Los.Santos.Dope.Wars.Classes;
using Los.Santos.Dope.Wars.Extension;
using Los.Santos.Dope.Wars.Persistence;
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
		private static readonly ObjectPool ObjectPool = new();
		private static NativeMenu SellMenu = null!;
		private static NativeMenu BuyMenu = null!;
		private static NativeMenu StatsMenu = null!;
		private static NativeItem StatsMenuItem = null!;
		private static NativeItem ToSellSwitch = null!;
		private static NativeItem ToBuySwitch = null!;
		private static GameState GameState = null!;
		private static PlayerStats PlayerStats = null!;
		private static PlayerStash PlayerStash = null!;
		private static DealerStash DealerStash = null!;
		internal bool MenuLoaded = false;

		/// <summary>
		/// The <see cref="ShowDealMenu"/> property of type <see cref="bool"/>, if set to true the deal menu should popup
		/// </summary>
		public static bool ShowDealMenu { get; set; } = false;

		/// <summary>
		/// The standard constructor for <see cref="DealMenu"/> class
		/// </summary>
		public DealMenu()
		{
			Tick += DealMenu_OnTick;
		}

		/// <summary>
		/// The <see cref="Init(PlayerStash, DealerStash, GameState)"/> method should be called from outside with the needed parameters
		/// </summary>
		/// <param name="playerStash"></param>
		/// <param name="dealerStash"></param>
		/// <param name="gameState"></param>
		public static void Init(PlayerStash playerStash, DealerStash dealerStash, GameState gameState)
		{
			PlayerStash = playerStash;
			DealerStash = dealerStash;
			GameState = gameState;
			PlayerStats = Utils.GetPlayerStatsFromModel(gameState);
		}

		private void DealMenu_OnTick(object sender, EventArgs e)
		{
			while (PlayerStash is null || DealerStash is null)
				Wait(50);

			if (ShowDealMenu)
			{
				if (!MenuLoaded)
				{
					MenuLoaded = true;
					SetupMenu();
					Wait(10);
					BuyMenu.Visible = true;
					StatsMenu.Visible = true;
				}
			}
			else
			{
				BuyMenu.Visible = false;
				SellMenu.Visible = false;
				StatsMenu.Visible = false;
				MenuLoaded = false;
			}

			while (SellMenu is null || BuyMenu is null)
				Wait(50);

			ObjectPool.Process();
		}

		private void SetupMenu()
		{
			try
			{
				SellMenu = new NativeMenuRight("Sell", $"Player Money: ${PlayerStash.Money}", GetMenuBannerColor());
				BuyMenu = new NativeMenuLeft("Buy", $"Dealer Money: ${DealerStash.Money}", GetMenuBannerColor());
				StatsMenu = new NativeMenuMiddle($"Statistics - {Utils.GetCharacterFromModel()}", GetMenuBannerColor()) { AcceptsInput = false };

				StatsMenuItem = GetStatsMenuItem();
				StatsMenu.Add(StatsMenuItem);

				ObjectPool.Add(BuyMenu);
				ObjectPool.Add(SellMenu);
				ObjectPool.Add(StatsMenu);

				ToSellSwitch = new NativeItem("Go to sell menu", "Want to sell instead of buying?");
				BuyMenu.Add(ToSellSwitch);

				ToBuySwitch = new NativeItem("Go to buy menu", "Want to buy instead of selling?");
				SellMenu.Add(ToBuySwitch);

				SellMenu.ItemActivated += Menu_OnItemActivated;
				BuyMenu.ItemActivated += Menu_OnItemActivated;

				foreach (Drug dealerDrug in DealerStash.Drugs)
				{
					string pon = GetGoodOrBadPrice(dealerDrug.CurrentPrice, dealerDrug.AveragePrice);
					string inPercent = GetDifferenceInPercent(dealerDrug.CurrentPrice, dealerDrug.AveragePrice);

					NativeListItem<int> nativeListItem = new($"{dealerDrug.Name}", 0)
					{
						Description = $"Market price:\t\t${dealerDrug.AveragePrice}\n" +
									$"Current price:\t\t{pon}${dealerDrug.CurrentPrice} ({inPercent})\n" +
									$"~w~Purchase price:\t{dealerDrug.Quantity} x {pon}${dealerDrug.CurrentPrice} ~w~= {pon}${dealerDrug.Quantity * dealerDrug.CurrentPrice}"
					};

					for (int j = 1; j <= dealerDrug.Quantity; j++)
						nativeListItem.Add(j);

					if (dealerDrug.Quantity.Equals(0))
						nativeListItem.Enabled = false;

					nativeListItem.SelectedIndex = dealerDrug.Quantity;
					nativeListItem.SelectedIndex = dealerDrug.Quantity;

					nativeListItem.Activated += NativeListItem_OnActivated;
					nativeListItem.ItemChanged += BuyListItem_OnItemChanged;
					BuyMenu.Add(nativeListItem);
				}

				foreach (Drug playerDrug in PlayerStash.Drugs)
				{
					// we need the current price of the drug, only the opposing dealer can give that...
					int currentDrugPrice = DealerStash.Drugs.Where(x => x.Name.Equals(playerDrug.Name)).Select(x => x.CurrentPrice).FirstOrDefault();
					playerDrug.CurrentPrice = currentDrugPrice;

					string pon = GetGoodOrBadPrice(playerDrug.CurrentPrice, playerDrug.PurchasePrice, true);
					string inPercent = GetDifferenceInPercent(playerDrug.CurrentPrice, playerDrug.PurchasePrice, true);

					NativeListItem<int> nativeListItem = new($"{playerDrug.Name}", 0)
					{
						Description = $"Purchase price:\t${playerDrug.PurchasePrice}\n" +
									$"Current price:\t\t{pon}${playerDrug.CurrentPrice} ({inPercent})\n" +
									$"~w~Selling price:\t\t{playerDrug.Quantity} x {pon}${playerDrug.CurrentPrice} ~w~= {pon}${playerDrug.Quantity * playerDrug.CurrentPrice}"
					};

					for (int j = 1; j <= playerDrug.Quantity; j++)
						nativeListItem.Add(j);

					if (playerDrug.Quantity.Equals(0))
						nativeListItem.Enabled = false;

					nativeListItem.SelectedIndex = playerDrug.Quantity;
					nativeListItem.SelectedIndex = playerDrug.Quantity;

					nativeListItem.Activated += NativeListItem_OnActivated;
					nativeListItem.ItemChanged += SellListItem_OnItemChanged;

					SellMenu.Add(nativeListItem);
				}
			}
			catch (Exception ex)
			{
				Logger.Error($"{nameof(SetupMenu)}\n{ex.Message}\n{ex.InnerException}\n{ex.Source}\n{ex.StackTrace}");
			}
		}

		/// <summary>
		/// Gets the stats menu item with filled properties, new and update
		/// </summary>
		/// <returns></returns>
		private static NativeItem GetStatsMenuItem()
		{
			string title = $"";
			string description = $"Current player level:\t\t{PlayerStats.CurrentLevel} / {PlayerStats.MaxLevel}\n" +
					$"Total spent money:\t\t\t${PlayerStats.SpentMoney}\n" +
					$"Total earned money:\t\t${PlayerStats.EarnedMoney}\n\n" +
					$"Profit:\t\t\t\t\t{((PlayerStats.EarnedMoney - PlayerStats.SpentMoney < 0) ? "~r~" : "~g~")}${PlayerStats.EarnedMoney - PlayerStats.SpentMoney}";

			List<NativeStatsInfo> nativeStatsInfos = new()
						{
								new NativeStatsInfo("Current bag filling level:", PlayerStats.CurrentBagSize * 100 / PlayerStats.MaxBagSize),
								new NativeStatsInfo("Experience to next level:", PlayerStats.CurrentExperience * 100 / PlayerStats.NextLevelExperience),
								new NativeStatsInfo("Police awareness level:", 50),
						};

			NativeItem nativeItem = new(title, description)
			{
				Enabled = false,
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
				PedHash.Franklin => Color.LightGreen,
				PedHash.Michael => Color.SkyBlue,
				PedHash.Trevor => Color.SandyBrown,
				_ => Color.Black
			};
		}

		/// <summary>
		/// Returns price difference in percent, already colored
		/// </summary>
		/// <param name="valueOne">Should be the current price</param>
		/// <param name="valueTwo">Should be the market or purchase price</param>
		/// <param name="isPlayer"></param>
		/// <returns><see cref="string"/></returns>
		private static string GetDifferenceInPercent(int valueOne, int valueTwo, bool isPlayer = false)
		{
			double resultValue = (valueOne / (double)valueTwo * 100) - 100;

			if (resultValue > 0)
				return $"{(isPlayer ? "~g~+" : "~r~+")}{resultValue:n2}%";
			else if (resultValue < 0)
				return $"{(isPlayer ? "~r~" : "~g~")}{resultValue:n2}%";
			else
				return $"~w~{resultValue:n2}%";
		}

		/// <summary>
		/// Returns the color it green, red or white string if it good, bad or neutral
		/// </summary>
		/// <param name="valueOne">Should be the current price</param>
		/// <param name="valueTwo">Should be the market or purchase price</param>
		/// <param name="isPlayer"></param>
		/// <returns><see cref="string"/></returns>
		private static string GetGoodOrBadPrice(int valueOne, int valueTwo, bool isPlayer = false)
		{
			if (valueOne > valueTwo)
				return isPlayer ? "~g~" : "~r~";
			if (valueOne < valueTwo)
				return isPlayer ? "~r~" : "~g~";
			return "~w~";
		}

		private void DrugMenuRefresh(NativeMenu sourceMenu, NativeMenu targetMenu, string drugName, int drugQuantity, int currentDrugPrice)
		{
			try
			{
				if (sourceMenu is NativeMenuLeft)
				{
					NativeListItem<int> targetMenuItem = targetMenu.Items.Where(x => x.Title.Equals(drugName)).FirstOrDefault() as NativeListItem<int>;

					int currentItemMax = targetMenuItem.Items.Max();

					for (int i = currentItemMax + 1; i <= currentItemMax + drugQuantity; i++)
						targetMenuItem.Items.Add(i);

					if (targetMenuItem.Items.Count > 1)
					{
						targetMenuItem.SelectedIndex = targetMenuItem.Items.Max();
						targetMenuItem.SelectedItem = targetMenuItem.Items.Max();
						targetMenuItem.Enabled = true;
					}
					else
					{
						targetMenuItem.Enabled = false;
					}
				}

				if (sourceMenu is NativeMenuRight)
				{
					NativeListItem<int> targetMenuItem = targetMenu.Items.Where(x => x.Title.Equals(drugName)).FirstOrDefault() as NativeListItem<int>;

					int currentItemMax = targetMenuItem.Items.Max();

					for (int i = currentItemMax + 1; i <= currentItemMax + drugQuantity; i++)
						targetMenuItem.Items.Add(i);

					if (targetMenuItem.Items.Count > 1)
					{
						targetMenuItem.SelectedIndex = targetMenuItem.Items.Max();
						targetMenuItem.SelectedItem = targetMenuItem.Items.Max();
						targetMenuItem.Enabled = true;
					}
					else
					{
						targetMenuItem.Enabled = false;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error($"{nameof(DrugMenuRefresh)}\n{ex.Message}\n{ex.InnerException}\n{ex.Source}\n{ex.StackTrace}");
			}
		}

		private void BuyListItem_OnItemChanged(object sender, ItemChangedEventArgs<int> e)
		{
			try
			{
				NativeListItem<int> nli = sender as NativeListItem<int>;

				Drug dealerDrug = DealerStash.Drugs.Where(x => x.Name.Equals(nli.Title)).SingleOrDefault();
				if (dealerDrug == null)
					return;

				string pon = GetGoodOrBadPrice(dealerDrug.CurrentPrice, dealerDrug.AveragePrice);
				string inPercent = GetDifferenceInPercent(dealerDrug.CurrentPrice, dealerDrug.AveragePrice);

				nli.Description = $"Market price:\t\t${dealerDrug.AveragePrice}\n" +
								$"Current price:\t\t{pon}${dealerDrug.CurrentPrice} ({inPercent})\n" +
								$"~w~Purchase price:\t{e.Object} x {pon}${dealerDrug.CurrentPrice} ~w~= {pon}${e.Object * dealerDrug.CurrentPrice}";
			}
			catch (Exception ex)
			{
				Logger.Error($"{nameof(BuyListItem_OnItemChanged)}\n{ex.Message}\n{ex.InnerException}\n{ex.Source}\n{ex.StackTrace}");
			}

		}

		private void SellListItem_OnItemChanged(object sender, ItemChangedEventArgs<int> e)
		{
			try
			{
				NativeListItem<int> nli = sender as NativeListItem<int>;

				Drug playerDrug = PlayerStash.Drugs.Where(x => x.Name.Equals(nli.Title)).SingleOrDefault();
				if (playerDrug == null)
					return;

				string pon = GetGoodOrBadPrice(playerDrug.CurrentPrice, playerDrug.PurchasePrice, true);
				string inPercent = GetDifferenceInPercent(playerDrug.CurrentPrice, playerDrug.PurchasePrice, true);

				nli.Description = $"Purchase price:\t${playerDrug.PurchasePrice}\n" +
								$"Current price:\t\t{pon}${playerDrug.CurrentPrice} ({inPercent})\n" +
								$"~w~Selling price:\t\t{e.Object} x {pon}${playerDrug.CurrentPrice} ~w~= {pon}${e.Object * playerDrug.CurrentPrice}";
			}
			catch (Exception ex)
			{
				Logger.Error($"{nameof(SellListItem_OnItemChanged)}\n{ex.Message}\n{ex.InnerException}\n{ex.Source}\n{ex.StackTrace}");
			}
		}

		private void Menu_OnItemActivated(object sender, ItemActivatedArgs e)
		{
			if (e.Item.Equals(ToSellSwitch) || e.Item.Equals(ToBuySwitch))
			{
				BuyMenu.Visible = !BuyMenu.Visible;
				SellMenu.Visible = !SellMenu.Visible;
			}
		}

		private void NativeListItem_OnActivated(object sender, EventArgs e)
		{
			Player player = Game.Player;
			NativeMenuLeft nativeMenuLeft;
			NativeMenuRight nativeMenuRight;

			try
			{
				if (sender is NativeMenuLeft)
				{
					nativeMenuLeft = sender as NativeMenuLeft;
					if (nativeMenuLeft.SelectedItem is NativeListItem<int>)
					{
						NativeListItem<int> nativeListItem = nativeMenuLeft.SelectedItem as NativeListItem<int>;
						// early saftey exit
						if (nativeListItem.SelectedItem.Equals(0))
							return;

						string drugName = nativeListItem.Title;
						int drugQuantity = nativeListItem.SelectedItem;
						int drugPrice = DealerStash.Drugs.Where(x => x.Name.Equals(nativeListItem.Title)).Select(x => x.CurrentPrice).SingleOrDefault();

						if (player.Money < (drugPrice * drugQuantity))
						{
							GTA.UI.Screen.ShowSubtitle("You don't have enough ~y~money bitch! Fuck off!");
							return;
						}

						//we buy
						PlayerStash.BuyDrug(drugName, drugQuantity, drugPrice);
						DealerStash.SellDrug(drugName, drugQuantity, drugPrice);

						int transactionValue = drugQuantity * drugPrice;
						PlayerStats.SpentMoney += transactionValue;
						GTA.UI.Screen.ShowSubtitle($"You got yourself {drugQuantity} packs of ~y~{drugName} ~w~with a total value of ~r~${transactionValue}.");
						Audio.PlaySoundFrontend("PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET");

						DrugMenuRefresh(nativeMenuLeft, SellMenu, drugName, drugQuantity, drugPrice);

						// the zero "0" is/should always be the last item
						// 30 items plus 0 means 31 take 15 out of it result in 16
						int resultingQuantity = nativeListItem.Items.Count - 1 - drugQuantity;

						for (int i = nativeListItem.Items.Count; i > resultingQuantity; i--)
							nativeListItem.Remove(i);

						// back on track set
						nativeListItem.SelectedIndex = resultingQuantity;
						nativeListItem.SelectedItem = resultingQuantity;

						// deactivate if result would be 0 aka zero
						if (resultingQuantity.Equals(0))
							nativeListItem.Enabled = false;
						else
							nativeListItem.Enabled = true;
					}
				}
				if (sender is NativeMenuRight)
				{
					nativeMenuRight = sender as NativeMenuRight;
					if (nativeMenuRight.SelectedItem is NativeListItem<int>)
					{
						NativeListItem<int> nativeListItem = nativeMenuRight.SelectedItem as NativeListItem<int>;
						// early saftey exit
						if (nativeListItem.SelectedItem.Equals(0))
							return;

						string drugName = nativeListItem.Title;
						int drugQuantity = nativeListItem.SelectedItem;
						int currentDrugPrice = DealerStash.Drugs.Where(x => x.Name.Equals(nativeListItem.Title)).Select(x => x.CurrentPrice).SingleOrDefault();
						int purchasePrice = PlayerStash.Drugs.Where(x => x.Name.Equals(nativeListItem.Title)).Select(x => x.PurchasePrice).SingleOrDefault();

						//we sell
						PlayerStash.SellDrug(drugName, drugQuantity, currentDrugPrice);
						DealerStash.BuyDrug(drugName, drugQuantity, currentDrugPrice);

						int transactionValue = drugQuantity * currentDrugPrice;
						int profit = (currentDrugPrice - purchasePrice) * drugQuantity;

						if (profit > 0)
							PlayerStats.AddExperiencePoints(profit);

						player.Money += transactionValue;
						PlayerStats.EarnedMoney += transactionValue;
						GTA.UI.Screen.ShowSubtitle($"You peddled {drugQuantity} packs of ~y~{drugName} ~w~for a total value of ~g~${transactionValue}.");
						Audio.PlaySoundFrontend("PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET");

						DrugMenuRefresh(nativeMenuRight, BuyMenu, drugName, drugQuantity, currentDrugPrice);

						// the zero "0" is/should always be the last item
						// 30 items plus 0 means 31 take 15 out of it result in 16
						int resultingQuantity = nativeListItem.Items.Count - 1 - drugQuantity;

						for (int i = nativeListItem.Items.Count; i > resultingQuantity; i--)
							nativeListItem.Remove(i);

						// back on track set
						nativeListItem.SelectedIndex = resultingQuantity;
						nativeListItem.SelectedItem = resultingQuantity;

						// deactivate if result would be 0 aka zero
						if (resultingQuantity.Equals(0))
							nativeListItem.Enabled = false;
						else
							nativeListItem.Enabled = true;
					}


				}

				StatsMenu.Remove(StatsMenuItem);
				StatsMenuItem = GetStatsMenuItem();
				StatsMenu.Add(StatsMenuItem);
			}
			catch (Exception ex)
			{
				Logger.Error($"{nameof(NativeListItem_OnActivated)}\n{ex.Message}\n{ex.InnerException}\n{ex.Source}\n{ex.StackTrace}");
			}
		}
	}
}
