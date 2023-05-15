using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Tests.Factories;

[TestClass]
public class DrugFactoryTests
{
	[TestMethod]
	public void CreateRandomDrugTest()
	{
		IDrug drug = DrugFactory.CreateRandomDrug();

		Assert.IsNotNull(drug);
	}

	[TestMethod()]
	public void CreateAllDrugsTest()
	{
		IEnumerable<IDrug> drugs = DrugFactory.CreateAllDrugs();

		Assert.IsNotNull(drugs);
		Assert.IsTrue(drugs.Any());
	}
}