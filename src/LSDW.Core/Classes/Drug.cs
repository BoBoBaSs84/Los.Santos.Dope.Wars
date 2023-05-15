using LSDW.Core.Enumerators;
using LSDW.Core.Extensions;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Classes;

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
	internal Drug(DrugType drugType, int quantity, int price)
	{
		DrugType = drugType;
		Quantity = quantity;
		Price = price;
	}

	public DrugType DrugType { get; }

	public string Name
		=> DrugType.GetDisplayName();

	public int MarketValue
		=> DrugType.GetMarketValue();

	public int Quantity { get; private set; }

	public int Price { get; private set; }

	public int PossibleProfit
		=> CalculatePossibleProfit();

	public void Add(int quantity, int price)
	{
		if (quantity < 1)
			throw new ArgumentOutOfRangeException(nameof(quantity));

		if (price < 0)
			throw new ArgumentOutOfRangeException(nameof(price));

		Price = ((Price * Quantity) + (price * quantity)) / (Quantity + quantity);
		Quantity += quantity;
	}

	public void Remove(int quantity)
	{
		if (quantity < 1)
			throw new ArgumentOutOfRangeException(nameof(quantity));

		if (Quantity - quantity < 0)
			throw new ArgumentOutOfRangeException(nameof(quantity));

		Quantity -= quantity;
	}

	private int CalculatePossibleProfit()
		=> Quantity.Equals(0) ? 0 : (MarketValue - Price) * Quantity;
}
