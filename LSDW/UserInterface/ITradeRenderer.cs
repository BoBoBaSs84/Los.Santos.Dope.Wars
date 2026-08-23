namespace LSDW.UserInterface;

/// <summary>
/// Everything the trade flow puts on screen. <c>DealerTrading</c> holds one and calls it once
/// per tick; the live implementation owns every <c>GTA.UI</c> element the flow uses.
/// </summary>
/// <remarks>
/// The seam exists because those elements cannot be constructed outside the game —
/// <c>GTA.UI.TextElement</c>'s constructor reaches into the SHVDN core assembly — which used to
/// make <c>DealerTrading</c> itself unconstructible and so untestable. A test double that draws
/// nothing leaves the whole trade flow reachable.
/// </remarks>
internal interface ITradeRenderer
{
  /// <summary>
  /// Draws the four panes: both menus, then both windows.
  /// </summary>
  /// <remarks>
  /// Both menus first, then both windows — <b>not</b> one loop over the two sides, which would
  /// emit menu, window, menu, window instead. Draw order is z-order here, the same way
  /// <c>DrugDeal.OnTick</c> documents it: later calls land on top. Only one pane is visible at a
  /// time today, so the rendered frame would not differ, but the native call order would — and
  /// that stops being harmless the moment two panes overlap.
  /// </remarks>
  /// <param name="buy">The buy side.</param>
  /// <param name="sell">The sell side.</param>
  void DrawPanes(TradeSide buy, TradeSide sell);

  /// <summary>
  /// Draws the stats panel, the bag bar, and the arrow pointing at whichever menu is open.
  /// </summary>
  /// <param name="activeMenuSide">
  /// The side whose menu is showing, or <see langword="null"/> when neither is — in which case
  /// the arrow is not drawn at all.
  /// </param>
  void DrawHud(TradeSide? activeMenuSide);
}
