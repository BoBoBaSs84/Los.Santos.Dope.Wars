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
	/// 
	/// </summary>
	/// <param name="quantity"></param>
	/// <param name="price"></param>
	void Add(int quantity, int price);	
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="quantity"></param>
	void Remove(int quantity);
}
