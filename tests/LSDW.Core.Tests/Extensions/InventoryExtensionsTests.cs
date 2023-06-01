using LSDW.Core.Extensions;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Models;

namespace LSDW.Core.Tests.Extensions;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public class InventoryExtensionsTests
{
	[TestMethod]
	public void RandomizeTest()
	{
		IInventory inventory = ModelFactory.CreateInventory();

		inventory.Randomize();

		Assert.AreNotEqual(0, inventory.Money);
	}
}