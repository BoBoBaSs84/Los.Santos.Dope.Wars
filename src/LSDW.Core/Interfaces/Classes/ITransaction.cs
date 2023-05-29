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
	/// The targets maximum inventory quantity.
	/// </summary>
	int MaximumTargetQuantity { get; }

	/// <summary>
	/// The result of the transaction.
	/// </summary>
	TransactionResult Result { get; }

	/// <summary>
	/// Adds the drug to the transaction.
	/// </summary>
	/// <param name="drug">The drug to add to the transaction.</param>
	void Add(IDrug drug);

	/// <summary>
	/// Adds the drugs to the transaction.
	/// </summary>
	/// <param name="drugs">The drugs to add to the transaction.</param>
	void Add(IEnumerable<IDrug> drugs);

	/// <summary>
	/// Commits the transaction from the source inventory to the target inventory.
	/// </summary>
	void Commit();
}
