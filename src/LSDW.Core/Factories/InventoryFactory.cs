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
	public static IInventoryCollection CreateInventory()
		=> new Inventory(new List<IDrug>());

	/// <summary>
	/// Should create a new inventory instance.
	/// </summary>
	/// <param name="drugs">The drugs to add to the inventory.</param>
	public static IInventoryCollection CreateInventory(IEnumerable<IDrug> drugs)
		=> new Inventory(drugs);
}
