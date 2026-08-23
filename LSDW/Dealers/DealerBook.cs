using LSDW.Drugs;

namespace LSDW.Dealers;

/// <summary>
/// One dealer's trading position: what he holds of each drug, what he charges for it, how
/// much more of it he still wants, and any special offer standing against that book.
/// </summary>
/// <remarks>
/// <para>
/// Split out of <see cref="DrugDealer"/>, which creates its blip in its constructor and so
/// needs the game to exist at all. Nothing here does — it is dictionaries and integers, the
/// same reason <see cref="MarketSupply"/>, <see cref="MarketDistribution"/> and
/// <see cref="MarketPricing"/> are separate from the draws in <see cref="DealerMarket"/>.
/// The dealer's economic state is checkable without launching GTA; keep it that way.
/// </para>
/// <para>
/// The three maps are written only through the methods below. That is the point of the type:
/// <c>amount + demand</c> holding constant across a trade used to be two lines in
/// <c>ConfirmPurchase</c> and two mirrored lines in <c>ConfirmSale</c>, kept in step by a
/// comment. Here it is one method each, and there is nowhere else to write them from.
/// </para>
/// </remarks>
internal sealed class DealerBook
{
  private readonly Dictionary<string, int> _amount = DrugCatalog.NewAmountMap();

  // NewPriceMap, not NewAmountMap: a dealer whose prices have never been set should read as
  // asking market value, not as giving everything away.
  private readonly Dictionary<string, int> _price = DrugCatalog.NewPriceMap();
  private readonly Dictionary<string, int> _demand = DrugCatalog.NewAmountMap();

  private int? _offerPreviousPrice;
  private int? _offerPreviousAmount;
  private int? _offerPreviousDemand;

  /// <summary>
  /// The amount of each drug in stock, keyed by drug name. This dealer's share of the
  /// cycle's supply, handed out by <see cref="DealerMarket.DistributeSupply"/>.
  /// </summary>
  public IReadOnlyDictionary<string, int> Amount => _amount;

  /// <summary>
  /// The price of each drug, keyed by drug name. Derived from <see cref="Amount"/> when the
  /// supply is distributed and fixed for the rest of the cycle — see
  /// <see cref="MarketPricing"/>. No trade moves it.
  /// </summary>
  public IReadOnlyDictionary<string, int> Price => _price;

  /// <summary>
  /// How much of each drug this dealer will still buy, keyed by drug name. Zero means he is
  /// stocked up and only sells.
  /// </summary>
  public IReadOnlyDictionary<string, int> Demand => _demand;

  /// <summary>
  /// The drug this dealer is running a special offer on, or <see langword="null"/>. Always
  /// set and cleared together with <see cref="Offer"/>, which is why both live here.
  /// </summary>
  public string? OfferDrug { get; private set; }

  /// <summary>
  /// Which way this dealer's offer runs, or <see cref="DealerOfferKind.None"/> when he is
  /// trading normally.
  /// </summary>
  public DealerOfferKind Offer { get; private set; }

  /// <summary>
  /// Whether a special offer stands. What <see cref="DrugDealer.RefreshBlip"/> flags.
  /// </summary>
  public bool HasOffer => Offer != DealerOfferKind.None;

  /// <summary>
  /// Whether this book has a position in <paramref name="drugName"/> at all — that is,
  /// whether the catalog this build shipped with knows the drug.
  /// </summary>
  /// <remarks>
  /// For <c>SaveService</c>, which must skip a drug a save file names but this build has
  /// since dropped, rather than adding it back as a key nothing else knows about.
  /// </remarks>
  /// <param name="drugName">The drug to look for.</param>
  public bool Knows(string drugName) => _amount.ContainsKey(drugName);

  /// <summary>
  /// Sets this dealer's whole position in one drug: what he has, what he charges, what he
  /// still wants.
  /// </summary>
  /// <remarks>
  /// All three together and never one at a time, because they are not independent — the
  /// share is what the price and the appetite are derived from. The distribution writes
  /// through here, and so does a load from the save file.
  /// </remarks>
  /// <param name="drugName">The drug.</param>
  /// <param name="amount">The stock he holds.</param>
  /// <param name="price">What he charges for it.</param>
  /// <param name="demand">How much more of it he wants.</param>
  public void SetPosition(string drugName, int amount, int price, int demand)
  {
    _amount[drugName] = amount;
    _price[drugName] = price;
    _demand[drugName] = demand;
  }

