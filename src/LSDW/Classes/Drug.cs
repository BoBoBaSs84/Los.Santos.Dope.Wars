using LSDW.Enumerators;
using LSDW.Interfaces.Classes;

namespace LSDW.Classes;

/// <summary>
/// The drug class.
/// </summary>
internal sealed class Drug : IDrug
{
	/// <summary>
	/// Initializes a instance of the drug class.
	/// </summary>
	/// <param name="drugType">The type of the drug.</param>
	/// <param name="quantity">The quantity of the drug.</param>
	/// <param name="price">The price of the drug.</param>
	public Drug(DrugType drugType, int quantity, int price)
	{
		DrugType = drugType;
		Quantity = quantity;
		Price = price;
	}
		
	public DrugType DrugType { get; }
	public int Quantity { get; private set; }
	public int Price { get; private set; }

	public void Add(int quantity, int price)
	{
		Price = (Price + price) / (Quantity + quantity);
		Quantity += quantity;
	}

	public void Remove(int quantity)
	{
		if (Quantity - quantity < 0)
			throw new ArgumentOutOfRangeException(nameof(quantity));
		Quantity -= quantity;
	}
}
