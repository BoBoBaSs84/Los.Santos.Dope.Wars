using LemonUI;

namespace LSDW.Abstractions.Presentation.Menus.Base;

/// <summary>
/// The menu base interface.
/// </summary>
public interface IMenu
{
	/// <summary>
	/// Add the menu to process pool.
	/// </summary>
	/// <param name="processables">The manager for menus and items.</param>
	void Add(ObjectPool processables);

	/// <summary>
	/// Sets the menu visibility.
	/// </summary>
	/// <param name="value"><see langword="true"/> or <see langword="false"/></param>
	void SetVisible(bool value);
}
