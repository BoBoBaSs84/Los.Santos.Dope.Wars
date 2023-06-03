using LSDW.Domain.Classes.Models.Base;
using LSDW.Domain.Constants;
using LSDW.Domain.Interfaces.Models;
using static LSDW.Domain.Classes.Models.Settings.Player;

namespace LSDW.Domain.Classes.Models;

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
	/// <param name="transactions">The transaction log entries for the player.</param>
	internal Player(IInventory inventory, int experience, IEnumerable<ITransaction> transactions)
	{
		Inventory = inventory;
		Experience = experience;
		Transactions = transactions.ToHashSet();
	}

	public IInventory Inventory { get; }

	public int Level
		=> GetCurrentLevel();

	public int Experience { get; private set; }

	public int ExperienceNextLevel
		=> GetNextLevelExpPoints();

	public int MaximumInventoryQuantity
		=> GetMaximumInventoryQuantity();

	public ICollection<ITransaction> Transactions { get; }

	public void AddExperience(int points)
		=> Experience += (int)(points * (double)ExperienceMultiplier);

	private int GetCurrentLevel()
		=> PlayerConstants.CalculateCurrentLevel(Experience);

	private int GetNextLevelExpPoints()
		=> PlayerConstants.CalculateExperienceNextLevel(Level);

	private int GetMaximumInventoryQuantity()
		=> StartingInventory + Level * InventoryExpansionPerLevel;
}
