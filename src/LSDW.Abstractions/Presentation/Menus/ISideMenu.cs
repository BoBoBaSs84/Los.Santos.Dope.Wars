using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Presentation.Items;
using LSDW.Abstractions.Presentation.Menus.Base;

namespace LSDW.Abstractions.Presentation.Menus;

/// <summary>
/// The side menu interface.
/// </summary>
public interface ISideMenu : IMenu
{
	/// <summary>
	/// Is the menu initialized?
	/// </summary>
	bool Initialized { get; }

	/// <summary>
	/// The item for switching between the side menus.
	/// </summary>
	ISwitchItem SwitchItem { get; }

	/// <summary>
	/// Cleans up the menu.
	/// </summary>
	void CleanUp();

	/// <summary>
	/// Initializes the menu.
	/// </summary>
	/// <param name="player">The player instance to use.</param>
	/// <param name="inventory">The inventory instance to use.</param>
	void Initialize(IPlayer player, IInventory inventory);
}
