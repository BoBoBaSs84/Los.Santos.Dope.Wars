using BB84.SourceGenerators.Attributes;
using LSDW.Domain.Models.Base;

namespace LSDW.Domain.Models;

/// <summary>
/// Represents the settings for the modification.
/// </summary>
[GenerateIniFile]
public sealed partial class Settings
{
	/// <summary>
	/// Gets the general settings.
	/// </summary>
	[GenerateIniFileSection]
	public GeneralSettings General { get; } = new();

	/// <summary>
	/// Gets the dealer settings.
	/// </summary>
	[GenerateIniFileSection]
	public DealerSettings Dealer { get; } = new();

	/// <summary>
	/// Gets the market settings.
	/// </summary>
	[GenerateIniFileSection]
	public MarketSettings Market { get; } = new();

	/// <summary>
	/// Gets the player settings.
	/// </summary>
	[GenerateIniFileSection]
	public PlayerSettings Player { get; } = new();

	/// <summary>
	/// Gets the trafficking settings.
	/// </summary>
	[GenerateIniFileSection]
	public TraffickingSettings Trafficking { get; } = new();
}

/// <summary>
/// Represents the general settings for the modification.
/// </summary>
public sealed partial class GeneralSettings : NotifiableBase
{
	private bool enableDebugMode;

	/// <summary>
	/// Gets or sets a value indicating whether debug mode is enabled for the modification.
	/// </summary>
	public bool EnableDebugMode
	{
		get => enableDebugMode;
		set => SetProperty(ref enableDebugMode, value, nameof(EnableDebugMode));
	}

	/// <summary>
	/// Gets or sets the name of the INI file.
	/// </summary>
	[GenerateIniFileValue]
	public string IniFileName { get; set; } = "LSDW.ini";

	/// <summary>
	/// Gets or sets the name of the log file.
	/// </summary>
	[GenerateIniFileValue]
	public string LogFileName { get; set; } = "LSDW.log";

	/// <summary>
	/// Gets or sets the name of the save file.
	/// </summary>
	[GenerateIniFileValue]
	public string SaveFileName { get; set; } = "LSDW.sav";
}

/// <summary>
/// Represents the settings for the dealer in the modification.
/// </summary>
public sealed class DealerSettings : NotifiableBase
{
	private int downTimeInHours = 48;
	private bool hasArmor = true;
	private bool hasWeapons = true;
	private int inventoryChangeIntervalInHours = 12;

	/// <summary>
	/// Gets or sets the downtime in hours for the dealer.
	/// </summary>
	[GenerateIniFileValue]
	public int DownTimeInHours
	{
		get => downTimeInHours;
		set => SetProperty(ref downTimeInHours, value, nameof(DownTimeInHours));
	}

	/// <summary>
	/// Gets or sets a value indicating whether the dealer has armor.
	/// </summary>
	[GenerateIniFileValue]
	public bool HasArmor
	{
		get => hasArmor;
		set => SetProperty(ref hasArmor, value, nameof(HasArmor));
	}

	/// <summary>
	/// Gets or sets a value indicating whether the dealer has weapons.
	/// </summary>
	[GenerateIniFileValue]
	public bool HasWeapons
	{
		get => hasWeapons;
		set => SetProperty(ref hasWeapons, value, nameof(HasWeapons));
	}

	/// <summary>
	/// Gets or sets the inventory change interval in hours for the dealer.
	/// </summary>
	[GenerateIniFileValue]
	public int InventoryChangeIntervalInHours
	{
		get => inventoryChangeIntervalInHours;
		set => SetProperty(ref inventoryChangeIntervalInHours, value, nameof(InventoryChangeIntervalInHours));
	}

	/// <summary>
	/// Gets the possible values for the downtime setting.
	/// </summary>
	public static int[] DownTimeInHoursValues
		=> [24, 48, 72, 96, 120, 144, 168];

	/// <summary>
	/// Gets the possible values for the inventory change interval setting.
	/// </summary>
	public static int[] InventoryChangeIntervalValues
		=> [24, 48, 72, 96, 120, 144, 168];
}
/// <summary>
/// Represents the settings for the market in the modification.
/// </summary>
public sealed class MarketSettings : NotifiableBase
{
	private int priceChangeIntervalInHours = 6;
	private float maximumDrugPriceFactor = 1.25f;
	private float minimumDrugPriceFactor = 0.75f;
	private float specialOfferChance = 0.1f;

	/// <summary>
	/// Gets or sets the price change interval in hours for the market.
	/// </summary>
	[GenerateIniFileValue]
	public int PriceChangeIntervalInHours
	{
		get => priceChangeIntervalInHours;
		set => SetProperty(ref priceChangeIntervalInHours, value, nameof(PriceChangeIntervalInHours));
	}

	/// <summary>
	/// Gets or sets the maximum drug price multiplier for the market.
	/// </summary>
	[GenerateIniFileValue]
	public float MaximumDrugPriceFactor
	{
		get => maximumDrugPriceFactor;
		set => SetProperty(ref maximumDrugPriceFactor, value, nameof(MaximumDrugPriceFactor));
	}

	/// <summary>
	/// Gets or sets the minimum drug price multiplier for the market.
	/// </summary>
	[GenerateIniFileValue]
	public float MinimumDrugPriceFactor
	{
		get => minimumDrugPriceFactor;
		set => SetProperty(ref minimumDrugPriceFactor, value, nameof(MinimumDrugPriceFactor));
	}

