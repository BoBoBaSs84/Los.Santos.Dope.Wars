using LSDW.Dealers;
using LSDW.Drugs;
using LSDW.Events;
using LSDW.Leveling;
using LSDW.Services;
using LSDW.Tests.Fakes;
using LSDW.UserInterface;

using GTA.Math;

namespace LSDW.Tests.Dealers;

/// <summary>
/// A confirmed trade at a dealer: the money that changes hands, the stock and appetite that move
/// against each other, and the events the rest of the mod reacts to.
/// </summary>
/// <remarks>
/// None of this was reachable until the panes became state and the drawing moved behind
/// <c>ITradeRenderer</c>. Two things still need a seam each, and both are used here: the wallet,
/// because <c>Game.Player.Money</c> is a native both ways, and the click.
/// <para>
/// The window is filled in by hand rather than through <c>ProcessKey</c>, because opening one for
/// real goes through the confirm key and the menu cursor; what is under test here is what
/// <c>ConfirmPurchase</c> and <c>ConfirmSale</c> do with a window that is already filled.
/// </para>
/// </remarks>
[TestClass]
public sealed class DealerTradingTests
{
  private const string Cocaine = "Cocaine";

  private sealed record Harness(
    DealerTrading Trading,
    DrugDealer Dealer,
    PlayerStash Stash,
    FakePlayerMoney Money,
    FakeAudioProvider Audio,
    FakeNotificationService Notifications,
    EventService Events,
    TradeSide Buy,
    TradeSide Sell);

  /// <summary>
  /// A dealer holding 20 of one drug at 900, wanting 10 more, with the panes open on him.
  /// </summary>
  private static Harness NewHarness(int money = 100_000)
  {
    EventService events = new();
    FakeNotificationService notifications = new();
    FakeAudioProvider audio = new();
    FakePlayerMoney wallet = new() { Money = money };
    PlayerStash stash = new();
    Progression progression = new(events);

    TradeSide buy = new("BUY", sellingDrugs: false, 705, new PointF(390, 300), 180f, stash);
    TradeSide sell = new("SELL", sellingDrugs: true, 0, new PointF(730, 300), 0f, stash);
    TradePanes panes = new(buy, sell, new FakeTradeRenderer());

    DealerTrading trading = new(events, notifications, audio, wallet, progression, stash, panes);

    DrugDealer dealer = new(new Vector3(0f, 0f, 0f), 0f, 1);
    dealer.Book.SetPosition(Cocaine, amount: 20, price: 900, demand: 10);

    trading.Open(dealer);

    return new Harness(trading, dealer, stash, wallet, audio, notifications, events, buy, sell);
  }

  /// <summary>
  /// Fills a side's window the way <c>OpenPurchaseWindow</c>/<c>OpenSaleWindow</c> would, and
  /// shows it.
  /// </summary>
  private static void OpenWindow(TradeSide side, int amount, int value, int normalValue)
  {
    side.MenuVisible = false;
    side.Window.DrugName = Cocaine;
    side.Window.Value = value;
    side.Window.NormalValue = normalValue;
    side.Window.OkBool = true;
    side.Window.MaxAmount = amount;
    side.Window.Amount = amount;
    side.WindowVisible = true;
  }

  /// <summary>Presses the configured confirm key.</summary>
  private static void Confirm(DealerTrading trading)
    => trading.ProcessKey(SettingsService.Binds.Confirm);

  [TestMethod]
  public void Construction_WithoutTheGame_Succeeds()
  {
    // The whole point of the render seam. Before it, this line threw: TextElement's
    // constructor reaches into the SHVDN core assembly.
    Harness harness = NewHarness();

    Assert.IsTrue(harness.Trading.IsOpen);
    Assert.AreSame(harness.Dealer, harness.Trading.CurrentDealer);
  }

