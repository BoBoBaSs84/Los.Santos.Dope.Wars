namespace LSDW.Core.Exceptions;

/// <summary>
/// The transaction exception class.
/// </summary>
public sealed class TransactionException : Exception
{
	/// <summary>
	/// Initializes a instance of the transaction exception class.
	/// </summary>
	/// <param name="message">The transaction exception message.</param>
	public TransactionException(string message) : base(message)
	{
	}

	/// <summary>
	/// Initializes a instance of the transaction exception class.
	/// </summary>
	/// <param name="message">The transaction exception message.</param>
	/// <param name="innerException">The transaction inner exception.</param>
	public TransactionException(string message, Exception innerException) : base(message, innerException)
	{
	}
}
