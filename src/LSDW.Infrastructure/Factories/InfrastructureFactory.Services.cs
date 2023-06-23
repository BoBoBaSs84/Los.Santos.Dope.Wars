using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Infrastructure.Services;

namespace LSDW.Infrastructure.Factories;

public static partial class InfrastructureFactory
{
	/// <summary>
	/// Creates a new logger service instance.
	/// </summary>
	public static ILoggerService CreateLoggerService()
		=> new LoggerService();

	/// <summary>
	/// Creates a new game state service instance.
	/// </summary>
	/// <param name="logger">The logger service instance to use.</param>
	public static IStateService CreateGameStateService(ILoggerService logger)
		=> new StateService(logger);

	/// <summary>
	/// Creates a new settings service instance.
	/// </summary>
	public static ISettingsService CreateSettingsService()
		=> new SettingsService();
}
