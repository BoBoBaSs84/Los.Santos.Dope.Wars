using LSDW.Core.Extensions;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Tests.Extensions;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public class InventoryExtensionsTests
{
	[TestMethod]
	public void RandomizeTest()
	{
		IInventory inventory = InventoryFactory.CreateInventory();

		inventory.Randomize(1);

		Assert.AreNotEqual(0, inventory.Money);
	}
}