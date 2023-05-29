namespace LSDW.Core.Classes;

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
		public static int DownTimeInHours { get; set; }
		public static bool WearsArmor { get; set; }
		public static bool WearsWeapons { get; set; }
	}

	/// <summary>
	/// The market settings class.
	/// </summary>
	public static class Market
	{
		public static decimal MaximumDrugValue { get; set; }
		public static decimal MinimumDrugValue { get; set; }
	}

	/// <summary>
	/// The player settings class.
	/// </summary>
	public static class Player
	{
		public static decimal ExperienceMultiplier { get; set; }
		public static bool LooseDrugsOnDeath { get; set; }
		public static bool LooseMoneyOnDeath { get; set; }
		public static bool LooseDrugsWhenBusted { get; set; }
		public static bool LooseMoneyWhenBusted { get; set; }
		public static int InventoryExpansionPerLevel { get; set; }
		public static int StartingInventory { get; set; }
	}
}
