using LSDW.Core.Enumerators;

namespace LSDW.Core.Classes;

/// <summary>
/// The transaction object class.
/// </summary>
public sealed class TransactionObject
{
	/// <summary>
	/// Initializes a instance of the transaction object class.
	/// </summary>
	/// <param name="drugType">The type of the drug.</param>
	/// <param name="quantity">The quantity of the drug.</param>
	/// <param name="price">The price of the drug.</param>
	public TransactionObject(DrugType drugType, int quantity, int price)
	{
		DrugType = drugType;
		Quantity = quantity;
		Price = price;
	}

	/// <summary>
	/// The type of the drug.
	/// </summary>
	public DrugType DrugType { get; }

	/// <summary>
	/// The quantity of the drug.
	/// </summary>
	public int Quantity { get; }

	/// <summary>
	/// The price of the drug.
	/// </summary>
	public int Price { get; }
}
