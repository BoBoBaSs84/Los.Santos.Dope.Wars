using LSDW.Interfaces.Services;

namespace LSDW.Tests.UnitTestHelpers;

internal sealed class TestDateTimeService : IDateTimeService
{
	public DateTime Now => DateTime.Now;

	public TimeSpan TimeOfDay => Now.TimeOfDay;
}
