using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Application.Managers;
using LSDW.Base.Tests.Helpers;
using LSDW.Domain.Factories;
using Moq;
using LSDW.Abstractions.Application.Models.Missions;

namespace LSDW.Domain.Tests.Factories;

public partial class DomainFactoryTests
{
	[TestMethod]
	public void CreateTraffickingMissionTest()
	{
		Mock<IServiceManager> serviceManagerMock = MockHelper.GetServiceManager();
		Mock<IProviderManager> providerManagerMock = MockHelper.GetProviderManager();
		ITrafficking? trafficking;

		trafficking = DomainFactory.CreateTraffickingMission(serviceManagerMock.Object, providerManagerMock.Object);

		Assert.IsNotNull(trafficking);
		Assert.AreEqual("Trafficking", trafficking.Name);
		Assert.AreEqual(MissionStatusType.Stopped, trafficking.Status);
	}
}
