using LSDW.Core.Enumerators;
using LSDW.Core.Extensions;
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
	public void CreatePlayerWithExperienceAndInventoryTest()
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

	[TestMethod()]
	public void CreateExistingPlayerTest()
	{
		IPlayer? player;
		IInventory inventory = InventoryFactory.CreateInventory();
		_ = inventory.Randomize();
		int experience = 500;
		IEnumerable<ILogEntry> logEntries = new List<ILogEntry>()
		{
			LogEntryFactory.CreateLogEntry(DateTime.Now, TransactionType.TRAFFIC, DrugType.COKE, 10, 1000),
			LogEntryFactory.CreateLogEntry(DateTime.Now.AddDays(-1), TransactionType.DEPOSIT, DrugType.METH, 10, 0),
		};

		player = PlayerFactory.CreatePlayer(inventory, experience, logEntries);

		Assert.IsNotNull(player);
		Assert.AreEqual(experience, player.Experience);
		Assert.AreEqual(inventory.Money, player.Inventory.Money);
		Assert.AreEqual(inventory.Count, player.Inventory.Count);
		Assert.AreEqual(inventory.TotalQuantity, player.Inventory.TotalQuantity);
		Assert.AreEqual(logEntries.Count(), player.Transactions.Count);
	}
}