using LemonUI.Menus;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Extensions;
using LSDW.Domain.Extensions;
using LSDW.Presentation.Menus.Base;

namespace LSDW.Presentation.Menus.DealMenu;
public sealed class DealMenu : MenuBase
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="drugs"></param>
	public DealMenu(ICollection<IDrug> drugs) : base("Buy", "Let's buy some drugs...")
	{
		foreach (IDrug drug in drugs)
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
		NativeListItem<int> item = AddListItem(drug.Type.GetDrugName(), string.Empty, activated, changed, drug.Quantity.GetArray());
		item.Enabled = drug.Quantity > 0;
		item.Tag = drug;
		item.SelectedIndex = drug.Quantity;
		item.ItemChanged += (sender, args) => OnItemChanged(item, args);
		return item;
	}

	private void OnItemActivated(NativeListItem<int> item, EventArgs args)
	{
		if (item.Tag is not IDrug drug)
			return;
		int quantityToMove = item.SelectedItem;

		drug.Remove(quantityToMove);
		item = AddDrugListItem(drug, OnItemActivated);
	}

	private void OnItemChanged(NativeListItem<int> item, ItemChangedEventArgs<int> args)
	{
		if (item.Tag is not IDrug drug)
			return;
	}
}
