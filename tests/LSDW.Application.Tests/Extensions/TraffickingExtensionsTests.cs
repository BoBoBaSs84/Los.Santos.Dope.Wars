using GTA.Math;
using LSDW.Abstractions.Application.Models.Missions;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Application.Extensions;
using LSDW.Application.Factories;
using LSDW.Base.Tests.Helpers;
using LSDW.Domain.Factories;
using LSDW.Infrastructure.Factories;

namespace LSDW.Application.Tests.Extensions;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "Unit tests.")]
public class TraffickingExtensionsTests
{
	private readonly Vector3 _zeroVector = new(0, 0, 0);
	private readonly ITrafficking _trafficking = ApplicationFactory.CreateTraffickingMission(MockHelper.GetServiceManager().Object, MockHelper.GetProviderManager().Object);
	private readonly IStateService _stateService = InfrastructureFactory.CreateGameStateService(MockHelper.GetLoggerService().Object);

	[TestMethod]
	public void ChangeDealerPricesNonDiscoveredTest()
	{
		_stateService.Dealers.Add(DomainFactory.CreateDealer(_zeroVector));

		_trafficking.ChangeDealerPrices(_stateService);

		Assert.AreEqual(0, _stateService.Dealers.First().Inventory.Sum(x => x.CurrentPrice));
	}

	[TestMethod]
	public void ChangeDealerPricesTest()
	{
		IDealer dealer = DomainFactory.CreateDealer(_zeroVector);
		dealer.Discovered = true;
		_stateService.Dealers.Add(dealer);

		_trafficking.ChangeDealerPrices(_stateService);

		Assert.AreNotEqual(0, _stateService.Dealers.First().Inventory.Sum(x => x.CurrentPrice));
	}

	[TestMethod]
	public void ChangeDealerInventoriesNonDiscoveredTest()
	{
		_stateService.Dealers.Add(DomainFactory.CreateDealer(_zeroVector));

		_trafficking.ChangeDealerInventories(_stateService);

		Assert.AreEqual(0, _stateService.Dealers.First().Inventory.Sum(x => x.CurrentPrice));
		Assert.AreEqual(0, _stateService.Dealers.First().Inventory.Sum(x => x.Quantity));
	}

	[TestMethod]
	public void ChangeDealerInventoriesTest()
	{
		IDealer dealer = DomainFactory.CreateDealer(_zeroVector);
		dealer.Discovered = true;
		_stateService.Dealers.Add(dealer);

		_trafficking.ChangeDealerInventories(_stateService);

		Assert.AreNotEqual(0, _stateService.Dealers.First().Inventory.Sum(x => x.CurrentPrice));
		Assert.AreNotEqual(0, _stateService.Dealers.First().Inventory.Sum(x => x.Quantity));
	}
}