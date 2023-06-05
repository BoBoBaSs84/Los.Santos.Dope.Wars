using GTA;
using GTA.Math;
using LSDW.Domain.Classes.Persistence;
using LSDW.Domain.Factories;
using LSDW.Domain.Interfaces.Actors;

namespace LSDW.Domain.Tests.Factories;

[TestClass]
public partial class DomainFactoryTests
{
	private readonly Vector3 zeroVector = new(0, 0, 0);
	private readonly PedHash pedHash = PedHash.Clown01SMY;

	[TestMethod]
	public void CreateDealerTest()
	{
		IDealer? dealer;

		dealer = DomainFactory.CreateDealer(zeroVector);

		Assert.IsNotNull(dealer);
		Assert.AreEqual(dealer.Position, zeroVector);
	}

	[TestMethod]
	public void CreateDealerWithParamsTest()
	{
		IDealer? dealer;

		dealer = DomainFactory.CreateDealer(zeroVector, pedHash);

		Assert.IsNotNull(dealer);
		Assert.AreEqual(dealer.Position, zeroVector);
		Assert.AreEqual(dealer.Hash, pedHash);
	}

	[TestMethod]
	public void CreateDealersTest()
	{
		IEnumerable<IDealer>? dealers;

		dealers = DomainFactory.CreateDealers();

		Assert.IsNotNull(dealers);
	}

	[TestMethod]
	public void CreateDealerFromStateTest()
	{
		DealerState state = GetDealerState();

		IDealer dealer = DomainFactory.CreateDealer(state);

		Assert.IsNotNull(dealer);
		Assert.IsNotNull(dealer.Inventory);
		Assert.AreEqual(state.ClosedUntil, dealer.ClosedUntil);
		Assert.AreEqual(state.Discovered, dealer.Discovered);
		Assert.AreEqual(state.Name, dealer.Name);
		Assert.AreEqual(state.Position, dealer.Position);
		Assert.AreEqual(state.Hash, dealer.Hash);
	}

	[TestMethod]
	public void CreateDealersFromStatesTest()
	{
		List<DealerState> states = new() { GetDealerState() };

		IEnumerable<IDealer> dealers = DomainFactory.CreateDealers(states);

		Assert.IsNotNull(dealers);
		Assert.AreEqual(states.Count, dealers.Count());
	}

	[TestMethod]
	public void CreateDealersFromGameState()
	{
		GameState gameState = new();

		IEnumerable<IDealer> dealers = DomainFactory.CreateDealers(gameState);

		Assert.IsNotNull(dealers);
	}

	private static DealerState GetDealerState()
		=> new()
		{
			ClosedUntil = DateTime.MinValue,
			Discovered = true,
			Name = "Dealer",
			Position = new(0, 0, 0),
			Hash = PedHash.AcidLabCook,
			Inventory = new()
		};
}