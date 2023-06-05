using LSDW.Abstractions.Presentation.Items;
using LSDW.Abstractions.Presentation.Menus.Base;

namespace LSDW.Abstractions.Presentation.Menus;

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
