using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Application.Models.Missions;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Infrastructure.Services;
using Moq;

namespace LSDW.Base.Tests.Helpers;

[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public static class MockHelper
{
	public static Mock<IAudioProvider> GetAudioProvider()
	{
		Mock<IAudioProvider> mock = new(MockBehavior.Loose);
		mock.SetupAllProperties();
		return mock;
	}

	public static Mock<INotificationProvider> GetNotificationProvider()
	{
		Mock<INotificationProvider> mock = new(MockBehavior.Loose);
		mock.SetupAllProperties();
		return mock;
	}

	public static Mock<IWorldProvider> GetWorldProvider()
	{
		Mock<IWorldProvider> mock = new(MockBehavior.Loose);
		mock.SetupAllProperties();
		mock.Object.Now = DateTime.MinValue.AddYears(100);
		mock.Object.TimeOfDay = DateTime.MinValue.AddYears(100).TimeOfDay;
		return mock;
	}

	public static Mock<IRandomProvider> GetRandomProvider()
	{
		Mock<IRandomProvider> mock = new(MockBehavior.Loose);
		mock.SetupAllProperties();
		return mock;
	}

	public static Mock<IPlayerProvider> GetPlayerProvider()
	{
		Mock<IPlayerProvider> mock = new();
		mock.SetupAllProperties();
		return mock;
	}

	public static Mock<IProviderManager> GetProviderManager()
	{
		Mock<IProviderManager> mock = new(MockBehavior.Loose);
		mock.Setup(x => x.AudioProvider).Returns(GetAudioProvider().Object);
		mock.Setup(x => x.NotificationProvider).Returns(GetNotificationProvider().Object);
		mock.Setup(x => x.PlayerProvider).Returns(GetPlayerProvider().Object);
		mock.Setup(x => x.RandomProvider).Returns(GetRandomProvider().Object);
		mock.Setup(x => x.WorldProvider).Returns(GetWorldProvider().Object);
		return mock;
	}

	public static Mock<IServiceManager> GetServiceManager()
	{
		Mock<IServiceManager> mock = new(MockBehavior.Loose);
		mock.Setup(x => x.SettingsService).Returns(GetSettingsService().Object);
		mock.Setup(x => x.StateService).Returns(GetStateService().Object);
		mock.Setup(x => x.LoggerService).Returns(GetLoggerService().Object);
		return mock;
	}

	public static Mock<ISettingsService> GetSettingsService()
		=> new(MockBehavior.Loose);

	public static Mock<ILoggerService> GetLoggerService()
		=> new(MockBehavior.Loose);

	public static Mock<IStateService> GetStateService()
	{
		Mock<IStateService> mock = new(MockBehavior.Loose);
		mock.Setup(x => x.Player).Returns(GetPlayer().Object);
		return mock;
	}

	public static Mock<ITrafficking> GetTrafficking()
	{
		Mock<ITrafficking> mock = new(MockBehavior.Loose);
		return mock;
	}

	public static Mock<IInventory> GetInventory()
	{
		Mock<IInventory> mock = new(MockBehavior.Loose);
		mock.SetupAllProperties();
		return mock;
	}

	public static Mock<IPlayer> GetPlayer()
	{
		Mock<IPlayer> mock = new(MockBehavior.Loose);
		mock.SetupAllProperties();
		mock.Setup(x => x.Inventory).Returns(GetInventory().Object);
		return mock;
	}

	public static Mock<IDealer> GetDealer()
	{
		Mock<IDealer> mock = new(MockBehavior.Loose);
		mock.SetupAllProperties();
		return mock;
	}

	public static Mock<IDrug> GetDrug(DrugType type, int quantity = default, int currentPrice = default)
	{
		Mock<IDrug> mock = new(MockBehavior.Loose);
		mock.Setup(x => x.Type).Returns(type);
		mock.Setup(x => x.Quantity).Returns(quantity);
		mock.Setup(x => x.Price).Returns(currentPrice);
		mock.SetupAllProperties();
		return mock;
	}
}
