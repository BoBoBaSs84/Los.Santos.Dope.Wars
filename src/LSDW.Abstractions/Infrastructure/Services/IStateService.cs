using LSDW.Abstractions.Domain.Models;

namespace LSDW.Abstractions.Infrastructure.Services;

/// <summary>
/// The state service interface.
/// </summary>
public interface IStateService
{
	/// <summary>
	/// The player instance to use.
	/// </summary>
	IPlayer Player { get; }

	/// <summary>
	/// The dealer collection instance to use.
	/// </summary>
	ICollection<IDealer> Dealers { get; }

	/// <summary>
	/// Loads the game state from file.
	/// </summary>
	/// <param name="decompress">Should the content be decompressed?</param>
	/// <remarks>
	/// If no save file is found, a new save file will be created.
	/// </remarks>
	/// <returns><see langword="true"/> or <see langword="false"/> if successful.</returns>
	bool Load(bool decompress = true);

	/// <summary>
	/// Saves the current game state.
	/// </summary>
	/// <param name="compress">Should the content be compressed?</param>
	/// <returns><see langword="true"/> or <see langword="false"/> if successful.</returns>
	bool Save(bool compress = true);
}
