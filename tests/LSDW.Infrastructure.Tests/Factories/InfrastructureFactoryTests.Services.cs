using LSDW.Abstractions.Infrastructure.Services;
using static LSDW.Infrastructure.Factories.InfrastructureFactory;

namespace LSDW.Infrastructure.Tests.Factories;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public partial class InfrastructureFactoryTests
{
	[TestMethod]
	public void CreateLoggerServiceTest()
	{
		ILoggerService? loggerService;

		loggerService = CreateLoggerService();

		Assert.IsNotNull(loggerService);
	}

	[TestMethod]
	public void CreateGameStateServiceTest()
	{
		ILoggerService loggerService = CreateLoggerService();
		IGameStateService? gameStateService;

		gameStateService = CreateGameStateService(loggerService);

		Assert.IsNotNull(gameStateService);
	}

	[TestMethod]
	public void CreateGameStateServiceWithoutLoggerTest()
	{
		IGameStateService? gameStateService;

		gameStateService = CreateGameStateService();

		Assert.IsNotNull(gameStateService);
	}

	[TestMethod]
	public void CreateSettingsServiceTest()
	{
		ISettingsService? settingsService;

		settingsService = CreateSettingsService();

		Assert.IsNotNull(settingsService);
	}
}