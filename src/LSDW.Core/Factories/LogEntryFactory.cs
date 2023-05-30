using LSDW.Core.Classes;
using LSDW.Core.Enumerators;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Factories;

/// <summary>
/// The log entry factory class.
/// </summary>
public static class LogEntryFactory
{
	/// <summary>
	/// Creates a new log entry instance.
	/// </summary>
	/// <param name="dateTime">The point in time of the transaction.</param>
	/// <param name="transactionType">The type of the transaction.</param>
	/// <param name="drugType">The drug type of the transaction.</param>
	/// <param name="quantity">The quantity of the transaction.</param>
	/// <param name="totalValue">The totatl value of the transaction.</param>
	public static ILogEntry CreateLogEntry(DateTime dateTime, TransactionType transactionType, DrugType drugType, int quantity, int totalValue)
		=> new LogEntry(dateTime, transactionType, drugType, quantity, totalValue);
}
