using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Extensions;
using LSDW.Domain.Extensions;
using LSDW.Domain.Helpers;
using LSDW.Domain.Models.Base;
using LSDW.Domain.Properties;

namespace LSDW.Domain.Models;

/// <summary>
/// The drug class.
/// </summary>
/// <remarks>
/// Inherits from the <see cref="NotificationBase"/> class and
/// implements the members of the <see cref="IDrug"/> interface.
/// </remarks>
internal sealed class Drug : NotificationBase, IDrug
{
	private int quantity;
	private int price;

	/// <summary>
	/// Initializes a instance of the drug class.
	/// </summary>
	/// <param name="drugType">The type of the drug.</param>
	/// <param name="quantity">The quantity of the drug.</param>
	/// <param name="currentPrice">The current price of the drug.</param>
	internal Drug(DrugType drugType, int quantity, int currentPrice)
	{
		Price = currentPrice;
		Type = drugType;
		Quantity = quantity;
	}

	public int Price
	{
		get => price;
		private set => SetProperty(ref price, value);
	}

	public DrugType Type { get; }

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

		Price = ((Price * Quantity) + (price * quantity)) / (Quantity + quantity);
		Quantity += quantity;
	}

	public void RandomizePrice(int playerLevel)
		=> Price = RandomHelper.GetInt(GetLowestPrice(playerLevel), GetHighestPrice(playerLevel));

	public void RandomizeQuantity(int playerLevel)
	{
		float nonZeroChance = Type.GetProbability();

		if (RandomHelper.GetDouble() > nonZeroChance)
		{
			Quantity = 0;
			return;
		}

		Quantity = RandomHelper.GetInt(GetLowestQuantity(playerLevel), GetHighestQuantity(playerLevel));
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
			Price = Quantity;
	}

	public void SpecialBuyOffer(int playerLevel)
	{
		Quantity = 0;
		Price = GetHighestPrice(playerLevel);
	}

	public void SpecialSellOffer(int playerLevel)
	{
		Quantity = GetHighestQuantity(playerLevel);
		Price = GetLowestPrice(playerLevel);
	}

	/// <summary>
	/// Returns the current highest possible price, depending on the current player level.
	/// </summary>
	/// <param name="playerLevel">The current player level.</param>
	private int GetHighestPrice(int playerLevel)
	{
		float maximumDrugPrice = Settings.Instance.Market.MaximumDrugPrice.Value;
		float playerfactor = playerLevel / (float)1000;
		float averagePrice = Type.GetAveragePrice();
		float highestPrice = (maximumDrugPrice + playerfactor) * averagePrice;

		return (int)highestPrice;
	}

	/// <summary>
	/// Returns the current lowest possible price, depending on the current player level.
	/// </summary>
	/// <param name="playerLevel">The current player level.</param>
	private int GetLowestPrice(int playerLevel)
	{
		float maximumDrugPrice = Settings.Instance.Market.MaximumDrugPrice.Value;
		float playerfactor = playerLevel / (float)1000;
		float averagePrice = Type.GetAveragePrice();
		float lowestPrice = (maximumDrugPrice - playerfactor) * averagePrice;

		return (int)lowestPrice;
	}

	/// <summary>
	/// Returns the current highest possible quantity, depending on the current player level.
	/// </summary>
	/// <param name="playerLevel">The current player level.</param>
	private static int GetHighestQuantity(int playerLevel)
		=> 5 + (playerLevel * 5);

	/// <summary>
	/// Returns the current lowest possible quantity, depending on the current player level.
	/// </summary>
	/// <param name="playerLevel">The current player level.</param>
	private static int GetLowestQuantity(int playerLevel)
		=> 0 + playerLevel;
}
