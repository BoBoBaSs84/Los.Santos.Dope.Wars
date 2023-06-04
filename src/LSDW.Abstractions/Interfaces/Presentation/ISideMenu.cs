using LSDW.Abstractions.Interfaces.Presentation.Base;

namespace LSDW.Abstractions.Interfaces.Presentation;

/// <summary>
/// The side menu interface.
/// </summary>
public interface ISideMenu : IMenu
{
	/// <summary>
	/// The item for switching between the side menus.
	/// </summary>
	ISwitchItem SwitchItem { get; }
}
