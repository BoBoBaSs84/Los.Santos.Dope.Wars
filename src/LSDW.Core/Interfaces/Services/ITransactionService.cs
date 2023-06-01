using LSDW.Core.Enumerators;
using System.Collections.Specialized;

namespace LSDW.Core.Interfaces.Services;

/// <summary>
/// The transaction service interface.
/// </summary>
public interface ITransactionService
{
	/// <summary>
	/// If a transaction was successful, this is the notifiaction for it.
	/// </summary>
	public event NotifyCollectionChangedEventHandler? TransactionsChanged;

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
