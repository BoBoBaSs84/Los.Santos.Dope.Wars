using LSDW.Classes.BaseClasses;
using LSDW.Constants;
using LSDW.Interfaces.Classes;
using LSDW.Properties;
using IF = LSDW.Factories.InventoryFactory;

namespace LSDW.Classes;

internal sealed class PlayerCharacter : NotificationBase, IPlayerCharacter
{
	private double currentExperience;

	internal PlayerCharacter()
	{
		Inventory = IF.CreateEmptyPlayerInventory();
		SpentMoney = default;
		EarnedMoney = default;
		currentExperience = default;
	}

	internal PlayerCharacter(PlayerInventory inventory, int spentMoney, int earnedMoney, int currentExperience)
	{
		Inventory = inventory;
		SpentMoney = spentMoney;
		EarnedMoney = earnedMoney;
		this.currentExperience = currentExperience;
	}

	public IInventory Inventory { get; }

	public int SpentMoney
	{
		get; private set;
	}

	public int EarnedMoney
	{
		get; private set;
	}

	public int CurrentLevel
		=> GetCurrentLevel();

	public int CurrentExperience
		=> (int)currentExperience;

	public int NextLevelExperience
		=> GetNextLevelExpPoints();

	public int MaximumInventoryQuantity
		=> GetMaximumInventoryQuantity();

	public void AddExperience(double points)
		=> currentExperience += points;

	private int GetCurrentLevel()
		=> PlayerConstants.CalculateCurrentLevel(currentExperience);

	private int GetNextLevelExpPoints()
		=> (int)PlayerConstants.CalculateExperienceNextLevel(CurrentLevel);

	private int GetMaximumInventoryQuantity()
		=> CurrentLevel * Settings.Default.InventoryCapacityExpansionPerLevel;
}
