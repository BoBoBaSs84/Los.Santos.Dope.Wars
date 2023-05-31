using LSDW.Core.Enumerators;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Classes;

/// <summary>
/// The transaction parameter class.
/// </summary>
public sealed class TransactionParameter
{
	/// <summary>
	/// Initializes a instance of the transaction parameter class.
	/// </summary>
	/// <param name="type">The type of the transaction.</param>
	/// <param name="drug">The type of the drug for the transaction.</param>
	/// <param name="source">The source inventory.</param>
	/// <param name="target">The target inventory.</param>
	/// <param name="maximumQuantity">The targets maximum inventory quantity.</param>
	public TransactionParameter(TransactionType type, IDrug drug, IInventory source, IInventory target, int maximumQuantity)
	{
		Type = type;
		Drug = drug;
		Source = source;
		Target = target;
		MaximumQuantity = maximumQuantity;
	}

	public TransactionType Type { get; }
	public IDrug Drug { get; }
	public IInventory Source { get; }
	public IInventory Target { get; }
	public int MaximumQuantity { get; }
}
