using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace LSDW.Core.Interfaces.Classes;

/// <summary>
/// The inventory interface.
/// </summary>
[SuppressMessage("Naming", "CA1710", Justification = "")]
public interface IInventory : ICollection<IDrug>, INotifyPropertyChanged
{
	/// <summary>
	/// The current drug money.
	/// </summary>
	/// <remarks>
	/// Used for transactions that require money.
	/// </remarks>
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
	/// Adds a drug to the inventory or add the drug quantity to the existing drugtype.
	/// </summary>
	/// <param name="drugToAdd">The drug to add to the inventory.</param>
	new void Add(IDrug drugToAdd);

	/// <summary>
	/// Adds the drugs to the inventory or add the drug quantity to the existing drugtype.
	/// </summary>
	/// <param name="drugsToAdd">The drugs to add to the inventory.</param>
	void Add(IEnumerable<IDrug> drugsToAdd);

	/// <summary>
	/// Adds the amount of money to the inventory.
	/// </summary>
	/// <param name="moneyToAdd">The amount of money to add to the inventory.</param>
	void Add(int moneyToAdd);

	/// <summary>
	/// Removes the drug from the inventory.
	/// </summary>
	/// <param name="drugToRemove">The drug to remove from the inventory.</param>
	new bool Remove(IDrug drugToRemove);

	/// <summary>
	/// Removes the drugs from the inventory.
	/// </summary>
	/// <param name="drugsToRemove">The drugs to remove from the inventory.</param>
	void Remove(IEnumerable<IDrug> drugsToRemove);

	/// <summary>
	/// Removes the amount of money from the inventory.
	/// </summary>
	/// <param name="moneyToRemove">The amount of money to remove from the inventory.</param>
	void Remove(int moneyToRemove);
}
