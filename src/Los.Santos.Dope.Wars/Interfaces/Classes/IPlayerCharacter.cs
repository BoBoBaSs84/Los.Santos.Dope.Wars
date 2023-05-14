namespace LSDW.Interfaces.Classes;

/// <summary>
/// The player character interface.
/// </summary>
public interface IPlayerCharacter
{
	/// <summary>
	/// The player inventory.
	/// </summary>
	IInventory Inventory { get; }

	/// <summary>
	/// The money spent on buying drugs.
	/// </summary>
	int SpentMoney { get; }

	/// <summary>
	/// The money earned on selling drugs.
	/// </summary>
	int EarnedMoney { get; }

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

	/// <summary>
	/// Should buy the desired drug.
	/// </summary>
	/// <param name="drug">The drug to buy.</param>
	void Buy(IDrug drug);

	/// <summary>
	/// Should buy the desired drugs.
	/// </summary>
	/// <param name="drugs">The drugs to buy.</param>
	void Buy(IEnumerable<IDrug> drugs);

	/// <summary>
	/// Should sell the desired drug.
	/// </summary>
	/// <param name="drug">The drug to sell.</param>
	void Sell(IDrug drug);

	/// <summary>
	/// Should sell the desired drugs.
	/// </summary>
	/// <param name="drugs">The drugs to sell.</param>
	void Sell(IEnumerable<IDrug> drugs);
}
