using LSDW.Enumerators;

namespace LSDW.Interfaces.Missions;

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
	/// Put code that needs to be looped each frame in here.
	/// </summary>
	void OnTick(object sender, EventArgs args);

	/// <summary>
	/// Should be used for cleaning up anything created
	/// </summary>
	void OnAborted(object sender, EventArgs args);
}
