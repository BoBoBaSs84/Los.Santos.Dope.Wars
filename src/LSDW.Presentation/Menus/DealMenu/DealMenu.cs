using LemonUI.Menus;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Abstractions.Extensions;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
using LSDW.Presentation.Menus.Base;

namespace LSDW.Presentation.Menus.DealMenu;
public sealed class DealMenu : MenuBase
{
	private readonly INotificationProvider _notificationProvider = DomainFactory.GetNotificationProvider();

	public DealMenu(ICollection<IDrug> drugs) : base("Buy", "Let's buy some drugs...")
	{
		foreach (IDrug drug in drugs)
		{
			NativeListItem<int> item = AddDrugListItem(drug, OnItemActivated);
			item.Tag = drug;
			item.SelectedIndex = drug.Quantity;
			item.ItemChanged += (sender, args) => OnItemChanged(item, args);
		}
	}

	private void OnItemActivated(NativeListItem<int> item, EventArgs args)
	{
		_notificationProvider.Show($"{nameof(OnItemActivated)}");
		_notificationProvider.Show($"Title:\t{item.Title}");
		_notificationProvider.Show($"Description:\t{item.Description}");
		_notificationProvider.Show($"SelectedItem:\t{item.SelectedItem}");
	}

	private void OnItemChanged(NativeListItem<int> item, ItemChangedEventArgs<int> args)
	{
		_notificationProvider.Show($"{nameof(OnItemChanged)}");
		_notificationProvider.Show($"SelectedItem:\t{item.SelectedItem}");
	}

	/// <summary>
	/// Adds a new drug list item to the menu.
	/// </summary>
	/// <param name="drug">The drug instance to add.</param>
	/// <param name="activated">The action to perform when activated.</param>
	/// <param name="changed">The action to perform when the selected item of the list changes.</param>
	private NativeListItem<int> AddDrugListItem(IDrug drug, Action<NativeListItem<int>, EventArgs>? activated = null, Action<NativeListItem<int>, ItemChangedEventArgs<int>>? changed = null)
		=> AddListItem(drug.Type.GetDrugName(), drug.Type.GetDrugDescription(), activated, changed, drug.Quantity.GetArray());
}
