using LemonUI;
using LemonUI.Menus;
using LSDW.Presentation.Bindings;
using GTAFont = GTA.UI.Font;

namespace LSDW.Presentation.Menus.Base;

/// <summary>
/// The abstract base menu class.
/// </summary>
public abstract class BaseMenu : NativeMenu
{
	/// <summary>
	/// Gets the menu object pool.
	/// </summary>
	protected ObjectPool MenuPool { get; }

	/// <summary>
	/// Gets the collection of property bindings associated with this menu.
	/// </summary>
	protected ICollection<IPropertyBinding> Bindings { get; } = [];

	/// <summary>
	///	Initializes a new instance of the <see cref="BaseMenu"/> class.
	/// </summary>
	/// <inheritdoc cref="BaseMenu(string, string, string)"/>/>
	protected BaseMenu(string title) : this(title, string.Empty, string.Empty)
	{ }

	/// <summary>
	///	Initializes a new instance of the <see cref="BaseMenu"/> class.
	/// </summary>
	/// <inheritdoc cref="BaseMenu(string, string, string)"/>/>
	protected BaseMenu(string title, string subtitle) : this(title, subtitle, string.Empty)
	{ }

	/// <summary>
	///	Initializes a new instance of the <see cref="BaseMenu"/> class.
	/// </summary>
	/// <param name="title">The banner text to display in the header.</param>
	/// <param name="subtitle">The name to display below the header.</param>
	/// <param name="description">The description of the menu.</param>
	protected BaseMenu(string title, string subtitle, string description) : base(title, subtitle, description)
		=> MenuPool = [this];

	/// <summary>
	/// Adds a new item to the menu.
	/// </summary>
	/// <param name="title">The title of the item to add.</param>
	/// <param name="description">The description when the item is selected.</param>
	/// <param name="activated">The action to perform when the item is activated.</param>
	/// <returns>The added native menu item.</returns>
	protected NativeItem AddItem(string title, string description = "", Action? activated = null)
		=> AddItem(title, description, activated, null);

	/// <summary>
	/// Adds a new item to the menu.
	/// </summary>
	/// <param name="title">The title of the item to add.</param>
	/// <param name="description">The description when the item is selected.</param>
	/// <param name="activated">The action to perform when the item is activated.</param>
	/// <param name="selected">The action to perform when the item is selected.</param>
	/// <returns>The added native menu item.</returns>
	protected NativeItem AddItem(string title, string description = "", Action? activated = null, Action? selected = null)
	{
		NativeItem item = new(title, description);
		item.Activated += (s, e) => activated?.Invoke();
		item.Selected += (s, e) => selected?.Invoke();
		Add(item);
		return item;
	}

	/// <summary>
	/// Adds a new checkbox item to the menu.
	/// </summary>
	/// <param name="title">The title of the item.</param>
	/// <param name="description">The description when the item is selected.</param>
	/// <returns>The added native checkbox item.</returns>
	protected NativeCheckboxItem AddCheckbox(string title, string description)
		=> AddCheckbox(title, description, default, null, null);

	/// <summary>
	/// Adds a new checkbox item to the menu.
	/// </summary>
	/// <param name="title">The title of the item.</param>
	/// <param name="defaultValue">The default value of the checkbox Checked state.</param>
	/// <param name="changed">The action to perform when the checkbox Checked state changes.</param>
	/// <returns>The checkbox item.</returns>
	protected NativeCheckboxItem AddCheckbox(string title, bool defaultValue = false, Action<bool>? changed = null)
		=> AddCheckbox(title, string.Empty, defaultValue, changed, null);

	/// <summary>
	/// Adds a new checkbox item to the menu.
	/// </summary>
	/// <param name="title">The title of the item.</param>
	/// <param name="description">The description when the item is selected.</param>
	/// <param name="defaultValue">The default value of the checkbox Checked state.</param>
	/// <param name="changed">The action to perform when the checkbox Checked state changes.</param>
	/// <returns>The added native checkbox item.</returns>
	protected NativeCheckboxItem AddCheckbox(string title, string description, bool defaultValue = false, Action<bool>? changed = null)
		=> AddCheckbox(title, description, defaultValue, changed, null);

	/// <summary>
	/// Adds a new checkbox item to the menu.
	/// </summary>
	/// <param name="title">The title of the item.</param>
	/// <param name="defaultValue">The default value of the checkbox Checked state.</param>
	/// <param name="changed">The action to perform when the checkbox Checked state changes.</param>
	/// <param name="selected">The action to perform when the checkbox is selected.</param>
	/// <returns>The added native checkbox item.</returns>
	protected NativeCheckboxItem AddCheckbox(string title, bool defaultValue = false, Action<bool>? changed = null, Action? selected = null)
		=> AddCheckbox(title, string.Empty, defaultValue, changed, selected);

