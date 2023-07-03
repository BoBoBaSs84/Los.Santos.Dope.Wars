using GTA;

namespace LSDW.Abstractions.Domain.Providers;

/// <summary>
/// The audio provider interface.
/// </summary>
public interface IAudioProvider
{
	/// <summary>
	/// Plays music from the game's music files.
	/// </summary>
	/// <param name="musicFile">The music file to play.</param>
	void PlayMusic(string musicFile);

	/// <summary>
	/// Cancels playing a music file.
	/// </summary>
	/// <param name="musicFile">The music file to stop.</param>
	void StopMusic(string musicFile);

	/// <summary>
	/// Plays a sound from the game's sound files at the specified entity.
	/// </summary>
	/// <param name="entity">The entity to play the sound at.</param>
	/// <param name="soundFile">The sound file to play.</param>
	/// <returns>The identifier of the active sound effect instance.</returns>
	int PlaySoundAt(Entity entity, string soundFile);

	/// <summary>
	/// Plays a sound from the game's sound files at the specified entity.
	/// </summary>
	/// <param name="entity">The entity to play the sound at.</param>
	/// <param name="soundFile">The sound file to play.</param>
	/// <param name="soundSet">The name of the sound inside the file.</param>
	/// <returns>The identifier of the active sound effect instance.</returns>
	int PlaySoundAt(Entity entity, string soundFile, string soundSet);

	/// <summary>
	/// Plays a sound from the game's sound files without transformation.
	/// </summary>
	/// <param name="soundFile">The sound file to play.</param>
	/// <returns>The identifier of the active sound effect instance.</returns>
	int PlaySoundFrontend(string soundFile);

	/// <summary>
	/// Plays a sound from the game's sound files without transformation.
	/// </summary>
	/// <param name="soundFile">The sound file to play.</param>
	/// <param name="soundSet">The name of the sound inside the file.</param>
	/// <returns>The identifier of the active sound effect instance.</returns>
	int PlaySoundFrontend(string soundFile, string soundSet);

	/// <summary>
	/// Cancels playing the specified sound instance.
	/// </summary>
	/// <param name="id">The identifier of the active sound effect instance.</param>
	void StopSound(int id);
}
