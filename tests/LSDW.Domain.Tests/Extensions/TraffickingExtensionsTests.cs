using GTA.Math;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Missions;
using LSDW.Abstractions.Domain.Models;
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
	private readonly Mock<ICollection<IDealer>> _dealersMock = new();
	private readonly Mock<IPlayer> _playerMock = new();

	[TestMethod]
	public void ChangeDealerPricesNonDiscoveredTest()
	{
		ITrafficking trafficking = DomainFactory.CreateTraffickingMission(_serviceManagerMock.Object, _providerManagerMock.Object);

		trafficking.ChangeDealerPrices(_dealersMock.Object, _playerMock.Object);
	}

	[TestMethod]
	public void ChangeDealerPricesTest()
	{
		ITrafficking trafficking = DomainFactory.CreateTraffickingMission(_serviceManagerMock.Object, _providerManagerMock.Object);

		trafficking.ChangeDealerPrices(_dealersMock.Object, _playerMock.Object);
	}

	[TestMethod]
	public void ChangeDealerInventoriesNonDiscoveredTest()
	{
		ITrafficking trafficking = DomainFactory.CreateTraffickingMission(_serviceManagerMock.Object, _providerManagerMock.Object);

		trafficking.ChangeDealerInventories(_dealersMock.Object, _playerMock.Object);
	}

	[TestMethod]
	public void ChangeDealerInventoriesTest()
	{
		ITrafficking trafficking = DomainFactory.CreateTraffickingMission(_serviceManagerMock.Object, _providerManagerMock.Object);

		trafficking.ChangeDealerInventories(_dealersMock.Object, _playerMock.Object);
	}
}