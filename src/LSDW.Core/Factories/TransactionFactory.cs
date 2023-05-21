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
	/// <param name="objects">The transaction objects to process.</param>
	/// <param name="maximumQuantity">The targets maximum inventory quantity.</param>
	public static ITransaction CreateTrafficTransaction(IEnumerable<TransactionObject> objects, int maximumQuantity = int.MaxValue)
		=> new Transaction(TransactionType.TRAFFIC, objects, maximumQuantity);

	/// <summary>
	/// Creates a new depositing transaction instance.
	/// </summary>
	/// <param name="objects">The transaction objects to process.</param>
	/// <param name="maximumQuantity">The targets maximum inventory quantity.</param>
	public static ITransaction CreateDepositTransaction(IEnumerable<TransactionObject> objects, int maximumQuantity = int.MaxValue)
		=> new Transaction(TransactionType.DEPOSIT, objects, maximumQuantity);
}
