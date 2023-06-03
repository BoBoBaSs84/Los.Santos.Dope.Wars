using LSDW.Domain.Enumerators;
using LSDW.Domain.Factories;
using LSDW.Domain.Helpers;
using LSDW.Domain.Interfaces.Models;

namespace LSDW.Domain.Extensions;

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
}
