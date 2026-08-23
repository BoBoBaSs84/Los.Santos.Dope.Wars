using LSDW.Dealers;
using LSDW.Drugs;

namespace LSDW.UserInterface;

/// <summary>
/// One side of the trade at a dealer — its <see cref="DrugMenu"/>, its
/// <see cref="TransactionWindow"/>, and the layout data that places both on screen.
/// <see cref="DealerTrading"/> holds two: one to buy from the dealer's stock, one to sell from
/// the player's bag.
/// </summary>
/// <remarks>
/// <para>
/// This replaces the hand-mirrored "left"/"right" field pairs, which named screen position
/// where the meaning was buy/sell. The offset is the layout parameter both panes feed into
/// <c>Screen.Width - 500 - offset</c>; it belongs to a side, not to a pane, which is why it is
/// captured once here.
/// </para>
/// <para>
/// State only, and constructible without the game. <see cref="ITradeRenderer"/> owns the
/// <c>GTA.UI</c> elements and reads a side to draw it, which is why <see cref="Title"/> and
/// <see cref="Offset"/> are exposed: the renderer builds its panes from them.
/// </para>
/// <para>
/// The keybind rule that used to live here still holds, one level up: <see cref="DrugMenu"/>
/// and <see cref="TransactionWindow"/> capture <c>SettingsService.Binds</c> at construction, so
/// a side must not be built before <c>SettingsService.Load</c> has run. <c>DrugDeal</c> loads
/// the settings first for exactly this reason.
/// </para>
/// <para>
/// <see cref="Menu"/> and <see cref="Window"/> are deliberately public rather than wrapped: a
/// confirmed purchase writes the new average bought price into the <i>sell</i> side's tile, so
/// the pair is not cleanly separable and the cross-side write needs a path. Hiding them behind
/// side-local methods breaks that silently — the symptom is a stale profit arrow, not a compile
/// error.
/// </para>
/// </remarks>
internal sealed class TradeSide
{
  private readonly PlayerStash _stash;

  /// <summary>
  /// The grid of drug tiles for this side.
  /// </summary>
  public DrugMenu Menu { get; } = new();

  /// <summary>
  /// The amount-and-confirm screen shown after a tile is picked on this side.
  /// </summary>
  public TransactionWindow Window { get; } = new();

  /// <summary>
  /// The header both panes show, e.g. <c>"BUY"</c>. Read by the renderer.
  /// </summary>
  public string Title { get; }

  /// <summary>
  /// Pixels to shift this side's panes left of the screen's right edge, on top of the shared
  /// 500. The two sides sit at 705 and 0. Read by the renderer.
  /// </summary>
  public int Offset { get; }

  /// <summary>
  /// Whether this side sells to the dealer rather than buying from them. Decides the sign of
  /// every price comparison the two panes make.
  /// </summary>
  public bool SellingDrugs { get; }

  /// <summary>
  /// Where the HUD arrow sits when this side's menu is the open one.
  /// </summary>
  public PointF ArrowPosition { get; }

  /// <summary>
  /// The rotation the HUD arrow takes when this side's menu is the open one, so it points at
  /// the pane rather than away from it.
  /// </summary>
  public float ArrowRotation { get; }

  /// <summary>
  /// The tile this side's cursor is sitting on.
  /// </summary>
  public TileState SelectedTile => Menu.SelectedTile;

  /// <summary>
  /// Whether this side's menu is on screen. The menu and the window are mutually exclusive in
  /// practice — opening the window hides the menu — but nothing enforces it.
  /// </summary>
  public bool MenuVisible
  {
    get => Menu.Visible;
    set => Menu.Visible = value;
  }

  /// <summary>
  /// Whether this side's transaction window is on screen.
  /// </summary>
  public bool WindowVisible
  {
    get => Window.Visible;
    set => Window.Visible = value;
  }

