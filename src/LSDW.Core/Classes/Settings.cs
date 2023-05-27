namespace LSDW.Core.Classes;

/// <summary>
/// The application settings class.
/// </summary>
public static class Settings
{
	public static string SettingsFileName => "LSDW.ini";

	public static string LogFileName => "LSDW.log";

	public static string SaveFileName => "LSDW.sav";

	/// <summary>
	/// The dealer settings class.
	/// </summary>
	public static class DealerSettings
	{
		public static decimal MaximumDrugValue { get; set; } = 1.15M;

		public static decimal MinimumDrugValue { get; set; } = 0.85M;

		public static int DownTimeInHours { get; set; } = 48;
	}

	/// <summary>
	/// The player settings class.
	/// </summary>
	public static class PlayerSettings
	{
		public static bool LooseDrugsOnDeath { get; set; } = true;

		public static bool LooseDrugsWhenBusted { get; set; } = true;

		public static int InventoryExpansionPerLevel { get; set; } = 10;

		public static int StartingInventory { get; set; } = 100;
	}
}
