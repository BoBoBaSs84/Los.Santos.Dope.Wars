namespace LSDW.Domain.Interfaces.Models;

/// <summary>
/// The player character interface.
/// </summary>
public interface IPlayer
{
	/// <summary>
	/// The player inventory.
	/// </summary>
	IInventory Inventory { get; }

	/// <summary>
	/// The current player level.
	/// </summary>
	int Level { get; }

	/// <summary>
	/// The current player experience points.
	/// </summary>
	int Experience { get; }

	/// <summary>
	/// Experience points needed to reach the next level.
	/// </summary>
	int ExperienceNextLevel { get; }

	/// <summary>
	/// The current maximum drug amount the player can carry.
	/// </summary>
	int MaximumInventoryQuantity { get; }

	/// <summary>
	/// The transaction count.
	/// </summary>
	int TransactionCount { get; }

	/// <summary>
	/// Adds the desired experience points to the players current experience.
	/// </summary>
	/// <param name="points">The experience points to add.</param>
	void AddExperience(int points);

	/// <summary>
	/// Adds the transaction to the players transactions.
	/// </summary>
	/// <param name="transaction">The transaction to add.</param>
	void AddTransaction(ITransaction transaction);

	/// <summary>
	/// Returns the player transactions.
	/// </summary>
	ICollection<ITransaction> GetTransactions();
}
