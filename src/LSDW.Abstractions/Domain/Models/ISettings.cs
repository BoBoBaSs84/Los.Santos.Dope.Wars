using LSDW.Abstractions.Domain.Models.Base;

namespace LSDW.Abstractions.Domain.Models;

/// <summary>
/// The settings interface.
/// </summary>
public partial interface ISettings
{
	/// <summary>
	/// The settings file name.
	/// </summary>
	string IniFileName { get; }

	/// <summary>
	/// The log file name.
	/// </summary>
	string LogFileName { get; }

	/// <summary>
	/// The save file name.
	/// </summary>
	string SaveFileName { get; }

	/// <summary>
	/// The dealer settings.
	/// </summary>
	IDealerSettings Dealer { get; }

	/// <summary>
	/// The market settings.
	/// </summary>
	IMarketSettings Market { get; }

	/// <summary>
	/// The player settings.
	/// </summary>
	IPlayerSettings Player { get; }

	/// <summary>
	/// The trafficking settings.
	/// </summary>
	ITraffickingSettings Trafficking { get; }
}

/// <summary>
/// The dealer settings interface.
/// </summary>
public partial interface IDealerSettings
{
	/// <summary>
	/// The down time in hours property.
	/// </summary>
	IBindableProperty<int> DownTimeInHours { get; }

	/// <summary>
	/// The dealer has armor property.
	/// </summary>
	IBindableProperty<bool> HasArmor { get; }

	/// <summary>
	/// The dealer has weapons property.
	/// </summary>
	IBindableProperty<bool> HasWeapons { get; }

	/// <summary>
	/// Returns the possible dealer down time values.
	/// </summary>
	/// <returns>The list of possible values.</returns>
	int[] GetDownTimeInHoursValues();
}

/// <summary>
/// The market settings interface.
/// </summary>
public partial interface IMarketSettings
{
	/// <summary>
	/// The inventory change interval property.
	/// </summary>
	IBindableProperty<int> InventoryChangeInterval { get; }

	/// <summary>
	/// The maximum drug price factor.
	/// </summary>
	IBindableProperty<float> MaximumDrugPrice { get; }

	/// <summary>
	/// The minimum drug price factor.
	/// </summary>
	IBindableProperty<float> MinimumDrugPrice { get; }

	/// <summary>
	/// The price change interval property.
	/// </summary>
	IBindableProperty<int> PriceChangeInterval { get; }

	/// <summary>
	/// The special offer chance property.
	/// </summary>
	IBindableProperty<float> SpecialOfferChance { get; }

	/// <summary>
	/// Returns the possible inventory change interval values.
	/// </summary>
	/// <returns>The list of possible values.</returns>
	int[] GetInventoryChangeIntervalValues();

	/// <summary>
	/// Returns the possible maximum drug price factor values.
	/// </summary>
	/// <returns>The list of possible values.</returns>
	float[] GetMaximumDrugPriceValues();

	/// <summary>
	/// Returns the possible minimum drug price factor values.
	/// </summary>
	/// <returns>The list of possible values.</returns>
	float[] GetMinimumDrugPriceValues();

	/// <summary>
	/// Returns the possible price change interval values.
	/// </summary>
	/// <returns>The list of possible values.</returns>
	int[] GetPriceChangeIntervalValues();

	/// <summary>
	/// Returns the possible special offer chance values.
	/// </summary>
	/// <returns>The list of possible values.</returns>
	float[] GetSpecialOfferChanceValues();
}

/// <summary>
/// The player settings interface.
/// </summary>
public partial interface IPlayerSettings
{
	/// <summary>
	/// The experience multiplier property.
	/// </summary>
	IBindableProperty<float> ExperienceMultiplier { get; }

	/// <summary>
	/// The inventory expansion per level property.
	/// </summary>
	IBindableProperty<int> InventoryExpansionPerLevel { get; }

	/// <summary>
	/// The loose drugs on death property.
	/// </summary>
	IBindableProperty<bool> LooseDrugsOnDeath { get; }

	/// <summary>
	/// The loose drugs when busted property.
	/// </summary>
	IBindableProperty<bool> LooseDrugsWhenBusted { get; }

	/// <summary>
	/// The loose money on death property.
	/// </summary>
	IBindableProperty<bool> LooseMoneyOnDeath { get; }

	/// <summary>
	/// The loose money when busted property.
	/// </summary>
	IBindableProperty<bool> LooseMoneyWhenBusted { get; }

	/// <summary>
	/// The starting inventory property.
	/// </summary>
	IBindableProperty<int> StartingInventory { get; }

	/// <summary>
	/// Returns the possible experience multiplier factor values.
	/// </summary>
	/// <returns>The list of possible values.</returns>
	float[] GetExperienceMultiplierValues();

	/// <summary>
	/// Returns the possible inventory expansion per level values.
	/// </summary>
	/// <returns>The list of possible values.</returns>
	int[] GetInventoryExpansionPerLevelValues();

	/// <summary>
	/// Returns the possible starting inventory values.
	/// </summary>
	/// <returns>The list of possible values.</returns>
	int[] GetStartingInventoryValues();
}

/// <summary>
/// The trafficking settings interface.
/// </summary>
public partial interface ITraffickingSettings
{
	/// <summary>
	/// The bust chance property.
	/// </summary>
	IBindableProperty<float> BustChance { get; }

	/// <summary>
	/// The discover dealer property.
	/// </summary>
	IBindableProperty<bool> DiscoverDealer { get; }

	/// <summary>
	/// The wanted level property.
	/// </summary>
	IBindableProperty<int> WantedLevel { get; }

	/// <summary>
	/// Returns the possible bust chance values.
	/// </summary>
	/// <returns>The list of possible values.</returns>
	float[] GetBustChanceValues();

	/// <summary>
	/// Returns the possible wanted level values.
	/// </summary>
	/// <returns>The list of possible values.</returns>
	int[] GetWantedLevelValues();
}
