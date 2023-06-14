using LSDW.Abstractions.Domain.Models;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;

namespace LSDW.Domain.Tests.Extensions;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public class InventoryExtensionsTests
{
	[TestMethod]
	public void RandomizeTest()
	{
		IInventory inventory = DomainFactory.CreateInventory();

		inventory.Randomize();

		Assert.AreNotEqual(0, inventory.Money);
	}
}