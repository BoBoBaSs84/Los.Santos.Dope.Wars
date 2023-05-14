using LSDW.Classes.BaseClasses;
using LSDW.Constants;
using LSDW.Interfaces.Classes;
using LSDW.Properties;
using IF = LSDW.Factories.InventoryFactory;

namespace LSDW.Classes;

internal sealed class PlayerCharacter : NotificationBase, IPlayerCharacter
{
	private double currentExperience;

	/// <summary>
	/// Initializes a instance of the player character class.
	/// </summary>
	internal PlayerCharacter()
	{
		Inventory = IF.CreatePlayerInventory();
		SpentMoney = default;
		EarnedMoney = default;
		currentExperience = default;
	}

	/// <summary>
	/// Initializes a instance of the player character class.
	/// </summary>
	/// <param name="inventory">The player inventory.</param>
	/// <param name="spentMoney">The money spent on buying drugs.</param>
	/// <param name="earnedMoney">The money earned on selling drugs.</param>
	/// <param name="experience">The player experience points.</param>
	internal PlayerCharacter(IInventory inventory, int spentMoney, int earnedMoney, int experience)
	{
		Inventory = inventory;
		SpentMoney = spentMoney;
		EarnedMoney = earnedMoney;
		currentExperience = experience;
	}

	public IInventory Inventory { get; }

	public int SpentMoney
	{
		get;
		private set;
	}

	public int EarnedMoney
	{
		get;
		private set;
	}

	public int CurrentLevel
		=> GetCurrentLevel();

	public int CurrentExperience
		=> (int)currentExperience;

	public int NextLevelExperience
		=> GetNextLevelExpPoints();

	public int MaximumInventoryQuantity
		=> GetMaximumInventoryQuantity();

	public void AddExperience(int points)
		=> currentExperience += points;

	private int GetCurrentLevel()
		=> PlayerConstants.CalculateCurrentLevel(currentExperience);

	private int GetNextLevelExpPoints()
		=> (int)PlayerConstants.CalculateExperienceNextLevel(CurrentLevel);

	private int GetMaximumInventoryQuantity()
		=> Settings.Default.StartingInventoryCapacity + (CurrentLevel * Settings.Default.InventoryCapacityExpansionPerLevel);
}
