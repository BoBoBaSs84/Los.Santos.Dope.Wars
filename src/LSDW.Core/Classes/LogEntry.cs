using LSDW.Core.Enumerators;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Classes;

/// <summary>
/// The log entry class.
/// </summary>
internal sealed class LogEntry : ILogEntry
{
	/// <summary>
	/// Initializes a instance of the log entry class.
	/// </summary>
	/// <param name="dateTime">The point in time of the transaction.</param>
	/// <param name="transactionType">The type of the transaction.</param>
	/// <param name="drugType">The drug type of the transaction.</param>
	/// <param name="quantity">The quantity of the transaction.</param>
	/// <param name="totalValue">The totatl value of the transaction.</param>
	internal LogEntry(DateTime dateTime, TransactionType transactionType, DrugType drugType, int quantity, int totalValue)
	{
		DateTime = dateTime;
		TransactionType = transactionType;
		DrugType = drugType;
		Quantity = quantity;
		TotalValue = totalValue;
	}

	public DateTime DateTime { get; }
	public TransactionType TransactionType { get; }
	public DrugType DrugType { get; }
	public int Quantity { get; }
	public int TotalValue { get; }
}
