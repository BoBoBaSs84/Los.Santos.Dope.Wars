using GTA.Math;
using LSDW.Factories;
using LSDW.Interfaces.Actors;

namespace LSDW.Tests.Factories;

[TestClass]
public class ActorFactoryTests
{
	[TestMethod]
	public void CreateDealerTest()
	{
		IDealer? dealer;

		dealer = ActorFactory.CreateDealer(new Vector3(287.011f, -991.685f, 33.108f));

		Assert.IsNotNull(dealer);
	}
}