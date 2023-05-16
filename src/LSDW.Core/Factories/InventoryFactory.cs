using LSDW.Core.Classes;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Factories;

/// <summary>
/// The inventory factory class.
/// </summary>
public static class InventoryFactory
{
	/// <summary>
	/// Should create a new empty inventory instance.
	/// </summary>
	/// <param name="money">The money to add to the inventory.</param>
	public static IInventory CreateInventory(int money = default)
		=> new Inventory(new List<IDrug>(), money);

	/// <summary>
	/// Should create a new inventory instance from a collection of drugs.
	/// </summary>
	/// <param name="drugs">The drugs to add to the inventory.</param>
	/// <param name="money">The money to add to the inventory.</param>
	public static IInventory CreateInventory(IEnumerable<IDrug> drugs, int money = default)
		=> new Inventory(drugs, money);
}
