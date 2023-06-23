using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Infrastructure.Factories;

namespace LSDW.Application.Managers;

/// <summary>
/// The service manager class.
/// </summary>
internal sealed class ServiceManager : IServiceManager
{
	private readonly Lazy<IStateService> _lazyStateService;
	private readonly Lazy<ILoggerService> _lazyLoggerService;
	private readonly Lazy<ISettingsService> _lazySettingsService;

	public ServiceManager()
	{
		_lazyLoggerService = new Lazy<ILoggerService>(InfrastructureFactory.CreateLoggerService);
		_lazyStateService = new Lazy<IStateService>(() => InfrastructureFactory.CreateGameStateService(_lazyLoggerService.Value));
		_lazySettingsService = new Lazy<ISettingsService>(InfrastructureFactory.CreateSettingsService);
	}

	public IStateService StateService
		=> _lazyStateService.Value;

	public ILoggerService LoggerService
		=> _lazyLoggerService.Value;

	public ISettingsService SettingsService
		=> _lazySettingsService.Value;
}
