using LSDW.Abstractions.Enumerators;

namespace LSDW.Abstractions.Application.Missions.Base;

/// <summary>
/// The mission interface.
/// </summary>
public interface IMission
{
	/// <summary>
	/// The current mission status.
	/// </summary>
	MissionStatusType Status { get; }

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
	/// An event that is raised when a key is lifted.
	/// </summary>
	void OnKeyUp(object sender, KeyEventArgs args);

	/// <summary>
	/// Put code that needs to be looped each frame in here.
	/// </summary>
	void OnTick(object sender, EventArgs args);
}
