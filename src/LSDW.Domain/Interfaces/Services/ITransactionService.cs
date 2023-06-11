using LSDW.Domain.Enumerators;

namespace LSDW.Domain.Interfaces.Services;

/// <summary>
/// The transaction service interface.
/// </summary>
public interface ITransactionService
{
	/// <summary>
	/// Contains the errors if the transaction was not successful.
	/// </summary>
	ICollection<string> Errors { get; }

	/// <summary>
	/// Commits the transaction from the source inventory to the target inventory.
	/// </summary>
	/// <param name="type">The type of the drug to transact.</param>
	/// <param name="quantity">The transaction quantity of the drug.</param>
	/// <param name="price">The transaction price of the drug.</param>
	/// <remarks>
	/// Returns <see langword="true"/> or <see langword="false"/> if successful.
	/// If not, see <see cref="Errors"/> for more information.
	/// </remarks>
	/// <returns>Success or not.</returns>
	bool Commit(DrugType type, int quantity, int price);
}
