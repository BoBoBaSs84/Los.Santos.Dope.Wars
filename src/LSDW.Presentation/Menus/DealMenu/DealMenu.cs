using LemonUI.Menus;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Services;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Extensions;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
using LSDW.Presentation.Menus.Base;
using LSDW.Presentation.Helpers;

namespace LSDW.Presentation.Menus.DealMenu;

/// <summary>
/// The deal menu class.
/// </summary>
public sealed class DealMenu : MenuBase
{
	private readonly TransactionType _type;
	private readonly IPlayer _player;
	private readonly ITransactionService _transactionService;
	private readonly IInventory _source;
	private readonly IInventory _target;

	/// <summary>
	/// Initializes a instance of the deal menu class.
	/// </summary>
	/// <param name="providerManager">The provider manager instance to use.</param>
	/// <param name="type"></param>
	/// <param name="player">The player instance to use.</param>
	/// <param name="inventory">The inventory instance to use.</param>
	public DealMenu(IProviderManager providerManager, TransactionType type, IPlayer player, IInventory inventory)
		: base(type.ToString(), type.GetDescription())
	{
		_type = type;
		_player = player;
		_transactionService = DomainFactory.CreateTransactionService(providerManager, type, _player, inventory);
		(_source, _target) = MenuHelper.GetInventories(type, player, inventory);

		Alignment = MenuHelper.GetAlignment(type);
		ItemCount = CountVisibility.Never;
		MaxItems = _source.Count;

		foreach (IDrug drug in _source)
			_ = AddDrugListItem(drug, OnItemActivated);
	}

	/// <summary>
	/// Adds a new drug list item to the menu.
	/// </summary>
	/// <param name="drug">The drug instance to add.</param>
	/// <param name="activated">The action to perform when activated.</param>
	/// <param name="changed">The action to perform when the selected item of the list changes.</param>
	private NativeListItem<int> AddDrugListItem(IDrug drug, Action<NativeListItem<int>, EventArgs>? activated = null, Action<NativeListItem<int>, ItemChangedEventArgs<int>>? changed = null)
	{
		NativeListItem<int> item = AddListItem(drug.Type.GetName(), string.Empty, activated, changed, drug.Quantity.GetArray());
		item = RefreshDrugListItem(item, drug);
		return item;
	}

	/// <summary>
	/// Determines all possible things for the <paramref name="item"/> itself.
	/// </summary>
	/// <param name="item">The item to refresh.</param>
	/// <param name="drug">The drug instance to use.</param>
	private NativeListItem<int> RefreshDrugListItem(NativeListItem<int> item, IDrug drug)
	{
		item.ItemChanged -= (sender, args) => OnItemChanged(item);
		item.SelectedIndex = 0;
		item.Enabled = drug.Quantity > 0;
		item.Tag = drug;
		item.Items = drug.Quantity.GetList();
		item.SelectedIndex = drug.Quantity;
		item.Description = RefreshItemDescription(item);
		item.ItemChanged += (sender, args) => OnItemChanged(item);
		return item;
	}

	private string RefreshItemDescription(NativeListItem<int> item)
	{
		if (item.Tag is not IDrug drug || drug.Quantity.Equals(0))
			return string.Empty;

		if (_type is TransactionType.BUY)
		{
			int sellingPrice = drug.Price;
			int averagePrice = drug.Type.GetAveragePrice();
			int currentQuantitity = item.SelectedItem;
			int totalPrice = sellingPrice * currentQuantitity;
			string gbn = GoodBadNetral(sellingPrice, averagePrice);

			string description = $"Sell price:\t${gbn}{sellingPrice}~w~\n" +
				$"Average price:\t${averagePrice}\n" +
				$"Total price:\t${gbn}{totalPrice}";

			return description;
		}

		if (_type is TransactionType.SELL)
		{
			int sellingPrice = _target.Where(x => x.Type.Equals(drug.Type)).Select(x => x.Price).First();
			int purchasePrice = drug.Price;
			int currentQuantitity = item.SelectedItem;
			int totalPrice = sellingPrice * currentQuantitity;
			string gbn = GoodBadNetral(purchasePrice, sellingPrice);

			string description = $"Sell price:\t${gbn}{sellingPrice}~w~\n" +
				$"Bought price:\t${purchasePrice}\n" +
				$"Total price:\t${gbn}{totalPrice}";

			return description;
		}

		return string.Empty;
	}

	private static string GoodBadNetral(int valueOne, int valueTwo)
		=> valueOne <= valueTwo ? valueOne < valueTwo ? "~g~" : "~w~" : "~r~";

	private void OnItemActivated(NativeListItem<int> item, EventArgs args)
	{
		if (item.Tag is not IDrug drug)
			return;

		bool success = _transactionService.Commit(drug.Type, item.SelectedItem, drug.Price);

		if (success)
		{
			_ = RefreshDrugListItem(item, drug);
			_transactionService.BustOrNoBust();
		}
	}

	private void OnItemChanged(NativeListItem<int> item)
		=> item.Description = RefreshItemDescription(item);
}
