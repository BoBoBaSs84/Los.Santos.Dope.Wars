using GTA;
using LSDW.Abstractions.Domain.Missions.Base;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Infrastructure.Services;

namespace LSDW.Domain.Missions.Base;

/// <summary>
/// The mission base class.
/// </summary>
/// <remarks>
/// Implements the members of the <see cref="IMission"/> interface.
/// </remarks>
internal abstract class Mission : IMission
{
	private readonly ILoggerService _loggerService;

	/// <summary>
	/// Initializes a instance of the mission class.
	/// </summary>
	/// <param name="loggerService">The loger service interface to use.</param>
	/// <param name="name">The name of the mission.</param>
	protected Mission(ILoggerService loggerService, string name)
	{
		_loggerService = loggerService;
		Name = name;
		Status = MissionStatusType.Stopped;
	}

	public string Name { get; }
	public MissionStatusType Status { get; private set; }

	public abstract void OnAborted(object sender, EventArgs args);
	public abstract void OnTick(object sender, EventArgs args);
	public virtual void StartMission()
	{
		_loggerService.Information($"{Name} started.");
		Status = MissionStatusType.Started;
		Game.IsMissionActive = true;
	}
	public virtual void StopMission()
	{
		_loggerService.Information($"{Name} stopped.");
		Status = MissionStatusType.Stopped;
		Game.IsMissionActive = false;
	}
}
