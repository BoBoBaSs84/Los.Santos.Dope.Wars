using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;
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

		settingsService = ServiceFactory.CreateSettingsService();

		Assert.IsNotNull(settingsService);
	}

	[TestMethod]
	public void CreateLoggerServiceTest()
	{
		ILoggerService? loggerService;

		loggerService = ServiceFactory.CreateLoggerService();

		Assert.IsNotNull(loggerService);
	}

	[TestMethod()]
	public void CreateGetGameStateServiceTest()
	{
		IGameStateService? gameStateService;
		IPlayer player = PlayerFactory.CreatePlayer();
		IEnumerable<IDealer> dealers = new List<IDealer>();

		gameStateService = ServiceFactory.CreateGameStateService(player, dealers);

		Assert.IsNotNull(gameStateService);
	}

	[TestMethod()]
	public void CreateGetGameStateServiceWithoutLoggerTest()
	{
		IGameStateService? gameStateService;
		ILoggerService logger = ServiceFactory.CreateLoggerService();
		IPlayer player = PlayerFactory.CreatePlayer();
		IEnumerable<IDealer> dealers = new List<IDealer>();

		gameStateService = ServiceFactory.CreateGameStateService(logger, player, dealers);

		Assert.IsNotNull(gameStateService);
	}
}