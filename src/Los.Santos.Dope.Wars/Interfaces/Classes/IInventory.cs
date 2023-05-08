using LSDW.Classes;

namespace LSDW.Interfaces.Classes;

/// <summary>
/// The inventory interface.
/// </summary>
internal interface IInventory : IEnumerable<Drug>
{
	/// <summary>
	/// The inventory count.
	/// </summary>
	int Count { get; }
	
	/// <summary>
	/// The total inventory quantity.
	/// </summary>
	int TotalQuantity { get; }

	/// <summary>
	/// The total inventory value.
	/// </summary>
	int TotalValue { get; }
	
	/// <summary>
	/// Should add a drug to the inventory or add the amount to existing one.
	/// </summary>
	/// <param name="drugToAdd"></param>
	void Add(Drug drugToAdd);

	/// <summary>
	/// Should remove a drug from the inventory.
	/// </summary>
	/// <param name="drugToRemove"></param>
	void Remove(Drug drugToRemove);
}
