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
}
