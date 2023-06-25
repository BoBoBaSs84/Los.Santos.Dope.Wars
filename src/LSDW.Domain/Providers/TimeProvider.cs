using GTA;
using LSDW.Abstractions.Domain.Providers;

namespace LSDW.Domain.Providers;

/// <summary>
/// The time provider class.
/// </summary>
/// <remarks>
/// Wrapper for the <see cref="World"/> properties.
/// </remarks>
internal sealed class TimeProvider : ITimeProvider
{
	public DateTime Now
	{
		get => World.CurrentDate;
		set => World.CurrentDate = value;
	}

	public TimeSpan TimeOfDay
	{
		get => World.CurrentTimeOfDay;
		set => World.CurrentTimeOfDay = value;
	}
}
