using LSDW.Domain.Enumerators;
using LSDW.Domain.Interfaces.Models;

namespace LSDW.Domain.Classes.Models;

/// <summary>
/// The transaction class.
/// </summary>
internal sealed class Transaction : ITransaction
{
	/// <summary>
	/// Initializes a instance of the transaction class.
	/// </summary>
	/// <param name="dateTime">The point in time of the transaction.</param>
	/// <param name="transactionType">The type of the transaction.</param>
	/// <param name="drugType">The drug type of the transaction.</param>
	/// <param name="quantity">The quantity of the transaction.</param>
	/// <param name="price">The unit price of the transaction.</param>
	internal Transaction(DateTime dateTime, TransactionType transactionType, DrugType drugType, int quantity, int price)
	{
		DateTime = dateTime;
		TransactionType = transactionType;
		DrugType = drugType;
		Quantity = quantity;
		Price = price;
	}

	public DateTime DateTime { get; }
	public TransactionType TransactionType { get; }
	public DrugType DrugType { get; }
	public int Quantity { get; }
	public int Price { get; }
}
