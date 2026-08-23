using BB84.SourceGenerators.Attributes;

namespace LSDW.Abstractions;

internal partial interface IAudioProvider;

[GenerateAbstraction(typeof(GTA.Audio), typeof(IAudioProvider), typeof(AudioProvider))]
internal sealed partial class AudioProvider;
