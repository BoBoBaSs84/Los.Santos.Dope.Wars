﻿using LSDW.Enumerators;
using LSDW.Extensions;
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
	internal Drug(DrugType drugType, int quantity, int price)
	{
		DrugType = drugType;
		Quantity = quantity;
		Price = price;
	}
	
	/// <inheritdoc/>
	public DrugType DrugType { get; }
	/// <inheritdoc/>
	public string Name => DrugType.GetDisplayName();
	/// <inheritdoc/>
	public int MarketValue => DrugType.GetMarketValue();
	/// <inheritdoc/>
	public int Quantity { get; private set; }
	/// <inheritdoc/>
	public int Price { get; private set; }
	/// <inheritdoc/>
	public int Profit => CalculatePossibleProfit();

	/// <inheritdoc/>
	public void Add(int quantity, int price)
	{
		if (quantity < 1)
			throw new ArgumentOutOfRangeException(nameof(quantity));

		if (price < 0)
			throw new ArgumentOutOfRangeException(nameof(price));

		var dt = System.DateTime.UtcNow.Year;

		Price = ((Price * Quantity) + (price * quantity)) / (Quantity + quantity);
		Quantity += quantity;
	}

	/// <inheritdoc/>
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