using LSDW.Core.Classes;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Factories;

/// <summary>
/// The character factory class.
/// </summary>
public static class CharacterFactory
{
	/// <summary>
	/// Should create a new player character instance.
	/// </summary>
	public static IPlayerCharacter CreateNewPlayer()
		=> new PlayerCharacter();

	/// <summary>
	/// Should create a existing player character instance.
	/// </summary>
	/// <param name="inventory">The player inventory.</param>
	/// <param name="experience">The player experience points.</param>
	public static IPlayerCharacter CreateExistingPlayer(IInventoryCollection inventory, int experience)
		=> new PlayerCharacter(inventory, experience);
}
