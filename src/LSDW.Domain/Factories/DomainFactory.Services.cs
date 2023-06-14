using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Services;
using LSDW.Abstractions.Enumerators;
using LSDW.Domain.Services;

namespace LSDW.Domain.Factories;

/// <summary>
/// The domain factory class.
/// </summary>
public static partial class DomainFactory
{
	/// <summary>
	/// Creates a new transaction service instance.
	/// </summary>
	/// <param name="transactionType">The type of the transaction.</param>
	/// <param name="source">The transaction source.</param>
	/// <param name="target">The transaction target.</param>
	/// <param name="maximumQuantity">The maximum target quantity.</param>
	public static ITransactionService CreateTransactionService(TransactionType transactionType, IInventory source, IInventory target, int maximumQuantity = int.MaxValue)
		=> new TransactionService(transactionType, source, target, maximumQuantity);
}
