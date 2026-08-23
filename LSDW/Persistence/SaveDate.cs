using System.Globalization;

using GTA.Chrono;

namespace LSDW.Persistence;

/// <summary>
/// Reads and writes the <c>yyyy-MM-dd</c> dates the save file stores.
/// </summary>
/// <remarks>
/// <para>
/// Hand-rolled rather than routed through <see cref="DateTime"/>, whose range is not
/// <see cref="GameClockDate"/>'s — the game clock starts in 2013 but is not bounded by anything
/// the BCL agrees with.
/// </para>
/// <para>
/// <see cref="GameClockDate"/> is a struct with static factories and read-only properties, so
/// <see cref="System.Xml.Serialization.XmlSerializer"/> cannot round-trip it: every date in the
/// document is a string with an <c>[XmlIgnore]</c> typed accessor beside it, and both go through
/// here. Internal, unlike the entry types themselves — the serializer only needs the types it
/// serialises to be public.
/// </para>
/// </remarks>
internal static class SaveDate
{
  /// <summary>
  /// Formats a date the way <see cref="Parse"/> reads it back.
  /// </summary>
  public static string Format(GameClockDate date)
    => string.Format(CultureInfo.InvariantCulture, "{0:D4}-{1:D2}-{2:D2}", date.Year, date.Month, date.Day);

  /// <summary>
  /// Reads <c>yyyy-MM-dd</c>, returning <see langword="null"/> for anything else. A date that
  /// will not parse costs whatever it was dating and nothing more.
  /// </summary>
  public static GameClockDate? Parse(string? text)
  {
    if (text is null)
    {
      return null;
    }

    string[] parts = text.Split('-');

    if (parts.Length != 3
      || !int.TryParse(parts[0], NumberStyles.None, CultureInfo.InvariantCulture, out int year)
      || !int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out int month)
      || !int.TryParse(parts[2], NumberStyles.None, CultureInfo.InvariantCulture, out int day))
    {
      return null;
    }

    return GameClockDate.TryFromYmd(year, month, day, out GameClockDate date) ? date : null;
  }
}
