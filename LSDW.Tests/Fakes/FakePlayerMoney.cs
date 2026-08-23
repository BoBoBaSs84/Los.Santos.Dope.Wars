using LSDW.Abstractions;

namespace LSDW.Tests.Fakes;

/// <summary>
/// A wallet holding whatever a test puts in it.
/// </summary>
/// <remarks>
/// Hand-written like the seam it stands behind. <c>GTA.Game.Player.Money</c> is a native both
/// ways, so the money gate on a purchase could not be reached before this existed.
/// </remarks>
internal sealed class FakePlayerMoney : IPlayerMoney
{
  /// <inheritdoc/>
  public int Money { get; set; }
}
