using LSDW.Domain.Models;
using LSDW.Presentation.Bindings;
using LSDW.Presentation.Extensions;
using LSDW.Presentation.Menus.Base;

namespace LSDW.Presentation.Menus;

/// <summary>
/// Represents the dealer settings menu, allowing users to configure settings related to the in-game dealers.
/// </summary>
internal sealed class DealerSettingsMenu : BaseMenu
{
	/// <summary>
	/// Initializes a new instance of the <see cref="DealerSettingsMenu"/> class.
	/// </summary>
	/// <param name="settings">The settings instance to be used by the dealer settings menu.</param>
	public DealerSettingsMenu(DealerSettings settings) : base("Dealer", "Dealer Settings", "Configure settings related to the in-game dealers.")
		=> AddDatabindings(settings);

	private void AddDatabindings(DealerSettings settings)
	{
		IPropertyBinding downTimeInHoursBinding = AddListItem("Down Time In Hours", "The amount of hours a dealer is unavailable after a raid.", DealerSettings.DownTimeInHoursValues)
			.BindTo(settings, nameof(DealerSettings.DownTimeInHours));
		Bindings.Add(downTimeInHoursBinding);

		IPropertyBinding hasArmorBinding = AddCheckbox("Has Armor", "Whether the dealer has armor in their inventory.")
			.BindTo(settings, nameof(DealerSettings.HasArmor));
		Bindings.Add(hasArmorBinding);

		IPropertyBinding hasWeaponsBinding = AddCheckbox("Has Weapons", "Whether the dealer has weapons in their inventory.")
			.BindTo(settings, nameof(DealerSettings.HasWeapons));
		Bindings.Add(hasWeaponsBinding);

		IPropertyBinding inventoryChangeIntervalInHoursBinding = AddListItem("Inventory Change Interval In Hours", "The amount of hours after which the dealer's inventory changes.", DealerSettings.InventoryChangeIntervalValues)
			.BindTo(settings, nameof(DealerSettings.InventoryChangeIntervalInHours));
		Bindings.Add(inventoryChangeIntervalInHoursBinding);
	}
}
