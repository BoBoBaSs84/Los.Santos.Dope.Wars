using LSDW.Core.Classes.Base;
using LSDW.Core.Constants;
using LSDW.Core.Interfaces.Classes;
using static LSDW.Core.Classes.Settings.Player;

namespace LSDW.Core.Classes;

/// <summary>
/// The player class.
/// </summary>
internal sealed class Player : Notification, IPlayer
{
	/// <summary>
	/// Initializes a instance of the player character class.
	/// </summary>
	/// <param name="inventory">The player inventory.</param>
	/// <param name="experience">The player experience points.</param>
	internal Player(IInventory inventory, int experience)
	{
		Inventory = inventory;
		Experience = experience;
	}

	public IInventory Inventory { get; }

	public int Level
		=> GetCurrentLevel();

	public int Experience { get; private set; }

	public int ExperienceNextLevel
		=> GetNextLevelExpPoints();

	public int MaximumInventoryQuantity
		=> GetMaximumInventoryQuantity();

	public void AddExperience(int points)
		=> Experience += (int)(points * (double)ExperienceMultiplier);

	private int GetCurrentLevel()
		=> PlayerConstants.CalculateCurrentLevel(Experience);

	private int GetNextLevelExpPoints()
		=> PlayerConstants.CalculateExperienceNextLevel(Level);

	private int GetMaximumInventoryQuantity()
		=> StartingInventory + (Level * InventoryExpansionPerLevel);
}
