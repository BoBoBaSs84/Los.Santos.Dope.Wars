using LSDW.Domain.Models;
using LSDW.Presentation.Bindings;
using LSDW.Presentation.Extensions;
using LSDW.Presentation.Menus.Base;

namespace LSDW.Presentation.Menus;

/// <summary>
/// Represents the player settings menu, allowing users to configure settings related to the player.
/// </summary>
internal sealed class PlayerSettingsMenu : BaseMenu
{
	/// <summary>
	/// Initializes a new instance of the <see cref="PlayerSettingsMenu"/> class.
	/// </summary>
	/// <param name="settings">The settings instance to be used by the player settings menu.</param>
	public PlayerSettingsMenu(PlayerSettings settings) : base("Player", "Player Settings", "Configure settings related to the player.")
		=> AddDatabindings(settings);

	private void AddDatabindings(PlayerSettings settings)
	{
		IPropertyBinding experienceMultiplierBinding = AddListItem("Experience Multiplier", "The experience multiplier for the player.", PlayerSettings.GetExperienceMultiplierValues)
			.BindTo(settings, nameof(PlayerSettings.ExperienceMultiplier));
		Bindings.Add(experienceMultiplierBinding);

		IPropertyBinding inventoryExpansionBinding = AddListItem("Inventory Expansion Per Level", "The inventory expansion per level for the player.", PlayerSettings.GetInventoryExpansionPerLevelValues)
			.BindTo(settings, nameof(PlayerSettings.InventoryExpansionPerLevel));
		Bindings.Add(inventoryExpansionBinding);

		IPropertyBinding looseDrugsOnDeathBinding = AddCheckbox("Loose Drugs On Death", "Whether the player loses drugs on death.")
			.BindTo(settings, nameof(PlayerSettings.LooseDrugsOnDeath));
		Bindings.Add(looseDrugsOnDeathBinding);

		IPropertyBinding looseDrugsWhenBustedBinding = AddCheckbox("Loose Drugs When Busted", "Whether the player loses drugs when busted.")
			.BindTo(settings, nameof(PlayerSettings.LooseDrugsWhenBusted));
		Bindings.Add(looseDrugsWhenBustedBinding);

		IPropertyBinding startingInventorySizeBinding = AddListItem("Starting Inventory Size", "The starting inventory size for the player.", PlayerSettings.GetStartingInventoryValues)
			.BindTo(settings, nameof(PlayerSettings.StartingInventorySize));
		Bindings.Add(startingInventorySizeBinding);
	}
}
