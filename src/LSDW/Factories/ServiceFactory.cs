using LSDW.Classes.Services;
using LSDW.Interfaces.Services;

namespace LSDW.Factories;

/// <summary>
/// The service factory class.
/// </summary>
public static class ServiceFactory
{
	/// <summary>
	/// Creates a new date time service instance.
	/// </summary>
	public static IDateTimeService CreateDateTimeService()
		=> new DateTimeService();

	/// <summary>
	/// Creates a new settings service instance.
	/// </summary>
	public static ISettingsService CreateSettingsService()
		=> new SettingsService();

	/// <summary>
	/// Creates a new logger service instance.
	/// </summary>
	public static ILoggerService CreateLoggerService()
		=> new LoggerService();

	/// <summary>
	/// Creates a new game state service instance.
	/// </summary>
	public static IGameStateService CreateGameStateService()
		=> new GameStateService(CreateLoggerService());

	/// <summary>
	/// Creates a new game state service instance.
	/// </summary>
	/// <param name="logger">The logger service instance to use.</param>
	/// <param name="player">The player instance to save.</param>
	/// <param name="dealers">The dealer instance colection to save.</param>
	public static IGameStateService CreateGameStateService(ILoggerService logger)
		=> new GameStateService(logger);
}
