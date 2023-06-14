using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Extensions;
using LSDW.Domain.Factories;
using LSDW.Domain.Helpers;

namespace LSDW.Domain.Extensions;

/// <summary>
/// The inventory extensions class.
/// </summary>
public static class InventoryExtensions
{
	/// <summary>
	/// Restocks the inventory depending on the player level.
	/// </summary>
	/// <remarks>
	/// This will do the following things:
	/// <list type="bullet">
	/// <item>The <see cref="IDrug.CurrentPrice"/></item>
	/// <item>The <see cref="IDrug.Quantity"/></item>
	/// <item>The <see cref="IInventory.Money"/></item>
	/// </list>
	/// If no <paramref name="playerLevel"/> is provided, no level bonuses are applied.
	/// </remarks>
	/// <param name="inventory">The inventory to restock.</param>
	/// <param name="playerLevel">The current player level.</param>
	public static IInventory Restock(this IInventory inventory, int playerLevel = 0)
	{
		inventory.Clear();
		IEnumerable<DrugType> drugTypes = DrugType.COKE.GetList();

		foreach (DrugType drugType in drugTypes)
		{
			IDrug drug = DomainFactory.CreateDrug(drugType);
			drug.RandomizeQuantity(playerLevel);
			drug.RandomizePrice(playerLevel);
			inventory.Add(drug);
		}

		int minMoney = (playerLevel + 1) * 50;
		int maxMoney = (playerLevel + 1) * 250;

		int money = RandomHelper.GetInt(minMoney, maxMoney);
		inventory.Add(money);

		return inventory;
	}

	/// <summary>
	/// Refreshes the inventory depending on the player level.
	/// </summary>
	/// <remarks>
	/// This will do the following things:
	/// <list type="bullet">
	/// <item>The <see cref="IDrug.CurrentPrice"/></item>
	/// </list>
	/// If no <paramref name="playerLevel"/> is provided, no level bonuses are applied.
	/// </remarks>
	/// <param name="inventory">The inventory to refresh.</param>
	/// <param name="playerLevel">The current player level.</param>
	/// <returns></returns>
	public static IInventory Refresh(this IInventory inventory, int playerLevel = 0)
	{
		foreach (IDrug drug in inventory)
			drug.RandomizePrice(playerLevel);
		return inventory;
	}
}
