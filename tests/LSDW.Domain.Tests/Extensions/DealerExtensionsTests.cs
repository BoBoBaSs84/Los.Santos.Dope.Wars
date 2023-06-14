using GTA.Math;
using LSDW.Abstractions.Application.Providers;
using LSDW.Abstractions.Domain.Models;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;

namespace LSDW.Domain.Tests.Extensions;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public class DealerExtensionsTests
{
	private readonly Vector3 _zeroVector = new(0, 0, 0);

	[TestMethod]
	public void ChangePricesTest()
	{
		ITimeProvider timeProvider = new TestTimeProvider();
		IDealer dealer = DomainFactory.CreateDealer(_zeroVector);

		dealer.ChangePrices(timeProvider, 100);

		Assert.AreNotEqual(default, dealer.Inventory.Sum(x => x.CurrentPrice));
		Assert.AreEqual(default, dealer.Inventory.Money);
		Assert.AreEqual(default, dealer.Inventory.TotalQuantity);
		Assert.AreEqual(DateTime.MaxValue, dealer.LastRefresh);
		Assert.AreEqual(DateTime.MinValue, dealer.LastRestock);
	}

	[TestMethod]
	public void ChangePricesCollectionTest()
	{
		ITimeProvider timeProvider = new TestTimeProvider();
	  ICollection<IDealer> dealers = new HashSet<IDealer>();
		
		for (int i = 0; i < 5; i++)
			dealers.Add(DomainFactory.CreateDealer(_zeroVector));

		dealers.ChangePrices(timeProvider, 100);

		foreach(IDealer dealer in dealers)
		{
			Assert.AreNotEqual(default, dealer.Inventory.Sum(x => x.CurrentPrice));
			Assert.AreEqual(default, dealer.Inventory.Money);
			Assert.AreEqual(default, dealer.Inventory.TotalQuantity);
			Assert.AreEqual(DateTime.MaxValue, dealer.LastRefresh);
			Assert.AreEqual(DateTime.MinValue, dealer.LastRestock);
		}
	}

	[TestMethod()]
	public void RestockInventoryTest()
	{
		ITimeProvider timeProvider = new TestTimeProvider();
		IDealer dealer = DomainFactory.CreateDealer(_zeroVector);

		dealer.RestockInventory(timeProvider, 100);

		Assert.AreNotEqual(default, dealer.Inventory.Sum(x => x.CurrentPrice));
		Assert.AreNotEqual(default, dealer.Inventory.Money);
		Assert.AreNotEqual(default, dealer.Inventory.TotalQuantity);
		Assert.AreEqual(DateTime.MaxValue, dealer.LastRestock);
		Assert.AreEqual(DateTime.MinValue, dealer.LastRefresh);
	}

	[TestMethod()]
	public void RestockCollectionTest()
	{
		Assert.Fail();
	}

	private class TestTimeProvider : ITimeProvider
	{
		public DateTime Now => DateTime.MaxValue;

		public TimeSpan TimeOfDay => Now.TimeOfDay;
	}
}