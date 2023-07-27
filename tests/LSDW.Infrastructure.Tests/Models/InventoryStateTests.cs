using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Domain.Factories;
using LSDW.Infrastructure.Factories;
using LSDW.Infrastructure.Models;

namespace LSDW.Infrastructure.Tests.Models;

[TestClass, ExcludeFromCodeCoverage]
public class InventoryStateTests
{
	[TestMethod]
	public void ShouldNotSerializeMoneyTest()
	{
		IInventory drugs = DomainFactory.CreateInventory();

		InventoryState state =
			InfrastructureFactory.CreateInventoryState(drugs);

		Assert.IsFalse(state.ShouldSerializeMoney());
	}

	[TestMethod]
	public void ShouldSerializeMoneyTest()
	{
		IInventory drugs = DomainFactory.CreateInventory(1000);

		InventoryState state =
			InfrastructureFactory.CreateInventoryState(drugs);

		Assert.IsTrue(state.ShouldSerializeMoney());
	}

	[TestMethod]
	public void ShouldNotSerializeDrugsTest()
	{
		IInventory drugs = DomainFactory.CreateInventory();

		InventoryState state =
			InfrastructureFactory.CreateInventoryState(drugs);

		Assert.IsFalse(state.ShouldSerializeDrugs());
	}

	[TestMethod]
	public void ShouldSerializeDrugsTest()
	{
		IDrug drug = DomainFactory.CreateDrug(DrugType.LSD, 10, 20);
		IInventory drugs = DomainFactory.CreateInventory();
		drugs.Add(drug);

		InventoryState state =
			InfrastructureFactory.CreateInventoryState(drugs);

		Assert.IsTrue(state.ShouldSerializeDrugs());
	}
}