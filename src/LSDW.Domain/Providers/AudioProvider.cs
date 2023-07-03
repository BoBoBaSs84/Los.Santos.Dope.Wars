using GTA;
using LSDW.Abstractions.Domain.Providers;

namespace LSDW.Domain.Providers;

internal sealed class AudioProvider : IAudioProvider
{
	public void PlayMusic(string musicFile)
		=> Audio.PlayMusic(musicFile);

	public int PlaySoundAt(Entity entity, string soundFile)
		=> Audio.PlaySoundAt(entity, soundFile);

	public int PlaySoundAt(Entity entity, string soundFile, string soundSet)
		=> PlaySoundAt(entity, soundFile, soundSet);

	public int PlaySoundFrontend(string soundFile)
		=> PlaySoundFrontend(soundFile);

	public int PlaySoundFrontend(string soundFile, string soundSet)
		=> PlaySoundFrontend(soundFile, soundSet);

	public void StopMusic(string musicFile)
		=> Audio.StopMusic(musicFile);

	public void StopSound(int id)
		=> Audio.StopSound(id);
}
