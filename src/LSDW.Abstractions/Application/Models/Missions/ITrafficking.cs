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
	/// Indicates if the left and right side menus are initialized.
	/// </summary>
	bool MenusInitialized { get; }

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

	/// <summary>
	/// Cleans up the side menus.
	/// </summary>
	void CleanUpMenus();

	/// <summary>
	/// Sets the left side menu.
	/// </summary>
	/// <param name="leftSideMenu">The left menu instance to set.</param>
	/// <param name="rightSideMenu">The right menu instance to set.</param>
	void SetMenus(ISideMenu leftSideMenu, ISideMenu rightSideMenu);
}