  [TestMethod]
  public void Open_FillsTheBuySideFromTheDealersBook()
  {
    Harness harness = NewHarness();

    Assert.AreEqual(20, harness.Buy.Menu.DrugDictionary[Cocaine].Amount);
    Assert.AreEqual(10, harness.Sell.Menu.DrugDictionary[Cocaine].Demand);
  }

  [TestMethod]
  public void Close_ForgetsTheDealerAndHidesEveryPane()
  {
    Harness harness = NewHarness();

    harness.Trading.Close();

    Assert.IsFalse(harness.Trading.IsOpen);
    Assert.IsFalse(harness.Buy.MenuVisible);
    Assert.IsFalse(harness.Sell.MenuVisible);
    Assert.IsFalse(harness.Buy.WindowVisible);
    Assert.IsFalse(harness.Sell.WindowVisible);
  }

  [TestMethod]
  public void ConfirmPurchase_MovesStockAndAppetiteInOppositeDirections()
  {
    // The market's load-bearing invariant, now checked through the flow that maintains it
    // rather than only on the book underneath.
    Harness harness = NewHarness();
    OpenWindow(harness.Buy, amount: 5, value: 900, normalValue: 850);
    int before = harness.Dealer.Book.Amount[Cocaine] + harness.Dealer.Book.Demand[Cocaine];

    Confirm(harness.Trading);

    Assert.AreEqual(15, harness.Dealer.Book.Amount[Cocaine]);
    Assert.AreEqual(15, harness.Dealer.Book.Demand[Cocaine]);
    Assert.AreEqual(before, harness.Dealer.Book.Amount[Cocaine] + harness.Dealer.Book.Demand[Cocaine]);
  }

  [TestMethod]
  public void ConfirmPurchase_DoesNotMoveHisPrice()
  {
    Harness harness = NewHarness();
    OpenWindow(harness.Buy, amount: 5, value: 900, normalValue: 850);

    Confirm(harness.Trading);

    Assert.AreEqual(900, harness.Dealer.Book.Price[Cocaine]);
  }

  [TestMethod]
  public void ConfirmPurchase_ChargesForItAndFillsTheBag()
  {
    Harness harness = NewHarness(money: 10_000);
    OpenWindow(harness.Buy, amount: 5, value: 900, normalValue: 850);

    Confirm(harness.Trading);

    Assert.AreEqual(10_000 - 4_500, harness.Money.Money);
    Assert.AreEqual(5, harness.Stash.Drugs[Cocaine]);
  }

  [TestMethod]
  public void ConfirmPurchase_WithoutTheMoney_BuysNothing()
  {
    // The gate that could not be reached before the wallet seam existed. It must refuse the
    // whole trade: no stock moves, no money moves, and the window stays open.
    Harness harness = NewHarness(money: 100);
    OpenWindow(harness.Buy, amount: 5, value: 900, normalValue: 850);

    Confirm(harness.Trading);

    Assert.AreEqual(100, harness.Money.Money);
    Assert.AreEqual(0, harness.Stash.Drugs[Cocaine]);
    Assert.AreEqual(20, harness.Dealer.Book.Amount[Cocaine]);
    Assert.IsTrue(harness.Buy.WindowVisible, "a refused purchase leaves the window up");
  }

  [TestMethod]
  public void ConfirmPurchase_WithoutTheMoney_SaysSoAndBuzzes()
  {
    Harness harness = NewHarness(money: 100);
    OpenWindow(harness.Buy, amount: 5, value: 900, normalValue: 850);

    Confirm(harness.Trading);

    Assert.Contains("You don't have enough money, bitch!", harness.Notifications.Tickers);
    Assert.Contains("NO", harness.Audio.Sounds);
  }

  [TestMethod]
  public void ConfirmPurchase_WithoutTheMoney_PublishesNothing()
  {
    // A refused purchase must not reach the stats, the save file or the police.
    Harness harness = NewHarness(money: 100);
    OpenWindow(harness.Buy, amount: 5, value: 900, normalValue: 850);
    List<string> published = [];
    harness.Events.Subscribe<DrugsBoughtEvent>(_ => published.Add(nameof(DrugsBoughtEvent)));
    harness.Events.Subscribe<SaveRequestedEvent>(_ => published.Add(nameof(SaveRequestedEvent)));
    harness.Events.Subscribe<DealerTradeEvent>(_ => published.Add(nameof(DealerTradeEvent)));

    Confirm(harness.Trading);

    Assert.IsEmpty(published);
  }

