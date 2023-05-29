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
	/// Was the last transaction successful?
	/// </summary>
	public bool Successful { get; private set; }

	/// <summary>
	/// The transaction messages to show.
	/// </summary>
	public ICollection<string> Messages { get; }

	/// <summary>
	/// Sets the result to failure.
	/// </summary>
	public void Failed()
		=> Successful = false;

	/// <summary>
	/// Sets the result to success.
	/// </summary>
	public void Success()
		=> Successful = true;
}
