using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Abstractions.Presentation.Menus;
using LSDW.Domain.Enumerators;
using LSDW.Domain.Interfaces.Models;
using LSDW.Presentation.Menus;

namespace LSDW.Presentation.Factories;

/// <summary>
/// The presentation factory class.
/// </summary>
public static class PresentationFactory
{
	/// <summary>
	/// Creates a new instance of the settings menu.
	/// </summary>
	/// <param name="settingsService">The settings service.</param>
	public static ISettingsMenu CreateSettingsMenu(ISettingsService settingsService)
		=> new SettingsMenu(settingsService);

	/// <summary>
	/// Creates a new instance of the side menu.
	/// </summary>
	/// <param name="menuType">The menu type.</param>
	/// <param name="player">The current player.</param>
	/// <param name="inventory">The opposition inventory.</param>
	public static ISideMenu CreateSideMenu(MenuType menuType, IPlayer player, IInventory inventory)
		=> new SideMenu(menuType, player, inventory);
}
