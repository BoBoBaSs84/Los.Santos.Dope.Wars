using LSDW.Core.Enumerators;
using LSDW.Core.Factories;
using LSDW.Core.Helpers;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Extensions;

/// <summary>
/// The inventory extensions class.
/// </summary>
public static class InventoryExtensions
{
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
			IDrug drug = DrugFactory.CreateDrug(drugType);
			_ = drug.RandomizeQuantity(playerLevel).RandomizePrice(playerLevel);
			inventory.Add(drug);
		}

		int money = GetRandomMoney(playerLevel);
		inventory.Add(money);

		return inventory;
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
