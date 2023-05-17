using LSDW.Core.Enumerators;
using LSDW.Core.Factories;
using LSDW.Core.Helpers;
using LSDW.Core.Interfaces.Classes;
using LSDW.Core.Properties;

namespace LSDW.Core.Extensions;

/// <summary>
/// The inventory extensions class.
/// </summary>
public static class InventoryExtensions
{
	private static readonly double MinValue = Settings.Default.MinimumDrugValue;
	private static readonly double MaxValue = Settings.Default.MaximumDrugValue;

	/// <summary>
	/// Randomizes the inventory depending on the player level.
	/// </summary>
	/// <remarks>
	/// This will randomizes the following things:
	/// <list type="bullet">
	/// <item>The <see cref="IDrug.Price"/></item>
	/// <item>The <see cref="IDrug.Quantity"/></item>
	/// <item>The <see cref="IInventory.Money"/></item>
	/// </list>
	/// If no <paramref name="playerLevel"/> is provided, no bonuses are applied.
	/// </remarks>
	/// <param name="inventory">The inventory to randomize.</param>
	/// <param name="playerLevel">The current player level.</param>
	public static IInventory Randomize(this IInventory inventory, int playerLevel = 0)
	{
		IEnumerable<DrugType> drugTypes = DrugType.COKE.GetList();
				
		inventory.Clear();

		foreach (DrugType drugType in drugTypes)
		{
			IDrug drug = DrugFactory.CreateDrug(
				drugType: drugType,
				quantity: GetRandomQuantity(drugType, playerLevel),
				price: GetRandomPrice(drugType, playerLevel)
				);

			inventory.Add(drug);
		}

		int money = GetRandomMoney(playerLevel);
		inventory.Add(money);

		return inventory;
	}

	/// <summary>
	/// Returns a random price for the provided drug.
	/// </summary>
	/// <remarks>
	/// The upper and lower price limits depend on the player
	/// level <b>(maximum ±10%)</b> and user settings.
	/// </remarks>
	/// <param name="drugType">The type of the drug.</param>
	/// <param name="playerLevel">The current player level.</param>
	private static int GetRandomPrice(DrugType drugType, int playerLevel)
	{
		double levelLimit = (double)playerLevel / 1000;
		double lowerLimit = (MinValue - levelLimit) * drugType.GetMarketValue();
		double upperLimit = (MaxValue + levelLimit) * drugType.GetMarketValue();
		return RandomHelper.GetInt(lowerLimit, upperLimit);
	}

	/// <summary>
	/// Returns a random quantity for the provided drug.
	/// </summary>
	/// <remarks>
	/// The zero quantity chance depends on the rank of the drug,
	/// the higher the rank the higher the no quantity chance.
	/// </remarks>
	/// <param name="drugType">The type of the drug.</param>
	/// <param name="playerLevel">The current player level.</param>
	private static int GetRandomQuantity(DrugType drugType, int playerLevel)
	{
		double nonZeroChance = (double)1 / drugType.GetRank();

		if (RandomHelper.GetDouble() > nonZeroChance)
			return 0;

		int minQuantity = 0 + playerLevel;
		int maxQuantity = 5 + (playerLevel * 5);

		return RandomHelper.GetInt(minQuantity, maxQuantity);
	}

	/// <summary>
	/// Returns a random amount of money for the inventory.
	/// </summary>
	/// <param name="playerLevel">The current player level.</param>
	private static int GetRandomMoney(int playerLevel)
	{
		int minMoney = (playerLevel + 1) * 50;
		int maxMoney = (playerLevel + 1) * 250;
		return RandomHelper.GetInt(minMoney, maxMoney);
	}
}
