using LSDW.Core.Extensions;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Tests.Extensions;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public class DrugExtensionsTests
{
	[TestMethod]
	public void RandomizeQuantityTest()
	{
		IDrug drug = DrugFactory.CreateDrug(Enumerators.DrugType.CANA);

		drug.RandomizeQuantity();

		Assert.AreNotEqual(0, drug.Quantity);
	}

	[TestMethod]
	public void RandomizePriceTest()
	{
		IDrug drug = DrugFactory.CreateDrug(Enumerators.DrugType.CANA);

		drug.RandomizePrice();

		Assert.AreNotEqual(0, drug.Price);
	}
}