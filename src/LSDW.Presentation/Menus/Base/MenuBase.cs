using LemonUI;
using LemonUI.Menus;
using LSDW.Abstractions.Presentation.Menus;
using GTAFont = GTA.UI.Font;

namespace LSDW.Presentation.Menus.Base;

/// <summary>
/// The menu base class.
/// </summary>
internal abstract class MenuBase : NativeMenu, IMenuBase
{
	/// <summary>
	/// The pool of menus.
	/// </summary>
	internal static readonly ObjectPool Processables = new();

	public IMenuBase? LatestMenu { get; private set; }

	/// <summary>
	///	Creates a new menu.
	/// </summary>
	/// <param name="title">The banner text to display in the header.</param>
	protected MenuBase(string title) : this(title, string.Empty, string.Empty)
	{ }

	/// <summary>
	///	Creates a new menu.
	/// </summary>
	/// <param name="title">The banner text to display in the header.</param>
	/// <param name="subtitle">The name to display below the header.</param>
	protected MenuBase(string title, string subtitle) : this(title, subtitle, string.Empty)
	{ }

	/// <summary>
	///	Creates a new menu.
	/// </summary>
	/// <param name="title">The text to display in the header.</param>
	/// <param name="subtitle">The name to display below the header.</param>
	/// <param name="description">The description of the menu.</param>
	protected MenuBase(string title, string subtitle, string description) : base(title, subtitle, description)
	{
		Shown += OnShown;
		Processables.Add(this);
	}

	/// <summary>
	/// Adds a new item to the menu.
	/// </summary>
	/// <param name="title">The title of the item.</param>
	/// <param name="description">The description when the item is selected.</param>
	/// <param name="activated">The action to perform when activated.</param>
	/// <returns>The item.</returns>
	protected NativeItem AddItem(string title, string description = "", Action? activated = null)
	{
		NativeItem item = new(title, description);
		item.Activated += (sender, args) => activated?.Invoke();
		Add(item);
		return item;
	}

	/// <summary>
	/// Adds a new checkbox item to the menu.
	/// </summary>
	/// <param name="title">The title of the item.</param>
	/// <param name="description">The description when the item is selected.</param>
	/// <param name="defaultValue">The default value of the checkbox Checked state.</param>
	/// <param name="changed">The action to perform when the checkbox Checked state changes.</param>
	/// <returns>The checkbox item.</returns>
	protected NativeCheckboxItem AddCheckbox(string title, string description = "", bool defaultValue = false, Action<bool>? changed = null)
	{
		NativeCheckboxItem item = new(title, description, defaultValue);
		item.CheckboxChanged += (sender, args) => changed?.Invoke(item.Checked);
		Add(item);
		return item;
	}

	/// <summary>
	/// Adds a new list item to the menu.
	/// </summary>
	/// <typeparam name="T">The object type of the values.</typeparam>
	/// <param name="title">The title of the item.</param>
	/// <param name="description">The description when the item is selected.</param>
	/// <param name="action">The action to perform when the selected item of the list changes.</param>
	/// <param name="items">The items array.</param>
	/// <returns>The list item.</returns>
	protected NativeListItem<T> AddListItem<T>(string title, string description = "", Action<T, int>? action = null, params T[] items)
	{
		NativeListItem<T> item = new(title, description, items);
		item.ItemChanged += (sender, args) => action?.Invoke(item.SelectedItem, item.SelectedIndex);
		Add(item);
		return item;
	}

	/// <summary>
	/// Adds a new list item to the menu.
	/// </summary>
	/// <typeparam name="T">The object type of the values.</typeparam>
	/// <param name="title">The title of the item.</param>
	/// <param name="description">The description when the item is selected.</param>
	/// <param name="activated">The action to perform when activated.</param>
	/// <param name="changed">The action to perform when the selected item of the list changes.</param>
	/// <param name="items">The items array.</param>
	/// <returns>The list item.</returns>
	protected NativeListItem<T> AddListItem<T>(string title, string description = "", Action<NativeListItem<T>, EventArgs>? activated = null, Action<NativeListItem<T>, ItemChangedEventArgs<T>>? changed = null, params T[] items)
	{
		NativeListItem<T> item = new(title, description, items);
		item.Activated += (sender, args) => activated?.Invoke(item, args);
		item.ItemChanged += (sender, args) => changed?.Invoke(item, args);
		Add(item);
		return item;
	}

	/// <summary>
	/// Adds a new submenu and associated item to the menu.
	/// </summary>
	/// <param name="subMenu">The Sub Menu to add.</param>
	/// <param name="altTitle">The alternative title of the item shown on the right.</param>
	/// <param name="altTitleFont">The font of alternative title item shown on the right.</param>
	/// <returns>The item associated with the sub menu.</returns>
	protected NativeSubmenuItem AddMenu(MenuBase subMenu, string altTitle = "Menu", GTAFont altTitleFont = GTAFont.ChaletComprimeCologne)
	{
		NativeSubmenuItem subMenuItem = AddSubMenu(subMenu);
		subMenuItem.AltTitle = altTitle;
		subMenuItem.AltTitleFont = altTitleFont;
		return subMenuItem;
	}

	private void OnShown(object sender, EventArgs args)
		=> LatestMenu = this;
}