  [TestMethod]
  public void ConfirmPurchase_PublishesItsThreeEventsInOrder()
  {
    Harness harness = NewHarness();
    OpenWindow(harness.Buy, amount: 5, value: 900, normalValue: 850);
    List<string> published = [];
    harness.Events.Subscribe<DrugsBoughtEvent>(_ => published.Add(nameof(DrugsBoughtEvent)));
    harness.Events.Subscribe<SaveRequestedEvent>(_ => published.Add(nameof(SaveRequestedEvent)));
    harness.Events.Subscribe<DealerTradeEvent>(_ => published.Add(nameof(DealerTradeEvent)));

    Confirm(harness.Trading);

    Assert.AreSequenceEqual(
      [nameof(DrugsBoughtEvent), nameof(SaveRequestedEvent), nameof(DealerTradeEvent)],
      published);
  }

  [TestMethod]
  public void ConfirmPurchase_AtZeroAmount_CancelsInstead()
  {
    // Buying nothing backs out. Selling nothing does not — see the sale test below; the two
    // are deliberately not mirrors.
    Harness harness = NewHarness();
    OpenWindow(harness.Buy, amount: 0, value: 900, normalValue: 850);

    Confirm(harness.Trading);

    Assert.IsFalse(harness.Buy.WindowVisible);
    Assert.IsTrue(harness.Buy.MenuVisible);
    Assert.Contains("CANCEL", harness.Audio.Sounds);
  }

  [TestMethod]
  public void ConfirmSale_MovesStockAndAppetiteTheOtherWay()
  {
    Harness harness = NewHarness();
    harness.Stash.AddPurchased(Cocaine, 8, 700);
    OpenWindow(harness.Sell, amount: 8, value: 900, normalValue: 700);
    int before = harness.Dealer.Book.Amount[Cocaine] + harness.Dealer.Book.Demand[Cocaine];

    Confirm(harness.Trading);

    Assert.AreEqual(28, harness.Dealer.Book.Amount[Cocaine]);
    Assert.AreEqual(2, harness.Dealer.Book.Demand[Cocaine]);
    Assert.AreEqual(before, harness.Dealer.Book.Amount[Cocaine] + harness.Dealer.Book.Demand[Cocaine]);
    Assert.AreEqual(0, harness.Stash.Drugs[Cocaine]);
  }

  [TestMethod]
  public void ConfirmSale_PublishesItsThreeEventsInADifferentOrder()
  {
    // Deliberately not the purchase's order: the dealer trade goes out before the sale, so
    // the bust roll happens against the trade that just completed.
    Harness harness = NewHarness();
    harness.Stash.AddPurchased(Cocaine, 8, 700);
    OpenWindow(harness.Sell, amount: 8, value: 900, normalValue: 700);
    List<string> published = [];
    harness.Events.Subscribe<DealerTradeEvent>(_ => published.Add(nameof(DealerTradeEvent)));
    harness.Events.Subscribe<DrugsSoldEvent>(_ => published.Add(nameof(DrugsSoldEvent)));
    harness.Events.Subscribe<SaveRequestedEvent>(_ => published.Add(nameof(SaveRequestedEvent)));

    Confirm(harness.Trading);

    Assert.AreSequenceEqual(
      [nameof(DealerTradeEvent), nameof(DrugsSoldEvent), nameof(SaveRequestedEvent)],
      published);
  }

