using LemonUI;
using LemonUI.Menus;
using LSDW.Core.Enumerators;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;
using SMH = LSDW.Helpers.SideMenuHelper;

namespace LSDW.Classes.UI;

/// <summary>
/// The side menu class.
/// </summary>
public sealed class SideMenu : NativeMenu
{
	private readonly Size _screenSize = GTA.UI.Screen.Resolution;
	private readonly ObjectPool _processables = new();
	private readonly MenuType _menuType;
	private readonly IPlayer _player;
	private readonly ITransaction _transaction;
	private readonly IInventory _source;
	private readonly IInventory _target;

	/// <summary>
	/// The menu switch item.
	/// </summary>
	internal SwitchItem SwitchItem { get; }

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

		(_source, _target) = SMH.GetInventories(menuType, player, inventory);
		int maximumQuantity = SMH.GetMaximumQuantity(menuType, player);

		_transaction = TransactionFactory.CreateTransaction(menuType, _source, _target, maximumQuantity);

		Alignment = SMH.GetAlignment(menuType);
		ItemCount = CountVisibility.Never;
		Offset = new PointF(_screenSize.Width / 64, _screenSize.Height / 36);
		UseMouse = false;
		TitleFont = GTA.UI.Font.Pricedown;
		Subtitle = SMH.GetSubtitle(menuType, _target.Money);

		SwitchItem = new(menuType);
		Add(SwitchItem);
		AddDrugListItems(_source, _target);

		_source.PropertyChanged += OnInventoryPropertyChanged;
		_target.PropertyChanged += OnInventoryPropertyChanged;

		_processables.Add(this);
	}

	internal void OnTick(object sender, EventArgs args)
		=> _processables.Process();

	private void OnMenuItemActivated(object sender, EventArgs args)
	{
		if (sender is not SideMenu menu || menu.SelectedItem is not DrugListItem item || item.SelectedItem.Equals(0) || item.Tag is not DrugType drugType)
			return;

		int price = _target.Where(x => x.DrugType.Equals(drugType)).Select(x => x.Price).Single();
		int quantity = item.SelectedItem;

		_transaction.Add(DrugFactory.CreateDrug(drugType, quantity, price));
		_transaction.Commit();

		if (!_transaction.Result.Successful)
			foreach (string message in _transaction.Result.Messages)
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
								on s.DrugType equals t.DrugType
								select new { s, t };

		foreach (var drug in drugs)
		{
			DrugListItem item = new(drug.s, drug.t);
			item.Activated += OnMenuItemActivated;
			Add(item);
		}
	}
}
