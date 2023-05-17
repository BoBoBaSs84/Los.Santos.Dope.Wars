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
		=> new Player();

	/// <summary>
	/// Should create a player character instance.
	/// </summary>
	/// <param name="inventory">The player inventory.</param>
	/// <param name="experience">The player experience points.</param>
	public static IPlayer CreatePlayer(IInventory inventory, int experience)
		=> new Player(inventory, experience);
}
