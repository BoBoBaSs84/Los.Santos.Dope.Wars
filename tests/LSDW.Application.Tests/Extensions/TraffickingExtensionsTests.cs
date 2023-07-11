using GTA.Math;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Application.Models.Missions;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Infrastructure.Services;
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
		ICollection<IDealer> dealers = new HashSet<IDealer>() { DomainFactory.CreateDealer(Vector3.Zero) };
		Mock<IStateService> stateServiceMock = new();
		stateServiceMock.Setup(x => x.Dealers).Returns(dealers);
		_serviceManagerMock.Setup(x => x.StateService).Returns(stateServiceMock.Object);
		ITrafficking trafficking = ApplicationFactory.CreateTraffickingMission(_serviceManagerMock.Object, _providerManagerMock.Object);

		trafficking.ChangeDealerPrices();

		Assert.AreEqual(0, dealers.First().Inventory.Sum(x => x.Price));
		Assert.AreEqual(0, dealers.First().Inventory.Money);
		Assert.AreEqual(0, dealers.First().Inventory.TotalQuantity);
	}

	[TestMethod]
	public void ChangeDealerPricesTest()
	{
		IPlayer player = DomainFactory.CreatePlayer();
		ICollection<IDealer> dealers = new HashSet<IDealer>() { DomainFactory.CreateDealer(Vector3.Zero) };
		dealers.First().Discovered = true;
		Mock<IStateService> stateServiceMock = new();
		stateServiceMock.Setup(x => x.Dealers).Returns(dealers);
		stateServiceMock.Setup(x => x.Player).Returns(player);
		_serviceManagerMock.Setup(x => x.StateService).Returns(stateServiceMock.Object);
		ITrafficking trafficking = ApplicationFactory.CreateTraffickingMission(_serviceManagerMock.Object, _providerManagerMock.Object);

		trafficking.ChangeDealerPrices();

		Assert.AreNotEqual(0, dealers.First().Inventory.Sum(x => x.Price));
		Assert.AreEqual(0, dealers.First().Inventory.Money);
		Assert.AreEqual(0, dealers.First().Inventory.TotalQuantity);
	}

	[TestMethod]
	public void ChangeDealerInventoriesNonDiscoveredTest()
	{
		ICollection<IDealer> dealers = new HashSet<IDealer>() { DomainFactory.CreateDealer(Vector3.Zero) };
		Mock<IStateService> stateServiceMock = new();
		stateServiceMock.Setup(x => x.Dealers).Returns(dealers);
		_serviceManagerMock.Setup(x => x.StateService).Returns(stateServiceMock.Object);
		ITrafficking trafficking = ApplicationFactory.CreateTraffickingMission(_serviceManagerMock.Object, _providerManagerMock.Object);

		trafficking.ChangeDealerInventories();

		Assert.AreEqual(default, dealers.First().Inventory.Money);
		Assert.AreEqual(default, dealers.First().Inventory.TotalQuantity);
		Assert.AreEqual(default, dealers.First().Inventory.TotalValue);
	}

	[TestMethod]
	public void ChangeDealerInventoriesTest()
	{
		IPlayer player = DomainFactory.CreatePlayer();
		ICollection<IDealer> dealers = new HashSet<IDealer>() { DomainFactory.CreateDealer(Vector3.Zero) };
		dealers.First().Discovered = true;
		Mock<IStateService> stateServiceMock = new();
		stateServiceMock.Setup(x => x.Dealers).Returns(dealers);
		stateServiceMock.Setup(x => x.Player).Returns(player);
		_serviceManagerMock.Setup(x => x.StateService).Returns(stateServiceMock.Object);
		ITrafficking trafficking = ApplicationFactory.CreateTraffickingMission(_serviceManagerMock.Object, _providerManagerMock.Object);

		trafficking.ChangeDealerInventories();

		Assert.AreNotEqual(default, dealers.First().Inventory.Money);
		Assert.AreNotEqual(default, dealers.First().Inventory.TotalQuantity);
		Assert.AreNotEqual(default, dealers.First().Inventory.TotalValue);
	}
}