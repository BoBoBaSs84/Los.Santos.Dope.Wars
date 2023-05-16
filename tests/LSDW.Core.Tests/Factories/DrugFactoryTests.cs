using LSDW.Core.Enumerators;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Tests.Factories;

[TestClass]
public class DrugFactoryTests
{
	[TestMethod]
	public void CreateRandomDrugTest()
	{
		IDrug? drug;

		drug = DrugFactory.CreateRandomDrug();

		Assert.IsNotNull(drug);
	}

	[TestMethod]
	public void CreateAllDrugsTest()
	{
		IEnumerable<IDrug>? drugs;

		drugs = DrugFactory.CreateRandomDrugs();

		Assert.IsNotNull(drugs);
		Assert.IsTrue(drugs.Any());
	}

	[TestMethod]
	public void CreateDrugTest()
	{
		DrugType drugType = DrugType.COKE;
		int quantity = 10;
		int price = 1000;

		IDrug drug = DrugFactory.CreateDrug(drugType, quantity, price);

		Assert.IsNotNull(drug);
		Assert.AreEqual(drugType, drug.DrugType);
		Assert.AreEqual(quantity, drug.Quantity);
		Assert.AreEqual(price, drug.Price);
	}
}