using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Application.Models.Missions;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Abstractions.Infrastructure.Services;
using Moq;

namespace LSDW.Base.Tests.Helpers;

[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public static class MockHelper
{
	public static Mock<INotificationProvider> GetNotificationProvider()
		=> new();

	public static Mock<ITimeProvider> GetTimeProvider()
	{
		Mock<ITimeProvider> mock = new();
		mock.Setup(x => x.Now).Returns(DateTime.MinValue.AddYears(100));
		mock.Setup(x => x.TimeOfDay).Returns(DateTime.MinValue.AddYears(100).TimeOfDay);
		return mock;
	}

	public static Mock<ILocationProvider> GetLocationProvider()
		=> new();

	public static Mock<IPlayerProvider> GetPlayerProvider()
		=> new();

	public static Mock<IProviderManager> GetProviderManager()
	{
		Mock<IProviderManager> mock = new();
		mock.Setup(x => x.LocationProvider).Returns(GetLocationProvider().Object);
		mock.Setup(x => x.TimeProvider).Returns(GetTimeProvider().Object);
		mock.Setup(x => x.NotificationProvider).Returns(GetNotificationProvider().Object);
		mock.Setup(x => x.PlayerProvider).Returns(GetPlayerProvider().Object);
		return mock;
	}

	public static Mock<IServiceManager> GetServiceManager()
	{
		Mock<IServiceManager> mock = new();
		mock.Setup(x => x.SettingsService).Returns(GetSettingsService().Object);
		mock.Setup(x => x.StateService).Returns(GetStateService().Object);
		mock.Setup(x => x.LoggerService).Returns(GetLoggerService().Object);
		return mock;
	}

	public static Mock<ISettingsService> GetSettingsService()
		=> new();

	public static Mock<ILoggerService> GetLoggerService()
		=> new();

	public static Mock<IStateService> GetStateService()
	{
		Mock<IStateService> mock = new();
		mock.Setup(x => x.Player).Returns(GetPlayer().Object);
		return mock;
	}

	public static Mock<ITrafficking> GetTrafficking()
	{
		Mock<ITrafficking> mock = new();
		mock.Setup(x => x.LocationProvider).Returns(GetLocationProvider().Object);
		mock.Setup(x => x.LoggerService).Returns(GetLoggerService().Object);
		mock.Setup(x => x.NotificationProvider).Returns(GetNotificationProvider().Object);
		mock.Setup(x => x.TimeProvider).Returns(GetTimeProvider().Object);
		return mock;
	}

	public static Mock<IInventory> GetInventory()
		=> new();

	public static Mock<IPlayer> GetPlayer()
		=> new();

	public static Mock<IDealer> GetDealer()
		=> new();
}
