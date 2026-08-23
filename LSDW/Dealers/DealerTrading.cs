using LSDW.Abstractions;
using LSDW.Drugs;
using LSDW.Events;
using LSDW.Leveling;
using LSDW.Services;
using LSDW.UserInterface;

namespace LSDW.Dealers;

/// <summary>
/// The buy/sell flow at a dealer: the two menus, the two transaction windows, the
/// bag HUD, and the money and stash changes a confirmed trade makes. Open for
/// exactly as long as the player is standing at a dealer, which
/// <see cref="Core.DrugDeal"/> decides.
/// </summary>
/// <remarks>
/// Holds no <c>GTA.UI</c> element of its own: the panes are state, and
/// <see cref="ITradeRenderer"/> draws them. That is what makes this type constructible outside
/// the game, and with it the money gate, the stock-and-appetite transfer and the event order of
/// a confirmed trade all testable.
/// </remarks>
internal sealed class DealerTrading
{
  private readonly EventService _events;
  private readonly INotificationService _notifications;
  private readonly IAudioProvider _audio;
  private readonly IPlayerMoney _money;
  private readonly Progression _progression;
  private readonly PlayerStash _stash;
  private readonly ITradeRenderer _renderer;
  private readonly TradeSide _buy;
  private readonly TradeSide _sell;

  /// <summary>
  /// The dealer being traded with, or <see langword="null"/> when the panes are closed.
  /// </summary>
  public DrugDealer? CurrentDealer { get; private set; }

  /// <summary>
  /// Gets whether the player is currently at a dealer and the panes are open.
  /// </summary>
  public bool IsOpen => CurrentDealer != null;

  /// <param name="events">The event bus a confirmed trade reports itself on.</param>
  /// <param name="notifications">Where a refused purchase is reported.</param>
  /// <param name="audio">The click every accepted key press makes.</param>
  /// <param name="money">The player's cash, which a purchase is gated on and spends.</param>
  /// <param name="progression">The level the price perks are read from.</param>
  /// <param name="stash">The bag both sides trade against.</param>
  /// <param name="panes">The two sides and the renderer that draws them.</param>
  public DealerTrading(
    EventService events,
    INotificationService notifications,
    IAudioProvider audio,
    IPlayerMoney money,
    Progression progression,
    PlayerStash stash,
    TradePanes panes)
  {
    _events = events;
    _notifications = notifications;
    _audio = audio;
    _money = money;
    _progression = progression;
    _stash = stash;

    _renderer = panes.Renderer;
    _buy = panes.Buy;
    _sell = panes.Sell;
  }

  /// <summary>
  /// Draws the four panes. Each is a no-op while it is hidden, so this is called
  /// every tick regardless of whether the player is at a dealer.
  /// </summary>
  public void DrawPanes()
    => _renderer.DrawPanes(_buy, _sell);

  /// <summary>
  /// Draws the stats panel, the bag bar and the arrow that points at the open pane.
  /// </summary>
  public void DrawHud()
    => _renderer.DrawHud(ActiveMenuSide);

