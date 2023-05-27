using GTA;
using LemonUI;
using LemonUI.Menus;
using LSDW.Core.Enumerators;
using LSDW.Core.Interfaces.Classes;
using LSDW.Factories;

namespace LSDW.UserInterfaces.Trafficking;

/// <summary>
/// The traffic menu class.
/// </summary>
public sealed class TrafficMenu : Script
{
	private readonly ObjectPool _objectPool = new();
	private readonly ITransaction? _transaction;
	private readonly TransactionType _transactionType;
	private readonly IInventory _leftSideInventory;
	private readonly IInventory _rightSideInventory;
	private readonly SideMenu _leftSideMenu;
	private readonly SideMenu _rightSideMenu;

	/// <summary>
	/// Shows the menu or not.
	/// </summary>
	public bool ShowMenu { get; set; }

	/// <summary>
	/// Initializes a instance of the traffic menu class.
	/// </summary>
	/// <param name="transactionType">The transaction type for the menu.</param>
	/// <param name="leftSideInventory">The inventory for the left side.</param>
	/// <param name="rightSideInventory">The inventory for the right side.</param>
	public TrafficMenu(TransactionType transactionType, IInventory leftSideInventory, IInventory rightSideInventory)
	{
		_transactionType = transactionType;
		_leftSideInventory = leftSideInventory;
		_rightSideInventory = rightSideInventory;
		_leftSideMenu = MenuFactory.GetLeftSideMenu(transactionType, leftSideInventory);
		_rightSideMenu = MenuFactory.GetRightSideMenu(transactionType, rightSideInventory);
		_objectPool.Add(_leftSideMenu);
		_objectPool.Add(_rightSideMenu);

		Tick += OnTick;
	}

	private void MenuSwitchItemActivated(object sender, EventArgs args)
	{
		bool leftSideVisible = _leftSideMenu.Visible;
		bool rightSideVisible = _rightSideMenu.Visible;

		_leftSideMenu.Visible = !leftSideVisible;
		_rightSideMenu.Visible = !rightSideVisible;
	}

	private void OnTick(object sender, EventArgs args)
	{
		if (ShowMenu)
		{
			_leftSideMenu.Visible = true;
			_rightSideMenu.Visible = false;
		}
		else
		{
			_leftSideMenu.Visible = false;
			_rightSideMenu.Visible = false;
		}
		_objectPool.Process();
	}

	private void DrugItemActivated(object sender, EventArgs args)
	{
		if (sender is DrugItem)
		{
			RefreshLeftMenu();
			RefreshRightMenu();
		}
	}

	private void RefreshLeftMenu()
		=> RefreshMenu(_leftSideMenu, MenuFactory.GetRightSideMenu(_transactionType, _leftSideInventory));

	private void RefreshRightMenu()
		=> RefreshMenu(_rightSideMenu, MenuFactory.GetRightSideMenu(_transactionType, _rightSideInventory));

	private void RefreshMenu(SideMenu oldMenu, SideMenu newMenu)
	{
		int selectedIndex = oldMenu.SelectedIndex;
		foreach (NativeItem item in oldMenu.Items)
			item.Activated -= DrugItemActivated;
		oldMenu.Clear();

		oldMenu = newMenu;

		foreach (IDrug drug in newMenu.Inventory)
		{
			DrugItem item = new(drug);
			item.Activated += DrugItemActivated;
			newMenu.Add(item);
		}

		if (selectedIndex > -1)
			newMenu.SelectedIndex = selectedIndex;
	}
}
