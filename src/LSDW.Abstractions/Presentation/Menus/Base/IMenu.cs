using LemonUI;

namespace LSDW.Abstractions.Presentation.Menus.Base;

/// <summary>
/// The menu base interface.
/// </summary>
public interface IMenu
{
	/// <summary>
	/// Gets or sets the menu visible.
	/// </summary>
	bool Visible { get; set; }

	/// <summary>
	/// Add the menu to process pool.
	/// </summary>
	/// <param name="processables">The manager for menus and items.</param>
	void Add(ObjectPool processables);
}
