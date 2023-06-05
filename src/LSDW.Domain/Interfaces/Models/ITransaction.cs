using LSDW.Domain.Enumerators;

namespace LSDW.Domain.Interfaces.Models;

/// <summary>
/// The rransaction interface.
/// </summary>
public interface ITransaction
{
	/// <summary>
	/// The point in time of the transaction.
	/// </summary>
	DateTime DateTime { get; }
	/// <summary>
	/// The type of the transaction.
	/// </summary>
	TransactionType Type { get; }
	/// <summary>
	/// The drug type of the transaction.
	/// </summary>
	DrugType DrugType { get; }
	/// <summary>
	/// The quantity of the transaction.
	/// </summary>
	int Quantity { get; }
	/// <summary>
	/// The unit price of the transaction.
	/// </summary>
	int Price { get; }
}
