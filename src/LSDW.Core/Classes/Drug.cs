using LSDW.Core.Classes.Base;
using LSDW.Core.Enumerators;
using LSDW.Core.Extensions;
using LSDW.Core.Helpers;
using LSDW.Core.Interfaces.Classes;
using static LSDW.Core.Classes.Settings.Market;
using RESX = LSDW.Core.Properties.Resources;

namespace LSDW.Core.Classes;

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

	public int Quantity
	{
		get => quantity;
		private set => SetProperty(ref quantity, value);
	}

	public int Price { get; private set; }

	public int PossibleProfit
		=> CalculatePossibleProfit();

	public void Add(int quantity, int price)
	{
		if (quantity < 1)
			return;

		if (price < 0)
		{
			string message = RESX.Exception_Drug_Add.FormatInvariant(price);
			throw new ArgumentOutOfRangeException(nameof(price), message);
		}

		Price = ((Price * Quantity) + (price * quantity)) / (Quantity + quantity);
		Quantity += quantity;
	}

	public void RandomizePrice(int playerLevel)
	{
		double minimumDrugValue = (double)MinimumDrugValue;
		double maximumDrugValue = (double)MaximumDrugValue;

		int marketValue = DrugType.GetMarketValue();
		double levelLimit = (double)playerLevel / 1000;
		double lowerLimit = (minimumDrugValue - levelLimit) * marketValue;
		double upperLimit = (maximumDrugValue + levelLimit) * marketValue;

		int newPrice = RandomHelper.GetInt(lowerLimit, upperLimit);
		SetPrice(newPrice);
	}

	public void RandomizeQuantity(int playerLevel)
	{
		float nonZeroChance = DrugType.GetProbability();

		if (RandomHelper.GetDouble() > nonZeroChance)
		{
			SetQuantity(0);
			return;
		}

		int minQuantity = 0 + playerLevel;
		int maxQuantity = 5 + (playerLevel * 5);

		int newQuantity = RandomHelper.GetInt(minQuantity, maxQuantity);
		SetQuantity(newQuantity);
	}

	public void Remove(int quantity)
	{
		if (quantity < 0)
			return;

		int resultingQuantity = Quantity - quantity;

		if (resultingQuantity < 0)
		{
			string message = RESX.Exception_Drug_Remove.FormatInvariant(resultingQuantity);
			throw new ArgumentOutOfRangeException(nameof(quantity), message);
		}

		Quantity -= quantity;

		if (Quantity.Equals(0))
			Price = 0;
	}

	public void SetPrice(int price)
		=> Price = price;

	public void SetQuantity(int quantity)
		=> Quantity = quantity;

	private int CalculatePossibleProfit()
		=> Quantity.Equals(0) ? 0 : (MarketValue - Price) * Quantity;
}
