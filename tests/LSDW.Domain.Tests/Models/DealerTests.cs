using GTA.Math;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Base.Tests.Helpers;
using LSDW.Domain.Factories;
using LSDW.Domain.Models;
using Moq;

namespace LSDW.Domain.Tests.Models;

[TestClass, ExcludeFromCodeCoverage]
public class DealerTests
{
	private readonly IDealer _dealer = DomainFactory.CreateDealer(Vector3.Zero);
	private readonly Mock<IWorldProvider> _worldProviderMock = MockHelper.GetWorldProvider();

	[TestMethod]
	public void SetClosedTest()
	{
		IWorldProvider provider = _worldProviderMock.Object;
		DateTime closedUnitl = provider.Now.AddHours(Settings.Instance.Dealer.DownTimeInHours.Value);

		_dealer.ClosedUntil = closedUnitl;

		Assert.IsTrue(_dealer.Closed);
		Assert.AreEqual(closedUnitl, _dealer.ClosedUntil);
	}

	[TestMethod]
	public void SetOpenTest()
	{
		IWorldProvider provider = _worldProviderMock.Object;
		_dealer.ClosedUntil = provider.Now.AddHours(Settings.Instance.Dealer.DownTimeInHours.Value);

		_dealer.ClosedUntil = null;

		Assert.IsFalse(_dealer.Closed);
		Assert.IsNull(_dealer.ClosedUntil);

	}

	[TestMethod]
	public void SetNextPriceChangeTest()
	{
		IWorldProvider provider = _worldProviderMock.Object;
		DateTime nextChange = provider.Now.AddHours(Settings.Instance.Market.PriceChangeInterval.Value);

		_dealer.NextPriceChange = nextChange;

		Assert.AreEqual(nextChange, _dealer.NextPriceChange);
	}

	[TestMethod]
	public void SetNextInventoryChangeTest()
	{
		IWorldProvider provider = _worldProviderMock.Object;
		DateTime nextChange = provider.Now.AddHours(Settings.Instance.Market.InventoryChangeInterval.Value);

		_dealer.NextInventoryChange = nextChange;

		Assert.AreEqual(nextChange, _dealer.NextInventoryChange);
	}
}