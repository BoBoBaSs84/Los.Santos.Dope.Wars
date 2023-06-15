using GTA.UI;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Services;
using LSDW.Abstractions.Enumerators;
using LSDW.Domain.Factories;

namespace LSDW.Domain.Tests.Classes.Services;

[TestClass]
public class TransactionServiceTests
{
	private readonly INotificationService _notificationService = new TestNotificationService();

	[TestMethod]
	public void CommitGiveSuccessTest()
	{
		IDrug drug = DomainFactory.CreateDrug(DrugType.COKE, 10, 90);
		IPlayer player = DomainFactory.CreatePlayer(32000);
		player.Inventory.Add(1000);
		IInventory inventory = DomainFactory.CreateInventory();
		inventory.Add(drug);

		ITransactionService transactionService =
			DomainFactory.CreateTransactionService(_notificationService, TransactionType.GIVE, inventory, player.Inventory, player.MaximumInventoryQuantity);

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
			DomainFactory.CreateTransactionService(_notificationService, TransactionType.BUY, inventory, player.Inventory, player.MaximumInventoryQuantity);

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
			DomainFactory.CreateTransactionService(_notificationService, TransactionType.BUY, inventory, player.Inventory, player.MaximumInventoryQuantity);

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
			DomainFactory.CreateTransactionService(_notificationService, TransactionType.GIVE, player.Inventory, inventory, 0);

		bool success = transactionService.Commit(drug.Type, drug.Quantity, drug.CurrentPrice);

		Assert.IsFalse(success);
	}

	private sealed class TestNotificationService : INotificationService
	{
		public Task Show(string message, bool blinking = false, int duration = 2500)
			=> Task.CompletedTask;
		public Task Show(NotificationIcon icon, string sender, string subject, string message, bool fadeIn = false, bool blinking = false, int duration = 2500)
			=> Task.CompletedTask;
		public void ShowHelpText(string helpText, int duration = -1, bool beep = true, bool looped = false)
			=> Trace.WriteLine($"{nameof(helpText)}: {helpText}");
		public void ShowHelpTextThisFrame(string helpText)
			=> Trace.WriteLine($"{nameof(helpText)}: {helpText}");
		public void ShowHelpTextThisFrame(string helpText, bool beep)
			=> Trace.WriteLine($"{nameof(helpText)}: {helpText}");
		public void ShowSubtitle(string message, int duration = 2500)
			=> Trace.WriteLine($"{nameof(message)}: {message}");
		public void ShowSubtitle(string message, int duration, bool drawImmediately = true)
			=> Trace.WriteLine($"{nameof(message)}: {message}");
	}
}
