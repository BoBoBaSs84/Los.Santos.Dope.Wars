using GTA;
using LSDW.Abstractions.Interfaces.Application.Providers;

namespace LSDW.Application.Providers;

/// <summary>
/// The game time provider.
/// </summary>
internal sealed class GameTimeProvider : ITimeProvider
{
	public DateTime Now => World.CurrentDate;

	public TimeSpan TimeOfDay => World.CurrentTimeOfDay;
}
