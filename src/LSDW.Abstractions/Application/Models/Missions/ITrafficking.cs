using LSDW.Abstractions.Application.Models.Missions.Base;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Abstractions.Presentation.Menus;

namespace LSDW.Abstractions.Application.Models.Missions;

/// <summary>
/// The trafficking mission interface.
/// </summary>
public interface ITrafficking : IMission
{
	/// <summary>
	/// The side menu on the left of the screen.
	/// </summary>
	ISideMenu LeftSideMenu { get; }

	/// <summary>
	/// The side menu on the right of the screen.
	/// </summary>
	ISideMenu RightSideMenu { get; }

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
