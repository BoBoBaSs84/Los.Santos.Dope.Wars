using LSDW.Abstractions.Infrastructure.Services;
using static LSDW.Infrastructure.Factories.InfrastructureFactory;

namespace LSDW.Infrastructure.Tests.Factories;

[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public partial class InfrastructureFactoryTests
{
	[TestMethod]
	public void CreateLoggerServiceTest()
	{
		ILoggerService? loggerService;

		loggerService = GetLoggerService();

		Assert.IsNotNull(loggerService);
	}

	[TestMethod]
	public void CreateGameStateServiceTest()
	{
		IStateService? gameStateService;

		gameStateService = GetStateService();

		Assert.IsNotNull(gameStateService);
	}

	[TestMethod]
	public void CreateGameStateServiceWithoutLoggerTest()
	{
		IStateService? gameStateService;

		gameStateService = GetStateService();

		Assert.IsNotNull(gameStateService);
	}

	[TestMethod]
	public void CreateSettingsServiceTest()
	{
		ISettingsService? settingsService;

		settingsService = GetSettingsService();

		Assert.IsNotNull(settingsService);
	}
}