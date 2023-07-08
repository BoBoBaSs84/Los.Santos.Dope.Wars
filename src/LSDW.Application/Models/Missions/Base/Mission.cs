using LSDW.Abstractions.Application.Models.Missions.Base;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Domain.Extensions;
using RESX = LSDW.Application.Properties.Resources;

namespace LSDW.Application.Models.Missions.Base;

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
	/// Initializes a instance of the mission base class.
	/// </summary>
	/// <param name="loggerService">The logger service instance to use.</param>
	/// <param name="name">The name of the mission.</param>
	protected Mission(ILoggerService loggerService, string name)
	{
		_loggerService = loggerService;

		Name = name;
		Status = MissionStatusType.STOPPED;
	}

	public string Name { get; }
	public MissionStatusType Status { get; private set; }

	public abstract void OnAborted(object sender, EventArgs args);

	public abstract void OnTick(object sender, EventArgs args);

	public virtual void StartMission()
	{
		_loggerService.Information(RESX.Mission_Information_Started.FormatInvariant(Name));
		Status = MissionStatusType.STARTED;
	}

	public virtual void StopMission()
	{
		_loggerService.Information(RESX.Mission_Information_Stopped.FormatInvariant(Name));
		Status = MissionStatusType.STOPPED;
	}
}
