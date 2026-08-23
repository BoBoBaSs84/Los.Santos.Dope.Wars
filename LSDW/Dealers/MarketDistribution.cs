namespace LSDW.Dealers;

/// <summary>
/// Splits a cycle's supply across the roster. One dealer ends up with a pile, his neighbour
/// with a handful, and a good few with nothing at all — which is what
/// <see cref="MarketPricing"/> then turns into prices.
/// </summary>
/// <remarks>
/// <para>
/// The weights come from <see cref="DealerMarket"/>, which draws them as
/// <c>-log(1 - NextDouble())</c>. Exponential weights normalised by their sum are a uniform
/// draw from the simplex — in plain terms, <i>every</i> way of dividing the supply is equally
/// likely. That is where the inequality comes from; it is a property of the distribution
/// rather than a skew anyone picked.
/// </para>
/// <para>
/// The draws stay out of this type on purpose, so the apportionment can be tested against
/// hand-written weight vectors.
/// </para>
/// </remarks>
internal static class MarketDistribution
{
  /// <summary>
  /// The chance, per dealer per drug, that he is dry this cycle and gets no weight at all.
  /// Whole number 0..100, to meet a <c>NextPercent()</c> roll.
  /// </summary>
  /// <remarks>
  /// A dry dealer is not a gap in the design, he is the point of it: with nothing to sell he
  /// prices at the ceiling and is the man to sell <i>to</i>.
  /// </remarks>
  public const int DryChance = 30;

  /// <summary>
  /// Divides <paramref name="total"/> across <paramref name="weights"/>, writing each
  /// dealer's share into <paramref name="shares"/>.
  /// </summary>
  /// <remarks>
  /// Sequential apportionment: each dealer takes his fraction of what is <i>left</i>, so the
  /// last dealer with any weight takes the remainder and the shares sum to
  /// <paramref name="total"/> exactly. Rounding cannot leak or invent units, which matters
  /// because the total is also what every price is measured against. The one exception is a
  /// roster where every weight is zero: nobody can be given anything, so the shares sum to
  /// zero and the supply simply never reaches the street.
  /// </remarks>
  /// <param name="weights">One weight per dealer. Zero — or negative — means he gets nothing.</param>
  /// <param name="total">The units to divide. Zero is valid and gives everyone nothing.</param>
  /// <param name="shares">The buffer to fill. Must be at least as long as <paramref name="weights"/>.</param>
  /// <exception cref="ArgumentNullException"><paramref name="weights"/> or <paramref name="shares"/> is null.</exception>
  /// <exception cref="ArgumentException"><paramref name="shares"/> is too short for <paramref name="weights"/>.</exception>
  /// <exception cref="ArgumentOutOfRangeException"><paramref name="total"/> is negative.</exception>
  public static void Apportion(IReadOnlyList<double> weights, int total, int[] shares)
  {
    if (weights is null)
    {
      throw new ArgumentNullException(nameof(weights));
    }

    if (shares is null)
    {
      throw new ArgumentNullException(nameof(shares));
    }

    if (shares.Length < weights.Count)
    {
      throw new ArgumentException("The share buffer is shorter than the weights.", nameof(shares));
    }

    if (total < 0)
    {
      throw new ArgumentOutOfRangeException(nameof(total), total, "A supply total cannot be negative.");
    }

    // The last dealer with weight is handed the remainder outright rather than computed like
    // the rest. His fraction is 1.0 by construction, but only to within floating-point error
    // after a roster's worth of subtractions, and "within error" would lose or conjure a unit.
    int lastWeighted = -1;
    double remainingWeight = 0.0;

    for (int index = 0; index < weights.Count; index++)
    {
      if (weights[index] > 0.0)
      {
        remainingWeight += weights[index];
        lastWeighted = index;
      }
    }

    int remainingTotal = total;

    for (int index = 0; index < weights.Count; index++)
    {
      double weight = weights[index];

      if (weight <= 0.0 || remainingTotal <= 0)
      {
        shares[index] = 0;
        continue;
      }

      if (index == lastWeighted)
      {
        shares[index] = remainingTotal;
        remainingTotal = 0;
        continue;
      }

      int share = (int)Math.Round(remainingTotal * (weight / remainingWeight));

      if (share > remainingTotal)
      {
        share = remainingTotal;
      }

      shares[index] = share;
      remainingTotal -= share;
      remainingWeight -= weight;
    }
  }
}
