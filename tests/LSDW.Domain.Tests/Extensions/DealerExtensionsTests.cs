using GTA.Math;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Extensions;
using LSDW.Abstractions.Models;
using LSDW.Base.Tests.Helpers;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
using LSDW.Domain.Properties;
using Moq;

namespace LSDW.Domain.Tests.Extensions;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public class DealerExtensionsTests
{
	private readonly Vector3 _zeroVector = new(0, 0, 0);
	private readonly Mock<IWorldProvider> _worldProviderMock = MockHelper.GetWorldProvider();

	[TestMethod]
	public void ChangePricesTest()
	{
		IWorldProvider provider = _worldProviderMock.Object;
		DateTime nextPriceChange = provider.Now.AddHours(Settings.Market.PriceChangeInterval);
		IDealer dealer = DomainFactory.CreateDealer(_zeroVector);

		dealer.ChangePrices(provider, 100);

		Assert.AreNotEqual(default, dealer.Inventory.Sum(x => x.Price));
		Assert.AreEqual(default, dealer.Inventory.Money);
		Assert.AreEqual(default, dealer.Inventory.TotalQuantity);
		Assert.AreEqual(nextPriceChange, dealer.NextPriceChange);
		Assert.AreEqual(DateTime.MinValue, dealer.NextInventoryChange);
	}

	[TestMethod]
	public void RestockInventoryTest()
	{
		IWorldProvider provider = _worldProviderMock.Object;
		DateTime nextPriceChange = provider.Now.AddHours(Settings.Market.PriceChangeInterval);
		DateTime nextInventoryChange = provider.Now.AddHours(Settings.Market.InventoryChangeInterval);
		IDealer dealer = DomainFactory.CreateDealer(_zeroVector);

		dealer.ChangeInventory(provider, 100);

		Assert.AreNotEqual(default, dealer.Inventory.Sum(x => x.Price));
		Assert.AreNotEqual(default, dealer.Inventory.Money);
		Assert.AreNotEqual(default, dealer.Inventory.TotalQuantity);
		Assert.AreEqual(nextInventoryChange, dealer.NextInventoryChange);
		Assert.AreEqual(nextPriceChange, dealer.NextPriceChange);
	}

	[TestMethod]
	public void CleanUpDealerTest()
	{
		IDealer dealer = DomainFactory.CreateDealer(_zeroVector);

		dealer.CleanUp();

		Assert.IsFalse(dealer.Created);
		Assert.IsFalse(dealer.BlipCreated);
	}

	[TestMethod]
	public void CleanUpDealersTest()
	{
		ICollection<IDealer> dealers = new HashSet<IDealer>();
		for (int i = 0; i < 5; i++)
			dealers.Add(MockHelper.GetDealer().Object);

		dealers.CleanUp();
	}

	[TestMethod]
	public void MakeSpecialBuyOfferTest()
	{
		IDealer dealer = DomainFactory.CreateDealer(Vector3.Zero);
		Mock<INotificationProvider> notificationProviderMock = MockHelper.GetNotificationProvider();
		Mock<IRandomProvider> randomProviderMock = MockHelper.GetRandomProvider();
		//randomProviderMock.Setup(x=>x.GetInt()).Returns(0);
		//randomProviderMock.Setup(x => x.GetFloat()).Returns(0f);
		Mock<IProviderManager> providerManagerMock = MockHelper.GetProviderManager();
		providerManagerMock.Setup(x => x.NotificationProvider).Returns(notificationProviderMock.Object);
		providerManagerMock.Setup(x => x.RandomProvider).Returns(randomProviderMock.Object);

		dealer.MakeSpecialOffer(providerManagerMock.Object, default);

		randomProviderMock.Verify(x => x.GetInt(dealer.Inventory.Count));
		randomProviderMock.Verify(x => x.GetFloat());
		notificationProviderMock.Verify(x => x.Show(dealer.Name, Resources.Dealer_Message_SpecialBuyOffer_Subject, Resources.Dealer_Message_SpecialBuyOffer_Message.FormatInvariant(DrugType.COKE.GetName()), true));
	}

	[TestMethod]
	public void MakeSpecialSellOfferTest()
	{
		IDealer dealer = DomainFactory.CreateDealer(Vector3.Zero);
		Mock<INotificationProvider> notificationProviderMock = MockHelper.GetNotificationProvider();
		Mock<IRandomProvider> randomProviderMock = MockHelper.GetRandomProvider();
		randomProviderMock.Setup(x => x.GetInt(dealer.Inventory.Count)).Returns(1);
		randomProviderMock.Setup(x => x.GetFloat()).Returns(0.51f);
		Mock<IProviderManager> providerManagerMock = MockHelper.GetProviderManager();
		providerManagerMock.Setup(x => x.NotificationProvider).Returns(notificationProviderMock.Object);
		providerManagerMock.Setup(x => x.RandomProvider).Returns(randomProviderMock.Object);

		dealer.MakeSpecialOffer(providerManagerMock.Object, default);

		randomProviderMock.Verify(x => x.GetInt(dealer.Inventory.Count));
		randomProviderMock.Verify(x => x.GetFloat());
		notificationProviderMock.Verify(x => x.Show(dealer.Name, Resources.Dealer_Message_SpecialSellOffer_Subject, Resources.Dealer_Message_SpecialSellOffer_Message.FormatInvariant(DrugType.SMACK.GetName()), true));
	}
}