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
	/// 
	/// </summary>
	/// <param name="inventory"></param>
	public static IInventory Randomize(this IInventory inventory)
	{
		inventory.Clear();
		inventory.Remove(inventory.Money);

		IEnumerable<IDrug> randomDrugs = DrugFactory.CreateRandomDrugs();
		foreach (IDrug drug in randomDrugs)
			inventory.Add(drug);

		int money = RandomHelper.GetInt(randomDrugs.Sum(x => x.Quantity * x.MarketValue));
		inventory.Add(money);

		return inventory;
	}
}
