using GTA;
using LSDW.Abstractions.Interfaces.Application;

namespace LSDW.Application.Services;

/// <summary>
/// The date time service class.
/// </summary>
internal sealed class GameTimeService : IDateTimeService
{
	public DateTime Now => World.CurrentDate;

	public TimeSpan TimeOfDay => World.CurrentTimeOfDay;
}
