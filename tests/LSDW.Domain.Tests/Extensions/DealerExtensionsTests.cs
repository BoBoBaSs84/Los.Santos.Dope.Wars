using GTA.Math;
using LSDW.Abstractions.Application.Providers;
using LSDW.Abstractions.Domain.Models;
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

	[TestMethod]
	public void ChangePricesTest()
	{
		Mock<ITimeProvider> mock = MockHelper.GetTimeProviderMock();
		IDealer dealer = DomainFactory.CreateDealer(_zeroVector);

		dealer.ChangePrices(mock.Object, 100);

		Assert.AreNotEqual(default, dealer.Inventory.Sum(x => x.CurrentPrice));
		Assert.AreEqual(default, dealer.Inventory.Money);
		Assert.AreEqual(default, dealer.Inventory.TotalQuantity);
		Assert.AreEqual(DateTime.MaxValue, dealer.LastRefresh);
		Assert.AreEqual(DateTime.MinValue, dealer.LastRestock);
	}

	[TestMethod]
	public void ChangePricesCollectionTest()
	{
		Mock<ITimeProvider> mock = MockHelper.GetTimeProviderMock();
		ICollection<IDealer> dealers = new HashSet<IDealer>();

		for (int i = 0; i < 5; i++)
			dealers.Add(DomainFactory.CreateDealer(_zeroVector));

		dealers.ChangePrices(mock.Object, 100);

		foreach (IDealer dealer in dealers)
		{
			Assert.AreNotEqual(default, dealer.Inventory.Sum(x => x.CurrentPrice));
			Assert.AreEqual(default, dealer.Inventory.Money);
			Assert.AreEqual(default, dealer.Inventory.TotalQuantity);
			Assert.AreEqual(DateTime.MaxValue, dealer.LastRefresh);
			Assert.AreEqual(DateTime.MinValue, dealer.LastRestock);
		}
	}

	[TestMethod]
	public void RestockInventoryTest()
	{
		Mock<ITimeProvider> mock = MockHelper.GetTimeProviderMock();
		IDealer dealer = DomainFactory.CreateDealer(_zeroVector);

		dealer.RestockInventory(mock.Object, 100);

		Assert.AreNotEqual(default, dealer.Inventory.Sum(x => x.CurrentPrice));
		Assert.AreNotEqual(default, dealer.Inventory.Money);
		Assert.AreNotEqual(default, dealer.Inventory.TotalQuantity);
		Assert.AreEqual(DateTime.MaxValue, dealer.LastRestock);
		Assert.AreEqual(DateTime.MinValue, dealer.LastRefresh);
	}

	[TestMethod]
	public void RestockCollectionTest()
	{
		Mock<ITimeProvider> mock = MockHelper.GetTimeProviderMock();
		ICollection<IDealer> dealers = new HashSet<IDealer>();

		for (int i = 0; i < 5; i++)
			dealers.Add(DomainFactory.CreateDealer(_zeroVector));

		dealers.RestockInventory(mock.Object, 100);

		foreach (IDealer dealer in dealers)
		{
			Assert.AreNotEqual(default, dealer.Inventory.Sum(x => x.CurrentPrice));
			Assert.AreNotEqual(default, dealer.Inventory.Money);
			Assert.AreNotEqual(default, dealer.Inventory.TotalQuantity);
			Assert.AreEqual(DateTime.MaxValue, dealer.LastRestock);
			Assert.AreEqual(DateTime.MinValue, dealer.LastRefresh);
		}
	}
}