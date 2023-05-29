using GTA.UI;
using LemonUI;
using LemonUI.Menus;
using LSDW.Core.Enumerators;
using LSDW.Core.Extensions;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;
using RESX = LSDW.Properties.Resources;

namespace LSDW.Classes.UI;

/// <summary>
/// The side menu class.
/// </summary>
public sealed class SideMenu : NativeMenu
{
	private readonly Size _screenSize = GTA.UI.Screen.Resolution;
	private readonly ObjectPool _processables = new();
	private readonly MenuType _menuType;
	private readonly IInventory _source;
	private readonly IInventory _target;
	private readonly ITransaction _transaction;

	/// <summary>
	/// The menu switch item.
	/// </summary>
	internal SwitchItem SwitchItem { get; }

	/// <summary>
	/// Initializes a instance of the side menu class.
	/// </summary>
	/// <param name="menuType">The menu type.</param>
	/// <param name="source">The source inventory.</param>
	/// <param name="target">The target inventory.</param>
	internal SideMenu(MenuType menuType, IInventory source, IInventory target) : base(GetTitle(menuType))
	{
		_menuType = menuType;
		_source = source;
		_target = target;
		_transaction = TransactionFactory.CreateTransaction(menuType, source, target);

		Alignment = GetAlignment(menuType);
		ItemCount = CountVisibility.Never;
		Offset = new PointF(_screenSize.Width / 64, _screenSize.Height / 36);
		UseMouse = false;
		TitleFont = GTA.UI.Font.Pricedown;
		Subtitle = GetSubtitle(menuType);

		SwitchItem = new(menuType);
		Add(SwitchItem);
		AddDrugListItems(source, target);

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
			Subtitle = GetSubtitle(_menuType);
	}

	/// <summary>
	/// Returns the alignment for the menu type.
	/// </summary>
	/// <param name="menuType">The type of the menu.</param>
	private static Alignment GetAlignment(MenuType menuType)
		=> menuType is MenuType.BUY or MenuType.RETRIEVE or MenuType.TAKE ? Alignment.Left : Alignment.Right;

	/// <summary>
	/// Returns the title for the menu type.
	/// </summary>
	/// <param name="menuType">The type of the menu.</param>
	private static string GetTitle(MenuType menuType)
		=> menuType switch
		{
			MenuType.BUY => RESX.UI_SideMenu_Title_Buy,
			MenuType.SELL => RESX.UI_SideMenu_Title_Sell,
			MenuType.RETRIEVE => RESX.UI_SideMenu_Title_Retrieve,
			MenuType.STORE => RESX.UI_SideMenu_Title_Store,
			MenuType.GIVE => RESX.UI_SideMenu_Title_Give,
			MenuType.TAKE => RESX.UI_SideMenu_Title_Take,
			_ => string.Empty
		};

	/// <summary>
	/// Returns the subtitle for the menu type.
	/// </summary>
	/// <param name="menuType">The type of the menu.</param>
	private string GetSubtitle(MenuType menuType)
		=> menuType switch
		{
			MenuType.BUY => RESX.UI_SideMenu_Subtitle_Buy.FormatInvariant(_target.Money),
			MenuType.SELL => RESX.UI_SideMenu_Subtitle_Sell.FormatInvariant(_target.Money),
			_ => string.Empty
		};

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
