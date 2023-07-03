using GTA;
using GTA.Math;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Base.Tests.Helpers;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
using LSDW.Infrastructure.Models;

namespace LSDW.Infrastructure.Tests.Models;

[TestClass]
public class DealerStateTests
{
	private readonly IWorldProvider _worldProvider = MockHelper.GetWorldProvider().Object;

	[TestMethod]
	public void NotDiscoveredShouldSerializeFalseTest()
	{
		IDealer dealer = DomainFactory.CreateDealer(Vector3.Zero, PedHash.Hooker01SFY);

		DealerState state = new(dealer);

		Assert.IsNotNull(state);
		Assert.IsFalse(state.ShouldSerializeClosedUntil());
		Assert.IsFalse(state.ShouldSerializeNextPriceChange());
		Assert.IsFalse(state.ShouldSerializeNextInventoryChange());
		Assert.IsFalse(state.ShouldSerializeHash());
		Assert.IsFalse(state.ShouldSerializeInventory());
		Assert.IsFalse(state.ShouldSerializeName());
	}

	[TestMethod]
	public void DiscoveredShouldSerializeTrueTest()
	{
		IDealer dealer = DomainFactory.CreateDealer(Vector3.Zero, PedHash.Hooker01SFY);
		dealer.Discovered = true;

		DealerState state = new(dealer);

		Assert.IsNotNull(state);
		Assert.IsFalse(state.ShouldSerializeClosedUntil());
		Assert.IsTrue(state.ShouldSerializeNextPriceChange());
		Assert.IsTrue(state.ShouldSerializeNextInventoryChange());
		Assert.IsTrue(state.ShouldSerializeHash());
		Assert.IsTrue(state.ShouldSerializeInventory());
		Assert.IsTrue(state.ShouldSerializeName());
	}

	[TestMethod]
	public void DiscoveredButClosedShouldSerializeFalseTest()
	{
		IDealer dealer = DomainFactory.CreateDealer(Vector3.Zero, PedHash.Hooker01SFY);
		dealer.Discovered = true;
		dealer.ClosedUntil = _worldProvider.Now;

		DealerState state = new(dealer);

		Assert.IsNotNull(state);
		Assert.IsTrue(state.ShouldSerializeClosedUntil());
		Assert.IsFalse(state.ShouldSerializeNextPriceChange());
		Assert.IsFalse(state.ShouldSerializeNextInventoryChange());
		Assert.IsFalse(state.ShouldSerializeHash());
		Assert.IsFalse(state.ShouldSerializeInventory());
		Assert.IsFalse(state.ShouldSerializeName());
	}
}