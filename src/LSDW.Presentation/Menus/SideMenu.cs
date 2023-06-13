using LemonUI;
using LemonUI.Menus;
using LSDW.Abstractions.Presentation.Items;
using LSDW.Abstractions.Presentation.Menus;
using LSDW.Domain.Enumerators;
using LSDW.Domain.Factories;
using LSDW.Domain.Interfaces.Models;
using LSDW.Domain.Interfaces.Services;
using LSDW.Presentation.Items;
using SMH = LSDW.Presentation.Helpers.SideMenuHelper;

namespace LSDW.Presentation.Menus;

/// <summary>
/// The side menu class.
/// </summary>
internal sealed class SideMenu : NativeMenu, ISideMenu
{
	private readonly ObjectPool _processables = new();
	private readonly Size _screenSize = GTA.UI.Screen.Resolution;
	private readonly IInventory _source;
	private readonly IInventory _target;
	private readonly IPlayer _player;
	private readonly ITransactionService _transaction;
	private readonly MenuType _menuType;
	private readonly TransactionType _transactionType;

	public ISwitchItem SwitchItem { get; }

	/// <summary>
	/// Initializes a instance of the side menu class.
	/// </summary>
	/// <param name="menuType">The menu type.</param>
	/// <param name="player">The current player.</param>
	/// <param name="inventory">The opposition inventory.</param>
	internal SideMenu(MenuType menuType, IPlayer player, IInventory inventory) : base(SMH.GetTitle(menuType))
	{
		_menuType = menuType;
		_player = player;

		(_source, _target) = SMH.GetInventories(_menuType, _player, inventory);

		int maximumQuantity = SMH.GetMaximumQuantity(_menuType, _player);
		_transactionType = SMH.GetTransactionType(_menuType);
		_transaction = DomainFactory.CreateTransactionService(_transactionType, _source, _target, maximumQuantity);

		Alignment = SMH.GetAlignment(_menuType);
		ItemCount = CountVisibility.Never;
		Offset = new PointF(_screenSize.Width / 64, _screenSize.Height / 36);
		UseMouse = false;
		TitleFont = GTA.UI.Font.Pricedown;
		Subtitle = SMH.GetSubtitle(_menuType, _target.Money);

		SwitchItem = new SwitchItem(_menuType);
		Add((SwitchItem)SwitchItem);
		AddDrugListItems(_source, _target);

		_source.PropertyChanged += OnInventoryPropertyChanged;
		_target.PropertyChanged += OnInventoryPropertyChanged;

		_processables.Add(this);
	}

	public void SetVisible(bool value)
		=> Visible = value;

	public void OnTick(object sender, EventArgs args)
		=> _processables.Process();

	private void OnMenuItemActivated(object sender, EventArgs args)
	{
		if (sender is not SideMenu menu || menu.SelectedItem is not DrugListItem item || item.SelectedItem.Equals(0) || item.Tag is not DrugType drugType)
			return;

		int price = _target.Where(x => x.Type.Equals(drugType)).Select(x => x.Price).Single();
		int quantity = item.SelectedItem;
		bool succes = _transaction.Commit(drugType, quantity, price);

		if (succes)
		{
			ITransaction transaction = DomainFactory.CreateTransaction(DateTime.Now, _transactionType, drugType, quantity, price);
			_player.AddTransaction(transaction);
		}

		foreach (string message in _transaction.Errors)
			GTA.UI.Screen.ShowSubtitle(message);
	}

	private void OnInventoryPropertyChanged(object sender, PropertyChangedEventArgs args)
	{
		if (args.PropertyName.Equals(nameof(_target.Money), StringComparison.Ordinal))
			Subtitle = SMH.GetSubtitle(_menuType, _target.Money);
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
