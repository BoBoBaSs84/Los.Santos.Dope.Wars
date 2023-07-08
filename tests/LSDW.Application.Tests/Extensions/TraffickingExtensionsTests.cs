using GTA.Math;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Application.Models.Missions;
using LSDW.Abstractions.Domain.Models;
using LSDW.Application.Extensions;
using LSDW.Application.Factories;
using LSDW.Base.Tests.Helpers;
using LSDW.Domain.Factories;
using Moq;

namespace LSDW.Application.Tests.Extensions;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "Unit tests.")]
public class TraffickingExtensionsTests
{
	private readonly Mock<IServiceManager> _serviceManagerMock;
	private readonly Mock<IProviderManager> _providerManagerMock;
	private readonly ITrafficking _trafficking;

	public TraffickingExtensionsTests()
	{
		_serviceManagerMock = MockHelper.GetServiceManager();
		_providerManagerMock = MockHelper.GetProviderManager();
		_trafficking = ApplicationFactory.CreateTraffickingMission(_serviceManagerMock.Object, _providerManagerMock.Object);

	}

	[TestMethod]
	public void ChangeDealerPricesNonDiscoveredTest()
	{
		IDealer dealer = DomainFactory.CreateDealer(Vector3.Zero);
		_trafficking.ChangeDealerPrices(_providerManagerMock.Object, _serviceManagerMock.Object);

		Assert.AreEqual(default, dealer.Inventory.Sum(x => x.Price));
		Assert.AreEqual(default, dealer.Inventory.Money);
		Assert.AreEqual(default, dealer.Inventory.TotalQuantity);
	}

	[TestMethod]
	public void ChangeDealerPricesTest()
	{
		IDealer dealer = DomainFactory.CreateDealer(Vector3.Zero);
		_trafficking.ChangeDealerPrices(_providerManagerMock.Object, _serviceManagerMock.Object);

		Assert.AreNotEqual(default, dealer.Inventory.Sum(x => x.Price));
		Assert.AreEqual(default, dealer.Inventory.Money);
		Assert.AreEqual(default, dealer.Inventory.TotalQuantity);
	}

	[TestMethod]
	public void ChangeDealerInventoriesNonDiscoveredTest()
	{
		IDealer dealer = DomainFactory.CreateDealer(Vector3.Zero);
		_trafficking.ChangeDealerPrices(_providerManagerMock.Object, _serviceManagerMock.Object);

		Assert.AreEqual(default, dealer.Inventory.Money);
		Assert.AreEqual(default, dealer.Inventory.TotalQuantity);
		Assert.AreEqual(default, dealer.Inventory.TotalValue);
	}

	[TestMethod]
	public void ChangeDealerInventoriesTest()
	{
		IDealer dealer = DomainFactory.CreateDealer(Vector3.Zero);
		_trafficking.ChangeDealerPrices(_providerManagerMock.Object, _serviceManagerMock.Object);

		Assert.AreNotEqual(default, dealer.Inventory.Money);
		Assert.AreNotEqual(default, dealer.Inventory.TotalQuantity);
		Assert.AreNotEqual(default, dealer.Inventory.TotalValue);
	}
}