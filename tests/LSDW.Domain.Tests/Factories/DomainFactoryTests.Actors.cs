using GTA;
using GTA.Math;
using LSDW.Abstractions.Domain.Actors;
using LSDW.Domain.Factories;

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
		ICollection<IDealer>? dealers;

		dealers = DomainFactory.CreateDealers();

		Assert.IsNotNull(dealers);
	}
}