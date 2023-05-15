namespace LSDW.Core.Interfaces.Classes;

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
	/// Should adds the desired experience points to the current experience.
	/// </summary>
	/// <param name="points">The experience points to add.</param>
	void AddExperience(int points);
}
