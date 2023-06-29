using LemonUI;
using LemonUI.Menus;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Abstractions.Presentation.Menus;
using RESX = LSDW.Presentation.Properties.Resources;

namespace LSDW.Presentation.Menus;

/// <summary>
/// The settings menu class.
/// </summary>
internal sealed partial class SettingsMenu : NativeMenu, ISettingsMenu
{
	private readonly ISettingsService _settingsService;

	/// <summary>
	/// Initializes a instance of the settings menu class.
	/// </summary>
	/// <param name="serviceManager">The service manager instance to use.</param>
	internal SettingsMenu(IServiceManager serviceManager) : base(RESX.UI_SettingsMenu_Title)
	{
		_settingsService = serviceManager.SettingsService;
		Name = RESX.UI_SettingsMenu_Name;
		Closing += OnClosing;
		AddMenuItems();
	}

	public void Add(ObjectPool processables)
		=> processables.Add(this);

	private void OnClosing(object sender, CancelEventArgs args)
		=> _settingsService.Save();
}
