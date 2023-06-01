using LSDW.Classes.Persistence;

namespace LSDW.Interfaces.Services;

/// <summary>
/// The game state service interface.
/// </summary>
public interface IGameStateService
{
	/// <summary>
	/// Loads the game state from file.
	/// </summary>
	GameState Load();

	/// <summary>
	/// Saves the current game state.
	/// </summary>
	/// <returns><see langword="true"/> or <see langword="false"/> if successful.</returns>
	bool Save();
}
