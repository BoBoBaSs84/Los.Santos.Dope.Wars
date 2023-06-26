using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Base.Tests.Helpers;
using LSDW.Domain.Factories;
using Moq;
using DealerSettings = LSDW.Domain.Models.Settings.Dealer;
using MarketSettings = LSDW.Domain.Models.Settings.Market;

namespace LSDW.Domain.Tests.Models;

[TestClass]
public class DealerTests
{
	private readonly IDealer _dealer = DomainFactory.CreateDealer(new(0, 0, 0));
	private readonly Mock<IWorldProvider> _worldProviderMock = MockHelper.GetWorldProvider();

	[TestMethod]
	public void SetClosedTest()
	{
		IWorldProvider provider = _worldProviderMock.Object;

		_dealer.SetClosed(provider);

		Assert.IsTrue(_dealer.Closed);
		Assert.AreEqual(provider.Now.AddHours(DealerSettings.DownTimeInHours), _dealer.ClosedUntil);
	}

	[TestMethod]
	public void SetOpenTest()
	{
		IWorldProvider provider = _worldProviderMock.Object;
		_dealer.SetClosed(provider);

		_dealer.SetOpen();

		Assert.IsFalse(_dealer.Closed);
		Assert.IsNull(_dealer.ClosedUntil);

	}

	[TestMethod]
	public void SetDiscoveredTest()
	{
		_dealer.SetDiscovered(true);

		Assert.IsTrue(_dealer.Discovered);
	}

	[TestMethod]
	public void SetNextPriceChangeTest()
	{
		IWorldProvider provider = _worldProviderMock.Object;

		_dealer.SetNextPriceChange(provider);

		Assert.AreEqual(provider.Now.AddHours(MarketSettings.PriceChangeInterval), _dealer.NextPriceChange);
	}

	[TestMethod]
	public void SetNextInventoryChangeTest()
	{
		IWorldProvider provider = _worldProviderMock.Object;

		_dealer.SetNextInventoryChange(provider);

		Assert.AreEqual(provider.Now.AddHours(MarketSettings.InventoryChangeInterval), _dealer.NextInventoryChange);
	}
}