using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Abstractions.Presentation.Menus;
using LSDW.Presentation.Menus.Base;
using LSDW.Presentation.Properties;
using System.ComponentModel;

namespace LSDW.Presentation.Menus;

/// <summary>
/// The settings menu class.
/// </summary>
internal sealed partial class SettingsMenu : MenuBase, ISettingsMenu
{
	private readonly ISettingsService _settingsService;

	/// <summary>
	/// Initializes a instance of the settings menu class.
	/// </summary>
	/// <param name="serviceManager">The service manager instance to use.</param>
	internal SettingsMenu(IServiceManager serviceManager) : base(Resources.UI_SettingsMenu_Title, Resources.UI_SettingsMenu_Name)
	{
		_settingsService = serviceManager.SettingsService;
		Closing += OnClosing;
		AddMenuItems();
	}

	private void OnClosing(object sender, CancelEventArgs args)
		=> _settingsService.Save();
}
