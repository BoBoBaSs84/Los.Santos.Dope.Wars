using LSDW.Domain.Models;
using LSDW.Presentation.Bindings;
using LSDW.Presentation.Extensions;
using LSDW.Presentation.Menus.Base;

namespace LSDW.Presentation.Menus;

/// <summary>
/// Represents the trafficking settings menu, allowing users to configure settings related to trafficking.
/// </summary>
internal sealed class TraffickingSettingsMenu : BaseMenu
{
	/// <summary>
	/// Initializes a new instance of the <see cref="TraffickingSettingsMenu"/> class.
	/// </summary>
	/// <param name="settings">The settings instance to be used by the trafficking settings menu.</param>
	public TraffickingSettingsMenu(TraffickingSettings settings) : base("Trafficking", "Trafficking Settings", "Configure settings related to trafficking.")
		=> AddDatabindings(settings);

	private void AddDatabindings(TraffickingSettings settings)
	{
		IPropertyBinding bustChanceBinding = AddListItem("Bust Chance", "The chance of getting busted while trafficking.", TraffickingSettings.GetBustChanceValues)
			.BindTo(settings, nameof(TraffickingSettings.BustChance));
		Bindings.Add(bustChanceBinding);

		IPropertyBinding discoverDealersBinding = AddCheckbox("Discover Dealers On Map", "Whether dealers need to be discovered on the map.")
			.BindTo(settings, nameof(TraffickingSettings.DiscoverDealersOnMap));
		Bindings.Add(discoverDealersBinding);

		IPropertyBinding wantedLevelBinding = AddListItem("Wanted Level Increase On Bust", "The wanted level increase when the player gets busted.", TraffickingSettings.GetWantedLevelValues)
			.BindTo(settings, nameof(TraffickingSettings.WantedLevelIncreaseOnBust));
		Bindings.Add(wantedLevelBinding);
	}
}
