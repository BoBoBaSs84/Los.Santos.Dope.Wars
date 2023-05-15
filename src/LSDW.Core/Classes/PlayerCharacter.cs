using LSDW.Core.Classes.Base;
using LSDW.Core.Constants;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;
using LSDW.Core.Properties;

namespace LSDW.Core.Classes;

internal sealed class PlayerCharacter : Notification, IPlayerCharacter
{
	private readonly int InventoryCapacity = Settings.Default.StartingInventoryCapacity;
	private readonly int ExpansionPerLevel = Settings.Default.InventoryCapacityExpansionPerLevel;

	/// <summary>
	/// Initializes a instance of the player character class.
	/// </summary>
	internal PlayerCharacter()
	{
		Inventory = InventoryFactory.CreateInventory();
		Experience = default;
	}

	/// <summary>
	/// Initializes a instance of the player character class.
	/// </summary>
	/// <param name="inventory">The player inventory.</param>
	/// <param name="experience">The player experience points.</param>
	internal PlayerCharacter(IInventoryCollection inventory, int experience)
	{
		Inventory = inventory;
		Experience = experience;
	}

	public IInventoryCollection Inventory { get; }

	public int Level
		=> GetCurrentLevel();

	public int Experience { get; private set; }

	public int ExperienceNextLevel
		=> GetNextLevelExpPoints();

	public int MaximumInventoryQuantity
		=> GetMaximumInventoryQuantity();

	public void AddExperience(int points)
		=> Experience += points;

	private int GetCurrentLevel()
		=> PlayerConstants.CalculateCurrentLevel(Experience);

	private int GetNextLevelExpPoints()
		=> PlayerConstants.CalculateExperienceNextLevel(Level);

	private int GetMaximumInventoryQuantity()
		=> InventoryCapacity + (Level * ExpansionPerLevel);
}
