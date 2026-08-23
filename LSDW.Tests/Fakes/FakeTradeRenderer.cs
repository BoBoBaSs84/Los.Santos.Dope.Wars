using LSDW.UserInterface;

namespace LSDW.Tests.Fakes;

/// <summary>
/// Draws nothing, and counts having been asked to.
/// </summary>
/// <remarks>
/// This is the whole point of <see cref="ITradeRenderer"/>: the real renderer's constructor
/// builds <c>GTA.UI</c> elements and cannot run headlessly, so the trade flow could not be
/// constructed at all. Nothing here asserts on what was drawn — that needs the game — only that
/// the flow asked.
/// </remarks>
internal sealed class FakeTradeRenderer : ITradeRenderer
{
  /// <summary>How many times the panes were drawn.</summary>
  internal int PaneDraws { get; private set; }

  /// <summary>How many times the HUD was drawn.</summary>
  internal int HudDraws { get; private set; }

  /// <summary>
  /// The side the HUD arrow was last pointed at, or <see langword="null"/> if neither menu was
  /// open on the last draw.
  /// </summary>
  internal TradeSide? LastActiveSide { get; private set; }

  /// <inheritdoc/>
  public void DrawPanes(TradeSide buy, TradeSide sell) => PaneDraws++;

  /// <inheritdoc/>
  public void DrawHud(TradeSide? activeMenuSide)
  {
    HudDraws++;
    LastActiveSide = activeMenuSide;
  }
}
