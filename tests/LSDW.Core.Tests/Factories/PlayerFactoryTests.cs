using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Tests.Factories;

[TestClass]
public class PlayerFactoryTests
{
	[TestMethod]
	public void CreatePlayerTest()
	{
		IPlayer? character;

		character = PlayerFactory.CreatePlayer();

		Assert.IsNotNull(character);
	}

	[TestMethod]
	public void CreateExistingPlayerTest()
	{
		IPlayer? character;
		IInventory inventory = InventoryFactory.CreateInventory();
		int money = 12000;
		inventory.Add(money);
		int experience = money;

		character = PlayerFactory.CreatePlayer(inventory, experience);

		Assert.IsNotNull(character);
		Assert.AreEqual(money, character.Inventory.Money);
		Assert.AreEqual(experience, character.Experience);
	}
}