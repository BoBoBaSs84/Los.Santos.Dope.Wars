namespace LSDW.Abstractions.Presentation.Menus;

/// <summary>
/// The menu base interface.
/// </summary>
public interface IMenuBase
{
	/// <summary>
	/// Indicates the visibility of the menu.
	/// </summary>
	bool Visible { get; }

	/// <summary>
	/// Toggles the visibility of the menu.
	/// </summary>
	void Toggle();
}