using LSDW.Core.Helpers;
using LSDW.Core.Interfaces.Classes;
using LSDW.Core.Properties;

namespace LSDW.Core.Extensions;

/// <summary>
/// The drug extensions class.
/// </summary>
public static class DrugExtensions
{
	private static readonly double MinValue = Settings.Default.MinimumDrugValue;
	private static readonly double MaxValue = Settings.Default.MaximumDrugValue;

	/// <summary>
	/// Randomizes the quantity for the provided drug.
	/// </summary>
	/// <remarks>
	/// The zero quantity chance depends on the rank of the drug,
	/// the higher the rank the higher the no quantity chance.
	/// </remarks>
	/// <param name="drug">The drug to change.</param>
	/// <param name="playerLevel">The current player level.</param>
	public static IDrug RandomizeQuantity(this IDrug drug, int playerLevel = 0)
	{
		double nonZeroChance = (double)1 / drug.DrugType.GetRank();

		if (RandomHelper.GetDouble() > nonZeroChance)
		{
			drug.SetQuantity(0);
			return drug;
		}

		int minQuantity = 0 + playerLevel;
		int maxQuantity = 5 + (playerLevel * 5);
		int newQuantity = RandomHelper.GetInt(minQuantity, maxQuantity);

		drug.SetQuantity(newQuantity);

		return drug;
	}

	/// <summary>
	/// Randomizes the price for the provided drug.
	/// </summary>
	/// <remarks>
	/// The upper and lower price limits depend on the player
	/// level <b>(maximum ±10%)</b> and the user settings.
	/// </remarks>
	/// <param name="drug">The drug to change.</param>
	/// <param name="playerLevel">The current player level.</param>
	public static IDrug RandomizePrice(this IDrug drug, int playerLevel = 0)
	{
		double levelLimit = (double)playerLevel / 1000;
		double lowerLimit = (MinValue - levelLimit) * drug.DrugType.GetMarketValue();
		double upperLimit = (MaxValue + levelLimit) * drug.DrugType.GetMarketValue();

		int newPrice = RandomHelper.GetInt(lowerLimit, upperLimit);
		drug.SetPrice(newPrice);

		return drug;
	}
}
