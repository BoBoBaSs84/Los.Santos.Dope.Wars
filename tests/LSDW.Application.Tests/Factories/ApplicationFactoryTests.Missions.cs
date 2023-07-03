using LSDW.Abstractions.Application.Models.Missions;
using LSDW.Abstractions.Enumerators;
using LSDW.Application.Factories;

namespace LSDW.Application.Tests.Factories;

public partial class ApplicationFactoryTests
{
	[TestMethod]
	public void CreateTraffickingMissionTest()
	{
		ITrafficking? trafficking;

		trafficking = ApplicationFactory.CreateTraffickingMission(_serviceManagerMock.Object, _providerManagerMock.Object);

		Assert.IsNotNull(trafficking);
		Assert.IsNotNull(trafficking.LoggerService);
		Assert.IsNotNull(trafficking.PlayerProvider);
		Assert.IsNotNull(trafficking.NotificationProvider);
		Assert.IsNotNull(trafficking.WorldProvider);
		Assert.AreEqual("Trafficking", trafficking.Name);
		Assert.AreEqual(MissionStatusType.STOPPED, trafficking.Status);
	}
}
