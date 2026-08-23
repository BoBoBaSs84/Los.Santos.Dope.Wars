using LSDW.Dealers;

namespace LSDW.Tests.Dealers;

/// <summary>
/// When the restock and price-refresh passes next fall due.
/// </summary>
[TestClass]
public sealed class MarketScheduleTests
{
  private const int Hour = 3600;

  [TestMethod]
  [DataRow(0, 24, 24, DisplayName = "Midnight, daily")]
  [DataRow(23 * Hour, 24, 24, DisplayName = "Late evening, daily")]
  [DataRow(0, 6, 6, DisplayName = "Midnight, six-hourly")]
  [DataRow(5 * Hour, 6, 6, DisplayName = "Just before a boundary")]
  [DataRow(7 * Hour, 6, 12, DisplayName = "Just after a boundary")]
  [DataRow(23 * Hour, 6, 24, DisplayName = "Last slot of the day rolls past midnight")]
  [DataRow(0, 1, 1, DisplayName = "Hourly")]
  [DataRow(23 * Hour, 1, 24, DisplayName = "Hourly, last hour")]
  public void HoursToNextBoundary_ReturnsHoursFromTodaysMidnight(int secondsFromMidnight, int intervalHours, int expected)
  {
    Assert.AreEqual(expected, MarketSchedule.HoursToNextBoundary(secondsFromMidnight, intervalHours));
  }

  [TestMethod]
  public void HoursToNextBoundary_ExactlyOnABoundary_SchedulesTheNextOne()
  {
    // Strictly greater than now, so a pass that fires on a boundary does not reschedule
    // itself for the same moment and run every tick.
    Assert.AreEqual(12, MarketSchedule.HoursToNextBoundary(6 * Hour, 6));
  }

  [TestMethod]
  [DataRow(0)]
  [DataRow(12 * Hour)]
  [DataRow(86399)]
  public void HoursToNextBoundary_WeeklyInterval_AlwaysLandsOnTheWeekBoundary(int secondsFromMidnight)
  {
    // 168 is the clamp ceiling. Nothing inside one day reaches it, so every time of day
    // schedules the same moment — a week from today's midnight.
    Assert.AreEqual(168, MarketSchedule.HoursToNextBoundary(secondsFromMidnight, 168));
  }

  [TestMethod]
  public void HoursToNextBoundary_ResultMayExceedADay()
  {
    // Documented: the caller adds this to today's midnight, so anything past 24 simply
    // lands on a later date.
    Assert.IsGreaterThan(24, MarketSchedule.HoursToNextBoundary(0, 48));
  }
}
