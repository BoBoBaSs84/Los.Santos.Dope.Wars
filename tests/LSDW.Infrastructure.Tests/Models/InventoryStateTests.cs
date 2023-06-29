using LSDW.Abstractions.Domain.Models;
using LSDW.Domain.Factories;
using LSDW.Infrastructure.Factories;
using LSDW.Infrastructure.Models;

namespace LSDW.Infrastructure.Tests.Models;

[TestClass]
public class InventoryStateTests
{
	[TestMethod]
	public void ShouldSerializeMoneyTest()
	{
		IInventory drugs = DomainFactory.CreateInventory();

		InventoryState state =
			InfrastructureFactory.CreateInventoryState(drugs);

		Assert.IsFalse(state.ShouldSerializeMoney());
	}
}