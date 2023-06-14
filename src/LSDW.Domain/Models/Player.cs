using LSDW.Abstractions.Domain.Models;
using LSDW.Domain.Constants;
using LSDW.Domain.Extensions;
using LSDW.Domain.Models.Base;
using static LSDW.Domain.Models.Settings.Player;

namespace LSDW.Domain.Models;

/// <summary>
/// The player class.
/// </summary>
internal sealed class Player : Notification, IPlayer
{
	private readonly ICollection<ITransaction> _transactions;

	/// <summary>
	/// Initializes a instance of the player character class.
	/// </summary>
	/// <param name="inventory">The player inventory.</param>
	/// <param name="experience">The player experience points.</param>
	/// <param name="transactions">The transactions for the player.</param>
	internal Player(IInventory inventory, int experience, ICollection<ITransaction> transactions)
	{
		Inventory = inventory;
		Experience = experience;
		_transactions = transactions;
	}

	public IInventory Inventory { get; }

	public int Level
		=> GetCurrentLevel();

	public int Experience { get; private set; }

	public int ExperienceNextLevel
		=> GetNextLevelExpPoints();

	public int MaximumInventoryQuantity
		=> GetMaximumInventoryQuantity();

	public int TransactionCount
		=> _transactions.Count;

	public void AddExperience(int points)
		=> Experience += (int)(points * ExperienceMultiplier);

	public void AddTransaction(ITransaction transaction)
	{
		AddExperience(transaction.GetValue());
		_transactions.Add(transaction);
	}

	public ICollection<ITransaction> GetTransactions()
		=> _transactions;

	private int GetCurrentLevel()
		=> PlayerConstants.CalculateCurrentLevel(Experience);

	private int GetNextLevelExpPoints()
		=> PlayerConstants.CalculateExperienceNextLevel(Level);

	private int GetMaximumInventoryQuantity()
		=> StartingInventory + Level * InventoryExpansionPerLevel;
}
