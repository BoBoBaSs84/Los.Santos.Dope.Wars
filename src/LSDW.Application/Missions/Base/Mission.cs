using GTA;
using LSDW.Abstractions.Application.Missions.Base;
using LSDW.Domain.Enumerators;

namespace LSDW.Application.Missions.Base;

/// <summary>
/// The mission base class.
/// </summary>
/// <remarks>
/// Implements the members of the <see cref="IMission"/> interface.
/// </remarks>
internal abstract class Mission : IMission
{
	public Mission()
		=> Status = MissionStatusType.Stopped;

	public MissionStatusType Status { get; private set; }

	public virtual void OnAborted(object sender, EventArgs args) { }
	public virtual void OnKeyUp(object sender, KeyEventArgs args) { }
	public virtual void OnTick(object sender, EventArgs args) { }
	public virtual void StartMission()
	{
		Status = MissionStatusType.Started;
		Game.IsMissionActive = true;
	}
	public virtual void StopMission()
	{
		Status = MissionStatusType.Stopped;
		Game.IsMissionActive = false;
	}
}
