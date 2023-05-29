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
	private readonly IDrug _sourceDrug;
	private readonly IDrug _targetDrug;

	/// <summary>
	/// Initializes a instance of the drug list item class.
	/// </summary>
	/// <param name="sourcedrug">The drug for this menu item.</param>
	/// <param name="tradePrice">The drug trade price.</param>
	public DrugListItem(IDrug sourcedrug, IDrug targetDrug) : base(sourcedrug.Name, sourcedrug.Quantity.GetArray())
	{
		_sourceDrug = sourcedrug;
		_targetDrug = targetDrug;

		Description = GetDescription(_sourceDrug.Quantity, _sourceDrug.Price, _targetDrug.Price);
		Enabled = !Equals(_sourceDrug.Quantity, 0);
		SelectedIndex = _sourceDrug.Quantity;
		Tag = sourcedrug.DrugType;

		_sourceDrug.PropertyChanged += OnPropertyChanged;
		ItemChanged += OnItemChanged;
	}

	private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
	{
		if (!args.PropertyName.Equals(nameof(_sourceDrug.Quantity), StringComparison.Ordinal))
			return;

		Enabled = !Equals(_sourceDrug.Quantity, 0);
		SelectedIndex = _sourceDrug.Quantity;
		Items = _sourceDrug.Quantity.GetArray().ToList();
	}

	private void OnItemChanged(object sender, ItemChangedEventArgs<int> args)
	{
		_logger.Information(sender.ToString());
		_logger.Information($"{args.Object}, {_sourceDrug.Price}, {_targetDrug.Price}");
		Description = GetDescription(args.Object, _sourceDrug.Price, _targetDrug.Price);
	}

	private static string GetDescription(int quantity, int sourcePrice, int targetPrice)
	{
		int totalTargetPrice = quantity * targetPrice;
		int totalSourcePrice = quantity * sourcePrice;
		int totalProfit = totalSourcePrice - totalTargetPrice;
		string description = $"Total price: {quantity} x ${sourcePrice} = ${totalSourcePrice}\n" +
			$"Trade price: {quantity} x ${targetPrice} = ${totalTargetPrice}\n" +
			$"Total profit: ${GetProfit(totalProfit)}";

		return description;
	}

	private static string GetProfit(int profit)
		=> profit < 0 ? $"~r~{profit}" : profit > 0 ? $"~g~{profit}" : $"~w~{profit}";
}
