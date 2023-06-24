using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Missions.Base;
using LSDW.Abstractions.Enumerators;

namespace LSDW.Domain.Missions.Base;

/// <summary>
/// The mission base class.
/// </summary>
/// <remarks>
/// Implements the members of the <see cref="IMission"/> interface.
/// </remarks>
internal abstract class Mission : IMission
{
	private readonly IServiceManager _serviceManager;

	/// <summary>
	/// Initializes a instance of the mission class.
	/// </summary>
	/// <param name="serviceManager">The service manager instance to use.</param>
	/// <param name="name">The name of the mission.</param>
	protected Mission(IServiceManager serviceManager, string name)
	{
		_serviceManager = serviceManager;
		Name = name;
		Status = MissionStatusType.Stopped;
	}

	public string Name { get; }
	public MissionStatusType Status { get; private set; }

	public abstract void OnAborted(object sender, EventArgs args);

	public abstract void OnTick(object sender, EventArgs args);

	public virtual void StartMission()
	{
		_serviceManager.LoggerService.Information($"{Name} started.");
		Status = MissionStatusType.Started;
	}

	public virtual void StopMission()
	{
		_serviceManager.LoggerService.Information($"{Name} stopped.");
		Status = MissionStatusType.Stopped;
	}
}
