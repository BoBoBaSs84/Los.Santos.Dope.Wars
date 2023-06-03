using LSDW.Abstractions.Interfaces.Infrastructure;
using LSDW.Infrastructure.Services;

namespace LSDW.Infrastructure.Factories;

/// <summary>
/// The infrastructure factory class.
/// </summary>
public static class InfrastructureFactory
{
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
	public static IGameStateService CreateGameStateService(ILoggerService logger)
		=> new GameStateService(logger);
}
