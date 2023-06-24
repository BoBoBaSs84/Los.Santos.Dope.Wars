using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Abstractions.Domain.Services;
using LSDW.Abstractions.Enumerators;
using LSDW.Domain.Services;

namespace LSDW.Domain.Factories;

public static partial class DomainFactory
{
	/// <summary>
	/// Creates a new transaction service instance.
	/// </summary>
	/// <param name="notificationProvider">The notification provider instance to use.</param>
	/// <param name="type">The type of the transaction.</param>
	/// <param name="source">The transaction source.</param>
	/// <param name="target">The transaction target.</param>
	/// <param name="maximumQuantity">The maximum target quantity.</param>
	public static ITransactionService CreateTransactionService(INotificationProvider notificationProvider, TransactionType type, IInventory source, IInventory target, int maximumQuantity = int.MaxValue)
		=> new TransactionService(notificationProvider, type, source, target, maximumQuantity);
}
