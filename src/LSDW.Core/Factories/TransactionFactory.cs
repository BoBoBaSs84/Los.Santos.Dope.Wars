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
	/// <param name="drugs">The drugs to transact.</param>
	/// <param name="maximumQuantity">The targets maximum inventory quantity.</param>
	public static ITransaction CreateTrafficTransaction(IEnumerable<IDrug> drugs, int maximumQuantity = int.MaxValue)
		=> new Transaction(TransactionType.TRAFFIC, drugs, maximumQuantity);

	/// <summary>
	/// Creates a new depositing transaction instance.
	/// </summary>
	/// <param name="drugs">The drugs to transact.</param>
	/// <param name="maximumQuantity">The targets maximum inventory quantity.</param>
	public static ITransaction CreateDepositTransaction(IEnumerable<IDrug> drugs, int maximumQuantity = int.MaxValue)
		=> new Transaction(TransactionType.DEPOSIT, drugs, maximumQuantity);
}
