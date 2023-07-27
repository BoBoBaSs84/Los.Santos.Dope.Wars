using GTA.Math;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Application.Models.Missions;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Application.Extensions;
using LSDW.Application.Factories;
using LSDW.Application.Properties;
using LSDW.Base.Tests.Helpers;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
using Moq;

namespace LSDW.Application.Tests.Extensions;

[TestClass, ExcludeFromCodeCoverage]
[SuppressMessage("Style", "IDE0058", Justification = "Unit tests.")]
public class TraffickingExtensionsTests
{
	private readonly Mock<IServiceManager> _serviceManagerMock;
	private readonly Mock<IProviderManager> _providerManagerMock;

	public TraffickingExtensionsTests()
	{
		_serviceManagerMock = MockHelper.GetServiceManager();
		_providerManagerMock = MockHelper.GetProviderManager();
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

	[TestMethod]
	public void LetDealerFleeAlreadyFledTest()
	{
		Mock<IDealer> dealerMock = MockHelper.GetDealer();
		dealerMock.Setup(x => x.CurrentTask).Returns(TaskType.FLEE);
		ITrafficking trafficking = ApplicationFactory.CreateTraffickingMission(_serviceManagerMock.Object, _providerManagerMock.Object);

		trafficking.LetDealerFlee(dealerMock.Object);

		dealerMock.VerifyAll();
	}

	[TestMethod]
	public void LetDealerFleeTest1()
	{
		Mock<IDealer> dealerMock = MockHelper.GetDealer();
		dealerMock.Setup(x => x.Created).Returns(true);
		Mock<INotificationProvider> notificationProviderMock = MockHelper.GetNotificationProvider();
		_providerManagerMock.Setup(x => x.NotificationProvider).Returns(notificationProviderMock.Object);
		ITrafficking trafficking = ApplicationFactory.CreateTraffickingMission(_serviceManagerMock.Object, _providerManagerMock.Object);

		trafficking.LetDealerFlee(dealerMock.Object);

		dealerMock.Verify(x => x.Flee(), Times.Once);
		notificationProviderMock.Verify(
			x => x.Show(
				Resources.Trafficking_Notification_Bust_Subject,
				Resources.Trafficking_Notification_Bust_Message.FormatInvariant(dealerMock.Object.Name),
				false));
	}

	[TestMethod]
	public void CloseDealerTest()
	{
		Mock<IDealer> dealerMock = MockHelper.GetDealer();
		Mock<INotificationProvider> notificationProviderMock = MockHelper.GetNotificationProvider();
		_providerManagerMock.Setup(x => x.NotificationProvider).Returns(notificationProviderMock.Object);
		ITrafficking trafficking = ApplicationFactory.CreateTraffickingMission(_serviceManagerMock.Object, _providerManagerMock.Object);

		trafficking.CloseDealer(dealerMock.Object);

		dealerMock.VerifyAll();
	}

	[TestMethod]
	public void DiscoverDealerTest()
	{
		Mock<IInventory> inventoryMock = MockHelper.GetInventory();
		Mock<IDealer> dealerMock = MockHelper.GetDealer();
		dealerMock.Setup(x => x.Inventory).Returns(inventoryMock.Object);
		Mock<INotificationProvider> notificationProviderMock = MockHelper.GetNotificationProvider();
		_providerManagerMock.Setup(x => x.NotificationProvider).Returns(notificationProviderMock.Object);
		ITrafficking trafficking = ApplicationFactory.CreateTraffickingMission(_serviceManagerMock.Object, _providerManagerMock.Object);

		trafficking.DiscoverDealer(dealerMock.Object);

		dealerMock.VerifyAll();
	}

	[TestMethod]
	public void TrackDealersPositionIsZeroTest()
	{
		ITrafficking trafficking = ApplicationFactory.CreateTraffickingMission(_serviceManagerMock.Object, _providerManagerMock.Object);

		trafficking.TrackDealers();
	}
}
