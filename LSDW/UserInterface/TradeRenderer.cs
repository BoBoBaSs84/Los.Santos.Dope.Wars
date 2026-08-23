using System.Diagnostics.CodeAnalysis;

using LSDW.Core;
using LSDW.Drugs;
using LSDW.Leveling;

using GTA.UI;

using Screen = GTA.UI.Screen;

namespace LSDW.UserInterface;

/// <summary>
/// The live <see cref="ITradeRenderer"/>: owns the four pane renderers, the bag HUD and the
/// stats panel, and so owns every <c>GTA.UI</c> element the trade flow draws.
/// </summary>
/// <remarks>
/// Built once, from the two sides it will draw — it takes their titles and offsets to place its
/// panes, which is why <see cref="TradeSide.Title"/> and <see cref="TradeSide.Offset"/> are
/// exposed. Constructing this is what needs the game; <c>DealerTrading</c> no longer does.
/// </remarks>
[SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = SpriteLifetime.Justification)]
internal sealed class TradeRenderer : ITradeRenderer
{
  private readonly PlayerStash _stash;
  private readonly PlayerStats _stats;
  private readonly StatsPanel _statsPanel;

  private readonly DrugMenuRenderer _buyMenu;
  private readonly DrugMenuRenderer _sellMenu;
  private readonly TransactionWindowRenderer _buyWindow;
  private readonly TransactionWindowRenderer _sellWindow;

  private readonly Sprite _bagBarSprite;
  private readonly Sprite _bagSprite;
  private readonly Sprite _arrowTip;
  private readonly TextElement _bagText;

  /// <summary>
  /// Builds a pane pair for each side, plus the shared HUD.
  /// </summary>
  /// <param name="buy">The buy side, for its title and offset.</param>
  /// <param name="sell">The sell side, same.</param>
  /// <param name="stash">The bag the HUD counter reads.</param>
  /// <param name="stats">The lifetime figures, and the load gate on the stats panel.</param>
  /// <param name="progression">The level and XP the panel's badge and bar show.</param>
  public TradeRenderer(TradeSide buy, TradeSide sell, PlayerStash stash, PlayerStats stats, Progression progression)
  {
    _stash = stash;
    _stats = stats;

    _buyMenu = new DrugMenuRenderer(buy.Title, buy.Offset);
    _sellMenu = new DrugMenuRenderer(sell.Title, sell.Offset);
    _buyWindow = new TransactionWindowRenderer(buy.Title, buy.SellingDrugs, buy.Offset);
    _sellWindow = new TransactionWindowRenderer(sell.Title, sell.SellingDrugs, sell.Offset);

    _bagSprite = new Sprite("mpinventory", "mp_specitem_package", new SizeF(40, 40), new PointF(Screen.Width / 2 - 100, 100), Color.White);
    _bagText = new TextElement(_stash.Bag + "/" + _stash.BagSize, new PointF(Screen.Width / 2 - 100 + 40, 110), 0.5f);
    _bagBarSprite = new Sprite("mpentry", "mp_modenotselected_gradient", new SizeF(580, 60), new PointF(Screen.Width / 2 - 350, 93), Color.White);
    _arrowTip = new Sprite("commonmenu", "arrowleft", new SizeF(40, 40), new PointF(Screen.Width - 420, 0), Color.White);

    _statsPanel = new StatsPanel(_stats, progression);
  }

  /// <inheritdoc/>
  public void DrawPanes(TradeSide buy, TradeSide sell)
  {
    _buyMenu.Draw(buy.Menu);
    _sellMenu.Draw(sell.Menu);
    _buyWindow.Draw(buy.Window);
    _sellWindow.Draw(sell.Window);
  }

  /// <inheritdoc/>
  public void DrawHud(TradeSide? activeMenuSide)
  {
    if (_stats.LoadSuccessful)
    {
      _statsPanel.DrawPanel();
      _statsPanel.DrawLevelBar();
    }

    _bagBarSprite.Draw();

    _bagText.Caption = (_stash.Bag == _stash.BagSize) ? "~r~" : "";
    _bagText.Caption += _stash.Bag + "/" + _stash.BagSize;

    _bagSprite.Draw();
    _bagText.Draw();

    // One arrow sprite, moved to whichever menu is open. The placement lives on the side
    // because it is layout data like the offset is; the sprite stays here because there is
    // only ever one of it on screen.
    if (activeMenuSide is TradeSide active)
    {
      _arrowTip.Position = active.ArrowPosition;
      _arrowTip.Rotation = active.ArrowRotation;
      _arrowTip.Draw();
    }
  }
}
