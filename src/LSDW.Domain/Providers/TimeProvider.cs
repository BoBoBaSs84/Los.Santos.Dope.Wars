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
	public DateTime Now => World.CurrentDate;
	public TimeSpan TimeOfDay => World.CurrentTimeOfDay;
}
