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

	/// <summary>
	/// Initializes a instance of the service manager class.
	/// </summary>
	public ServiceManager()
	{
		_lazyLoggerService = new Lazy<ILoggerService>(InfrastructureFactory.GetLoggerService);
		_lazyStateService = new Lazy<IStateService>(InfrastructureFactory.GetStateService);
		_lazySettingsService = new Lazy<ISettingsService>(InfrastructureFactory.GetSettingsService);
	}

	/// <summary>
	/// The service manager singleton instance.
	/// </summary>
	public readonly static ServiceManager Instance = new();

	public IStateService StateService
		=> _lazyStateService.Value;

	public ILoggerService LoggerService
		=> _lazyLoggerService.Value;

	public ISettingsService SettingsService
		=> _lazySettingsService.Value;
}
