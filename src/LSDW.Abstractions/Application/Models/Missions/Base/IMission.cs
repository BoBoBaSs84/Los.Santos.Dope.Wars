using LSDW.Abstractions.Domain.Providers;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Infrastructure.Services;

namespace LSDW.Abstractions.Application.Models.Missions.Base;

/// <summary>
/// The mission interface.
/// </summary>
public interface IMission
{
	/// <summary>
	/// The name of the mission.
	/// </summary>
	string Name { get; }

	/// <summary>
	/// The current mission status.
	/// </summary>
	MissionStatusType Status { get; }

	/// <summary>
	/// The logger service instance to use.
	/// </summary>
	ILoggerService LoggerService { get; }

	/// <summary>
	/// The state service instance to use.
	/// </summary>
	IStateService StateService { get; }

	/// <summary>
	/// The audio provider instance to use.
	/// </summary>
	IAudioProvider AudioProvider { get; }

	/// <summary>
	/// The game provider instance to use.
	/// </summary>
	IGameProvider GameProvider { get; }

	/// <summary>
	/// The notification provider instance to use.
	/// </summary>
	INotificationProvider NotificationProvider { get; }

	/// <summary>
	/// The player provider instance to use.
	/// </summary>
	IPlayerProvider PlayerProvider { get; }

	/// <summary>
	/// The screen provider instance to use.
	/// </summary>
	IScreenProvider ScreenProvider { get; }

	/// <summary>
	/// The location provider instance to use.
	/// </summary>
	IWorldProvider WorldProvider { get; }

	/// <summary>
	/// Should be used for starting the mission.
	/// </summary>
	void StartMission();

	/// <summary>
	/// Should be used for stopping the mission and cleaning up anything created.
	/// </summary>
	void StopMission();

	/// <summary>
	/// Should be used for cleaning up anything created if something goes wrong.
	/// </summary>
	void OnAborted(object sender, EventArgs args);

	/// <summary>
	/// Put code that needs to be looped each frame in here.
	/// </summary>
	void OnTick(object sender, EventArgs args);
}
