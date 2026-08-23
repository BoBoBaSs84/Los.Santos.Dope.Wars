namespace LSDW.UserInterface;

/// <summary>
/// The numbers a <see cref="TransactionWindow"/> shows the player, as strings: the price
/// comparison, the running total, and whether the trade in front of them is a profit or a
/// loss.
/// </summary>
/// <remarks>
/// <para>
/// Pure functions of their arguments, and static for that reason. Split out of
/// <see cref="TransactionWindow"/>, whose every property setter writes into a
/// <c>GTA.UI.TextElement</c> — which is what kept this arithmetic off the test suite despite
/// it carrying the subtlest rule in the trade flow (see <see cref="SummaryText"/>).
/// </para>
/// <para>
/// The colour tags (<c>~g~</c>, <c>~r~</c>) are part of the strings because GTA reads them
/// inline; they are not a styling decision this type is making.
/// </para>
/// </remarks>
internal static class TransactionSummary
{
  /// <summary>
  /// The reference price, this dealer's price, and the gap between them. When buying, paying
  /// under the market is the win; when selling, it is the other way round.
  /// </summary>
  /// <param name="sellingDrugs">Whether the player is selling to the dealer.</param>
  /// <param name="value">What this dealer is charging or paying.</param>
  /// <param name="normalValue">
  /// The reference: the market price when buying, what the player paid when selling.
  /// </param>
  public static string MoneyText(bool sellingDrugs, int value, int normalValue)
  {
    int delta = sellingDrugs ? (value - normalValue) : (normalValue - value);

    return "$" + normalValue + "\n$" + value + "\n" + Delta(delta);
  }

  /// <summary>
  /// The four-line variant, for the sell side only: what the market says, what the player
  /// paid, what this dealer offers, and the margin.
  /// </summary>
  /// <remarks>
  /// Always measures <paramref name="value"/> against <paramref name="normalValue"/> in the
  /// selling direction, because nothing sets this on a buy window — doing so would relabel
  /// that window's three lines to four.
  /// </remarks>
  /// <param name="globalValue">The market value of the drug.</param>
  /// <param name="normalValue">What the player paid for it.</param>
  /// <param name="value">What this dealer is paying.</param>
  public static string MoneyTextWithMarket(int globalValue, int normalValue, int value)
    => "$" + globalValue + "\n$" + normalValue + "\n$" + value + "\n" + Delta(value - normalValue);

  /// <summary>
  /// The bags-by-price header and the profit or loss it implies.
  /// </summary>
  /// <remarks>
  /// The profitability branch reads the <b>per-unit</b> delta, never the total. At
  /// <paramref name="amount"/> zero — the state a window opens in — the total is zero either
  /// way, so branching on it would flip the label from a green "Profit $0" to a red
  /// "Loss $0" for no reason the player could see. <paramref name="normalValue"/> is the
  /// bought price when selling and the market price when buying, so the same sign means
  /// opposite things in the two directions.
  /// </remarks>
  /// <param name="sellingDrugs">Whether the player is selling to the dealer.</param>
  /// <param name="amount">How many units the window is sitting on.</param>
  /// <param name="value">What this dealer is charging or paying, per unit.</param>
  /// <param name="normalValue">The reference price, per unit.</param>
  public static string SummaryText(bool sellingDrugs, int amount, int value, int normalValue)
  {
    string header = amount + " bags X $" + value + " = $" + value * amount;

    int unitDelta = normalValue - value;
    int totalDelta = unitDelta * amount;
    bool profitable = sellingDrugs ? (unitDelta < 0) : (unitDelta >= 0);

    return profitable
      ? header + "\n\n~g~Potential Profit of $" + Math.Abs(totalDelta)
      : header + "\n\n~r~Potential Loss of $" + Math.Abs(totalDelta);
  }

  /// <summary>
  /// A signed money figure, coloured green at or above zero and red below it.
  /// </summary>
  /// <param name="delta">The figure, already in the caller's direction.</param>
  public static string Delta(int delta)
    => (delta >= 0) ? ("~g~$" + delta) : ("~r~$" + delta);
}
