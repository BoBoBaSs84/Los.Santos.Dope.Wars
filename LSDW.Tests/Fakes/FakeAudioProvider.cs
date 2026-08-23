using LSDW.Abstractions;

using GTA;
using GTA.Math;

namespace LSDW.Tests.Fakes;

/// <summary>
/// Records the frontend sounds a subsystem asked for instead of playing them.
/// </summary>
/// <remarks>
/// Only <see cref="PlaySoundFrontendAndForget"/> is recorded, because it is the only member of
/// the generated surface the mod calls. The rest throw rather than returning a default: a test
/// that reaches one has found a new call site, and should be told so rather than quietly passing.
/// </remarks>
internal sealed class FakeAudioProvider : IAudioProvider
{
  /// <summary>
  /// The sound names asked for, in order. The set name is not recorded: nothing asserts on it,
  /// and the two the mod uses are decided by the sound, not independently.
  /// </summary>
  internal List<string> Sounds { get; } = [];

  /// <inheritdoc/>
  public void PlaySoundFrontendAndForget(string soundName, string setName, bool enableOnReplay)
    => Sounds.Add(soundName);

  /// <inheritdoc/>
  public void PlayMusic(string musicFile) => throw new NotSupportedException();

  /// <inheritdoc/>
  public void StopMusic(string musicFile) => throw new NotSupportedException();

  /// <inheritdoc/>
  public ScriptSound GetSoundId() => throw new NotSupportedException();

  /// <inheritdoc/>
  public void PlaySoundAndForget(string soundName, string setName, bool enableOnReplay) => throw new NotSupportedException();

  /// <inheritdoc/>
  public void PlaySoundFromEntityAndForget(Entity entity, string soundName, string setName) => throw new NotSupportedException();

  /// <inheritdoc/>
  public void PlaySoundFromPositionAndForget(Vector3 position, string soundName, string setName, bool isExteriorLoc) => throw new NotSupportedException();

  /// <inheritdoc/>
  public void SetAudioFlag(AudioFlags flag, bool toggle) => throw new NotSupportedException();

  /// <inheritdoc/>
  public int PlaySoundAt(Entity entity, string soundFile) => throw new NotSupportedException();

  /// <inheritdoc/>
  public int PlaySoundAt(Entity entity, string soundFile, string soundSet) => throw new NotSupportedException();

  /// <inheritdoc/>
  public int PlaySoundAt(Vector3 position, string soundFile) => throw new NotSupportedException();

  /// <inheritdoc/>
  public int PlaySoundAt(Vector3 position, string soundFile, string soundSet) => throw new NotSupportedException();

  /// <inheritdoc/>
  public int PlaySoundFrontend(string soundFile) => throw new NotSupportedException();

  /// <inheritdoc/>
  public int PlaySoundFrontend(string soundFile, string soundSet) => throw new NotSupportedException();

  /// <inheritdoc/>
  public void StopSound(int id) => throw new NotSupportedException();

  /// <inheritdoc/>
  public void ReleaseSound(int id) => throw new NotSupportedException();

  /// <inheritdoc/>
  public bool HasSoundFinished(int id) => throw new NotSupportedException();
}
