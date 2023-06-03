using LSDW.Factories;
using LSDW.Interfaces.Missions;
using LSDW.Interfaces.Services;
using LSDW.Tests.UnitTestHelpers;

namespace LSDW.Tests.Factories;

[TestClass]
public class MissionFactoryTests
{
	[TestMethod]
	public void CreateTraffickingMissionTest()
	{
		IDateTimeService timeService = new TestDateTimeService();
		ILoggerService logger = ServiceFactory.CreateLoggerService();
		IGameStateService stateService = ServiceFactory.CreateGameStateService();

		IMission? mission;

		mission = MissionFactory.CreateTraffickingMission(timeService, logger, stateService);

		Assert.IsNotNull(mission);
	}
}