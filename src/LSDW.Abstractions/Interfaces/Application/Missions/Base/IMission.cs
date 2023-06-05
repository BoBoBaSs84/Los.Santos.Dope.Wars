using LSDW.Domain.Enumerators;

namespace LSDW.Abstractions.Interfaces.Application.Missions.Base;

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
	/// Should be used for cleaning up anything created.
	/// </summary>
	void CleanUp();

	/// <summary>
	/// Put code that needs to be looped each frame in here.
	/// </summary>
	void OnTick(object sender, EventArgs args);

	/// <summary>
	/// Should be used for cleaning up anything created if something goes wrong.
	/// </summary>
	void OnAborted(object sender, EventArgs args);
}
