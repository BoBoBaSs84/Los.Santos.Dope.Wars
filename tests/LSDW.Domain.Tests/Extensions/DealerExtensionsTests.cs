using Microsoft.VisualStudio.TestTools.UnitTesting;
using GTA.Math;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Abstractions.Models;
using LSDW.Base.Tests.Helpers;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
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

		Assert.AreNotEqual(default, dealer.Inventory.Sum(x => x.CurrentPrice));
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

		Assert.AreNotEqual(default, dealer.Inventory.Sum(x => x.CurrentPrice));
		Assert.AreNotEqual(default, dealer.Inventory.Money);
		Assert.AreNotEqual(default, dealer.Inventory.TotalQuantity);
		Assert.AreEqual(nextInventoryChange, dealer.NextInventoryChange);
		Assert.AreEqual(nextPriceChange, dealer.NextPriceChange);
	}

	[TestMethod]
	public void CleanUpTest()
	{
		IDealer dealer = DomainFactory.CreateDealer(_zeroVector);

		dealer.CleanUp();

		Assert.IsFalse(dealer.Created);
		Assert.IsFalse(dealer.BlipCreated);
	}
}