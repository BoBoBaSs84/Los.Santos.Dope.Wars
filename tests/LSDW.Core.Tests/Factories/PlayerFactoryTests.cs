using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Tests.Factories;

[TestClass]
public class PlayerFactoryTests
{
	[TestMethod]
	public void CreatePlayerTest()
	{
		IPlayer? player;

		player = PlayerFactory.CreatePlayer();

		Assert.IsNotNull(player);
		Assert.AreEqual(0, player.Experience);
	}

	[TestMethod]
	public void CreatePlayerWithExperienceTest()
	{
		IPlayer? player;
		int experience = 38934;

		player = PlayerFactory.CreatePlayer(experience);

		Assert.IsNotNull(player);
		Assert.AreEqual(experience, player.Experience);
	}

	[TestMethod]
	public void CreateExistingPlayerTest()
	{
		IPlayer? player;
		IInventory inventory = InventoryFactory.CreateInventory();
		int money = 12000;
		inventory.Add(money);
		int experience = money;

		player = PlayerFactory.CreatePlayer(inventory, experience);

		Assert.IsNotNull(player);
		Assert.AreEqual(money, player.Inventory.Money);
		Assert.AreEqual(experience, player.Experience);
	}
}