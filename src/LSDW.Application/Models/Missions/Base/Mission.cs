using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Application.Models.Missions.Base;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Infrastructure.Services;

namespace LSDW.Application.Models.Missions.Base;

/// <summary>
/// The mission base class.
/// </summary>
/// <remarks>
/// Implements the members of the <see cref="IMission"/> interface.
/// </remarks>
internal abstract class Mission : IMission
{
	/// <summary>
	/// Initializes a instance of the mission base class.
	/// </summary>
	/// <param name="serviceManager">The service manager instance to use.</param>
	/// <param name="providerManager">The provider manager instance to use.</param>
	/// <param name="name">The name of the mission.</param>
	protected Mission(IServiceManager serviceManager, IProviderManager providerManager, string name)
	{
		Name = name;
		Status = MissionStatusType.STOPPED;

		LoggerService = serviceManager.LoggerService;
		LocationProvider = providerManager.LocationProvider;
		NotificationProvider = providerManager.NotificationProvider;
		PlayerProvider = providerManager.PlayerProvider;
		TimeProvider = providerManager.TimeProvider;
	}

	public string Name { get; }
	public MissionStatusType Status { get; private set; }
	public ILoggerService LoggerService { get; }
	public ILocationProvider LocationProvider { get; }
	public INotificationProvider NotificationProvider { get; }
	public IPlayerProvider PlayerProvider { get; }
	public ITimeProvider TimeProvider { get; }

	public abstract void OnAborted(object sender, EventArgs args);

	public abstract void OnTick(object sender, EventArgs args);

	public virtual void StartMission()
	{
		LoggerService.Information($"{Name} started.");
		Status = MissionStatusType.STARTED;
	}

	public virtual void StopMission()
	{
		LoggerService.Information($"{Name} stopped.");
		Status = MissionStatusType.STOPPED;
	}
}
