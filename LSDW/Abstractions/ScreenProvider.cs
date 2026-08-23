using BB84.SourceGenerators.Attributes;

namespace LSDW.Abstractions;

internal partial interface IScreenProvider;

[GenerateAbstraction(typeof(GTA.UI.Screen), typeof(IScreenProvider), typeof(ScreenProvider))]
internal sealed partial class ScreenProvider;
