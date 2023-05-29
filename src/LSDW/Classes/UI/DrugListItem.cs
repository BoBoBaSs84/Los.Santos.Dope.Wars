using LemonUI.Menus;
using LSDW.Core.Extensions;
using LSDW.Core.Interfaces.Classes;
using LSDW.Factories;
using LSDW.Interfaces.Services;

namespace LSDW.Classes.UI;

/// <summary>
/// The drug item class.
/// </summary>
public sealed class DrugListItem : NativeListItem<int>
{
	private readonly ILoggerService _logger = ServiceFactory.CreateLoggerService();
	private readonly IDrug _drug;
	private readonly int _tradePrice;

	/// <summary>
	/// Initializes a instance of the drug list item class.
	/// </summary>
	/// <param name="drug">The drug for this menu item.</param>
	/// <param name="tradePrice">The drug trade price.</param>
	public DrugListItem(IDrug drug, int tradePrice = 0) : base(drug.Name, drug.Quantity.GetArray())
	{
		_drug = drug;
		_tradePrice = tradePrice;

		Description = GetDescription(_drug.Quantity, _drug.Price, _tradePrice);
		Enabled = !Equals(_drug.Quantity, 0);
		SelectedIndex = _drug.Quantity;
		Tag = drug.DrugType;

		_drug.PropertyChanged += OnPropertyChanged;
		ItemChanged += OnItemChanged;
	}

	private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
	{
		if (!args.PropertyName.Equals(nameof(_drug.Quantity), StringComparison.Ordinal))
			return;

		Enabled = !Equals(_drug.Quantity, 0);
		SelectedIndex = _drug.Quantity;
		Items = _drug.Quantity.GetArray().ToList();
	}

	private void OnItemChanged(object sender, ItemChangedEventArgs<int> args)
	{
		_logger.Information(sender.ToString());
		Description = GetDescription(args.Object, _drug.Price, _tradePrice);
	}

	private static string GetDescription(int quantity, int price, int tradePrice)
	{
		int totalTradePrice = quantity * tradePrice;
		int totalPrice = quantity * price;
		int totalProfit = totalPrice - totalTradePrice;
		string description = $"Total price: {quantity} x ${price} = ${totalPrice}\n" +
			$"Trade price: {quantity} x ${tradePrice} = ${totalTradePrice}\n" +
			$"Total profit: ${GetProfit(totalProfit)}";

		return description;
	}

	private static string GetProfit(int profit)
		=> profit < 0 ? $"~r~{profit}" : profit > 0 ? $"~g~{profit}" : $"~w~{profit}";
}
