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
	private readonly ITrafficking _trafficking = ApplicationFactory.CreateTraffickingMission(MockHelper.GetServiceManager().Object, MockHelper.GetProviderManager().Object);
	private readonly IStateService _stateService = InfrastructureFactory.GetStateService();

	[TestMethod]
	public void ChangeDealerPricesNonDiscoveredTest()
	{
		IDealer dealer = DomainFactory.CreateDealer(Vector3.Zero);
		_stateService.Dealers.Add(dealer);

		_trafficking.ChangeDealerPrices(_stateService);

		Assert.AreEqual(default, dealer.Inventory.Sum(x => x.Price));
		Assert.AreEqual(default, dealer.Inventory.Money);
		Assert.AreEqual(default, dealer.Inventory.TotalQuantity);
	}

	[TestMethod]
	public void ChangeDealerPricesTest()
	{
		IDealer dealer = DomainFactory.CreateDealer(Vector3.Zero);
		dealer.Discovered = true;
		_stateService.Dealers.Add(dealer);

		_trafficking.ChangeDealerPrices(_stateService);

		Assert.AreNotEqual(default, dealer.Inventory.Sum(x => x.Price));
		Assert.AreEqual(default, dealer.Inventory.Money);
		Assert.AreEqual(default, dealer.Inventory.TotalQuantity);
	}

	[TestMethod]
	public void ChangeDealerInventoriesNonDiscoveredTest()
	{
		IDealer dealer = DomainFactory.CreateDealer(Vector3.Zero);
		_stateService.Dealers.Add(dealer);

		_trafficking.ChangeDealerInventories(_stateService);

		Assert.AreEqual(default, dealer.Inventory.Money);
		Assert.AreEqual(default, dealer.Inventory.TotalQuantity);
		Assert.AreEqual(default, dealer.Inventory.TotalValue);
	}

	[TestMethod]
	public void ChangeDealerInventoriesTest()
	{
		IDealer dealer = DomainFactory.CreateDealer(Vector3.Zero);
		dealer.Discovered = true;
		_stateService.Dealers.Add(dealer);

		_trafficking.ChangeDealerInventories(_stateService);

		Assert.AreNotEqual(default, dealer.Inventory.Money);
		Assert.AreNotEqual(default, dealer.Inventory.TotalQuantity);
		Assert.AreNotEqual(default, dealer.Inventory.TotalValue);
	}
}