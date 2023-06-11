using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Infrastructure.Factories;

namespace LSDW.Infrastructure.Tests.Factories;

[TestClass]
public class InfrastructureFactoryTests
{
	[TestMethod]
	public void CreateLoggerServiceTest()
	{
		ILoggerService? loggerService;

		loggerService = InfrastructureFactory.CreateLoggerService();

		Assert.IsNotNull(loggerService);
	}

	[TestMethod]
	public void CreateGameStateServiceTest()
	{
		ILoggerService loggerService = InfrastructureFactory.CreateLoggerService();
		IGameStateService? gameStateService;

		gameStateService = InfrastructureFactory.CreateGameStateService(loggerService);

		Assert.IsNotNull(gameStateService);
	}

	[TestMethod]
	public void CreateGameStateServiceWithoutLoggerTest()
	{
		IGameStateService? gameStateService;

		gameStateService = InfrastructureFactory.CreateGameStateService();

		Assert.IsNotNull(gameStateService);
	}

	[TestMethod]
	public void CreateSettingsServiceTest()
	{
		ISettingsService? settingsService;

		settingsService = InfrastructureFactory.CreateSettingsService();

		Assert.IsNotNull(settingsService);
	}
}