using LSDW.Persistence;

using GTA.Chrono;

namespace LSDW.Tests.Persistence;

/// <summary>
/// The <c>yyyy-MM-dd</c> format every date in the save document goes through.
/// </summary>
/// <remarks>
/// Covered only indirectly until now, through <c>DealerEntry.CooldownUntil</c>. Worth its own
/// class because it is the format owner: both dated fields read and write through here, and the
/// parse is hand-rolled rather than routed through <see cref="DateTime"/>, whose range is not
/// <see cref="GameClockDate"/>'s.
/// </remarks>
[TestClass]
public sealed class SaveDateTests
{
  /// <summary>
  /// A date the test can rely on. Copied in <c>SaveFileFormatTests</c> rather than shared: it is
  /// two lines, and an arrange-helper is worth more beside the assertions that use it than in a
  /// file of its own.
  /// </summary>
  private static GameClockDate Date(int year, int month, int day)
  {
    Assert.IsTrue(GameClockDate.TryFromYmd(year, month, day, out GameClockDate date),
      $"{year}-{month}-{day} is not a date the test can use");

    return date;
  }

  [TestMethod]
  [DataRow(2013, 5, 18, "2013-05-18", DisplayName = "The date the game clock starts on")]
  [DataRow(2013, 12, 31, "2013-12-31", DisplayName = "Two-digit month and day")]
  [DataRow(7, 1, 2, "0007-01-02", DisplayName = "Padded to fixed width")]
  public void Format_ProducesTheFixedWidthForm(int year, int month, int day, string expected)
  {
    Assert.AreEqual(expected, SaveDate.Format(Date(year, month, day)));
  }

  [TestMethod]
  [DataRow(2013, 5, 18)]
  [DataRow(7, 1, 2)]
  [DataRow(2013, 2, 28)]
  [DataRow(2016, 2, 29, DisplayName = "Leap day")]
  public void FormatThenParse_ReturnsTheSameDate(int year, int month, int day)
  {
    GameClockDate date = Date(year, month, day);

    Assert.AreEqual(date, SaveDate.Parse(SaveDate.Format(date)));
  }

  [TestMethod]
  public void Parse_Null_IsNoDate()
  {
    // The absent attribute, which is what a dealer with no standing cooldown writes.
    Assert.IsNull(SaveDate.Parse(null));
  }

  [TestMethod]
  [DataRow("", DisplayName = "Empty")]
  [DataRow("not-a-date", DisplayName = "Not numeric")]
  [DataRow("2013-05", DisplayName = "Too few parts")]
  [DataRow("2013-05-18-01", DisplayName = "Too many parts")]
  [DataRow("-2013-05-18", DisplayName = "Leading separator splits into four")]
  [DataRow("20130518", DisplayName = "No separators")]
  public void Parse_MalformedText_IsNoDate(string text)
  {
    // A date that will not parse costs whatever it was dating and nothing more — the rest of
    // the document still loads.
    Assert.IsNull(SaveDate.Parse(text));
  }

  [TestMethod]
  [DataRow("+2013-05-18", DisplayName = "Signed year")]
  [DataRow("2013-+5-18", DisplayName = "Signed month")]
  [DataRow("2013- 5-18", DisplayName = "Space-padded month")]
  [DataRow("2013-05-18 ", DisplayName = "Trailing space")]
  public void Parse_SignedOrPaddedComponents_IsNoDate(string text)
  {
    // NumberStyles.None, deliberately: the writer emits exactly one form, so anything else is
    // a hand edit and is refused rather than guessed at.
    Assert.IsNull(SaveDate.Parse(text));
  }

  [TestMethod]
  [DataRow("2013-13-18", DisplayName = "Month past twelve")]
  [DataRow("2013-05-45", DisplayName = "Day past the month")]
  [DataRow("2013-02-30", DisplayName = "Day the month does not have")]
  [DataRow("2013-00-18", DisplayName = "Month zero")]
  [DataRow("2013-05-00", DisplayName = "Day zero")]
  public void Parse_ComponentsOutOfRange_IsNoDate(string text)
  {
    // Parsed as numbers first and only then handed to TryFromYmd, which is what rejects a day
    // February does not have.
    Assert.IsNull(SaveDate.Parse(text));
  }

  [TestMethod]
  public void Parse_ReadsTheFormItsOwnWriterEmits()
  {
    // The pairing the save file depends on: what Format writes, Parse reads, with no other
    // form accepted in between.
    Assert.AreEqual(Date(2013, 5, 18), SaveDate.Parse("2013-05-18"));
  }
}
