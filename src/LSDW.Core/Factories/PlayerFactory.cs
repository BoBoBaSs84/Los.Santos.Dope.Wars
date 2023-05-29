using LSDW.Core.Classes;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Factories;

/// <summary>
/// The player factory class.
/// </summary>
public static class PlayerFactory
{
	/// <summary>
	/// Creates a new player instance.
	/// </summary>
	public static IPlayer CreatePlayer()
		=> new Player(InventoryFactory.CreateInventory(), 0);

	/// <summary>
	/// Creates a new player instance.
	/// </summary>
	/// <param name="experience">The player experience points.</param>
	public static IPlayer CreatePlayer(int experience)
		=> new Player(InventoryFactory.CreateInventory(), experience);

	/// <summary>
	/// Creates a new player instance.
	/// </summary>
	/// <param name="inventory">The player inventory.</param>
	/// <param name="experience">The player experience points.</param>
	public static IPlayer CreatePlayer(IInventory inventory, int experience)
		=> new Player(inventory, experience);
}
