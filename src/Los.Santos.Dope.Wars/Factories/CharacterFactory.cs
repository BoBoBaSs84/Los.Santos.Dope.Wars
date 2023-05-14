using LSDW.Classes;
using LSDW.Interfaces.Classes;

namespace LSDW.Factories;

/// <summary>
/// The character factory class.
/// </summary>
public static class CharacterFactory
{
	/// <summary>
	/// Should create a new player character instance.
	/// </summary>
	public static IPlayerCharacter CreatePlayerCharacter()
		=> new PlayerCharacter();

	/// <summary>
	/// Should create a new player character instance.
	/// </summary>
	/// <param name="inventory">The player inventory.</param>
	/// <param name="spentMoney">The money spent on buying drugs.</param>
	/// <param name="earnedMoney">The money earned on selling drugs.</param>
	/// <param name="experience">The player experience points.</param>
	public static IPlayerCharacter CreatePlayerCharacter(IInventory inventory, int spentMoney, int earnedMoney, int experience)
		=> new PlayerCharacter(inventory, spentMoney, earnedMoney, experience);
}