	/// <summary>
	/// Gets or sets the chance for a special offer to appear in the market.
	/// </summary>
	[GenerateIniFileValue]
	public float SpecialOfferChance
	{
		get => specialOfferChance;
		set => SetProperty(ref specialOfferChance, value, nameof(SpecialOfferChance));
	}

	/// <summary>
	/// Gets the possible values for the special offer chance setting.
	/// </summary>
	public static float[] SpecialOfferChanceValues
		=> [0.5f, 0.10f, 0.15f, 0.20f, 0.25f];

	/// <summary>
	/// Gets the possible values for the maximum drug price multiplier settings.
	/// </summary>
	public static float[] MaximumDrugPriceValues
		=> [1.05f, 1.1f, 1.15f, 1.2f, 1.25f];

	/// <summary>
	/// Gets the possible values for the minimum drug price multiplier settings.
	/// </summary>
	public static float[] MinimumDrugPriceValues
		=> [0.75f, 0.8f, 0.85f, 0.9f, 0.95f];

	/// <summary>
	/// Gets the possible values for the price change interval setting.
	/// </summary>
	public static int[] PriceChangeIntervalValues
		=> [3, 6, 8, 12, 24];
}

/// <summary>
/// Represents the settings for the player in the modification.
/// </summary>
public sealed class PlayerSettings : NotifiableBase
{
	private float experienceMultiplier = 1f;
	private int inventoryExpansionPerLevel = 10;
	private bool looseDrugsOnDeath = true;
	private bool looseDrugsWhenBusted = true;
	private int startingInventorySize = 100;

	/// <summary>
	/// Gets or sets the experience multiplier for the player.
	/// </summary>
	[GenerateIniFileValue]
	public float ExperienceMultiplier
	{
		get => experienceMultiplier;
		set => SetProperty(ref experienceMultiplier, value, nameof(ExperienceMultiplier));
	}

	/// <summary>
	/// Gets or sets the inventory expansion per level for the player.
	/// </summary>
	[GenerateIniFileValue]
	public int InventoryExpansionPerLevel
	{
		get => inventoryExpansionPerLevel;
		set => SetProperty(ref inventoryExpansionPerLevel, value, nameof(InventoryExpansionPerLevel));
	}

	/// <summary>
	/// Gets or sets a value indicating whether the player loses drugs on death.
	/// </summary>
	[GenerateIniFileValue]
	public bool LooseDrugsOnDeath
	{
		get => looseDrugsOnDeath;
		set => SetProperty(ref looseDrugsOnDeath, value, nameof(LooseDrugsOnDeath));
	}

	/// <summary>
	/// Gets or sets a value indicating whether the player loses drugs when busted.
	/// </summary>
	[GenerateIniFileValue]
	public bool LooseDrugsWhenBusted
	{
		get => looseDrugsWhenBusted;
		set => SetProperty(ref looseDrugsWhenBusted, value, nameof(LooseDrugsWhenBusted));
	}

	/// <summary>
	/// Gets or sets the starting inventory size for the player.
	/// </summary>
	[GenerateIniFileValue]
	public int StartingInventorySize
	{
		get => startingInventorySize;
		set => SetProperty(ref startingInventorySize, value, nameof(StartingInventorySize));
	}

	/// <summary>
	/// Gets the possible values for the experience multiplier setting.
	/// </summary>
	public static float[] GetExperienceMultiplierValues
		=> [0.75f, 0.8f, 0.85f, 0.9f, 0.95f, 1f, 1.05f, 1.1f, 1.15f, 1.2f, 1.25f];

	/// <summary>
	/// Gets the possible values for the inventory expansion per level setting.
	/// </summary>
	public static int[] GetInventoryExpansionPerLevelValues
		=> [0, 5, 10, 15, 25, 30, 35, 40, 45, 50];

	/// <summary>
	/// Gets the possible values for the starting inventory size setting.
	/// </summary>
	public static int[] GetStartingInventoryValues
		=> [50, 75, 100, 125, 150];
}

/// <summary>
/// Represents the settings for trafficking in the modification.
/// </summary>
public sealed class TraffickingSettings : NotifiableBase
{
	private float bustChance = 0.2f;
	private bool discoverDealersOnMap = true;
	private int wantedLevelIncreaseOnBust = 2;

	/// <summary>
	/// Gets or sets the chance of getting busted while trafficking.
	/// </summary>
	[GenerateIniFileValue]
	public float BustChance
	{
		get => bustChance;
		set => SetProperty(ref bustChance, value, nameof(BustChance));
	}

	/// <summary>
	/// Gets or sets a value indicating whether dealers need to be discovered on the map.
	/// </summary>
	[GenerateIniFileValue]
	public bool DiscoverDealersOnMap
	{
		get => discoverDealersOnMap;
		set => SetProperty(ref discoverDealersOnMap, value, nameof(DiscoverDealersOnMap));
	}

	/// <summary>
	/// Gets or sets the wanted level increase when the player gets busted.
	/// </summary>
	[GenerateIniFileValue]
	public int WantedLevelIncreaseOnBust
	{
		get => wantedLevelIncreaseOnBust;
		set => SetProperty(ref wantedLevelIncreaseOnBust, value, nameof(WantedLevelIncreaseOnBust));
	}

	/// <summary>
	/// Gets the possible values for the bust chance setting.
	/// </summary>
	public static float[] GetBustChanceValues
		=> [0.05f, 0.1f, 0.15f, 0.2f, 0.25f, 0.3f, 0.35f, 0.4f, 0.45f, 0.5f];

	/// <summary>
	/// Gets the possible values for the wanted level setting.
	/// </summary>
	public static int[] GetWantedLevelValues
		=> [1, 2, 3, 4];
}