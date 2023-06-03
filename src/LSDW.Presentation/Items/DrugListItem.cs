using LemonUI.Menus;
using LSDW.Domain.Extensions;
using LSDW.Domain.Interfaces.Models;

namespace LSDW.Presentation.Items;

/// <summary>
/// The drug item class.
/// </summary>
public sealed class DrugListItem : NativeListItem<int>
{
	private readonly IDrug _sourceDrug;
	private readonly IDrug _targetDrug;

	/// <summary>
	/// Initializes a instance of the drug list item class.
	/// </summary>
	/// <param name="sourcedrug">The drug for this menu item.</param>
	/// <param name="targetDrug">The target drug for this menu item.</param>
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
		=> Description = GetDescription(args.Object, _sourceDrug.Price, _targetDrug.Price);

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
