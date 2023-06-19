using LSDW.Abstractions.Application.Providers;
using LSDW.Abstractions.Domain.Missions.Base;
using LSDW.Abstractions.Domain.Services;
using LSDW.Abstractions.Infrastructure.Services;

namespace LSDW.Abstractions.Domain.Missions;

/// <summary>
/// The trafficking mission interface.
/// </summary>
public interface ITrafficking : IMission
{
	/// <summary>
	/// The logger service to use for the trafficking mission.
	/// </summary>
	ILoggerService LoggerService { get; }

	/// <summary>
	/// The notification service to use for the trafficking mission.
	/// </summary>
	INotificationService NotificationService { get; }

	/// <summary>
	/// The time provider to use for the trafficking mission.
	/// </summary>
	ITimeProvider TimeProvider { get; }


}
