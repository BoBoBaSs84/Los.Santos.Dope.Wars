namespace LSDW.Core.Interfaces.Classes;

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
	void Commit();
}
