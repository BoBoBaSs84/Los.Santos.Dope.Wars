using LSDW.Abstractions.Interfaces.Application.Providers;

namespace LSDW.Application.Tests.Helpers;

internal sealed class RealTimeProvider : ITimeProvider
{
	public DateTime Now => DateTime.Now;

	public TimeSpan TimeOfDay => DateTime.Now.TimeOfDay;
}
