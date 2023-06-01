using LSDW.Core.Interfaces.Models;
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
		IPlayer player = Core.Factories.ModelFactory.CreatePlayer();

		IMission? mission;

		mission = MissionFactory.CreateTraffickingMission(timeService, logger, player);

		Assert.IsNotNull(mission);
	}
}