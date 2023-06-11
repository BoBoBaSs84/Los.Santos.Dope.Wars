namespace LSDW.Domain.Classes.Models;

/// <summary>
/// The application settings class.
/// </summary>
public static class Settings
{
	public const string IniFileName = "LSDW.ini";
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

		public static List<int> GetDownTimeInHoursValues()
			=> new int[] { 24, 48, 72, 96, 120, 144, 168 }.ToList();
	}

	/// <summary>
	/// The market settings class.
	/// </summary>
	public static class Market
	{
		public static float MaximumDrugPrice { get; set; } = 1.15f;
		public static float MinimumDrugPrice { get; set; } = 0.85f;

		public static List<float> GetMinimumDrugPriceValues()
			=> new float[] { 0.75f, 0.8f, 0.85f, 0.9f, 0.95f }.ToList();

		public static List<float> GetMaximumDrugPriceValues()
			=> new float[] { 1.05f, 1.1f, 1.15f, 1.2f, 1.25f }.ToList();
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

		public static List<float> GetExperienceMultiplierValues()
			=> new float[] { 0.75f, 0.8f, 0.85f, 0.9f, 0.95f, 1f, 1.05f, 1.1f, 1.15f, 1.2f, 1.25f }.ToList();

		public static List<int> GetInventoryExpansionPerLevelValues()
			=> new int[] { 0, 5, 10, 15, 25, 30, 35, 40, 45, 50 }.ToList();

		public static List<int> GetStartingInventoryValues()
			=> new int[] { 50, 75, 100, 125, 150 }.ToList();
	}

	/// <summary>
	/// The trafficking settings class.
	/// </summary>
	public static class Trafficking
	{
		public static float BustChance { get; set; } = 0.1f;
		public static int WantedLevel { get; set; } = 2;

		public static List<float> GetBustChanceValues()
			=> new float[] { 0.05f, 0.1f, 0.15f, 0.2f, 0.25f, 0.3f, 0.35f, 0.4f, 0.45f, 0.5f }.ToList();

		public static List<int> GetWantedLevelValues()
			=> new int[] { 1, 2, 3, 4, 5 }.ToList();
	}
}
