using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Application.Models.Missions;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Models.Base;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Infrastructure.Services;
using Moq;

namespace LSDW.Base.Tests.Helpers;

[ExcludeFromCodeCoverage]
[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public static class MockHelper
{
	public static Mock<IAudioProvider> GetAudioProvider()
	{
		Mock<IAudioProvider> mock = new(MockBehavior.Loose);
		mock.SetupAllProperties();
		return mock;
	}

	public static Mock<IGameProvider> GetGameProvider()
	{
		Mock<IGameProvider> mock = new(MockBehavior.Loose);
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
		Mock<IPlayerProvider> mock = new(MockBehavior.Loose);
		mock.SetupAllProperties();
		return mock;
	}

	public static Mock<IScreenProvider> GetScreenProvider()
	{
		Mock<IScreenProvider> mock = new(MockBehavior.Loose);
		mock.SetupAllProperties();
		return mock;
	}

	public static Mock<IProviderManager> GetProviderManager()
	{
		Mock<IProviderManager> mock = new(MockBehavior.Loose);
		mock.Setup(x => x.AudioProvider).Returns(GetAudioProvider().Object);
		mock.Setup(x => x.GameProvider).Returns(GetGameProvider().Object);
		mock.Setup(x => x.NotificationProvider).Returns(GetNotificationProvider().Object);
		mock.Setup(x => x.PlayerProvider).Returns(GetPlayerProvider().Object);
		mock.Setup(x => x.ScreenProvider).Returns(GetScreenProvider().Object);
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
	{
		Mock<ISettingsService> mock = new(MockBehavior.Loose);
		mock.SetupAllProperties();
		mock.Setup(x => x.Dealer).Returns(GetDealerSettings().Object);
		mock.Setup(x => x.Market).Returns(GetMarketSettings().Object);
		mock.Setup(x => x.Player).Returns(GetPlayerSettings().Object);
		mock.Setup(x => x.Trafficking).Returns(GetTraffickingSettings().Object);
		return mock;
	}

	public static Mock<IDealerSettings> GetDealerSettings()
	{
		Mock<IDealerSettings> mock = new(MockBehavior.Loose);
		mock.Setup(x => x.DownTimeInHours).Returns(GetBindableProperty(48).Object);
		mock.Setup(x => x.HasArmor).Returns(GetBindableProperty(true).Object);
		mock.Setup(x => x.HasWeapons).Returns(GetBindableProperty(true).Object);
		return mock;
	}

	public static Mock<IMarketSettings> GetMarketSettings()
	{
		Mock<IMarketSettings> mock = new(MockBehavior.Loose);
		mock.Setup(x => x.InventoryChangeInterval).Returns(GetBindableProperty(24).Object);
		mock.Setup(x => x.MaximumDrugPrice).Returns(GetBindableProperty(1.15f).Object);
		mock.Setup(x => x.MinimumDrugPrice).Returns(GetBindableProperty(0.85f).Object);
		mock.Setup(x => x.PriceChangeInterval).Returns(GetBindableProperty(6).Object);
		mock.Setup(x => x.SpecialOfferChance).Returns(GetBindableProperty(0.1f).Object);
		return mock;
	}

	public static Mock<IPlayerSettings> GetPlayerSettings()
	{
		Mock<IPlayerSettings> mock = new(MockBehavior.Loose);
		mock.Setup(x => x.ExperienceMultiplier).Returns(GetBindableProperty(1f).Object);
		mock.Setup(x => x.InventoryExpansionPerLevel).Returns(GetBindableProperty(10).Object);
		mock.Setup(x => x.LooseDrugsOnDeath).Returns(GetBindableProperty(true).Object);
		mock.Setup(x => x.LooseDrugsWhenBusted).Returns(GetBindableProperty(true).Object);
		mock.Setup(x => x.LooseMoneyOnDeath).Returns(GetBindableProperty(true).Object);
		mock.Setup(x => x.LooseMoneyWhenBusted).Returns(GetBindableProperty(true).Object);
		mock.Setup(x => x.StartingInventory).Returns(GetBindableProperty(100).Object);
		return mock;
	}

	public static Mock<ITraffickingSettings> GetTraffickingSettings()
	{
		Mock<ITraffickingSettings> mock = new(MockBehavior.Loose);
		mock.Setup(x => x.BustChance).Returns(GetBindableProperty(0.1f).Object);
		mock.Setup(x => x.DiscoverDealer).Returns(GetBindableProperty(true).Object);
		mock.Setup(x => x.WantedLevel).Returns(GetBindableProperty(2).Object);
		return mock;
	}

	public static Mock<ILoggerService> GetLoggerService()
	{
		Mock<ILoggerService> mock = new(MockBehavior.Loose);
		mock.SetupAllProperties();
		return mock;
	}

	public static Mock<IStateService> GetStateService()
	{
		Mock<IStateService> mock = new(MockBehavior.Loose);
		mock.Setup(x => x.Player).Returns(GetPlayer().Object);
		return mock;
	}

	public static Mock<ITrafficking> GetTrafficking()
	{
		Mock<ITrafficking> mock = new(MockBehavior.Loose);
		mock.SetupAllProperties();
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

	public static Mock<IBindableProperty<T>> GetBindableProperty<T>(T value = default!) where T : IEquatable<T>
	{
		Mock<IBindableProperty<T>> mock = new(MockBehavior.Loose);
		mock.SetupAllProperties();
		mock.Object.Value = value;
		return mock;
	}
}
