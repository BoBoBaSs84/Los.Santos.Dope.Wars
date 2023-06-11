using LemonUI;
using LemonUI.Menus;
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
	private readonly ObjectPool _processables = new();

	/// <summary>
	/// Initializes a instance of the settings menu class.
	/// </summary>
	/// <param name="settingsService">The settings service.</param>
	internal SettingsMenu(ISettingsService settingsService) : base(RESX.UI_SettingsMenu_Title)
	{
		_settingsService = settingsService;

		Subtitle = RESX.UI_SettingsMenu_Subtitle;
		Closing += OnClosing;

		AddMenuItems();

		_processables.Add(this);
	}

	public void OnTick(object sender, EventArgs args)
		=> _processables.Process();

	public void SetVisible(bool value)
		=> Visible = value;

	private void OnClosing(object sender, CancelEventArgs args)
		=> _settingsService.Save();
}
