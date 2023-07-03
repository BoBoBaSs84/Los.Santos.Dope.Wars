using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Services;
using LSDW.Abstractions.Enumerators;
using LSDW.Domain.Services;

namespace LSDW.Domain.Factories;

public static partial class DomainFactory
{
	/// <summary>
	/// Creates a new transaction service instance.
	/// </summary>
	/// <param name="providerManager">The provider manager instance to use.</param>
	/// <param name="type">The type of the transaction.</param>
	/// <param name="player">The player and his inventory.</param>
	/// <param name="inventory">The opposing inventory.</param>
	public static ITransactionService CreateTransactionService(IProviderManager providerManager, TransactionType type, IPlayer player, IInventory inventory)
		=> new TransactionService(providerManager, type, player, inventory);
}
