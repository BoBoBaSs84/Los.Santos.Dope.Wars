using GTA;

namespace LSDW.Abstractions;

/// <summary>
/// The player's cash on hand.
/// </summary>
/// <remarks>
/// Hand-written, unlike its neighbours in this folder, and deliberately so. A generated
/// abstraction of <c>GTA.Game</c> hands back a <c>GTA.Player</c> whose every member calls a
/// native — there is no <c>Money</c> on <c>Game</c> itself — so the seam would be one a test
/// double could not stand behind. Two properties written by hand can be.
/// </remarks>
internal interface IPlayerMoney
{
  /// <summary>
  /// Gets or sets the player's money.
  /// </summary>
  int Money { get; set; }
}

/// <summary>
/// The live implementation, over <c>GTA.Game.Player</c>.
/// </summary>
internal sealed class PlayerMoney : IPlayerMoney
{
  /// <inheritdoc/>
  public int Money
  {
    get => Game.Player.Money;
    set => Game.Player.Money = value;
  }
}
