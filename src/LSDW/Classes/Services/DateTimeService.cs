using GTA;
using LSDW.Interfaces.Services;

namespace LSDW.Classes.Services;

/// <summary>
/// The date time service class.
/// </summary>
internal sealed class DateTimeService : IDateTimeService
{
	public DateTime Now => World.CurrentDate;

	public TimeSpan TimeOfDay => World.CurrentTimeOfDay;
}
