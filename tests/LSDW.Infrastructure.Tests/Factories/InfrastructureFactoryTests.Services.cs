using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Infrastructure.Factories;

namespace LSDW.Infrastructure.Tests.Factories;

[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public partial class InfrastructureFactoryTests
{
	[TestMethod]
	public void CreateLoggerServiceTest()
	{
		ILoggerService? loggerService;

		loggerService = InfrastructureFactory.GetLoggerService();

		Assert.IsNotNull(loggerService);
	}

	[TestMethod]
	public void CreateGameStateServiceTest()
	{
		IStateService? gameStateService;

		gameStateService = InfrastructureFactory.GetStateService();

		Assert.IsNotNull(gameStateService);
	}

	[TestMethod]
	public void CreateGameStateServiceWithoutLoggerTest()
	{
		IStateService? gameStateService;

		gameStateService = InfrastructureFactory.GetStateService();

		Assert.IsNotNull(gameStateService);
	}

	[TestMethod]
	public void CreateSettingsServiceTest()
	{
		ISettingsService? settingsService;

		settingsService = InfrastructureFactory.GetSettingsService();

		Assert.IsNotNull(settingsService);
		Assert.IsNotNull(settingsService.Dealer);
		Assert.IsNotNull(settingsService.Market);
		Assert.IsNotNull(settingsService.Player);
		Assert.IsNotNull(settingsService.Trafficking);
	}
}