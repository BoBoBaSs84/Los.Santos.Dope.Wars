namespace LSDW.Abstractions.Presentation.Menus;

/// <summary>
/// The menu base interface.
/// </summary>
public interface IMenuBase
{
	/// <summary>
	/// The latest active menu. This is used to determine which menu to return to when closing a menu.
	/// </summary>
	IMenuBase? LatestMenu { get; }

	/// <summary>
	/// Indicates the visibility of the menu.
	/// </summary>
	bool Visible { get; set; }
}