using LSDW.Classes.UI;
using LSDW.Core.Enumerators;
using LSDW.Core.Interfaces.Models;
using LSDW.Interfaces.Services;

namespace LSDW.Factories;

/// <summary>
/// The menu factory class.
/// </summary>
public static class MenuFactory
{
	/// <summary>
	/// Creates a new instance of the settings menu.
	/// </summary>
	/// <param name="settingsService">The settings service.</param>
	/// <param name="loggerService">The logger service.</param>
	public static SettingsMenu CreateSettingsMenu(ISettingsService settingsService, ILoggerService loggerService)
		=> new(settingsService, loggerService);

	/// <summary>
	/// Creates a new instance of the side menu.
	/// </summary>
	/// <param name="menuType">The menu type.</param>
	/// <param name="player">The current player.</param>
	/// <param name="inventory">The opposition inventory.</param>
	public static SideMenu CreateSideMenu(MenuType menuType, IPlayer player, IInventory inventory)
		=> new(menuType, player, inventory);
}
