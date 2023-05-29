using LSDW.Core.Classes;
using LSDW.Core.Enumerators;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Factories;

/// <summary>
/// The transaction factory class.
/// </summary>
public static class TransactionFactory
{
	/// <summary>
	/// Creates a new trafficking transaction instance.
	/// </summary>
	/// <param name="source">The source inventory.</param>
	/// <param name="target">The target inventory.</param>
	/// <param name="maximumQuantity">The targets maximum inventory quantity.</param>
	public static ITransaction CreateTrafficTransaction(IInventory source, IInventory target, int maximumQuantity = int.MaxValue)
		=> new Transaction(TransactionType.TRAFFIC, source, target, maximumQuantity);

	/// <summary>
	/// Creates a new depositing transaction instance.
	/// </summary>
	/// <param name="source">The source inventory.</param>
	/// <param name="target">The target inventory.</param>
	/// <param name="maximumQuantity">The targets maximum inventory quantity.</param>
	public static ITransaction CreateDepositTransaction(IInventory source, IInventory target, int maximumQuantity = int.MaxValue)
		=> new Transaction(TransactionType.DEPOSIT, source, target, maximumQuantity);

	/// <summary>
	/// Creates a new transaction instance.
	/// </summary>
	/// <param name="menuType">The menu type that uses the transaction.</param>
	/// <param name="source">The source inventory.</param>
	/// <param name="target">The target inventory.</param>
	/// <param name="maximumQuantity">The targets maximum inventory quantity.</param>
	public static ITransaction CreateTransaction(MenuType menuType, IInventory source, IInventory target, int maximumQuantity = int.MaxValue)
		=> menuType.Equals(MenuType.SELL) || menuType.Equals(MenuType.BUY)
			? CreateTrafficTransaction(source, target, maximumQuantity)
			: CreateDepositTransaction(source, target, maximumQuantity);
}
