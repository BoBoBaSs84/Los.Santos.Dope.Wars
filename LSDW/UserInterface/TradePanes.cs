using LSDW.Core;
using LSDW.Drugs;
using LSDW.Leveling;

namespace LSDW.UserInterface;

/// <summary>
/// The two sides of the trade and the thing that draws them, handed to
/// <c>DealerTrading</c> as one argument because they are built together and useless apart.
/// </summary>
/// <remarks>
/// The renderer takes both sides at construction, so something has to build all three in order;
/// this is that something. <see cref="CreateLive"/> is the in-game path and is the only part
/// that needs GTA — a test constructs the sides itself and pairs them with a renderer that
/// draws nothing.
/// </remarks>
internal sealed class TradePanes
{
  /// <summary>The side that buys from the dealer's stock.</summary>
  public TradeSide Buy { get; }

  /// <summary>The side that sells from the player's bag.</summary>
  public TradeSide Sell { get; }

  /// <summary>What puts the two of them on screen.</summary>
  public ITradeRenderer Renderer { get; }

  /// <summary>
  /// Pairs two sides with a renderer.
  /// </summary>
  /// <param name="buy">The buy side.</param>
  /// <param name="sell">The sell side.</param>
  /// <param name="renderer">The renderer, which must have been built from these two sides.</param>
  public TradePanes(TradeSide buy, TradeSide sell, ITradeRenderer renderer)
  {
    Buy = buy;
    Sell = sell;
    Renderer = renderer;
  }

  /// <summary>
  /// Builds the real thing: both sides, and a <see cref="TradeRenderer"/> over them.
  /// </summary>
  /// <remarks>
  /// The layout numbers live here and nowhere else. The sides capture the configured nav keys
  /// as they are constructed, so <c>SettingsService.Load</c> must already have run — see
  /// <see cref="TradeSide"/>.
  /// </remarks>
  /// <param name="stash">The bag both sides trade against and the HUD counts.</param>
  /// <param name="stats">The lifetime figures the stats panel shows.</param>
  /// <param name="progression">The level the panel's badge and bar show.</param>
  public static TradePanes CreateLive(PlayerStash stash, PlayerStats stats, Progression progression)
  {
    TradeSide buy = new("BUY", sellingDrugs: false, 705, new PointF(390, 300), 180f, stash);
    TradeSide sell = new("SELL", sellingDrugs: true, 0, new PointF(730, 300), 0f, stash);

    return new TradePanes(buy, sell, new TradeRenderer(buy, sell, stash, stats, progression));
  }
}
