using LemonUI;
using LemonUI.Menus;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Services;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Presentation.Items;
using LSDW.Abstractions.Presentation.Menus;
using LSDW.Domain.Factories;
using LSDW.Presentation.Items;
using SMH = LSDW.Presentation.Helpers.SideMenuHelper;

namespace LSDW.Presentation.Menus;

/// <summary>
/// The side menu class.
/// </summary>
internal sealed class SideMenu : NativeMenu, ISideMenu
{
	private readonly Size _screenSize = GTA.UI.Screen.Resolution;
	private readonly IInventory _source;
	private readonly IInventory _target;
	private readonly IPlayer _player;
	private readonly ITransactionService _transaction;
	private readonly TransactionType _type;

	public ISwitchItem SwitchItem { get; }

	/// <summary>
	/// Initializes a instance of the side menu class.
	/// </summary>
	/// <param name="type">The type of the transaction.</param>
	/// <param name="serviceManager">The service manager instance to use.</param>
	/// <param name="inventory">The opposition inventory.</param>
	internal SideMenu(TransactionType type, IServiceManager serviceManager, IInventory inventory) : base(SMH.GetTitle(type))
	{
		_type = type;
		_player = serviceManager.StateService.Player;

		(_source, _target) = SMH.GetInventories(type, _player, inventory);

		int maximumQuantity = SMH.GetMaximumQuantity(type, _player);
		_transaction = DomainFactory.CreateTransactionService(serviceManager.NotificationService, type, _source, _target, maximumQuantity);

		Alignment = SMH.GetAlignment(type);
		ItemCount = CountVisibility.Never;
		Offset = new PointF(_screenSize.Width / 64, _screenSize.Height / 36);
		UseMouse = false;
		TitleFont = GTA.UI.Font.Pricedown;
		Subtitle = SMH.GetSubtitle(type, _target.Money);

		SwitchItem = new SwitchItem(type);
		Add((SwitchItem)SwitchItem);
		AddDrugListItems(_source, _target);

		_source.PropertyChanged += OnInventoryPropertyChanged;
		_target.PropertyChanged += OnInventoryPropertyChanged;
	}

	public void Add(ObjectPool processables)
		=> processables.Add(this);

	public void SetVisible(bool value)
		=> Visible = value;

	private void OnMenuItemActivated(object sender, EventArgs args)
	{
		if (sender is not SideMenu menu || menu.SelectedItem is not DrugListItem item || item.SelectedItem.Equals(0) || item.Tag is not DrugType drugType)
			return;

		int price = _target.Where(x => x.Type.Equals(drugType)).Select(x => x.CurrentPrice).Single();
		int quantity = item.SelectedItem;
		bool succes = _transaction.Commit(drugType, quantity, price);

		if (succes)
		{
			ITransaction transaction = DomainFactory.CreateTransaction(DateTime.Now, _type, drugType, quantity, price);
			_player.AddTransaction(transaction);
		}
	}

	private void OnInventoryPropertyChanged(object sender, PropertyChangedEventArgs args)
	{
		if (args.PropertyName.Equals(nameof(_target.Money), StringComparison.Ordinal))
			Subtitle = SMH.GetSubtitle(_type, _target.Money);
	}

	/// <summary>
	/// Adds the drug list items to the menu.
	/// </summary>
	/// <param name="source">The source inventory.</param>
	/// <param name="target">The target inventory.</param>
	private void AddDrugListItems(IInventory source, IInventory target)
	{
		var drugs = from s in source
								join t in target
								on s.Type equals t.Type
								select new { s, t };

		foreach (var drug in drugs)
		{
			DrugListItem item = new(drug.s, drug.t);
			item.Activated += OnMenuItemActivated;
			Add(item);
		}
	}
}
