using LSDW.Core.Classes;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Factories;

/// <summary>
/// The inventory factory class.
/// </summary>
public static class InventoryFactory
{
	/// <summary>
	/// Creates a new inventory instance.
	/// </summary>
	public static IInventory CreateInventory()
		=> new Inventory(DrugFactory.CreateAllDrugs().ToList(), 0);

	/// <summary>
	/// Creates a new inventory instance.
	/// </summary>
	/// <param name="money">The money to add to the inventory.</param>
	public static IInventory CreateInventory(int money)
		=> new Inventory(DrugFactory.CreateAllDrugs().ToList(), money);

	/// <summary>
	/// Creates a new inventory instance.
	/// </summary>
	/// <param name="drugs">The collection of drugs to add to the inventory.</param>
	/// <param name="money">The money to add to the inventory.</param>
	public static IInventory CreateInventory(IEnumerable<IDrug> drugs, int money)
		=> new Inventory(drugs, money);
}
