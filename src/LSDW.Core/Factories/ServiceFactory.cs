using LSDW.Core.Enumerators;
using LSDW.Core.Interfaces.Models;
using LSDW.Core.Interfaces.Services;
using LSDW.Core.Services;

namespace LSDW.Core.Factories;

/// <summary>
/// The service factory class.
/// </summary>
public static class ServiceFactory
{
	/// <summary>
	/// Creates a new transaction service instance.
	/// </summary>
	/// <param name="parameter"></param>
	public static ITransactionService CreateTransactionService(TransactionType transactionType, IInventory source, IInventory target, int maximumQuantity = int.MaxValue)
		=> new TransactionService(transactionType, source, target, maximumQuantity);
}
