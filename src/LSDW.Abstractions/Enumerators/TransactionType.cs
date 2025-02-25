using System.ComponentModel;

namespace LSDW.Abstractions.Enumerators;

/// <summary>
/// The transaction type enumerator.
/// </summary>
public enum TransactionType
{
	/// <summary>
	/// The buy type enumerator.
	/// </summary>
	[Description("Let's buy some drugs...")]
	BUY,
	/// <summary>
	/// The sell type enumerator.
	/// </summary>
	[Description("Let's sell some drugs...")]
	SELL,
	/// <summary>
	/// The take type enumerator.
	/// </summary>
	[Description("Let's take some drugs...")]
	TAKE,
	/// <summary>
	/// The store type enumerator.
	/// </summary>
	[Description("Let's give some drugs...")]
	GIVE
}
