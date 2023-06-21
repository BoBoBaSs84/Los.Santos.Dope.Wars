using GTA.Math;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Base.Tests.Helpers;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
using LSDW.Domain.Models;
using Moq;

namespace LSDW.Domain.Tests.Extensions;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public class DealerExtensionsTests
{
	private readonly Vector3 _zeroVector = new(0, 0, 0);

	[TestMethod]
	public void ChangePricesTest()
	{
		Mock<ITimeProvider> mock = MockHelper.GetTimeProviderMock();
		DateTime nextPriceChange = mock.Object.Now.AddHours(Settings.Market.PriceChangeInterval);
		IDealer dealer = DomainFactory.CreateDealer(_zeroVector);

		dealer.ChangePrices(mock.Object, 100);

		Assert.AreNotEqual(default, dealer.Inventory.Sum(x => x.CurrentPrice));
		Assert.AreEqual(default, dealer.Inventory.Money);
		Assert.AreEqual(default, dealer.Inventory.TotalQuantity);
		Assert.AreEqual(nextPriceChange, dealer.NextPriceChange);
		Assert.AreEqual(DateTime.MinValue, dealer.NextInventoryChange);
	}

	[TestMethod]
	public void ChangePricesCollectionTest()
	{
		Mock<ITimeProvider> mock = MockHelper.GetTimeProviderMock();
		DateTime nextPriceChange = mock.Object.Now.AddHours(Settings.Market.PriceChangeInterval);
		ICollection<IDealer> dealers = new HashSet<IDealer>();

		for (int i = 0; i < 5; i++)
			dealers.Add(DomainFactory.CreateDealer(_zeroVector));

		dealers.ChangePrices(mock.Object, 100);

		foreach (IDealer dealer in dealers)
		{
			Assert.AreNotEqual(default, dealer.Inventory.Sum(x => x.CurrentPrice));
			Assert.AreEqual(default, dealer.Inventory.Money);
			Assert.AreEqual(default, dealer.Inventory.TotalQuantity);
			Assert.AreEqual(nextPriceChange, dealer.NextPriceChange);
			Assert.AreEqual(DateTime.MinValue, dealer.NextInventoryChange);
		}
	}

	[TestMethod]
	public void RestockInventoryTest()
	{
		Mock<ITimeProvider> mock = MockHelper.GetTimeProviderMock();
		DateTime nextPriceChange = mock.Object.Now.AddHours(Settings.Market.PriceChangeInterval);
		DateTime nextInventoryChange = mock.Object.Now.AddHours(Settings.Market.InventoryChangeInterval);
		IDealer dealer = DomainFactory.CreateDealer(_zeroVector);

		dealer.ChangeInventory(mock.Object, 100);

		Assert.AreNotEqual(default, dealer.Inventory.Sum(x => x.CurrentPrice));
		Assert.AreNotEqual(default, dealer.Inventory.Money);
		Assert.AreNotEqual(default, dealer.Inventory.TotalQuantity);
		Assert.AreEqual(nextInventoryChange, dealer.NextInventoryChange);
		Assert.AreEqual(nextPriceChange, dealer.NextPriceChange);
	}

	[TestMethod]
	public void RestockCollectionTest()
	{
		Mock<ITimeProvider> mock = MockHelper.GetTimeProviderMock();
		DateTime nextPriceChange = mock.Object.Now.AddHours(Settings.Market.PriceChangeInterval);
		DateTime nextInventoryChange = mock.Object.Now.AddHours(Settings.Market.InventoryChangeInterval);
		ICollection<IDealer> dealers = new HashSet<IDealer>();

		for (int i = 0; i < 5; i++)
			dealers.Add(DomainFactory.CreateDealer(_zeroVector));

		dealers.ChangeInventories(mock.Object, 100);

		foreach (IDealer dealer in dealers)
		{
			Assert.AreNotEqual(default, dealer.Inventory.Sum(x => x.CurrentPrice));
			Assert.AreNotEqual(default, dealer.Inventory.Money);
			Assert.AreNotEqual(default, dealer.Inventory.TotalQuantity);
			Assert.AreEqual(nextInventoryChange, dealer.NextInventoryChange);
			Assert.AreEqual(nextPriceChange, dealer.NextPriceChange);
		}
	}
}