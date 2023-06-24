using LSDW.Abstractions.Enumerators;

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
