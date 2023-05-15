namespace LSDW.Interfaces.Classes;

/// <summary>
/// The player character interface.
/// </summary>
public interface IPlayerCharacter
{
	/// <summary>
	/// The player inventory.
	/// </summary>
	IInventoryCollection Inventory { get; }

	/// <summary>
	/// The current player level.
	/// </summary>
	int CurrentLevel { get; }

	/// <summary>
	/// The current player experience points.
	/// </summary>
	int CurrentExperience { get; }

	/// <summary>
	/// Experience points needed to reach the next level.
	/// </summary>
	int NextLevelExperience { get; }

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
