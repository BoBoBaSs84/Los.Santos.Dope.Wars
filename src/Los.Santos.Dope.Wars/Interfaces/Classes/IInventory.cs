using LSDW.Classes;

namespace LSDW.Interfaces.Classes;

/// <summary>
/// The inventory interface.
/// </summary>
public interface IInventory : IEnumerable<IDrug>
{
	/// <summary>
	/// The inventory count.
	/// </summary>
	int Count { get; }
	
	/// <summary>
	/// The current dirty drug money.
	/// </summary>
	int Money { get; }

	/// <summary>
	/// The total inventory quantity.
	/// </summary>
	int TotalQuantity { get; }

	/// <summary>
	/// The total inventory value.
	/// </summary>
	int TotalMarketValue { get; }

	/// <summary>
	/// The total inventory profit compared to the nominal market prices.
	/// </summary>
	int TotalProfit { get; }
	
	/// <summary>
	/// Should add a drug to the inventory or add the amount to existing one.
	/// </summary>
	/// <param name="drugToAdd"></param>
	void Add(IDrug drugToAdd);

	/// <summary>
	/// Should add the amount of money to the inventory.
	/// </summary>
	/// <param name="moneyToAdd">The amount of money to add.</param>
	void Add(int moneyToAdd);

	/// <summary>
	/// Should remove a drug from the inventory.
	/// </summary>
	/// <param name="drugToRemove"></param>
	void Remove(IDrug drugToRemove);

	/// <summary>
	/// Should remove the amount of money from the inventory.
	/// </summary>
	/// <param name="moneyToRemove">The amount of money to remove.</param>
	void Remove(int moneyToRemove);
}
