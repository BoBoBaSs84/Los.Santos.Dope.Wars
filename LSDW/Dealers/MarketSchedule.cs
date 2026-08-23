namespace LSDW.Dealers;

/// <summary>
/// When the market next moves: the arithmetic behind the <c>[Economy]</c> restock and
/// price-refresh intervals, which are anchored to midnight rather than to when the
/// script loaded.
/// </summary>
internal static class MarketSchedule
{
  private const int SecondsPerHour = 3600;

  /// <summary>
  /// How many hours after midnight the next refresh falls, given how far into the day it
  /// is now. Always strictly greater than the current hour, so a refresh that fires
  /// exactly on a boundary schedules the following one rather than itself.
  /// </summary>
  /// <param name="secondsFromMidnight">Seconds elapsed today, 0..86399.</param>
  /// <param name="intervalHours">The configured interval, already clamped to 1..168.</param>
  /// <returns>
  /// Hours from <b>today's</b> midnight — which may exceed 24, and does whenever the
  /// interval is longer than a day or does not divide one. The caller adds it to
  /// midnight, so a result past 24 simply lands on a later date.
  /// </returns>
  public static int HoursToNextBoundary(int secondsFromMidnight, int intervalHours)
    => ((secondsFromMidnight / SecondsPerHour / intervalHours) + 1) * intervalHours;
}
