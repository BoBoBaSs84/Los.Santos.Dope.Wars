namespace LSDW.Core.Classes;

/// <summary>
/// The transaction result class.
/// </summary>
public sealed class TransactionResult
{
	/// <summary>
	/// Initializes a instance of the transaction result class.
	/// </summary>
	public TransactionResult()
	{
		Successful = default;
		Messages = new HashSet<string>();
	}

	/// <summary>
	/// Was the transaction successful?
	/// </summary>
	public bool Successful { get; set; }

	/// <summary>
	/// The transaction messages to show.
	/// </summary>
	public ICollection<string> Messages { get; set; }
}
