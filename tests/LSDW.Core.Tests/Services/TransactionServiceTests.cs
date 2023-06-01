using LSDW.Core.Enumerators;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Models;
using LSDW.Core.Interfaces.Services;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace LSDW.Core.Tests.Services;

[TestClass]
public class TransactionServiceTests
{
	private readonly ICollection<ITransaction> _transactions = new HashSet<ITransaction>();

	[TestMethod]
	public void CommitDepositSuccessTest()
	{
		IDrug drug = ModelFactory.CreateDrug(DrugType.COKE, 10, 90);
		IPlayer player = ModelFactory.CreatePlayer(32000);
		player.Inventory.Add(1000);
		IInventory inventory = ModelFactory.CreateInventory();
		inventory.Add(drug);

		ITransactionService transactionService =
			ServiceFactory.CreateTransactionService(TransactionType.DEPOSIT, inventory, player.Inventory, player.MaximumInventoryQuantity);

		transactionService.TransactionsChanged += OnTransactionsChanged;

		bool success = transactionService.Commit(drug.DrugType, drug.Quantity, drug.Price);

		Assert.IsTrue(success);
		Assert.AreEqual(0, inventory.TotalQuantity);
		Assert.AreEqual(drug.Quantity, player.Inventory.TotalQuantity);
		Assert.AreEqual(drug.Quantity * drug.Price, player.Inventory.TotalValue);
		Assert.AreEqual(1000, player.Inventory.Money);
	}

	[TestMethod]
	public void CommitTrafficSuccessTest()
	{
		IDrug drug = ModelFactory.CreateDrug(DrugType.COKE, 10, 90);
		IPlayer player = ModelFactory.CreatePlayer();
		player.Inventory.Add(1000);
		IInventory inventory = ModelFactory.CreateInventory();
		inventory.Add(drug);

		ITransactionService transactionService =
			ServiceFactory.CreateTransactionService(TransactionType.TRAFFIC, inventory, player.Inventory, player.MaximumInventoryQuantity);

		bool success = transactionService.Commit(drug.DrugType, drug.Quantity, drug.Price);

		Assert.IsTrue(success);
		Assert.AreEqual(0, inventory.TotalQuantity);
		Assert.AreEqual(drug.Quantity, player.Inventory.TotalQuantity);
		Assert.AreEqual(drug.Quantity * drug.Price, player.Inventory.TotalValue);
		Assert.AreEqual(100, player.Inventory.Money);
		Assert.AreEqual(900, inventory.Money);
	}

	[TestMethod]
	public void CommitTrafficNotEnoughMoneyTest()
	{
		IDrug drug = ModelFactory.CreateDrug(DrugType.COKE, 10, 90);
		IPlayer player = ModelFactory.CreatePlayer();
		player.Inventory.Add(800);
		IInventory inventory = ModelFactory.CreateInventory();
		inventory.Add(drug);

		ITransactionService transactionService =
			ServiceFactory.CreateTransactionService(TransactionType.TRAFFIC, inventory, player.Inventory, player.MaximumInventoryQuantity);

		bool success = transactionService.Commit(drug.DrugType, drug.Quantity, drug.Price);

		Assert.IsFalse(success);
		Assert.IsTrue(transactionService.Errors.Any());
	}

	[TestMethod]
	public void CommitDepositNotEnoughInventoryTest()
	{
		IDrug drug = ModelFactory.CreateDrug(DrugType.COKE, 100, 100);
		IPlayer player = ModelFactory.CreatePlayer();
		IInventory inventory = ModelFactory.CreateInventory();
		inventory.Add(drug);

		ITransactionService transactionService =
			ServiceFactory.CreateTransactionService(TransactionType.DEPOSIT, player.Inventory, inventory, 0);

		bool success = transactionService.Commit(drug.DrugType, drug.Quantity, drug.Price);

		Assert.IsFalse(success);
		Assert.IsTrue(transactionService.Errors.Any());
	}

	private void OnTransactionsChanged(object sender, NotifyCollectionChangedEventArgs args)
	{
		if (sender is not ObservableCollection<ITransaction> transactions)
			return;

		if (args.Action == NotifyCollectionChangedAction.Add)
			_transactions.Add(transactions[args.NewStartingIndex]);
	}
}