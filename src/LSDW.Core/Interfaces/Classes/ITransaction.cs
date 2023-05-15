using LSDW.Core.Classes;
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
	/// The transaction objects to process.
	/// </summary>
	IEnumerable<TransactionObject> Objects { get; }

	/// <summary>
	/// The targets maximum inventory quantity.
	/// </summary>
	int MaximumTargetQuantity { get; }

	/// <summary>
	/// Should commit the transaction from the <paramref name="source"/> to the <paramref name="target"/>.
	/// </summary>
	/// <param name="source">The source inventory.</param>
	/// <param name="target">The target inventory.</param>
	/// <returns>The result of the transaction.</returns>
	TransactionResult Commit(IInventoryCollection source, IInventoryCollection target);
}
