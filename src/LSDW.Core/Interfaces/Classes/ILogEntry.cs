using LSDW.Core.Enumerators;

namespace LSDW.Core.Interfaces.Classes;

/// <summary>
/// The log entry interface.
/// </summary>
public interface ILogEntry
{
	/// <summary>
	/// The point in time of the transaction.
	/// </summary>
	DateTime DateTime { get; }
	/// <summary>
	/// The type of the transaction.
	/// </summary>
	TransactionType TransactionType { get; }
	/// <summary>
	/// The drug type of the transaction.
	/// </summary>
	DrugType DrugType { get; }
	/// <summary>
	/// The quantity of the transaction.
	/// </summary>
	int Quantity { get; }
	/// <summary>
	/// The totatl value of the transaction.
	/// </summary>
	int TotalValue { get; }
}
