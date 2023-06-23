using LSDW.Abstractions.Domain.Missions.Base;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Abstractions.Infrastructure.Services;

namespace LSDW.Abstractions.Domain.Missions;

/// <summary>
/// The trafficking mission interface.
/// </summary>
public interface ITrafficking : IMission
{
	/// <summary>
	/// The logger service instance to use.
	/// </summary>
	ILoggerService LoggerService { get; }
	/// <summary>
	/// The location provider instance to use.
	/// </summary>
	ILocationProvider LocationProvider { get; }
	/// <summary>
	/// The notification provider instance to use.
	/// </summary>
	INotificationProvider NotificationProvider { get; }
	/// <summary>
	/// The time provider instance to use.
	/// </summary>
	ITimeProvider TimeProvider { get; }
}
