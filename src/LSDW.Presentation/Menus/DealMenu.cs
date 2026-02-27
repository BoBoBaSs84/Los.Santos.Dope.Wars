#nullable enable
using System;
using System.Drawing;
using System.Linq;
using LemonUI.Menus;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Services;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Extensions;
using LSDW.Abstractions.Presentation.Menus;
using LSDW.Domain.Factories;
using LSDW.Presentation.Helpers;
using LSDW.Presentation.Menus.Base;
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

    // Keep strong references to per-item handlers so we can unsubscribe correctly.
    private readonly System.Collections.Generic.Dictionary<NativeListItem<int>, ItemChangedEventHandler<int>>
        _itemChangedHandlers = new();

    public TransactionType Type { get; }
    public event EventHandler? SwitchItemActivated;

    /// <summary>
    /// Initializes a instance of the deal menu class.
    /// </summary>
    /// <param name="providerManager">The provider manager instance to use.</param>
    /// <param name="type">The transaction type that defines the menu.</param>
    /// <param name="player">The player instance to use.</param>
    /// <param name="inventory">The inventory instance to use.</param>
    internal DealMenu(IProviderManager providerManager, TransactionType type, IPlayer player, IInventory inventory)
        : base(type.ToString(), type.GetDescription())
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

        var quantity = Math.Max(0, item.SelectedItem);
        if (quantity == 0)
            return;

        bool success = _transactionService.Commit(drug.Type, quantity, drug.Price);

        if (success)
        {
            SetDrugListItem(item, drug);
            _transactionService.BustOrNoBust();
        }
        else
        {
            // Show feedback by flashing the description with a warning color prefix.
            item.Description = $"~r~Transaction failed.~w~\n{item.Description}";
        }
    }

    private void OnItemChanged(NativeListItem<int> item) => SetItemDescription(item);

    private void OnSwitchActivated() => SwitchItemActivated?.Invoke(this, EventArgs.Empty);

    /// <summary>
    /// Adds a new drug list item to the menu.
    /// </summary>
    /// <param name="drug">The drug instance to add.</param>
    /// <param name="activated">The action to perform when activated.</param>
    /// <param name="changed">The action to perform when the selected item of the list changes.</param>
    private void AddDrugListItem(
        IDrug drug,
        Action<NativeListItem<int>, EventArgs>? activated = null,
        Action<NativeListItem<int>, ItemChangedEventArgs<int>>? changed = null)
    {
        NativeListItem<int> item = AddListItem(
            drug.Type.GetName(),
            string.Empty,
            activated,
            changed,
            drug.Quantity.GetArray());

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
        // Properly unsubscribe previous handler for this item, if any.
        if (_itemChangedHandlers.TryGetValue(item, out var oldHandler))
        {
            item.ItemChanged -= oldHandler;
            _itemChangedHandlers.Remove(item);
        }

        item.Enabled = drug.Quantity > 0;
        item.Tag = drug;

        // Rebuild items (0..Quantity) and clamp the selected index.
        item.Items = drug.Quantity.GetList();
        item.SelectedIndex = item.Items.Count > 0 ? Math.Min(drug.Quantity, item.Items.Count - 1) : 0;

        // Attach stable handler that passes the sender back.
        ItemChangedEventHandler<int> handler = (sender, args) => OnItemChanged(sender);
        item.ItemChanged += handler;
        _itemChangedHandlers[item] = handler;

        SetItemDescription(item);
    }

    /// <summary>
    /// Sets the item description depending on the Tag and TransactionType.
    /// </summary>
    private void SetItemDescription(NativeListItem<int> item)
    {
        if (item.Tag is not IDrug drug || drug.Quantity == 0)
        {
            item.Description = "~w~No stock available.";
            return;
        }

        int qty = Math.Max(0, item.SelectedItem);

        if (Type is TransactionType.BUY)
        {
            // Player buys FROM the source, so this is a "Buy price".
            int buyPrice = drug.Price;
            int average = drug.Type.GetAveragePrice();
            int total = buyPrice * qty;
            string gbn = GoodBadNetral(buyPrice, average);

            item.Description =
                $"Buy price:\t${gbn}{FormatMoney(buyPrice)}~w~\n" +
                $"Average:\t${FormatMoney(average)}\n" +
                $"Total:\t\t${gbn}{FormatMoney(total)}";
            return;
        }

        if (Type is TransactionType.SELL)
        {
            // Player sells TO the target (dealer’s buying price).
            int targetPrice = _target.FirstOrDefault(x => x.Type.Equals(drug.Type))?.Price ?? drug.Price;
            int purchasePrice = drug.Price;
            int total = targetPrice * qty;
            string gbn = GoodBadNetral(purchasePrice, targetPrice);

            item.Description =
                $"Sell price:\t${gbn}{FormatMoney(targetPrice)}~w~\n" +
                $"Bought price:\t${FormatMoney(purchasePrice)}\n" +
                $"Total:\t\t${gbn}{FormatMoney(total)}";
            return;
        }

        // Fallback for other transaction types
        item.Description = $"~w~Qty: {qty}  Price: ${FormatMoney(drug.Price)}";
    }

    /// <summary>
    /// Compares two values and returns a GTA color code (green/white/red).
    /// </summary>
    private static string GoodBadNetral(int valueOne, int valueTwo) => GoodBadNeutral(valueOne, valueTwo);

    private static string GoodBadNeutral(int valueOne, int valueTwo)
    {
        if (valueOne < valueTwo) return "~g~";
        if (valueOne == valueTwo) return "~w~";
        return "~r~";
    }

    private static string FormatMoney(int value) => value.ToString("N0");

    /// <summary>
    /// Returns the menu offset.
    /// </summary>
    private PointF GetMenuOffset()
    {
        Size screenSize = _providerManager.ScreenProvider.Resolution;
        return new PointF(screenSize.Width / 64f, screenSize.Height / 36f);
    }
}
