using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Extensions;
using LSDW.Abstractions.Models;
using LSDW.Domain.Extensions;
using LSDW.Domain.Helpers;
using LSDW.Domain.Models.Base;
using LSDW.Domain.Properties;

namespace LSDW.Domain.Models;

/// <summary>
/// The drug class.
/// </summary>
/// <remarks>
/// Inherits from the <see cref="Notification"/> class and
/// implements the members of the <see cref="IDrug"/>
/// </remarks>
internal sealed class Drug : Notification, IDrug
{
	private int quantity;

	/// <summary>
	/// Initializes a instance of the drug class.
	/// </summary>
	/// <param name="drugType">The type of the drug.</param>
	/// <param name="quantity">The quantity of the drug.</param>
	/// <param name="currentPrice">The current price of the drug.</param>
	internal Drug(DrugType drugType, int quantity, int currentPrice)
	{
		AveragePrice = drugType.GetAverageDrugPrice();
		CurrentPrice = currentPrice;
		Name = drugType.GetDrugName();
		Type = drugType;
		Quantity = quantity;
	}

	public int AveragePrice { get; }

	public int CurrentPrice { get; private set; }

	public DrugType Type { get; }

	public string Name { get; }

	public int Quantity
	{
		get => quantity;
		private set => SetProperty(ref quantity, value);
	}

	public void Add(int quantity, int price)
	{
		if (quantity < 1)
			return;

		if (price < 0)
		{
			string message = Resources.Exception_Drug_Add.FormatInvariant(price);
			throw new ArgumentOutOfRangeException(nameof(price), message);
		}

		CurrentPrice = (CurrentPrice * Quantity + price * quantity) / (Quantity + quantity);
		Quantity += quantity;
	}

	public void RandomizePrice(int playerLevel)
	{
		float minimumDrugValue = Settings.Market.MinimumDrugPrice;
		float maximumDrugValue = Settings.Market.MaximumDrugPrice;

		float levelLimit = (float)playerLevel / 1000;
		float lowerLimit = (minimumDrugValue - levelLimit) * AveragePrice;
		float upperLimit = (maximumDrugValue + levelLimit) * AveragePrice;

		CurrentPrice = RandomHelper.GetInt(lowerLimit, upperLimit);
	}

	public void RandomizeQuantity(int playerLevel)
	{
		float nonZeroChance = Type.GetDrugProbability();

		if (RandomHelper.GetDouble() > nonZeroChance)
		{
			Quantity = 0;
			return;
		}

		int minQuantity = 0 + playerLevel;
		int maxQuantity = 5 + playerLevel * 5;

		Quantity = RandomHelper.GetInt(minQuantity, maxQuantity);
	}

	public void Remove(int quantity)
	{
		if (quantity < 0)
			return;

		int resultingQuantity = Quantity - quantity;

		if (resultingQuantity < 0)
		{
			string message = Resources.Exception_Drug_Remove.FormatInvariant(resultingQuantity);
			throw new ArgumentOutOfRangeException(nameof(quantity), message);
		}

		Quantity -= quantity;

		if (Quantity.Equals(0))
			CurrentPrice = Quantity;
	}
}