	/// <summary>
	/// Adds a new checkbox item to the menu.
	/// </summary>
	/// <param name="title">The title of the item.</param>
	/// <param name="description">The description when the item is selected.</param>
	/// <param name="defaultValue">The default value of the checkbox Checked state.</param>
	/// <param name="changed">The action to perform when the checkbox Checked state changes.</param>
	/// <param name="selected">The action to perform when the checkbox is selected.</param>
	/// <returns>The added native checkbox item.</returns>
	protected NativeCheckboxItem AddCheckbox(string title, string description, bool defaultValue = false, Action<bool>? changed = null, Action? selected = null)
	{
		NativeCheckboxItem item = new(title, description, defaultValue);
		item.CheckboxChanged += (s, e) => changed?.Invoke(item.Checked);
		item.Selected += (s, e) => selected?.Invoke();
		Add(item);
		return item;
	}

	/// <summary>
	/// Adds a new list item to the menu.
	/// </summary>
	/// <typeparam name="T">The object type of the values.</typeparam>
	/// <param name="title">The title of the item.</param>
	/// <param name="description">The description when the item is selected.</param>
	/// <param name="items">The items array.</param>
	/// <returns>The added native list item.</returns>
	protected NativeListItem<T> AddListItem<T>(string title, string description, params T[] items)
		=> AddListItem(title, description, activated: null, changed: null, selected: null, items);

	/// <summary>
	/// Adds a new list item to the menu.
	/// </summary>
	/// <typeparam name="T">The object type of the values.</typeparam>
	/// <param name="title">The title of the list item.</param>
	/// <param name="activated">The action to perform when the item is activated.</param>
	/// <param name="items">The items to add to the list.</param>
	/// <returns>The added native list item.</returns>
	protected NativeListItem<T> AddListItem<T>(string title, Action<T>? activated = null, params T[] items)
		=> AddListItem(title, string.Empty, activated, null, null, items);

	/// <summary>
	/// Adds a new list item to the menu.
	/// </summary>
	/// <typeparam name="T">The object type of the values.</typeparam>
	/// <param name="title">The title of the list item.</param>
	/// <param name="description">The description when the item is selected.</param>
	/// <param name="activated">The action to perform when the item is activated.</param>
	/// <param name="items">The items to add to the list.</param>
	/// <returns>The added native list item.</returns>
	protected NativeListItem<T> AddListItem<T>(string title, string description, Action<T>? activated = null, params T[] items)
		=> AddListItem(title, description, activated, null, null, items);

	/// <summary>
	/// Adds a new list item to the menu.
	/// </summary>
	/// <typeparam name="T">The object type of the values.</typeparam>
	/// <param name="title">The title of the item.</param>
	/// <param name="activated">The action to perform when the item is activated.</param>
	/// <param name="changed">The action to perform when the selected item of the list changes.</param>
	/// <param name="items">The items to add to the list.</param>
	/// <returns>The added native list item.</returns>
	protected NativeListItem<T> AddListItem<T>(string title, Action<T>? activated = null, Action<T, int>? changed = null, params T[] items)
		=> AddListItem(title, string.Empty, activated, changed, null, items);

	/// <summary>
	/// Adds a new list item to the menu.
	/// </summary>
	/// <typeparam name="T">The object type of the values.</typeparam>
	/// <param name="title">The title of the item.</param>
	/// <param name="description">The description when the item is selected.</param>
	/// <param name="activated">The action to perform when the item is activated.</param>
	/// <param name="changed">The action to perform when the selected item of the list changes.</param>
	/// <param name="selected">The action to perform when the item is selected.</param>
	/// <param name="items">The items array.</param>
	/// <returns>The added native list item.</returns>
	protected NativeListItem<T> AddListItem<T>(string title, string description, Action<T>? activated = null, Action<T, int>? changed = null, Action? selected = null, params T[] items)
	{
		NativeListItem<T> item = new(title, description, items);
		item.Activated += (s, e) => activated?.Invoke(item.SelectedItem);
		item.ItemChanged += (s, e) => changed?.Invoke(item.SelectedItem, item.SelectedIndex);
		item.Selected += (s, e) => selected?.Invoke();
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
	protected NativeSubmenuItem AddMenu(BaseMenu subMenu, string altTitle = "Menu", GTAFont altTitleFont = GTAFont.ChaletComprimeCologne)
	{
		NativeSubmenuItem subMenuItem = AddSubMenu(subMenu);
		subMenuItem.AltTitle = altTitle;
		subMenuItem.AltTitleFont = altTitleFont;
		return subMenuItem;
	}
}
