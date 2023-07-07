using GTA;
using LSDW.Abstractions.Domain.Providers;

namespace LSDW.Domain.Providers;

/// <summary>
/// The audio provider class.
/// </summary>
/// <remarks>
/// Wrapper for the <see cref="Audio"/> methods.
/// </remarks>
internal sealed class AudioProvider : IAudioProvider
{
	/// <summary>
	/// The singleton instance of the audio provider.
	/// </summary>
	internal static readonly AudioProvider Instance = new();

	public void PlayMusic(string musicFile)
		=> Audio.PlayMusic(musicFile);

	public int PlaySoundAt(Entity entity, string soundFile)
		=> Audio.PlaySoundAt(entity, soundFile);

	public int PlaySoundAt(Entity entity, string soundFile, string soundSet)
		=> Audio.PlaySoundAt(entity, soundFile, soundSet);

	public int PlaySoundFrontend(string soundFile)
		=> Audio.PlaySoundFrontend(soundFile);

	public int PlaySoundFrontend(string soundFile, string soundSet)
		=> Audio.PlaySoundFrontend(soundFile, soundSet);

	public void StopMusic(string musicFile)
		=> Audio.StopMusic(musicFile);

	public void StopSound(int id)
		=> Audio.StopSound(id);
}
