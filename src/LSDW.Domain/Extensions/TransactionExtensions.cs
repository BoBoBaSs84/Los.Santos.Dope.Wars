using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Extensions;

namespace LSDW.Domain.Extensions;

/// <summary>
/// The transaction extensions class.
/// </summary>
public static class TransactionExtensions
{
	/// <summary>
	/// Returns the transaction value based on the following key points:
	/// <list type="bullet">
	/// <item>The <see cref="DrugType"/> market value</item>
	/// <item>The <see cref="TransactionType.BUY"/> or <see cref="TransactionType.SELL"/></item>
	/// <item>The <see cref="IDrug.CurrentPrice"/></item>
	/// </list>
	/// </summary>
	/// <param name="transaction">The transaction to be evaluated.</param>
	/// <returns>The transaction value.</returns>
	public static int GetValue(this ITransaction transaction)
	{
		int quantity = transaction.Quantity;
		int averagePrice = transaction.DrugType.GetAveragePrice() * quantity;
		int currentPrice = transaction.Price * quantity;
		int value = default;

		if (transaction.Type is TransactionType.BUY)
			value = averagePrice - currentPrice;

		if (transaction.Type is TransactionType.SELL)
			value = currentPrice - averagePrice;

		return value > 0 ? value : default;
	}
}
