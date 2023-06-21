using GTA.Math;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Missions;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
using Moq;

namespace LSDW.Domain.Tests.Extensions;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "Unit tests.")]
public class TraffickingExtensionsTests
{
	private readonly Vector3 _zeroVector = new(0, 0, 0);
	private readonly Mock<IServiceManager> _serviceManagerMock = new();
	private readonly Mock<IProviderManager> _providerManagerMock = new();

	[TestMethod]
	public void ChangeDealerPricesNonDiscoveredTest()
	{
		ITrafficking trafficking = DomainFactory.CreateTraffickingMission(_serviceManagerMock.Object, _providerManagerMock.Object);

		trafficking.ChangeDealerPrices();
	}

	[TestMethod]
	public void ChangeDealerPricesTest()
	{
		ITrafficking trafficking = DomainFactory.CreateTraffickingMission(_serviceManagerMock.Object, _providerManagerMock.Object);

		trafficking.ChangeDealerPrices();
	}

	[TestMethod]
	public void ChangeDealerInventoriesNonDiscoveredTest()
	{
		ITrafficking trafficking = DomainFactory.CreateTraffickingMission(_serviceManagerMock.Object, _providerManagerMock.Object);

		trafficking.ChangeDealerInventories();
	}

	[TestMethod]
	public void ChangeDealerInventoriesTest()
	{
		ITrafficking trafficking = DomainFactory.CreateTraffickingMission(_serviceManagerMock.Object, _providerManagerMock.Object);

		trafficking.ChangeDealerInventories();
	}
}