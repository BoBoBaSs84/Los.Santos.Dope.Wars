using LemonUI.Menus;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Extensions;
using LSDW.Domain.Extensions;

namespace LSDW.Presentation.Items;

/// <summary>
/// The drug item class.
/// </summary>
internal sealed class DrugListItem : NativeListItem<int>
{
	private readonly IDrug _drug;
	private readonly int _comparisonPrice;

	/// <summary>
	/// Initializes a instance of the drug list item class.
	/// </summary>
	/// <param name="drug">The drug for this menu item.</param>
	/// <param name="comparisonPrice">Comparison price used as a reference basis.</param>
	internal DrugListItem(IDrug drug, int comparisonPrice) : base(drug.Type.GetName(), drug.Quantity.GetArray())
	{
		_drug = drug;
		_comparisonPrice = comparisonPrice;

		Description = GetDescription(_drug.Quantity, _drug.Price, _comparisonPrice);
		Enabled = _drug.Quantity > 0;
		SelectedItem = _drug.Quantity;
		Tag = drug.Type;

		_drug.PropertyChanged += OnPropertyChanged;
		ItemChanged += OnItemChanged;
	}

	private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
	{
		if (!args.PropertyName.Equals(nameof(_drug.Quantity), StringComparison.Ordinal))
			return;

		Enabled = _drug.Quantity > 0;
		Items = _drug.Quantity.GetList();
		SelectedItem = _drug.Quantity;
	}

	private void OnItemChanged(object sender, ItemChangedEventArgs<int> args)
		=> Description = GetDescription(args.Object, _drug.Price, _comparisonPrice);

	private static string GetDescription(int quantity, int sourcePrice, int targetPrice)
	{
		if (quantity.Equals(0))
			return string.Empty;

		int totalTargetPrice = quantity * targetPrice;
		int totalSourcePrice = quantity * sourcePrice;
		int totalProfit = totalTargetPrice - totalSourcePrice;
		string description = $"Total price:\t{quantity} x ${sourcePrice} = ${totalSourcePrice}\n" +
			$"Trade price:\t{quantity} x ${targetPrice} = ${totalTargetPrice}\n" +
			$"Total profit:\t${GetProfit(totalProfit)}";

		return description;
	}

	private static string GetProfit(int profit)
		=> profit < 0 ? $"~r~{profit}" : profit > 0 ? $"~g~{profit}" : $"~w~{profit}";
}

