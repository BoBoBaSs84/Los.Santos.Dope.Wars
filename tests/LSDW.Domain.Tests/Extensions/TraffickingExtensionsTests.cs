using LSDW.Abstractions.Domain.Missions;
using LSDW.Abstractions.Application.Providers;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Services;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Base.Tests.Helpers;
using LSDW.Domain.Factories;
using Moq;
using LSDW.Domain.Extensions;
using GTA.Math;

namespace LSDW.Domain.Tests.Extensions;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "Unit tests.")]
public class TraffickingExtensionsTests
{
	private readonly Vector3 _zeroVector = new(0, 0, 0);
	private readonly Mock<IPlayer> _player = MockHelper.GetPlayerMock();
	private readonly Mock<ITimeProvider> _timeProvider = MockHelper.GetTimeProviderMock();
	private readonly Mock<ILoggerService> _loggerService = MockHelper.GetLoggerServiceMock();
	private readonly Mock<INotificationService> _notificationService = MockHelper.GetNotificationServiceMock();	

	[TestMethod]
	public void ChangeDealerPricesNonDiscoveredTest()
	{
		ICollection<IDealer> dealers = DomainFactory.CreateDealers();
		dealers.Add(DomainFactory.CreateDealer(_zeroVector));

		ITrafficking trafficking = DomainFactory.CreateTraffickingMission(_player.Object, dealers, _timeProvider.Object, _loggerService.Object, _notificationService.Object);

		trafficking.ChangeDealerPrices(dealers, _player.Object);
	}

	[TestMethod]
	public void ChangeDealerPricesTest()
	{
		_timeProvider.Setup(x=>x.Now).Returns(DateTime.Now);
		IDealer dealer = DomainFactory.CreateDealer(_zeroVector);
		dealer.SetDiscovered(true);
		ICollection<IDealer> dealers = DomainFactory.CreateDealers();
		dealers.Add(DomainFactory.CreateDealer(_zeroVector));
		dealers.Add(dealer);

		ITrafficking trafficking = DomainFactory.CreateTraffickingMission(_player.Object, dealers, _timeProvider.Object, _loggerService.Object, _notificationService.Object);

		trafficking.ChangeDealerPrices(dealers, _player.Object);
	}

	[TestMethod]
	public void ChangeDealerInventoriesNonDiscoveredTest()
	{
		ICollection<IDealer> dealers = DomainFactory.CreateDealers();
		dealers.Add(DomainFactory.CreateDealer(_zeroVector));

		ITrafficking trafficking = DomainFactory.CreateTraffickingMission(_player.Object, dealers, _timeProvider.Object, _loggerService.Object, _notificationService.Object);

		trafficking.ChangeDealerInventories(dealers, _player.Object);
	}

	[TestMethod]
	public void ChangeDealerInventoriesTest()
	{
		_timeProvider.Setup(x => x.Now).Returns(DateTime.Now);
		IDealer dealer = DomainFactory.CreateDealer(_zeroVector);
		dealer.SetDiscovered(true);
		ICollection<IDealer> dealers = DomainFactory.CreateDealers();
		dealers.Add(DomainFactory.CreateDealer(_zeroVector));
		dealers.Add(dealer);

		ITrafficking trafficking = DomainFactory.CreateTraffickingMission(_player.Object, dealers, _timeProvider.Object, _loggerService.Object, _notificationService.Object);

		trafficking.ChangeDealerInventories(dealers, _player.Object);
	}
}