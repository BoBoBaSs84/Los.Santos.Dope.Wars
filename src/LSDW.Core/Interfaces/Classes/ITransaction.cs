using LSDW.Core.Enumerators;

namespace LSDW.Core.Interfaces.Classes;

/// <summary>
/// The transaction interface.
/// </summary>
public interface ITransaction
{
	/// <summary>
	/// The type of the transaction.
	/// </summary>
	TransactionType Type { get; }

	/// <summary>
	/// The drugs to transact.
	/// </summary>
	IEnumerable<IDrug> Drugs { get; }

	/// <summary>
	/// Is the transaction completed?
	/// </summary>
	bool IsCompleted { get; }

	/// <summary>
	/// The targets maximum inventory quantity.
	/// </summary>
	int MaximumTargetQuantity { get; }

	/// <summary>
	/// Should do the transaction from the <paramref name="source"/> to the <paramref name="target"/>.
	/// </summary>
	/// <param name="source">The source inventory.</param>
	/// <param name="target">The target inventory.</param>
	void Transact(IInventoryCollection source, IInventoryCollection target);
}
