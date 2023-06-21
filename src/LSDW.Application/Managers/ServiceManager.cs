using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Services;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Domain.Factories;
using LSDW.Infrastructure.Factories;

namespace LSDW.Application.Managers;

/// <summary>
/// The service manager class.
/// </summary>
internal sealed class ServiceManager : IServiceManager
{
	private readonly Lazy<IGameStateService> _lazyStateService;
	private readonly Lazy<ILoggerService> _lazyLoggerService;
	private readonly Lazy<INotificationService> _lazyNotificationService;
	private readonly Lazy<ISettingsService> _lazySettingsService;

	public ServiceManager()
	{
		_lazyLoggerService = new Lazy<ILoggerService>(InfrastructureFactory.CreateLoggerService);
		_lazyStateService = new Lazy<IGameStateService>(() => InfrastructureFactory.CreateGameStateService(_lazyLoggerService.Value));
		_lazyNotificationService = new Lazy<INotificationService>(DomainFactory.CreateNotificationService);
		_lazySettingsService = new Lazy<ISettingsService>(InfrastructureFactory.CreateSettingsService);
	}

	public IGameStateService StateService
		=> _lazyStateService.Value;

	public ILoggerService LoggerService
		=> _lazyLoggerService.Value;

	public INotificationService NotificationService
		=> _lazyNotificationService.Value;

	public ISettingsService SettingsService
		=> _lazySettingsService.Value;
}