  [TestMethod]
  public void ConfirmSale_ReportsGrossAndMargin()
  {
    // Two figures from one event: the gross take for the money box, the margin over what the
    // player paid for the XP. Bought at 700, sold at 900, eight of them.
    Harness harness = NewHarness();
    harness.Stash.AddPurchased(Cocaine, 8, 700);
    OpenWindow(harness.Sell, amount: 8, value: 900, normalValue: 700);
    DrugsSoldEvent? sale = null;
    harness.Events.Subscribe<DrugsSoldEvent>(published => sale = published);

    Confirm(harness.Trading);

    Assert.IsNotNull(sale);
    Assert.AreEqual(7_200, sale.Revenue);
    Assert.AreEqual(1_600, sale.Profit);
  }

  [TestMethod]
  public void ConfirmSale_AtZeroAmount_CompletesAZeroTrade()
  {
    // Not a mirror of the purchase, and documented as such on ConfirmSale: confirming a zero
    // sell goes through rather than backing out. Behaviour from before the refactors that
    // produced these tests — decide it is a bug before "fixing" it.
    Harness harness = NewHarness();
    OpenWindow(harness.Sell, amount: 0, value: 900, normalValue: 700);
    bool sold = false;
    harness.Events.Subscribe<DrugsSoldEvent>(_ => sold = true);

    Confirm(harness.Trading);

    Assert.IsTrue(sold);
    Assert.AreEqual(20, harness.Dealer.Book.Amount[Cocaine]);
  }

  [TestMethod]
  public void ConfirmSale_WithCancelHighlighted_BacksOut()
  {
    Harness harness = NewHarness();
    harness.Stash.AddPurchased(Cocaine, 8, 700);
    OpenWindow(harness.Sell, amount: 8, value: 900, normalValue: 700);
    harness.Sell.Window.OkBool = false;

    Confirm(harness.Trading);

    Assert.AreEqual(8, harness.Stash.Drugs[Cocaine], "the bag keeps what it had");
    Assert.IsFalse(harness.Sell.WindowVisible);
    Assert.Contains("CANCEL", harness.Audio.Sounds);
  }

  [TestMethod]
  public void ConfirmPurchase_WritesTheNewCostBasisIntoTheSellTile()
  {
    // The cross-side write TradeSide's remarks warn about: buying moves the player's cost
    // basis, and the SELL tile's arrow measures against it. Its symptom when broken is a
    // stale arrow, not a compile error, which is why it is pinned here.
    Harness harness = NewHarness();
    OpenWindow(harness.Buy, amount: 5, value: 900, normalValue: 850);

    Confirm(harness.Trading);

    Assert.AreEqual(harness.Stash.BoughtPrice[Cocaine], harness.Sell.Menu.DrugDictionary[Cocaine].MarketPrice);
  }

  [TestMethod]
  public void MenuKeys_ThatMoveTheCursor_Click()
  {
    Harness harness = NewHarness();

    harness.Trading.ProcessKey(SettingsService.Binds.MenuDown);

    Assert.Contains("SELECT", harness.Audio.Sounds);
  }

  [TestMethod]
  public void MenuKeys_AgainstAWall_StaySilent()
  {
    // The cursor opens at the top row, so up is a wall. A menu that answered back at every
    // edge would click constantly.
    Harness harness = NewHarness();

    harness.Trading.ProcessKey(SettingsService.Binds.MenuUp);

    Assert.IsEmpty(harness.Audio.Sounds);
  }

  [TestMethod]
  public void PushingRightOffTheBuyMenu_CrossesToTheSellMenu()
  {
    // The hand-over the cursor's right-hand bound exists to make meaningful.
    Harness harness = NewHarness();

    for (int column = 0; column < DrugCatalog.LastColumn; column++)
    {
      harness.Trading.ProcessKey(SettingsService.Binds.MenuRight);
    }

    Assert.IsTrue(harness.Buy.MenuVisible, "still on the buy side while it has columns left");

    harness.Trading.ProcessKey(SettingsService.Binds.MenuRight);

    Assert.IsFalse(harness.Buy.MenuVisible);
    Assert.IsTrue(harness.Sell.MenuVisible);
  }
}