  /// <summary>
  /// Builds both panes' state for one side of the trade.
  /// </summary>
  /// <param name="title">The header shown on both panes, e.g. <c>"BUY"</c>.</param>
  /// <param name="sellingDrugs">Whether this side sells to the dealer.</param>
  /// <param name="offset">
  /// Pixels to shift the panes left of the screen's right edge, on top of the shared 500. The
  /// two sides sit at 705 and 0.
  /// </param>
  /// <param name="arrowPosition">Where the HUD arrow sits while this side's menu is open.</param>
  /// <param name="arrowRotation">Which way the HUD arrow points while this side's menu is open.</param>
  /// <param name="stash">The bag this side reads the player's holdings and cost basis from.</param>
  public TradeSide(string title, bool sellingDrugs, int offset, PointF arrowPosition, float arrowRotation, PlayerStash stash)
  {
    _stash = stash;
    Title = title;
    Offset = offset;
    SellingDrugs = sellingDrugs;
    ArrowPosition = arrowPosition;
    ArrowRotation = arrowRotation;

    Window.SellingDrugs = sellingDrugs;
  }

  /// <summary>
  /// Fills this side's tiles with what the dealer has in stock. <b>Buy side only</b> — the sell
  /// side's amounts are what the player is carrying, from <see cref="ApplyPlayerStash"/>.
  /// </summary>
  public void ApplyDealerStock(DrugDealer dealer)
  {
    foreach (KeyValuePair<string, int> stock in dealer.Book.Amount)
    {
      Menu.DrugDictionary[stock.Key].Amount = stock.Value;
    }
  }

  /// <summary>
  /// Fills this side's tiles with the dealer's asking prices, each run through
  /// <paramref name="priceFor"/>.
  /// </summary>
  /// <param name="dealer">The dealer whose prices to read.</param>
  /// <param name="priceFor">
  /// The perk-adjusted price for this side. <c>BuyPrice</c> and <c>SellPrice</c> stay on
  /// <see cref="DealerTrading"/> and are passed in, because they read <c>Perks</c> and a side
  /// has no business knowing about levelling.
  /// </param>
  public void ApplyDealerPrices(DrugDealer dealer, Func<int, int> priceFor)
  {
    foreach (KeyValuePair<string, int> price in dealer.Book.Price)
    {
      Menu.DrugDictionary[price.Key].Price = priceFor(price.Value);
    }
  }

  /// <summary>
  /// Fills this side's tiles with how much of each drug the dealer will still take.
  /// <b>Sell side only</b> — a purchase is limited by his stock, which
  /// <see cref="ApplyDealerStock"/> already supplies.
  /// </summary>
  public void ApplyDealerDemand(DrugDealer dealer)
  {
    foreach (KeyValuePair<string, int> demand in dealer.Book.Demand)
    {
      Menu.DrugDictionary[demand.Key].Demand = demand.Value;
    }
  }

  /// <summary>
  /// Fills this side's tiles with what the player is carrying. <b>Sell side only</b> — see
  /// <see cref="ApplyDealerStock"/>.
  /// </summary>
  public void ApplyPlayerStash()
  {
    foreach (KeyValuePair<string, int> carried in _stash.Drugs)
    {
      Menu.DrugDictionary[carried.Key].Amount = carried.Value;
    }
  }

  /// <summary>
  /// Seeds each tile's comparison basis and trade direction — what the tile's arrow measures
  /// this dealer's price against. The buy side compares against the market price; the sell side
  /// compares against what the player actually paid.
  /// </summary>
  /// <remarks>
  /// Only <c>DealerTrading.Open</c> calls this, and <c>UpdateWindows</c> deliberately does not.
  /// Amounts and prices move on every completed trade, but the comparison basis only moves when
  /// the player buys, and <c>ConfirmPurchase</c> writes that single tile itself.
  /// </remarks>
  public void SeedComparisonPrices()
  {
    foreach (KeyValuePair<string, TileState> tile in Menu.DrugDictionary)
    {
      tile.Value.MarketPrice = SellingDrugs
        ? _stash.BoughtPrice[tile.Key]
        : PlayerStash.MarketValue[tile.Key];

      tile.Value.SellingDrugs = SellingDrugs;
    }
  }
}
