using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Services;
using LSDW.Abstractions.Enumerators;
using LSDW.Base.Tests.Helpers;
using LSDW.Domain.Factories;
using Moq;

namespace LSDW.Domain.Tests.Services;

[TestClass]
public class TransactionServiceTests
{
	private readonly Mock<IProviderManager> _providerManagerMock = MockHelper.GetProviderManager();

	[TestMethod]
	public void CommitGiveSuccessTest()
	{
		IDrug drug = DomainFactory.CreateDrug(DrugType.COKE, 10, 90);
		IPlayer player = DomainFactory.CreatePlayer(32000);
		player.Inventory.Add(1000);
		IInventory inventory = DomainFactory.CreateInventory();
		inventory.Add(drug);

		ITransactionService transactionService =
			DomainFactory.CreateTransactionService(_providerManagerMock.Object, TransactionType.GIVE, inventory, player.Inventory, player.MaximumInventoryQuantity);

		bool success = transactionService.Commit(drug.Type, drug.Quantity, drug.CurrentPrice);

		Assert.IsTrue(success);
		Assert.AreEqual(0, inventory.TotalQuantity);
		Assert.AreEqual(drug.Quantity, player.Inventory.TotalQuantity);
		Assert.AreEqual(drug.Quantity * drug.CurrentPrice, player.Inventory.TotalValue);
		Assert.AreEqual(1000, player.Inventory.Money);
	}

	[TestMethod]
	public void CommitBuySuccessTest()
	{
		IDrug drug = DomainFactory.CreateDrug(DrugType.COKE, 10, 90);
		IPlayer player = DomainFactory.CreatePlayer();
		player.Inventory.Add(1000);
		IInventory inventory = DomainFactory.CreateInventory();
		inventory.Add(drug);

		ITransactionService transactionService =
			DomainFactory.CreateTransactionService(_providerManagerMock.Object, TransactionType.BUY, inventory, player.Inventory, player.MaximumInventoryQuantity);

		bool success = transactionService.Commit(drug.Type, drug.Quantity, drug.CurrentPrice);

		Assert.IsTrue(success);
		Assert.AreEqual(0, inventory.TotalQuantity);
		Assert.AreEqual(drug.Quantity, player.Inventory.TotalQuantity);
		Assert.AreEqual(drug.Quantity * drug.CurrentPrice, player.Inventory.TotalValue);
		Assert.AreEqual(100, player.Inventory.Money);
		Assert.AreEqual(900, inventory.Money);
	}

	[TestMethod]
	public void CommitBuyNotEnoughMoneyTest()
	{
		IDrug drug = DomainFactory.CreateDrug(DrugType.COKE, 10, 90);
		IPlayer player = DomainFactory.CreatePlayer();
		player.Inventory.Add(800);
		IInventory inventory = DomainFactory.CreateInventory();
		inventory.Add(drug);

		ITransactionService transactionService =
			DomainFactory.CreateTransactionService(_providerManagerMock.Object, TransactionType.BUY, inventory, player.Inventory, player.MaximumInventoryQuantity);

		bool success = transactionService.Commit(drug.Type, drug.Quantity, drug.CurrentPrice);

		Assert.IsFalse(success);
	}

	[TestMethod]
	public void CommitGiveNotEnoughInventoryTest()
	{
		IDrug drug = DomainFactory.CreateDrug(DrugType.COKE, 100, 100);
		IPlayer player = DomainFactory.CreatePlayer();
		IInventory inventory = DomainFactory.CreateInventory();
		inventory.Add(drug);

		ITransactionService transactionService =
			DomainFactory.CreateTransactionService(_providerManagerMock.Object, TransactionType.GIVE, player.Inventory, inventory, 0);

		bool success = transactionService.Commit(drug.Type, drug.Quantity, drug.CurrentPrice);

		Assert.IsFalse(success);
	}
}
