using LSDW.Core.Classes;
using LSDW.Core.Enumerators;
using LSDW.Core.Interfaces.Classes;

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
	public static ITransactionService CreateTransactionService(TransactionParameter parameter)
		=> new TransactionService(parameter);
}