  /// <summary>
  /// Moves stock out and appetite in by <paramref name="amount"/>: the player has bought
  /// from this dealer.
  /// </summary>
  /// <remarks>
  /// Taking a dealer's stock leaves him that much shorter, so he is now willing to buy back
  /// what he just sold. Holding the two in step is what keeps <c>amount + demand</c> at the
  /// target <see cref="MarketPricing.Demand"/> set for him, without touching the price he
  /// opened the cycle with — and that is what stops the player emptying a bag into the one
  /// dealer paying the most.
  /// </remarks>
  /// <param name="drugName">The drug traded.</param>
  /// <param name="amount">How much of it changed hands.</param>
  public void PlayerBought(string drugName, int amount)
  {
    _amount[drugName] -= amount;
    _demand[drugName] += amount;
  }

  /// <summary>
  /// Moves stock in and appetite out by <paramref name="amount"/>: the player has sold to
  /// this dealer. The mirror of <see cref="PlayerBought"/> — what he takes on, he no longer
  /// needs.
  /// </summary>
  /// <remarks>
  /// Deliberately unclamped, and not to be "fixed" here. Nothing in this method stops the
  /// appetite going negative; what stops it is the sell window, which caps the amount at
  /// <see cref="Demand"/> before it ever gets here. The cap is a UI concern and cannot move
  /// into the book without the book deciding how much a player may sell.
  /// </remarks>
  /// <param name="drugName">The drug traded.</param>
  /// <param name="amount">How much of it changed hands.</param>
  public void PlayerSold(string drugName, int amount)
  {
    _amount[drugName] += amount;
    _demand[drugName] -= amount;
  }

  /// <summary>
  /// Puts this dealer on an <see cref="DealerOfferKind.InDemand"/> offer: he needs the drug,
  /// so he has none of it and pays over the odds for it.
  /// </summary>
  /// <remarks>
  /// No stock is what also makes him buy-only, exactly as an ordinary dry dealer is. The
  /// premium needs the appetite to go with it — see <see cref="DealerOffer.OfferQuantity"/>.
  /// </remarks>
  /// <param name="drugName">The drug on offer.</param>
  /// <param name="price">What he pays for it.</param>
  /// <param name="quantity">How much of it he wants.</param>
  public void ApplyInDemand(string drugName, int price, int quantity)
  {
    RememberPosition(drugName);

    SetPosition(drugName, amount: 0, price, demand: quantity);

    OfferDrug = drugName;
    Offer = DealerOfferKind.InDemand;
  }

  /// <summary>
  /// Puts this dealer on a <see cref="DealerOfferKind.ClearOut"/> offer: he is dumping the
  /// drug cheap and is not in the market to buy any.
  /// </summary>
  /// <remarks>
  /// No appetite, because a clear-out that also paid over the odds would let the player sell
  /// it straight back.
  /// </remarks>
  /// <param name="drugName">The drug on offer.</param>
  /// <param name="price">What he sells it for.</param>
  /// <param name="quantity">How much of it he is shifting.</param>
  public void ApplyClearOut(string drugName, int price, int quantity)
  {
    RememberPosition(drugName);

    SetPosition(drugName, amount: quantity, price, demand: 0);

    OfferDrug = drugName;
    Offer = DealerOfferKind.ClearOut;
  }

  /// <summary>
  /// Ends any standing offer, putting back the price, stock and demand it skewed. Safe to
  /// call on a book with no offer, which is how the distribution can clear every dealer
  /// unconditionally before dealing a fresh market.
  /// </summary>
  public void ClearOffer()
  {
    if (OfferDrug is string drugName)
    {
      if (_offerPreviousPrice is int price)
      {
        _price[drugName] = price;
      }

      if (_offerPreviousAmount is int amount)
      {
        _amount[drugName] = amount;
      }

      if (_offerPreviousDemand is int demand)
      {
        _demand[drugName] = demand;
      }
    }

    OfferDrug = null;
    Offer = DealerOfferKind.None;
    _offerPreviousPrice = null;
    _offerPreviousAmount = null;
    _offerPreviousDemand = null;
  }

  /// <summary>
  /// Remembers a drug's position before an offer overwrites it, so ending the offer is an
  /// exact undo. An offer skews all three numbers, so all three are kept.
  /// </summary>
  private void RememberPosition(string drugName)
  {
    _offerPreviousPrice = _price[drugName];
    _offerPreviousAmount = _amount[drugName];
    _offerPreviousDemand = _demand[drugName];
  }
}
