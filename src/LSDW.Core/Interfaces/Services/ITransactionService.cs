using LSDW.Core.Enumerators;

namespace LSDW.Core.Interfaces.Services;

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
	/// <remarks>
	/// Returns <see langword="true"/> or <see langword="false"/> if successful.
	/// If not, see <see cref="Errors"/> for more information.
	/// </remarks>
	/// <returns>Success or not.</returns>
	bool Commit(DrugType drugType, int quantity, int price);
}
