namespace LSDW.Domain.Classes.Models;

/// <summary>
/// The application settings class.
/// </summary>
public static class Settings
{
	/// <summary>
	/// The settings file name.
	/// </summary>
	public const string IniFileName = "LSDW.ini";
	/// <summary>
	/// The log file name.
	/// </summary>
	public const string LogFileName = "LSDW.log";
	/// <summary>
	/// The save file name.
	/// </summary>
	public const string SaveFileName = "LSDW.sav";

	/// <summary>
	/// The dealer settings class.
	/// </summary>
	public static class Dealer
	{
		/// <summary>
		/// The down time in hours property.
		/// </summary>
		public static int DownTimeInHours { get; set; } = 48;
		/// <summary>
		/// Is the dealer armored?
		/// </summary>
		public static bool HasArmor { get; set; } = true;
		/// <summary>
		/// Is the dealer armed?
		/// </summary>
		public static bool HasWeapons { get; set; } = true;
		/// <summary>
		/// Returns the possible dealer down time values.
		/// </summary>
		public static List<int> GetDownTimeInHoursValues()
			=> new int[] { 24, 48, 72, 96, 120, 144, 168 }.ToList();
	}

	/// <summary>
	/// The market settings class.
	/// </summary>
	public static class Market
	{
		/// <summary>
		/// The maximum drug price factor.
		/// </summary>
		public static float MaximumDrugPrice { get; set; } = 1.15f;
		/// <summary>
		/// The minimum drug price factor.
		/// </summary>
		public static float MinimumDrugPrice { get; set; } = 0.85f;
		/// <summary>
		/// Returns the possible maximum price factor values.
		/// </summary>
		public static List<float> GetMaximumDrugPriceValues()
			=> new float[] { 1.05f, 1.1f, 1.15f, 1.2f, 1.25f }.ToList();
		/// <summary>
		/// Returns the possible minimum price factor values.
		/// </summary>
		public static List<float> GetMinimumDrugPriceValues()
			=> new float[] { 0.75f, 0.8f, 0.85f, 0.9f, 0.95f }.ToList();
	}

	/// <summary>
	/// The player settings class.
	/// </summary>
	public static class Player
	{
		/// <summary>
		/// The experience multiplier property.
		/// </summary>
		public static float ExperienceMultiplier { get; set; } = 1.0f;
		/// <summary>
		/// The loose drugs on death property.
		/// </summary>
		public static bool LooseDrugsOnDeath { get; set; } = true;
		/// <summary>
		/// The loose money on death property.
		/// </summary>
		public static bool LooseMoneyOnDeath { get; set; } = true;
		/// <summary>
		/// The loose drugs when busted property.
		/// </summary>
		public static bool LooseDrugsWhenBusted { get; set; } = true;
		/// <summary>
		/// The loose money when busted property.
		/// </summary>
		public static bool LooseMoneyWhenBusted { get; set; } = true;
		/// <summary>
		/// The inventory expansion per level property.
		/// </summary>
		public static int InventoryExpansionPerLevel { get; set; } = 10;
		/// <summary>
		/// The starting inventory property.
		/// </summary>
		public static int StartingInventory { get; set; } = 100;
		/// <summary>
		/// Returns the possible experience multiplier factor values.
		/// </summary>
		public static List<float> GetExperienceMultiplierValues()
			=> new float[] { 0.75f, 0.8f, 0.85f, 0.9f, 0.95f, 1f, 1.05f, 1.1f, 1.15f, 1.2f, 1.25f }.ToList();
		/// <summary>
		/// Returns the possible inventory expansion per level values.
		/// </summary>
		public static List<int> GetInventoryExpansionPerLevelValues()
			=> new int[] { 0, 5, 10, 15, 25, 30, 35, 40, 45, 50 }.ToList();
		/// <summary>
		/// Returns the possible starting inventory values.
		/// </summary>
		public static List<int> GetStartingInventoryValues()
			=> new int[] { 50, 75, 100, 125, 150 }.ToList();
	}

	/// <summary>
	/// The trafficking settings class.
	/// </summary>
	public static class Trafficking
	{
		/// <summary>
		/// The discover dealer property.
		/// </summary>
		public static bool DiscoverDealer { get; set; } = true;
		/// <summary>
		/// The bust chance property.
		/// </summary>
		public static float BustChance { get; set; } = 0.1f;
		/// <summary>
		/// The wanted level property.
		/// </summary>
		public static int WantedLevel { get; set; } = 2;
		/// <summary>
		/// Returns the possible bust chance values.
		/// </summary>
		public static List<float> GetBustChanceValues()
			=> new float[] { 0.05f, 0.1f, 0.15f, 0.2f, 0.25f, 0.3f, 0.35f, 0.4f, 0.45f, 0.5f }.ToList();
		/// <summary>
		/// Returns the possible wanted level values.
		/// </summary>
		public static List<int> GetWantedLevelValues()
			=> new int[] { 1, 2, 3 }.ToList();
	}
}
