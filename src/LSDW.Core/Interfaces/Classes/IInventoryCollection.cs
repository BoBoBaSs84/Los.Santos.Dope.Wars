namespace LSDW.Core.Interfaces.Classes;

/// <summary>
/// The inventory interface.
/// </summary>
public interface IInventoryCollection : ICollection<IDrug>
{
	/// <summary>
	/// The inventory count.
	/// </summary>
	new int Count { get; }

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
	/// Should add a drug to the inventory or add the drug quantity to the existing drugtype.
	/// </summary>
	/// <param name="drugToAdd"></param>
	new void Add(IDrug drugToAdd);

	/// <summary>
	/// Should add the amount of money to the inventory.
	/// </summary>
	/// <param name="moneyToAdd">The amount of money to add.</param>
	void Add(int moneyToAdd);

	/// <summary>
	/// Should remove the drug from the inventory, if the resulting quantity is zero the drug will be removed completely.
	/// </summary>
	/// <param name="drugToRemove"></param>
	new bool Remove(IDrug drugToRemove);

	/// <summary>
	/// Should remove the amount of money from the inventory.
	/// </summary>
	/// <param name="moneyToRemove">The amount of money to remove.</param>
	void Remove(int moneyToRemove);
}
