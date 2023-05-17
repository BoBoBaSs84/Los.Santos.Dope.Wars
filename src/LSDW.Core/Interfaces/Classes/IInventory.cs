using System.Diagnostics.CodeAnalysis;

namespace LSDW.Core.Interfaces.Classes;

/// <summary>
/// The inventory interface.
/// </summary>
[SuppressMessage("Naming", "CA1710", Justification = "")]
public interface IInventory : ICollection<IDrug>
{
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
	/// <param name="drugToAdd">The drug to add to the inventory.</param>
	new void Add(IDrug drugToAdd);

	/// <summary>
	/// Should add the drugs to the inventory or add the drug quantity to the existing drugtype.
	/// </summary>
	/// <param name="drugsToAdd">The drugs to add to the inventory.</param>
	void Add(IEnumerable<IDrug> drugsToAdd);

	/// <summary>
	/// Should add the amount of money to the inventory.
	/// </summary>
	/// <param name="moneyToAdd">The amount of money to add to the inventory.</param>
	void Add(int moneyToAdd);

	/// <summary>
	/// Should remove the drug from the inventory, if the resulting quantity is zero the drug will be removed completely.
	/// </summary>
	/// <param name="drugToRemove">The drug to remove from the inventory.</param>
	new bool Remove(IDrug drugToRemove);

	/// <summary>
	/// Should remove the drugs from the inventory.
	/// </summary>
	/// <param name="drugsToRemove">The drugs to remove from the inventory.</param>
	void Remove(IEnumerable<IDrug> drugsToRemove);

	/// <summary>
	/// Should remove the amount of money from the inventory.
	/// </summary>
	/// <param name="moneyToRemove">The amount of money to remove from the inventory.</param>
	void Remove(int moneyToRemove);
}