  /// <summary>
  /// The click a menu or window makes when a key press moved something. Silent presses stay
  /// silent — see <see cref="AmountSelector.TryProcessKey"/> and <see cref="MenuCursor.TryMove"/>.
  /// </summary>
  private void Click()
    => _audio.PlaySoundFrontendAndForget("SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);

  /// <summary>
  /// Shows the BUY menu, filled with this dealer's stock and prices.
  /// </summary>
  public void Open(DrugDealer dealer)
  {
    _buy.MenuVisible = true;
    CurrentDealer = dealer;

    _buy.ApplyDealerStock(dealer);
    _buy.ApplyDealerPrices(dealer, BuyPrice);
    _sell.ApplyDealerPrices(dealer, SellPrice);
    _sell.ApplyDealerDemand(dealer);

    // Opening is the only place the comparison basis is seeded — UpdateWindows leaves it
    // alone on purpose. See TradeSide.SeedComparisonPrices.
    _buy.SeedComparisonPrices();
    _sell.SeedComparisonPrices();

    _sell.ApplyPlayerStash();
  }

  /// <summary>
  /// Hides every pane and forgets the dealer.
  /// </summary>
  public void Close()
  {
    CurrentDealer = null;
    _buy.MenuVisible = false;
    _sell.MenuVisible = false;
    _buy.WindowVisible = false;
    _sell.WindowVisible = false;
  }

  /// <summary>
  /// The side whose menu is showing, or <see langword="null"/> when neither is — because
  /// the panes are closed, or because a transaction window has replaced one of them.
  /// </summary>
  private TradeSide? ActiveMenuSide
    => _buy.MenuVisible ? _buy : _sell.MenuVisible ? _sell : null;

  public void ProcessKey(Keys key)
  {
    // First: route the key to whichever pane is open. Moving between the BUY and
    // SELL menus happens here, before the confirm handling below re-reads Visible.
    if (_buy.WindowVisible)
    {
      if (_buy.Window.ProcessKey(key) != AmountChange.None)
      {
        Click();
      }
    }
    else if (_sell.WindowVisible)
    {
      if (_sell.Window.ProcessKey(key) != AmountChange.None)
      {
        Click();
      }
    }
    else if (ActiveMenuSide is TradeSide active)
    {
      // The side is read before CrossMenus can flip the visibility, so the key goes to
      // the menu that was on screen when it was pressed rather than to the one that may
      // have just replaced it. At the shared edge the departing menu's own ProcessKey
      // moves nothing and stays silent; handing the key to the arriving menu instead would
      // step that menu's cursor and play a second SELECT.
      CrossMenus(active, key);

      if (active.Menu.ProcessKey(key))
      {
        Click();
      }
    }

    // Second: the confirm key accepts whatever is now on screen.
    if (key != SettingsService.Binds.Confirm)
    {
      return;
    }

    if (_buy.WindowVisible)
    {
      ConfirmPurchase();
    }
    else if (_sell.WindowVisible)
    {
      ConfirmSale();
    }
    else if (_buy.MenuVisible && _buy.SelectedTile.Amount > 0)
    {
      OpenPurchaseWindow();
    }
    else if (_sell.MenuVisible && _sell.SelectedTile.Amount > 0 && _sell.SelectedTile.Demand > 0)
    {
      // The demand check belongs here as well as on the tile's colour: a dealer with no
      // appetite would otherwise open a window capped at zero, which reads as a bug rather
      // than as a dealer who does not want it.
      OpenSaleWindow();
    }
  }

  /// <summary>
  /// Hands the cursor over to the other menu when it is already against the edge the two
  /// menus share and the player pushes further that way. Does nothing otherwise.
  /// </summary>
  /// <remarks>
  /// This stays on <see cref="DealerTrading"/> rather than moving to <see cref="TradeSide"/>
  /// because it is about the pair: neither side knows the other exists.
  /// </remarks>
  /// <param name="from">The side currently showing its menu.</param>
  /// <param name="key">The key just pressed.</param>
  private void CrossMenus(TradeSide from, Keys key)
  {
    TradeSide to;
    int edge;
    Keys crossKey;

    if (from == _buy)
    {
      // BUY sits to the left of SELL, so leaving it means pushing right off its last
      // column. The bound comes from the catalog's grid, not from the 2x3 the six
      // current drugs happen to produce.
      to = _sell;
      edge = DrugCatalog.LastColumn;
      crossKey = SettingsService.Binds.MenuRight;
    }
    else
    {
      to = _buy;
      edge = 0;
      crossKey = SettingsService.Binds.MenuLeft;
    }

    if (from.Menu.CurrentColumn != edge || key != crossKey)
    {
      return;
    }

    from.MenuVisible = false;
    to.MenuVisible = true;
    _audio.PlaySoundFrontendAndForget("SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
  }

  /// <summary>
  /// Backs out of a transaction without making it: plays the cancel sound and puts the
  /// side's menu back in place of its window.
  /// </summary>
  /// <remarks>
  /// The two confirm methods disagreed on when the sound played — <see cref="ConfirmPurchase"/>
  /// before hiding the window, <see cref="ConfirmSale"/> after. Extracting had to pick one,
  /// and picked the purchase order. Both happen within the same tick, so nothing is audibly
  /// different.
  /// </remarks>
  /// <param name="side">The side whose window is being dismissed.</param>
  private void CancelTransaction(TradeSide side)
  {
    _audio.PlaySoundFrontendAndForget("CANCEL", "HUD_LIQUOR_STORE_SOUNDSET", false);
    CloseTransaction(side);
  }

  /// <summary>
  /// Swaps a side's transaction window back out for its menu. Both confirm methods end
  /// this way, whether the trade went through or was cancelled.
  /// </summary>
  /// <remarks>
  /// Each caller keeps this at the point in its own sequence where it already was —
  /// <see cref="ConfirmPurchase"/> after <see cref="UpdateWindows"/>,
  /// <see cref="ConfirmSale"/> before it. Only the pair of assignments moved here, not
  /// their position.
  /// </remarks>
  /// <param name="side">The side whose window is closing.</param>
  private static void CloseTransaction(TradeSide side)
  {
    side.WindowVisible = false;
    side.MenuVisible = true;
  }

  private void ConfirmPurchase()
  {
    int value = _buy.Window.Value;
    int amount = _buy.Window.Amount;

    if (!_buy.Window.OkBool || amount == 0)
    {
      CancelTransaction(_buy);
      return;
    }

    if (_money.Money < value * amount)
    {
      _audio.PlaySoundFrontendAndForget("NO", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
      _notifications.Ticker("You don't have enough money, bitch!");
      return;
    }

    string drug = _buy.Window.DrugName;

    _money.Money -= value * amount;
    _stash.AddPurchased(drug, amount, value);

    // Stock out, appetite in. Both halves live on the book, which is the only thing that can
    // write them — see DealerBook.PlayerBought for why they move together.
    CurrentDealer!.Book.PlayerBought(drug, amount);

    // Cross-side write: buying moves the player's cost basis, which is what the SELL
    // tile's arrow measures against. UpdateWindows below does not reseed it.
    _sell.Menu.DrugDictionary[drug].MarketPrice = _stash.BoughtPrice[drug];

    UpdateWindows();
    _audio.PlaySoundFrontendAndForget("PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET", false);
    CloseTransaction(_buy);

    _events.Publish(new DrugsBoughtEvent(value * amount));
    _events.Publish(new SaveRequestedEvent());
    _events.Publish(new DealerTradeEvent(CurrentDealer!));
  }

  /// <remarks>
  /// This is deliberately not a mirror of <see cref="ConfirmPurchase"/> and must not be
  /// tidied into one. It has no money gate, it moves the dealer's stock the other way,
  /// it publishes its three events in a different order — and it does <b>not</b> treat
  /// <c>Amount == 0</c> as a cancel, so confirming a zero sell completes a zero trade
  /// instead of backing out. That last one has been the behaviour since before the
  /// refactor; decide it is a bug before "fixing" it.
  /// </remarks>
  private void ConfirmSale()
  {
    if (!_sell.Window.OkBool)
    {
      CancelTransaction(_sell);
      return;
    }

    int value = _sell.Window.Value;
    int amount = _sell.Window.Amount;
    string drug = _sell.Window.DrugName;

    _audio.PlaySoundFrontendAndForget("PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET", false);
    _stash.Remove(drug, amount);

    // The mirror of ConfirmPurchase: what he takes on, he no longer needs.
    CurrentDealer!.Book.PlayerSold(drug, amount);
    CloseTransaction(_sell);

    UpdateWindows();
    _events.Publish(new DealerTradeEvent(CurrentDealer!));

    // The sale records its gross take (PlayerStats) and its margin over what the player
    // paid (Progression turns it into XP; a loss grants none). One event, two reactions,
    // both before the save below.
    _events.Publish(new DrugsSoldEvent(value * amount, (value - _stash.BoughtPrice[drug]) * amount));

    _events.Publish(new SaveRequestedEvent());
  }

  private void OpenPurchaseWindow()
  {
    _buy.MenuVisible = false;

    string drug = _buy.Menu.CurrentItem;
    int stock = _buy.Menu.DrugDictionary[drug].Amount;
    int space = _stash.BagSize - _stash.Bag;

    // Assignment order is load-bearing: these are computed setters that read each other.
    // SellingDrugs decides the sign the money text formats with, Value and NormalValue
    // feed it, and Amount's setter reads both back to build the summary line.
    _buy.Window.SellingDrugs = false;
    _buy.Window.DrugName = drug;
    _buy.Window.Value = _buy.Menu.DrugDictionary[drug].Price;
    _buy.Window.NormalValue = PlayerStash.MarketValue[drug];
    _buy.Window.OkBool = true;
    _buy.Window.MaxAmount = (space > stock) ? stock : space;
    _buy.Window.Amount = 0;
    _buy.WindowVisible = true;
  }

  private void OpenSaleWindow()
  {
    _sell.MenuVisible = false;

    string drug = _sell.Menu.CurrentItem;

    // Same ordering constraint as OpenPurchaseWindow, plus GlobalValue: its setter reads
    // Value and NormalValue to lay out the four-line sell text, so it has to come after
    // both. Setting it on the BUY window would relabel that window's three lines.
    _sell.Window.SellingDrugs = true;
    _sell.Window.DrugName = drug;
    _sell.Window.Value = _sell.Menu.DrugDictionary[drug].Price;
    _sell.Window.NormalValue = _stash.BoughtPrice[drug];
    _sell.Window.GlobalValue = PlayerStash.MarketValue[drug];
    _sell.Window.OkBool = true;

    // Whichever runs out first: what the player is carrying, or what this dealer still wants.
    // He has no cash ceiling, but he does have a book to balance.
    int carried = _sell.Menu.DrugDictionary[drug].Amount;
    int wanted = CurrentDealer!.Book.Demand[drug];

    _sell.Window.MaxAmount = (wanted < carried) ? wanted : carried;
    _sell.Window.Amount = 0;
    _sell.WindowVisible = true;
  }

  private void UpdateWindows()
  {
    if (CurrentDealer != null)
    {
      _buy.ApplyDealerStock(CurrentDealer);
      _buy.ApplyDealerPrices(CurrentDealer, BuyPrice);
      _sell.ApplyDealerPrices(CurrentDealer, SellPrice);
      _sell.ApplyDealerDemand(CurrentDealer);
      _sell.ApplyPlayerStash();
    }

  }

  /// <summary>
  /// What the player pays: the dealer's price plus his cut, less any perk discount.
  /// </summary>
  private int BuyPrice(int dealerPrice)
    => MarketPricing.Ask(dealerPrice, Perks.BuyPriceMultiplier(_progression.Level));

  /// <summary>
  /// What the player is paid: the dealer's price less his cut, plus any perk bonus. Always
  /// below <see cref="BuyPrice"/> at the same dealer — see
  /// <see cref="MarketPricing.MarketSpread"/>.
  /// </summary>
  private int SellPrice(int dealerPrice)
    => MarketPricing.Bid(dealerPrice, Perks.SellPriceMultiplier(_progression.Level));
}
