﻿using LemonUI.Menus;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Services;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Extensions;
using LSDW.Abstractions.Presentation.Menus;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
using LSDW.Presentation.Helpers;
using LSDW.Presentation.Menus.Base;
using System.Drawing;
using GTAFont = GTA.UI.Font;

namespace LSDW.Presentation.Menus;

/// <summary>
/// The deal menu class.
/// </summary>
internal sealed class DealMenu : MenuBase, IDealMenu
{
	private readonly IProviderManager _providerManager;
	private readonly IPlayer _player;
	private readonly ITransactionService _transactionService;
	private readonly IInventory _source;
	private readonly IInventory _target;

	public TransactionType Type { get; }
	public event EventHandler? SwitchItemActivated;

	/// <summary>
	/// Initializes a instance of the deal menu class.
	/// </summary>
	/// <param name="providerManager">The provider manager instance to use.</param>
	/// <param name="type">The transaction type that defines the menu.</param>
	/// <param name="player">The player instance to use.</param>
	/// <param name="inventory">The inventory instance to use.</param>
	internal DealMenu(IProviderManager providerManager, TransactionType type, IPlayer player, IInventory inventory) : base(type.ToString(), type.GetDescription())
	{
		_providerManager = providerManager;
		Type = type;
		_player = player;
		_transactionService = DomainFactory.CreateTransactionService(providerManager, type, _player, inventory);
		(_source, _target) = MenuHelper.GetInventories(type, player, inventory);

		Alignment = MenuHelper.GetAlignment(type);
		BannerText.Font = GTAFont.Pricedown;
		ItemCount = CountVisibility.Never;
		Offset = GetMenuOffset();

		AddSwitchItem(type);
		foreach (IDrug drug in _source)
			AddDrugListItem(drug, OnItemActivated);

		MaxItems = Items.Count;
	}

	private void OnItemActivated(NativeListItem<int> item, EventArgs args)
	{
		if (item.Tag is not IDrug drug)
			return;

		bool success = _transactionService.Commit(drug.Type, item.SelectedItem, drug.Price);

		if (success)
		{
			SetDrugListItem(item, drug);
			_transactionService.BustOrNoBust();
		}
	}

	private void OnItemChanged(NativeListItem<int> item)
		=> SetItemDescription(item);

	private void OnSwitchActivated()
		=> SwitchItemActivated?.Invoke(this, EventArgs.Empty);

	/// <summary>
	/// Adds a new drug list item to the menu.
	/// </summary>
	/// <param name="drug">The drug instance to add.</param>
	/// <param name="activated">The action to perform when activated.</param>
	/// <param name="changed">The action to perform when the selected item of the list changes.</param>
	private void AddDrugListItem(IDrug drug, Action<NativeListItem<int>, EventArgs>? activated = null, Action<NativeListItem<int>, ItemChangedEventArgs<int>>? changed = null)
	{
		NativeListItem<int> item = AddListItem(drug.Type.GetName(), string.Empty, activated, changed, drug.Quantity.GetArray());
		SetDrugListItem(item, drug);
	}

	/// <summary>
	/// Adds a new menu Switch item to the menu.
	/// </summary>
	/// <param name="type">The transaction type that defines the menu.</param>
	private void AddSwitchItem(TransactionType type) =>
		AddItem(SwitchItemHelper.GetTitle(type), SwitchItemHelper.GetDescription(type), OnSwitchActivated);

	/// <summary>
	/// Sets all possible things for the <paramref name="item"/> itself.
	/// </summary>
	/// <param name="item">The item to refresh.</param>
	/// <param name="drug">The drug instance to use.</param>
	private void SetDrugListItem(NativeListItem<int> item, IDrug drug)
	{
		item.ItemChanged -= (sender, args) => OnItemChanged(item);
		item.SelectedIndex = 0;
		item.Enabled = drug.Quantity > 0;
		item.Tag = drug;
		item.Items = drug.Quantity.GetList();
		item.SelectedIndex = drug.Quantity;
		item.ItemChanged += (sender, args) => OnItemChanged(item);
		SetItemDescription(item);
	}

	/// <summary>
	/// Sets the item description depending on the information in 
	/// the <see cref="NativeItem.Tag"/> and the <see cref="TransactionType"/>.
	/// </summary>
	/// <param name="item">The item to refresh.</param>
	private void SetItemDescription(NativeListItem<int> item)
	{
		if (item.Tag is not IDrug drug || drug.Quantity.Equals(0))
			return;

		if (Type is TransactionType.BUY)
		{
			int sellingPrice = drug.Price;
			int averagePrice = drug.Type.GetAveragePrice();
			int currentQuantitity = item.SelectedItem;
			int totalPrice = sellingPrice * currentQuantitity;
			string gbn = GoodBadNetral(sellingPrice, averagePrice);

			string description = $"Sell price:\t${gbn}{sellingPrice}~w~\n" +
				$"Average price:\t${averagePrice}\n" +
				$"Total price:\t${gbn}{totalPrice}";

			item.Description = description;
		}

		if (Type is TransactionType.SELL)
		{
			int sellingPrice = _target.Where(x => x.Type.Equals(drug.Type)).Select(x => x.Price).First();
			int purchasePrice = drug.Price;
			int currentQuantitity = item.SelectedItem;
			int totalPrice = sellingPrice * currentQuantitity;
			string gbn = GoodBadNetral(purchasePrice, sellingPrice);

			string description = $"Sell price:\t${gbn}{sellingPrice}~w~\n" +
				$"Bought price:\t${purchasePrice}\n" +
				$"Total price:\t${gbn}{totalPrice}";

			item.Description = description;
		}
	}

	/// <summary>
	/// Compares to values with each other.
	/// </summary>
	/// <param name="valueOne">The first vlaue.</param>
	/// <param name="valueTwo">The second value.</param>
	/// <returns>The good, bad or neutral string.</returns>
	private static string GoodBadNetral(int valueOne, int valueTwo)
		=> valueOne <= valueTwo ? valueOne < valueTwo ? "~g~" : "~w~" : "~r~";

	/// <summary>
	/// Returns the menu offset.
	/// </summary>
	private PointF GetMenuOffset()
	{
		Size screenSize = _providerManager.ScreenProvider.Resolution;
		return new PointF(screenSize.Width / 64, screenSize.Height / 36);
	}
}
