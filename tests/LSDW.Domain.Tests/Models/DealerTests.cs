using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Base.Tests.Helpers;
using LSDW.Domain.Factories;
using Moq;
using DealerSettings = LSDW.Abstractions.Models.Settings.Dealer;
using MarketSettings = LSDW.Abstractions.Models.Settings.Market;

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
		DateTime closedUnitl = provider.Now.AddHours(DealerSettings.DownTimeInHours);

		_dealer.ClosedUntil = closedUnitl;

		Assert.IsTrue(_dealer.Closed);
		Assert.AreEqual(closedUnitl, _dealer.ClosedUntil);
	}

	[TestMethod]
	public void SetOpenTest()
	{
		IWorldProvider provider = _worldProviderMock.Object;
		_dealer.ClosedUntil = provider.Now.AddHours(DealerSettings.DownTimeInHours);

		_dealer.ClosedUntil = null;

		Assert.IsFalse(_dealer.Closed);
		Assert.IsNull(_dealer.ClosedUntil);

	}

	[TestMethod]
	public void SetNextPriceChangeTest()
	{
		IWorldProvider provider = _worldProviderMock.Object;
		DateTime nextChange = provider.Now.AddHours(MarketSettings.PriceChangeInterval);

		_dealer.NextPriceChange = nextChange;

		Assert.AreEqual(nextChange, _dealer.NextPriceChange);
	}

	[TestMethod]
	public void SetNextInventoryChangeTest()
	{
		IWorldProvider provider = _worldProviderMock.Object;
		DateTime nextChange = provider.Now.AddHours(MarketSettings.InventoryChangeInterval);

		_dealer.NextInventoryChange = nextChange;

		Assert.AreEqual(nextChange, _dealer.NextInventoryChange);
	}
}