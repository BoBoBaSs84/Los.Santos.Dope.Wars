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
	/// The result of the transaction.
	/// </summary>
	TransactionResult Result { get; }

	/// <summary>
	/// Should commit the transaction from the source inventory to the target inventory.
	/// </summary>
	void Commit();
}
