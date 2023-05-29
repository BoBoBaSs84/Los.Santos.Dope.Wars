using LemonUI.Menus;

namespace LSDW.Extensions;

/// <summary>
/// The native menu extensions class.
/// </summary>
public static class NativeMenuExtensions
{
	/// <summary>
	/// Adds multiple items to the menu.
	/// </summary>
	/// <param name="nativeMenu">The menu to work with.</param>
	/// <param name="items">The items to add.</param>
	public static NativeMenu Add(this NativeMenu nativeMenu, IEnumerable<NativeItem> items)
	{
		foreach (NativeItem item in items)
			nativeMenu.Add(item);
		return nativeMenu;
	}

	/// <summary>
	/// Adds a multiple menus as a submenu with an item.
	/// </summary>
	/// <param name="nativeMenu">The menu to work with.</param>
	/// <param name="menus">The items to add.</param>
	public static NativeMenu Add(this NativeMenu nativeMenu, IEnumerable<NativeMenu> menus)
	{
		foreach (NativeMenu menu in menus)
			nativeMenu.Add(menu);
		return nativeMenu;
	}

	/// <summary>
	/// Removes multiple items from the menu.
	/// </summary>
	/// <param name="nativeMenu">The menu to work with.</param>
	/// <param name="items">The items to remove.</param>
	public static NativeMenu Remove(this NativeMenu nativeMenu, IEnumerable<NativeItem> items)
	{
		foreach (NativeItem item in items)
			nativeMenu.Remove(item);
		return nativeMenu;
	}
}
