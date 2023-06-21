using GTA;
using LSDW.Abstractions.Domain.Providers;

namespace LSDW.Domain.Providers;

/// <summary>
/// The time provider class.
/// </summary>
internal sealed class TimeProvider : ITimeProvider
{
	/// <summary>
	/// Initializes a instance of the time provider class.
	/// </summary>
	internal TimeProvider()
	{
		Now = World.CurrentDate;
		TimeOfDay = World.CurrentTimeOfDay;
	}

	public DateTime Now { get; }
	public TimeSpan TimeOfDay { get; }
}
