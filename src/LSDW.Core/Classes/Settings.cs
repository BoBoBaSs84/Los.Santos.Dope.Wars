﻿namespace LSDW.Core.Classes;

/// <summary>
/// The application settings class.
/// </summary>
public static class Settings
{
	public const string SettingsFileName = "LSDW.ini";
	public const string LogFileName = "LSDW.log";
	public const string SaveFileName = "LSDW.sav";

	/// <summary>
	/// The dealer settings class.
	/// </summary>
	public static class Dealer
	{
		public static int DownTimeInHours { get; set; } = 48;
		public static bool HasArmor { get; set; } = true;
		public static bool HasWeapons { get; set; } = true;
	}

	/// <summary>
	/// The market settings class.
	/// </summary>
	public static class Market
	{
		public static float MaximumDrugValue { get; set; } = 1.2f;
		public static float MinimumDrugValue { get; set; } = 0.8f;
	}

	/// <summary>
	/// The player settings class.
	/// </summary>
	public static class Player
	{
		public static float ExperienceMultiplier { get; set; } = 1.0f;
		public static bool LooseDrugsOnDeath { get; set; } = true;
		public static bool LooseMoneyOnDeath { get; set; } = true;
		public static bool LooseDrugsWhenBusted { get; set; } = true;
		public static bool LooseMoneyWhenBusted { get; set; } = true;
		public static int InventoryExpansionPerLevel { get; set; } = 10;
		public static int StartingInventory { get; set; } = 100;
	}
}
