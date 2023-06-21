using LSDW.Abstractions.Domain.Providers;
using LSDW.Abstractions.Infrastructure.Services;

namespace LSDW.Abstractions.Application.Managers;

/// <summary>
/// The service manager interface.
/// </summary>
public interface IServiceManager
{
	/// <summary>
	/// The game state service.
	/// </summary>
	IGameStateService StateService { get; }
	/// <summary>
	/// The logger service.
	/// </summary>
	ILoggerService LoggerService { get; }
	/// <summary>
	/// The settings service.
	/// </summary>
	ISettingsService SettingsService { get; }
}
