using LSDW.Core.Classes.Base;
using LSDW.Core.Constants;
using LSDW.Core.Interfaces.Classes;
using LSDW.Core.Properties;
using IF = LSDW.Core.Factories.InventoryFactory;

namespace LSDW.Core.Classes;

internal sealed class PlayerCharacter : Notification, IPlayerCharacter
{
	private readonly int InventoryCapacity = Settings.Default.StartingInventoryCapacity;
	private readonly int ExpansionPerLevel = Settings.Default.InventoryCapacityExpansionPerLevel;

	private int currentExperience;

	/// <summary>
	/// Initializes a instance of the player character class.
	/// </summary>
	internal PlayerCharacter()
	{
		Inventory = IF.CreateInventory();
		currentExperience = default;
	}

	/// <summary>
	/// Initializes a instance of the player character class.
	/// </summary>
	/// <param name="inventory">The player inventory.</param>
	/// <param name="experience">The player experience points.</param>
	internal PlayerCharacter(IInventoryCollection inventory, int experience)
	{
		Inventory = inventory;
		currentExperience = experience;
	}

	public IInventoryCollection Inventory { get; }

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
		=> InventoryCapacity + (CurrentLevel * ExpansionPerLevel);
}
