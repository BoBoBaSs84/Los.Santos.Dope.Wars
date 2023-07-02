using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Abstractions.Domain.Services;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Models;
using LSDW.Base.Tests.Helpers;
using LSDW.Domain.Factories;
using LSDW.Domain.Properties;
using Moq;

namespace LSDW.Domain.Tests.Services;

[TestClass]
public class TransactionServiceTests
{
	private readonly Mock<IProviderManager> _providerManagerMock = MockHelper.GetProviderManager();
	private readonly Mock<IInventory> _inventoryMock = MockHelper.GetInventory();

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

	[DataTestMethod]
	[DataRow(TransactionType.GIVE)]
	[DataRow(TransactionType.TAKE)]
	public void BustOrNoBustGiveTakeTest(TransactionType transactionType)
	{
		IProviderManager providerManager = _providerManagerMock.Object;
		ITransactionService transactionService =
			DomainFactory.CreateTransactionService(providerManager, transactionType, _inventoryMock.Object, _inventoryMock.Object);

		transactionService.BustOrNoBust();		
	}

	[TestMethod]
	public void BustOrNoBustBelowBustChanceTest()
	{
		TransactionType transactionType = TransactionType.BUY;
		Mock<INotificationProvider> notificationProviderMock = MockHelper.GetNotificationProvider();
		Mock<IRandomProvider> mockRandomProvider = MockHelper.GetRandomProvider();
		mockRandomProvider.Setup(x => x.GetFloat()).Returns(0.15f);
		Mock<IPlayerProvider> mockPlayerProvider = MockHelper.GetPlayerProvider();
		Mock<IProviderManager> providerManagerMock = new(MockBehavior.Loose);
		providerManagerMock.Setup(x=>x.PlayerProvider).Returns(mockPlayerProvider.Object);
		providerManagerMock.Setup(x=>x.RandomProvider).Returns(mockRandomProvider.Object);
		providerManagerMock.Setup(x=>x.NotificationProvider).Returns(notificationProviderMock.Object);
		ITransactionService transactionService =
			DomainFactory.CreateTransactionService(providerManagerMock.Object, transactionType, _inventoryMock.Object, _inventoryMock.Object);

		transactionService.BustOrNoBust();
	}

	[TestMethod]
	public void BustOrNoBustAboveBustChanceTest()
	{
		TransactionType transactionType = TransactionType.BUY;
		Mock<INotificationProvider> notificationProviderMock = MockHelper.GetNotificationProvider();
		Mock<IRandomProvider> mockRandomProvider = MockHelper.GetRandomProvider();
		mockRandomProvider.Setup(x => x.GetFloat()).Returns(0.05f);
		Mock<IPlayerProvider> playerProviderMock = MockHelper.GetPlayerProvider();
		Mock<IProviderManager> providerManagerMock = new(MockBehavior.Loose);
		providerManagerMock.Setup(x => x.PlayerProvider).Returns(playerProviderMock.Object);
		providerManagerMock.Setup(x => x.RandomProvider).Returns(mockRandomProvider.Object);
		providerManagerMock.Setup(x => x.NotificationProvider).Returns(notificationProviderMock.Object);
		ITransactionService transactionService =
			DomainFactory.CreateTransactionService(providerManagerMock.Object, transactionType, _inventoryMock.Object, _inventoryMock.Object);

		transactionService.BustOrNoBust();

		notificationProviderMock.Verify(x => x.ShowSubtitle(Resources.Transaction_Message_Bust, 2500));
		Assert.AreEqual(Settings.Trafficking.WantedLevel, playerProviderMock.Object.WantedLevel);
	}
}
