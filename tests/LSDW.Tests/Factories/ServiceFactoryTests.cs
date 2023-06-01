using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Models;
using LSDW.Factories;
using LSDW.Interfaces.Actors;
using LSDW.Interfaces.Services;

namespace LSDW.Tests.Factories;

[TestClass]
public class ServiceFactoryTests
{
	[TestMethod]
	public void CreateSettingsServiceTest()
	{
		ISettingsService? settingsService;

		settingsService = LSDW.Factories.ServiceFactory.CreateSettingsService();

		Assert.IsNotNull(settingsService);
	}

	[TestMethod]
	public void CreateLoggerServiceTest()
	{
		ILoggerService? loggerService;

		loggerService = LSDW.Factories.ServiceFactory.CreateLoggerService();

		Assert.IsNotNull(loggerService);
	}

	[TestMethod()]
	public void CreateGetGameStateServiceTest()
	{
		IGameStateService? gameStateService;
		IPlayer player = ModelFactory.CreatePlayer();
		IEnumerable<IDealer> dealers = new List<IDealer>();

		gameStateService = LSDW.Factories.ServiceFactory.CreateGameStateService(player, dealers);

		Assert.IsNotNull(gameStateService);
	}

	[TestMethod()]
	public void CreateGetGameStateServiceWithoutLoggerTest()
	{
		IGameStateService? gameStateService;
		ILoggerService logger = LSDW.Factories.ServiceFactory.CreateLoggerService();
		IPlayer player = ModelFactory.CreatePlayer();
		IEnumerable<IDealer> dealers = new List<IDealer>();

		gameStateService = LSDW.Factories.ServiceFactory.CreateGameStateService(logger, player, dealers);

		Assert.IsNotNull(gameStateService);
	}
}