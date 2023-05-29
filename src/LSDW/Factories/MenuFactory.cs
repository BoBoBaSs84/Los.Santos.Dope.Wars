using LSDW.Classes.UI;
using LSDW.Core.Enumerators;
using LSDW.Core.Interfaces.Classes;
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
	/// <param name="source">The source inventory.</param>
	/// <param name="target">The target inventory.</param>
	public static SideMenu CreateSideMenu(MenuType menuType, IInventory source, IInventory target)
		=> new(menuType, source, target);
}
