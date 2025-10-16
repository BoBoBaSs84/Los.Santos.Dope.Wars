using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Domain.Factories;
using LSDW.Domain.Models;

namespace LSDW.Domain.Tests.Models;

[TestClass, ExcludeFromCodeCoverage]
public class PlayerTests
{
	[ClassInitialize]
	public static void ClassInitialize(TestContext context)
	{
		Settings.Instance.Player.ExperienceMultiplier.Value = 1;
		Settings.Instance.Player.StartingInventory.Value = 100;
		Settings.Instance.Player.InventoryExpansionPerLevel.Value = 10;
	}

	[TestMethod]
	public void AddExperienceTest()
	{
		IPlayer player = DomainFactory.CreatePlayer();
		int pointsToAdd = 500;

		player.AddExperience(pointsToAdd);

		Assert.AreEqual(pointsToAdd, player.Experience);
	}

	[TestMethod]
	public void CurrentLevelTest()
	{
		IPlayer player = DomainFactory.CreatePlayer();
		int pointsToAdd = 1500;

		player.AddExperience(pointsToAdd);

		Assert.AreEqual(1, player.Level);
	}

	[TestMethod]
	public void MaximumInventoryQuantityTest()
	{
		IPlayer player = DomainFactory.CreatePlayer();
		int pointsToAdd = 10000;

		player.AddExperience(pointsToAdd);

		Assert.AreEqual(120, player.MaximumInventoryQuantity);
	}

	[TestMethod]
	public void NextLevelExperienceTest()
	{
		int experience = 1000;

		IPlayer player = DomainFactory.CreatePlayer(experience);

		Assert.AreEqual(8000, player.ExperienceNextLevel);
	}

	[TestMethod]
	public void AddGoodTransactionTest()
	{
		IPlayer player = DomainFactory.CreatePlayer();
		ITransaction transaction =
			DomainFactory.CreateTransaction(DateTime.Now, TransactionType.BUY, DrugType.COKE, 10, 50);

		player.AddTransaction(transaction);

		Assert.AreNotEqual(default, player.Experience);
	}

	[TestMethod]
	public void AddBadTransactionTest()
	{
		IPlayer player = DomainFactory.CreatePlayer();
		ITransaction transaction =
			DomainFactory.CreateTransaction(DateTime.Now, TransactionType.BUY, DrugType.COKE, 10, 150);

		player.AddTransaction(transaction);

		Assert.AreEqual(default, player.Experience);
	}

	[TestMethod]
	public void GetTransactionsTest()
	{
		IPlayer player = DomainFactory.CreatePlayer();
		ITransaction transaction =
			DomainFactory.CreateTransaction(DateTime.Now, TransactionType.BUY, DrugType.COKE, 10, 50);

		player.AddTransaction(transaction);
		ICollection<ITransaction> transactions = player.GetTransactions();

		Assert.IsNotNull(transactions);
		Assert.HasCount(player.TransactionCount, transactions);
	}

	[TestMethod]
	public void PlayerSettingsChangeTest()
	{
		Settings.Instance.Player.ExperienceMultiplier.Value = 2.0f;
		Settings.Instance.Player.StartingInventory.Value = 150;
		Settings.Instance.Player.InventoryExpansionPerLevel.Value = 25;

		IPlayer player = DomainFactory.CreatePlayer();
		ITransaction transaction =
			DomainFactory.CreateTransaction(DateTime.Now, TransactionType.SELL, DrugType.COKE, 100, 200);

		player.AddTransaction(transaction);

		Assert.AreEqual(20000, player.Experience);
		Assert.AreEqual(27000, player.ExperienceNextLevel);
		Assert.AreEqual(2, player.Level);
		Assert.AreEqual(200, player.MaximumInventoryQuantity);
		Assert.AreEqual(1, player.TransactionCount);
	}
}