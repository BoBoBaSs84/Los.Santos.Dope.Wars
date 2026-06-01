using LSDW.Domain.Models;
using LSDW.Presentation.Bindings;
using LSDW.Presentation.Extensions;
using LSDW.Presentation.Menus.Base;

namespace LSDW.Presentation.Menus;

internal sealed class GeneralSettingsMenu : BaseMenu
{
	/// <summary>
	/// Initializes a new instance of the <see cref="GeneralSettingsMenu"/> class.
	/// </summary>
	/// <param name="settings">The settings instance to be used by the general settings menu.</param>
	public GeneralSettingsMenu(GeneralSettings settings) : base("General", "General Settings", "Configure general settings for the modification.")
		=> AddDatabindings(settings);

	private void AddDatabindings(GeneralSettings settings)
	{
		IPropertyBinding enableDebugModeBinding = AddCheckbox("Enable Debug Mode", "Toggle debug mode for the modification.")
			.BindTo(settings, nameof(GeneralSettings.EnableDebugMode));
		Bindings.Add(enableDebugModeBinding);
	}
}
