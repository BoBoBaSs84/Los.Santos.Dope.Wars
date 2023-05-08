using LSDW.Enumerators;

namespace LSDW.Interfaces.Classes;

/// <summary>
/// The drug interface.
/// </summary>
internal interface IDrug
{
	/// <summary>
	/// The type of the drug.
	/// </summary>
	DrugType DrugType { get; }

	/// <summary>
	/// The quantity of the drug.
	/// </summary>
	int Quantity { get; }

	/// <summary>
	/// The price of the drug.
	/// </summary>
	int Price { get; }

	/// <summary>
	/// The profit of the drug compared to the nominal market price.
	/// </summary>
	int Profit { get; }

	/// <summary>
	/// Should add the quantity and price to the drug.
	/// </summary>
	/// <param name="quantity">The quantity of the drug to add.</param>
	/// <param name="price">The price of the drug to add.</param>
	void Add(int quantity, int price);	
	
	/// <summary>
	/// Should remove the quantity from the drug.
	/// </summary>
	/// <param name="quantity">The quantity of the drug to remove.</param>
	void Remove(int quantity);
}
