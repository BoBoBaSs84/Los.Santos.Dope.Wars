using LSDW.Core.Classes;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Factories;

/// <summary>
/// The inventory factory class.
/// </summary>
public static class InventoryFactory
{
	/// <summary>
	/// Should create a new player inventory instance.
	/// </summary>
	public static IInventoryCollection CreatePlayerInventory()
		=> new PlayerInventory(new List<IDrug>());

	/// <summary>
	/// Should create a new player inventory instance.
	/// </summary>
	/// <param name="drugs">The drugs to add to the inventory.</param>
	public static IInventoryCollection CreatePlayerInventory(List<IDrug> drugs)
		=> new PlayerInventory(drugs);
}
