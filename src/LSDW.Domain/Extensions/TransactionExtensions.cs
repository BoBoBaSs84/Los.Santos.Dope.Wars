using LSDW.Domain.Enumerators;
using LSDW.Domain.Interfaces.Models;

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
	/// <item>The <see cref="IDrug.Price"/></item>
	/// </list>
	/// </summary>
	/// <param name="transaction">The transaction to be evaluated.</param>
	/// <returns>The transaction value.</returns>
	public static int GetValue(this ITransaction transaction)
	{
		int quantity = transaction.Quantity;
		int marketValue = transaction.DrugType.GetMarketValue() * quantity;
		int price = transaction.Price * quantity;
		int value = default;

		if (transaction.Type is TransactionType.BUY)
			value = marketValue - price;

		if (transaction.Type is TransactionType.SELL)
			value = price - marketValue;

		return value > 0 ? value : default;
	}
}
