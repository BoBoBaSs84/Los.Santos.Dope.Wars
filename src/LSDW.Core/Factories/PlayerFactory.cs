using LSDW.Core.Classes;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Factories;

/// <summary>
/// The character factory class.
/// </summary>
public static class PlayerFactory
{
	/// <summary>
	/// Should create a player character instance.
	/// </summary>
	public static IPlayer CreatePlayer()
		=> new PlayerCharacter();

	/// <summary>
	/// Should create a player character instance.
	/// </summary>
	/// <param name="inventory">The player inventory.</param>
	/// <param name="experience">The player experience points.</param>
	public static IPlayer CreatePlayer(IInventory inventory, int experience)
		=> new PlayerCharacter(inventory, experience);

	/// <summary>
	/// Should create a player character instance from a saved player state.
	/// </summary>
	/// <param name="playerState">The saved player state.</param>
	public static IPlayer CreatePlayer(PlayerState playerState)
	{
		IInventory inventory = InventoryFactory.CreateInventory(playerState.Inventory);
		return new PlayerCharacter(inventory, playerState.Experience);
	}
}
