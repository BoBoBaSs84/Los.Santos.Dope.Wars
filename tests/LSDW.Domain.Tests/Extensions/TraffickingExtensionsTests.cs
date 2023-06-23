using GTA.Math;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Missions;
using LSDW.Abstractions.Domain.Models;
using LSDW.Base.Tests.Helpers;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
using Moq;

namespace LSDW.Domain.Tests.Extensions;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "Unit tests.")]
public class TraffickingExtensionsTests
{
	private readonly Vector3 _zeroVector = new(0, 0, 0);
	private readonly Mock<IServiceManager> _serviceManagerMock = MockHelper.GetServiceManager();
	private readonly Mock<IProviderManager> _providerManagerMock = MockHelper.GetProviderManager();
	private readonly ICollection<IDealer> _dealers = DomainFactory.CreateDealers();
	private readonly IPlayer _player = DomainFactory.CreatePlayer();

	[TestMethod]
	public void ChangeDealerPricesNonDiscoveredTest()
	{
		ITrafficking trafficking = DomainFactory.CreateTraffickingMission(_serviceManagerMock.Object, _providerManagerMock.Object);
		_dealers.Add(DomainFactory.CreateDealer(_zeroVector));

		trafficking.ChangeDealerPrices(_dealers, _player);

		Assert.AreEqual(0, _dealers.First().Inventory.Sum(x => x.CurrentPrice));
	}

	[TestMethod]
	public void ChangeDealerPricesTest()
	{
		ITrafficking trafficking = DomainFactory.CreateTraffickingMission(_serviceManagerMock.Object, _providerManagerMock.Object);
		IDealer dealer = DomainFactory.CreateDealer(_zeroVector);
		dealer.SetDiscovered(true);
		_dealers.Add(dealer);

		trafficking.ChangeDealerPrices(_dealers, _player);

		Assert.AreNotEqual(0, _dealers.First().Inventory.Sum(x => x.CurrentPrice));
	}

	[TestMethod]
	public void ChangeDealerInventoriesNonDiscoveredTest()
	{
		ITrafficking trafficking = DomainFactory.CreateTraffickingMission(_serviceManagerMock.Object, _providerManagerMock.Object);
		_dealers.Add(DomainFactory.CreateDealer(_zeroVector));

		trafficking.ChangeDealerInventories(_dealers, _player);

		Assert.AreEqual(0, _dealers.First().Inventory.Sum(x => x.CurrentPrice));
		Assert.AreEqual(0, _dealers.First().Inventory.Sum(x => x.Quantity));
	}

	[TestMethod]
	public void ChangeDealerInventoriesTest()
	{
		ITrafficking trafficking = DomainFactory.CreateTraffickingMission(_serviceManagerMock.Object, _providerManagerMock.Object);
		IDealer dealer = DomainFactory.CreateDealer(_zeroVector);
		dealer.SetDiscovered(true);
		_dealers.Add(dealer);

		trafficking.ChangeDealerInventories(_dealers, _player);

		Assert.AreNotEqual(0, _dealers.First().Inventory.Sum(x => x.CurrentPrice));
		Assert.AreNotEqual(0, _dealers.First().Inventory.Sum(x => x.Quantity));
	}
}