namespace LSDW.Activities;

/// <summary>
/// The sliding window of recent taco sales that decides when selling too fast draws
/// police attention. Holds the last four sale timestamps and reports whether they fall
/// inside the caller's gap threshold.
/// </summary>
/// <remarks>
/// Deliberately free of GTA types: it takes the threshold as an argument and reports heat
/// rather than raising the wanted level, so the whole window is drivable headlessly. The
/// window is never cleared between sessions — sales spanning two sessions draw heat as
/// though they were one, which is the behaviour this was lifted from. It is cleared by a
/// script reload, which is what owning the window rather than leaving it ambient buys.
/// </remarks>
internal sealed class TacoHeatWindow
{
  /// <summary>Sliding window of the last four sale timestamps, in Unix seconds.</summary>
  private readonly List<int> _saleTimestamps = [];

  /// <summary>
  /// Records a completed sale and reports whether the last four of them happened close
  /// enough together to draw police attention.
  /// </summary>
  /// <param name="gapThreshold">
  /// The largest total span, in seconds, across the four most recent sales that still
  /// counts as selling too fast.
  /// </param>
  /// <returns>
  /// <see langword="true"/> if the window is full and its span is at or under
  /// <paramref name="gapThreshold"/>; otherwise <see langword="false"/>.
  /// </returns>
  /// <remarks>
  /// The early return covers counts 0..3, so <b>the fifth</b> sale is the first that can
  /// draw heat, not the fourth. Off by one from what "a window of four" suggests, and
  /// lifted as-is.
  /// </remarks>
  public bool RecordSale(int gapThreshold)
  {
    // Keep a sliding window of the last four sale timestamps. Selling too fast
    // draws police attention.
    if (_saleTimestamps.Count < 4)
    {
      _saleTimestamps.Add(UnixNow());
      return false;
    }

    _saleTimestamps.RemoveAt(0);
    _saleTimestamps.Add(UnixNow());

    return DrawsHeat(_saleTimestamps, gapThreshold);
  }

  /// <summary>
  /// Whether the span from the oldest to the newest of <paramref name="timestamps"/> is
  /// at or under <paramref name="gapThreshold"/>.
  /// </summary>
  /// <param name="timestamps">The window, in ascending order, in Unix seconds.</param>
  /// <param name="gapThreshold">The largest span that still counts as selling too fast.</param>
  /// <remarks>
  /// Split out from <see cref="RecordSale"/> so the maths can be driven from synthetic
  /// timestamps instead of the wall clock, which is the only way to check it without the
  /// game. It is the original loop unchanged: <c>previous</c> is both the running
  /// subtraction and the next pair's right-hand side, which is what makes it read like an
  /// error. Summing consecutive gaps telescopes, so the result is simply
  /// <c>last - first</c> — and it is left in this form rather than simplified, because a
  /// refactor is not where a behaviour change belongs.
  /// <para>
  /// Static while the rest of the type is not: it is a pure function of its arguments, and
  /// the tests call it without a window to hang it off.
  /// </para>
  /// </remarks>
  public static bool DrawsHeat(IReadOnlyList<int> timestamps, int gapThreshold)
  {
    int previous = timestamps[timestamps.Count - 1];
    int totalGap = 0;
    for (int i = timestamps.Count - 2; i >= 0; i--)
    {
      previous -= timestamps[i];
      totalGap += previous;
      previous = timestamps[i];
    }

    return totalGap <= gapThreshold;
  }

  private static int UnixNow()
    => (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
}
