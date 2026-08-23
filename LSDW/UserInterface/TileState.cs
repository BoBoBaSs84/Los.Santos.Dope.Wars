namespace LSDW.UserInterface;

/// <summary>
/// How this dealer's price compares to the tile's reference price, from the player's side of
/// the trade.
/// </summary>
internal enum TileDeal
{
  /// <summary>The two prices match; no arrow is drawn.</summary>
  Neutral,

  /// <summary>In the player's favour — cheap to buy, or dear to sell into.</summary>
  Good,

  /// <summary>Against them.</summary>
  Bad
}

/// <summary>
/// What one drug tile knows: how much there is, what it costs, what that is being measured
/// against, and how much the dealer will still take. From those it decides the two things the
/// tile's appearance is driven by — whether it can be acted on at all, and whether the price
/// in front of the player is a good one.
/// </summary>
/// <remarks>
/// <para>
/// Split out of <see cref="DrugMenuItem"/>, which holds four <c>GTA.UI</c> elements and writes
/// into them from its own setters. The decisions are here and are pure; the alpha levels,
/// colours and arrow rotation that express them stay on the tile, because those are
/// appearance rather than state.
/// </para>
/// <para>
/// This is where <i>an empty dealer only buys and a flooded one only sells</i> reaches the
/// player: both fall out of <see cref="Tradeable"/> rather than being special-cased anywhere.
/// </para>
/// </remarks>
internal sealed class TileState
{
  /// <summary>
  /// The <see cref="Demand"/> of a tile nobody has constrained — the buy side, and any tile
  /// before <c>DealerTrading.Open</c> has filled it in.
  /// </summary>
  public const int NoLimit = int.MaxValue;

  /// <summary>
  /// How much there is to trade: the dealer's stock on the buy side, what the player is
  /// carrying on the sell side.
  /// </summary>
  public int Amount { get; set; }

  /// <summary>What this dealer charges or pays for it.</summary>
  public int Price { get; set; }

  /// <summary>
  /// What <see cref="Price"/> is measured against: the market value on the buy side, what the
  /// player actually paid on the sell side.
  /// </summary>
  public int MarketPrice { get; set; }

  /// <summary>Whether this tile sells to the dealer rather than buying from him.</summary>
  public bool SellingDrugs { get; set; }

  /// <summary>
  /// How much of this drug the dealer will still take. <b>Sell side only</b> — the buy side
  /// leaves it at <see cref="NoLimit"/>, because what limits a purchase is his stock, which
  /// <see cref="Amount"/> already carries there.
  /// </summary>
  public int Demand { get; set; } = NoLimit;

  /// <summary>
  /// Whether this tile can be acted on at all: something to trade, and on the sell side a
  /// dealer still willing to take it. A dealer sitting on a glut buys nothing, so his sell
  /// tiles read as unavailable however much the player is carrying.
  /// </summary>
  public bool Tradeable => Amount > 0 && (!SellingDrugs || Demand > 0);

  /// <summary>
  /// Which way the price runs for the player. When buying, a price below the reference is the
  /// win; when selling it is the other way round.
  /// </summary>
  public TileDeal Deal
  {
    get
    {
      if (Price == MarketPrice)
      {
        return TileDeal.Neutral;
      }

      bool good = SellingDrugs ? (Price > MarketPrice) : (Price < MarketPrice);

      return good ? TileDeal.Good : TileDeal.Bad;
    }
  }
}
