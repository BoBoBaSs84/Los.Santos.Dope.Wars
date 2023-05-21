using LemonUI.Menus;
using LSDW.Core.Extensions;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.UserInterfaces.Trafficking;

/// <summary>
/// The drug item class.
/// </summary>
public sealed class DrugItem : NativeListItem<int>
{
	/// <summary>
	/// Initializes a instance of the drug item class.
	/// </summary>
	/// <param name="drug">The drug for this menu item.</param>
	public DrugItem(IDrug drug) : base(drug.Name, drug.Quantity.GetArray())
	{
		Enabled = !Equals(drug.Quantity, 0);
		ItemChanged += OnItemChanged;
		SelectedIndex = drug.Quantity;
		Tag = drug;
	}

	private void OnItemChanged(object sender, ItemChangedEventArgs<int> args)
		=> throw new NotImplementedException();
}
