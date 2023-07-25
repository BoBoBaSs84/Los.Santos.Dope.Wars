using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Application.Models.Missions.Base;
using LSDW.Abstractions.Domain.Providers;
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
	/// <summary>
	/// Initializes a instance of the mission class.
	/// </summary>
	/// <param name="serviceManager">The service manager instance to use.</param>
	/// <param name="providerManager">The provider manager instance to use.</param>	
	/// <param name="name">The name of the mission.</param>
	protected Mission(IServiceManager serviceManager, IProviderManager providerManager, string name = nameof(Mission))
	{
		Name = name;
		Status = MissionStatusType.STOPPED;

		LoggerService = serviceManager.LoggerService;
		SettingsService = serviceManager.SettingsService;
		StateService = serviceManager.StateService;
		AudioProvider = providerManager.AudioProvider;
		GameProvider = providerManager.GameProvider;
		NotificationProvider = providerManager.NotificationProvider;
		PlayerProvider = providerManager.PlayerProvider;
		ScreenProvider = providerManager.ScreenProvider;
		WorldProvider = providerManager.WorldProvider;
	}

	public string Name { get; }
	public MissionStatusType Status { get; private set; }
	public ILoggerService LoggerService { get; }
	public ISettingsService SettingsService { get; }
	public IStateService StateService { get; }
	public IAudioProvider AudioProvider { get; }
	public IGameProvider GameProvider { get; }
	public INotificationProvider NotificationProvider { get; }
	public IPlayerProvider PlayerProvider { get; }
	public IScreenProvider ScreenProvider { get; }
	public IWorldProvider WorldProvider { get; }

	public abstract void OnAborted(object sender, EventArgs args);
	public abstract void OnTick(object sender, EventArgs args);

	public virtual void StartMission()
	{
		LoggerService.Information(RESX.Mission_Information_Started.FormatInvariant(Name));
		Status = MissionStatusType.STARTED;
	}

	public virtual void StopMission()
	{
		LoggerService.Information(RESX.Mission_Information_Stopped.FormatInvariant(Name));
		Status = MissionStatusType.STOPPED;
	}
}
