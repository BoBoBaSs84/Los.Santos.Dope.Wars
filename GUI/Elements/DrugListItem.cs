using LemonUI.Menus;
using Los.Santos.Dope.Wars.Classes;
using System;

namespace Los.Santos.Dope.Wars.GUI.Elements
{
	/// <summary>
	/// The <see cref="DrugListItem"/> class serves as the buy and sell menu list item, abstracts from <see cref="NativeListItem{T}"/>
	/// </summary>
	public class DrugListItem : NativeListItem<int>
	{
		#region fields
		private readonly bool _isPlayerItem;
		#endregion

		#region constructor
		/// <summary>
		/// The <see cref="DrugListItem"/> standard constructor
		/// </summary>
		/// <param name="drug"></param>
		/// <param name="isPlayerItem"></param>
		public DrugListItem(Drug drug, bool isPlayerItem = false) : base($" {drug.Name}", GetIntArrayFromDrugQuantity(drug.Quantity))
		{
			_isPlayerItem = isPlayerItem;
			if (drug.Quantity.Equals(0))
				Enabled = false;
			Tag = drug;
			SelectedIndex = drug.Quantity;
			LeftBadgeSet = GetDrugBadgeSet(drug.Name);
			RefreshDescription(drug, drug.Quantity, isPlayerItem);
			ItemChanged += OnItemChanged;
		}
		#endregion

		#region private methods
		/// <summary>
		/// The <see cref="OnItemChanged(object, ItemChangedEventArgs{int})"/> simply refreshes the item description if the item value has changed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnItemChanged(object sender, ItemChangedEventArgs<int> e)
		{
			if (Tag is not Drug drug)
				return;
			RefreshDescription(drug, e.Object, _isPlayerItem);
		}

		/// <summary>
		/// The <see cref="GetIntArrayFromDrugQuantity(int)"/> returns an array of integers starting from zero ...
		/// </summary>
		/// <param name="quantity"></param>
		/// <returns><see cref="Array"/></returns>
		private static int[] GetIntArrayFromDrugQuantity(int quantity)
		{
			int[] array = new int[quantity + 1];
			for (int i = 0; i <= quantity; i++)
				array[i] = i;
			return array;
		}

		/// <summary>
		/// The <see cref="GetDrugBadgeSet(string)"/> method returns a corresponding badgeset
		/// </summary>
		/// <param name="drugName"></param>
		/// <returns><see cref="BadgeSet"/></returns>
		private static BadgeSet GetDrugBadgeSet(string drugName)
		{
			BadgeSet cokeBadgeSet = new("mpinventory", "mp_specitem_coke", "mp_specitem_coke_black");
			BadgeSet methBadgeSet = new("mpinventory", "mp_specitem_meth", "mp_specitem_meth_black");
			BadgeSet weedBadgeSet = new("mpinventory", "mp_specitem_weed", "mp_specitem_weed_black");
			BadgeSet heroBadgeSet = new("mpinventory", "mp_specitem_heroin", "mp_specitem_heroin_black");
			BadgeSet defaultBadgeSet = new("mpinventory", "mp_specitem_cash", "mp_specitem_cash_black");

			if (Enums.DrugTypes.Cocaine.ToString().Equals(drugName))
				return cokeBadgeSet;
			if (Enums.DrugTypes.Heroin.ToString().Equals(drugName))
				return heroBadgeSet;
			if (Enums.DrugTypes.Marijuana.ToString().Equals(drugName))
				return weedBadgeSet;
			if (Enums.DrugTypes.Hashish.ToString().Equals(drugName))
				return weedBadgeSet;
			if (Enums.DrugTypes.Mushrooms.ToString().Equals(drugName))
				return weedBadgeSet;
			if (Enums.DrugTypes.Amphetamine.ToString().Equals(drugName))
				return cokeBadgeSet;
			if (Enums.DrugTypes.PCP.ToString().Equals(drugName))
				return cokeBadgeSet;
			if (Enums.DrugTypes.Methamphetamine.ToString().Equals(drugName))
				return methBadgeSet;
			if (Enums.DrugTypes.Ketamine.ToString().Equals(drugName))
				return cokeBadgeSet;
			if (Enums.DrugTypes.Mescaline.ToString().Equals(drugName))
				return methBadgeSet;
			if (Enums.DrugTypes.Ecstasy.ToString().Equals(drugName))
				return methBadgeSet;
			if (Enums.DrugTypes.Acid.ToString().Equals(drugName))
				return methBadgeSet;
			if (Enums.DrugTypes.MDMA.ToString().Equals(drugName))
				return methBadgeSet;
			if (Enums.DrugTypes.Crack.ToString().Equals(drugName))
				return heroBadgeSet;
			if (Enums.DrugTypes.Oxycodone.ToString().Equals(drugName))
				return methBadgeSet;
			else return defaultBadgeSet;
		}

		/// <summary>
		/// The <see cref="RefreshDescription(Drug, int, bool)"/> method refreshes the item description
		/// </summary>
		/// <param name="drug"></param>
		/// <param name="drugQuantity"></param>
		/// <param name="isPlayerItem"></param>
		private void RefreshDescription(Drug drug, int drugQuantity, bool isPlayerItem)
		{
			int valueOne = drug.CurrentPrice;
			int valueTwo = isPlayerItem ? drug.PurchasePrice : drug.AveragePrice;

			string pon = GetGoodOrBadPrice(valueOne, valueTwo, isPlayerItem);
			string inPercent = GetDifferenceInPercent(valueOne, valueTwo, isPlayerItem);

			Description = isPlayerItem ? $"Purchase price:\t${drug.PurchasePrice}\n" : $"Market price:\t\t${drug.AveragePrice}\n";
			Description = Description +
				$"Current price:\t\t{pon}${drug.CurrentPrice} ({inPercent})\n" +
				$"~w~Purchase price:\t{drugQuantity} x {pon}${drug.CurrentPrice} ~w~= {pon}${drugQuantity * drug.CurrentPrice}";
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
		#endregion
	}
}