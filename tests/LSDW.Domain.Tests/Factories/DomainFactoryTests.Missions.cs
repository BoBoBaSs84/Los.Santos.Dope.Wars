using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Missions;
using LSDW.Domain.Factories;
using Moq;

namespace LSDW.Domain.Tests.Factories;

public partial class DomainFactoryTests
{
	[TestMethod]
	public void CreateTraffickingMissionTest()
	{
		Mock<IServiceManager> serviceManagerMock = new();
		Mock<IProviderManager> providerManagerMock = new();
		ITrafficking? trafficking;

		trafficking = DomainFactory.CreateTraffickingMission(serviceManagerMock.Object, providerManagerMock.Object);

		Assert.IsNotNull(trafficking);
	}
}
