using LSDW.Core.Extensions;
using LSDW.Core.Interfaces.Classes;
using LSDW.Core.Factories;
using LSDW.Core.Helpers;

namespace LSDW.Core.Tests.Extensions;

[TestClass]
public class ClassExtensionsTests
{
	[TestMethod]
	public void ToXmlStringTest()
	{
		IEnumerable<IDrug> drugs = DrugFactory.CreateAllDrugs();
		IInventory inventory = InventoryFactory.CreateInventory(drugs);
		inventory.Add(RandomHelper.GetInt(10000, 25000));
		int experience = RandomHelper.GetInt(25000, 75000);
		IPlayer player = PlayerFactory.CreatePlayer(inventory, experience);

		string xmlString = player.ToXmlString();


	}

	[TestMethod()]
	public void FromXmlStringTest()
	{

	}
}