using LSDW.Abstractions.Domain.Models;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;

namespace LSDW.Domain.Tests.Extensions;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public class InventoryExtensionsTests
{
	[TestMethod]
	public void RestockTest()
	{
		IInventory inventory = DomainFactory.CreateInventory();

		inventory.Restock();

		Assert.AreNotEqual(default, inventory.Money);
	}

	[TestMethod()]
	public void RefreshTest()
	{
		IInventory inventory = DomainFactory.CreateInventory();

		inventory.Refresh();

		Assert.AreEqual(default, inventory.Money);
	}
}