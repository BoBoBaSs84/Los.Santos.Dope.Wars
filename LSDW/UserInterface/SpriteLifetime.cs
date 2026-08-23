namespace LSDW.UserInterface;

/// <summary>
/// The reason the five types holding <see cref="GTA.UI.Sprite"/> fields suppress
/// <c>CA1001</c> rather than implementing <see cref="IDisposable"/>.
/// </summary>
/// <remarks>
/// Kept as one shared constant so the reasoning is written once. A
/// <c>Justification</c> has to be a compile-time constant, so it cannot just be a
/// comment at each site.
/// </remarks>
internal static class SpriteLifetime
{
  /// <summary>
  /// Why the sprite fields are never disposed.
  /// </summary>
  public const string Justification =
    "Every GTA.UI.Sprite field here is built once and drawn for the life of the script. " +
    "Sprite.Dispose decrements a shared per-texture-dictionary refcount and, at the last " +
    "holder, issues SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED -- so disposing these " +
    "would release the very dictionaries the HUD needs resident. What is left unreleased " +
    "is two small CoTaskMem blocks per sprite, freed with the process; reloading the " +
    "script leaks roughly forty of them, which is bounded and not observable in game.";
}
