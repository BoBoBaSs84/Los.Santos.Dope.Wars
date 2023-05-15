using LSDW.Core.Enumerators;
using LSDW.Core.Exceptions;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Classes;

/// <summary>
/// The transaction class.
/// </summary>
internal sealed class Transaction : ITransaction
{
	/// <summary>
	/// Initializes a instance of the transaction class.
	/// </summary>
	/// <param name="type">The type of the transaction.</param>
	/// <param name="drugs">The drugs to transact.</param>
	/// <param name="maximumQuantity">The targets maximum inventory quantity.</param>
	internal Transaction(TransactionType type, IEnumerable<IDrug> drugs, int maximumQuantity)
	{
		Type = type;
		Drugs = drugs;
		MaximumTargetQuantity = maximumQuantity;
	}

	public TransactionType Type { get; }

	public IEnumerable<IDrug> Drugs { get; }

	public bool IsCompleted { get; private set; }

	public int MaximumTargetQuantity { get; }

	public void Transact(IInventoryCollection source, IInventoryCollection target)
	{
		if (Type.Equals(TransactionType.TRAFFIC))
			if (!CheckTargetMoney(target))
				throw new TransactionException("Not enough money for transaction.");

		if (GetResultingTargetQuantity(target) > MaximumTargetQuantity)
			throw new TransactionException("Not enough room for transaction.");

		foreach (IDrug drug in Drugs)
		{
			_ = source.Remove(drug);
			target.Add(drug);
		}

		if (Type.Equals(TransactionType.TRAFFIC))
		{
			int transactionValue = Drugs.Sum(drug => drug.Price * drug.Quantity);
			source.Add(transactionValue);
			target.Remove(transactionValue);
		}
	}

	/// <summary>
	/// Returns <see langword="true"/> or <see langword="true"/> if the target has enough money for the transaction.
	/// </summary>
	/// <param name="target">The target inventory.</param>
	private bool CheckTargetMoney(IInventoryCollection target)
		=> target.Money > Drugs.Sum(drug => drug.Price * drug.Quantity);

	/// <summary>
	/// Returns the resulting target inventory quantity.
	/// </summary>
	private int GetResultingTargetQuantity(IInventoryCollection target)
		=> Drugs.Sum(drug => drug.Quantity) + target.Sum(drug => drug.Quantity);
}
