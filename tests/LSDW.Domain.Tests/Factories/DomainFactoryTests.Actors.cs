using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Domain.Factories;

namespace LSDW.Domain.Tests.Factories;

public partial class DomainFactoryTests
{
	[TestMethod]
	public void CreateDealerTest()
	{
		IDealer? dealer;

		dealer = DomainFactory.CreateDealer(zeroVector);

		Assert.IsNotNull(dealer);
		Assert.AreEqual(dealer.Position, zeroVector);
		Assert.AreEqual(TaskType.NOTASK, dealer.CurrentTask);
	}

	[TestMethod]
	public void CreateDealerWithParamsTest()
	{
		IDealer? dealer;

		dealer = DomainFactory.CreateDealer(zeroVector, pedHash);

		Assert.IsNotNull(dealer);
		Assert.AreEqual(dealer.Position, zeroVector);
		Assert.AreEqual(dealer.Hash, pedHash);
		Assert.AreEqual(TaskType.NOTASK, dealer.CurrentTask);
	}

	[TestMethod]
	public void CreateDealersTest()
	{
		ICollection<IDealer>? dealers;

		dealers = DomainFactory.CreateDealers();

		Assert.IsNotNull(dealers);
	}
}