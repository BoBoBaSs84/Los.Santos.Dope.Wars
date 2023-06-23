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
	private readonly Mock<ITimeProvider> _timeProviderMock = MockHelper.GetTimeProvider();

	[TestMethod]
	public void SetClosedTest()
	{
		ITimeProvider timeProvider = _timeProviderMock.Object;

		_dealer.SetClosed(timeProvider);

		Assert.IsTrue(_dealer.Closed);
		Assert.AreEqual(timeProvider.Now.AddHours(DealerSettings.DownTimeInHours), _dealer.ClosedUntil);
	}

	[TestMethod]
	public void SetOpenTest()
	{
		ITimeProvider timeProvider = _timeProviderMock.Object;
		_dealer.SetClosed(timeProvider);

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
		ITimeProvider timeProvider = _timeProviderMock.Object;

		_dealer.SetNextPriceChange(timeProvider);

		Assert.AreEqual(timeProvider.Now.AddHours(MarketSettings.PriceChangeInterval), _dealer.NextPriceChange);
	}

	[TestMethod]
	public void SetNextInventoryChangeTest()
	{
		ITimeProvider timeProvider = _timeProviderMock.Object;

		_dealer.SetNextInventoryChange(timeProvider);

		Assert.AreEqual(timeProvider.Now.AddHours(MarketSettings.InventoryChangeInterval), _dealer.NextInventoryChange);
	}
}