using LSDW.Abstractions.Enumerators;

namespace LSDW.Abstractions.Domain.Services;

/// <summary>
/// The transaction service interface.
/// </summary>
public interface ITransactionService
{
	/// <summary>
	/// Commits the transaction from the source inventory to the target inventory.
	/// </summary>
	/// <param name="type">The type of the drug to transact.</param>
	/// <param name="quantity">The transaction quantity of the drug.</param>
	/// <param name="price">The transaction price of the drug.</param>
	/// <returns></returns>
	bool Commit(DrugType type, int quantity, int price);
}
