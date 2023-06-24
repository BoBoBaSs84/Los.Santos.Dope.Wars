using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Application.Models.Missions;
using LSDW.Abstractions.Enumerators;
using LSDW.Application.Factories;
using LSDW.Base.Tests.Helpers;
using Moq;

namespace LSDW.Application.Tests.Factories;

[TestClass]
public class ApplicationFactoryTests
{
	[TestMethod]
	public void CreateTraffickingMissionTest()
	{
		Mock<IServiceManager> serviceManagerMock = MockHelper.GetServiceManager();
		Mock<IProviderManager> providerManagerMock = MockHelper.GetProviderManager();
		ITrafficking? trafficking;

		trafficking = ApplicationFactory.CreateTraffickingMission(serviceManagerMock.Object, providerManagerMock.Object);

		Assert.IsNotNull(trafficking);
		Assert.AreEqual("Trafficking", trafficking.Name);
		Assert.AreEqual(MissionStatusType.Stopped, trafficking.Status);
	}
}