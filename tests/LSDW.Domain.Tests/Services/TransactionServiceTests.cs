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
	private readonly Mock<IPlayer> _playerMock = MockHelper.GetPlayer();
	private readonly Mock<IInventory> _inventoryMock = MockHelper.GetInventory();

	[TestMethod]
	public void CommitGiveSuccessTest()
	{
		IDrug drug = DomainFactory.CreateDrug(DrugType.COKE, 10, 100);
		IPlayer player = DomainFactory.CreatePlayer();
		player.Inventory.Add(drug);
		IInventory inventory = DomainFactory.CreateInventory();
		inventory.Add(1000);

		ITransactionService transactionService =
			DomainFactory.CreateTransactionService(_providerManagerMock.Object, TransactionType.GIVE, player, inventory);

		bool success = transactionService.Commit(drug.Type, drug.Quantity, drug.CurrentPrice);

		Assert.IsTrue(success);
		Assert.AreEqual(10, inventory.TotalQuantity);
		Assert.AreEqual(drug.Quantity, inventory.TotalQuantity);
		Assert.AreEqual(drug.Quantity * drug.CurrentPrice, inventory.TotalValue);
		Assert.AreEqual(1000, inventory.Money);
		Assert.AreEqual(0, player.Inventory.Money);
		Assert.AreEqual(1, player.TransactionCount);
	}

	[TestMethod]
	public void CommitBuySuccessTest()
	{
		Mock<IPlayerProvider> playerProviderMock = MockHelper.GetPlayerProvider();
		playerProviderMock.Object.Money = 1000;
		Mock<INotificationProvider> notificationProviderMock = MockHelper.GetNotificationProvider();
		IDrug drug = DomainFactory.CreateDrug(DrugType.COKE, 10, 90);
		IPlayer player = DomainFactory.CreatePlayer();
		IInventory inventory = DomainFactory.CreateInventory();
		inventory.Add(drug);
		_ = _providerManagerMock.Setup(x => x.PlayerProvider).Returns(playerProviderMock.Object);
		_ = _providerManagerMock.Setup(x => x.NotificationProvider).Returns(notificationProviderMock.Object);

		ITransactionService transactionService =
			DomainFactory.CreateTransactionService(_providerManagerMock.Object, TransactionType.BUY, player, inventory);

		bool success = transactionService.Commit(drug.Type, drug.Quantity, drug.CurrentPrice);

		Assert.IsTrue(success);
		Assert.AreEqual(0, inventory.TotalQuantity);
		Assert.AreEqual(drug.Quantity, player.Inventory.TotalQuantity);
		Assert.AreEqual(drug.Quantity * drug.CurrentPrice, player.Inventory.TotalValue);
		Assert.AreEqual(100, playerProviderMock.Object.Money);
		Assert.AreEqual(900, inventory.Money);
	}

	[TestMethod]
	public void CommitSellSuccessTest()
	{
		Mock<IPlayerProvider> playerProviderMock = MockHelper.GetPlayerProvider();
		playerProviderMock.Object.Money = 0;
		Mock<INotificationProvider> notificationProviderMock = MockHelper.GetNotificationProvider();
		IDrug drug = DomainFactory.CreateDrug(DrugType.COKE, 10, 90);
		IPlayer player = DomainFactory.CreatePlayer();
		player.Inventory.Add(drug);
		IInventory inventory = DomainFactory.CreateInventory(1000);
		_ = _providerManagerMock.Setup(x => x.PlayerProvider).Returns(playerProviderMock.Object);
		_ = _providerManagerMock.Setup(x => x.NotificationProvider).Returns(notificationProviderMock.Object);

		ITransactionService transactionService =
			DomainFactory.CreateTransactionService(_providerManagerMock.Object, TransactionType.SELL, player, inventory);

		bool success = transactionService.Commit(drug.Type, drug.Quantity, drug.CurrentPrice);

		Assert.IsTrue(success);
		Assert.AreEqual(drug.Quantity, inventory.TotalQuantity);
		Assert.AreEqual(default, player.Inventory.TotalQuantity);
		Assert.AreEqual(drug.Quantity * drug.CurrentPrice, inventory.TotalValue);
		Assert.AreEqual(900, playerProviderMock.Object.Money);
		Assert.AreEqual(100, inventory.Money);
	}

	[TestMethod]
	public void CommitBuyNotEnoughMoneyTest()
	{
		IDrug drug = DomainFactory.CreateDrug(DrugType.COKE, 10, 90);
		IPlayer player = DomainFactory.CreatePlayer();
		IInventory inventory = DomainFactory.CreateInventory();
		inventory.Add(drug);

		ITransactionService transactionService =
			DomainFactory.CreateTransactionService(_providerManagerMock.Object, TransactionType.BUY, player, inventory);

		bool success = transactionService.Commit(drug.Type, drug.Quantity, drug.CurrentPrice);

		Assert.IsFalse(success);
	}

	[TestMethod]
	public void CommitSellNotEnoughMoneyTest()
	{
		IDrug drug = DomainFactory.CreateDrug(DrugType.COKE, 10, 90);
		IPlayer player = DomainFactory.CreatePlayer();
		player.Inventory.Add(drug);
		IInventory inventory = DomainFactory.CreateInventory(800);

		ITransactionService transactionService =
			DomainFactory.CreateTransactionService(_providerManagerMock.Object, TransactionType.SELL, player, inventory);

		bool success = transactionService.Commit(drug.Type, drug.Quantity, drug.CurrentPrice);

		Assert.IsFalse(success);
	}

	[TestMethod]
	public void CommitGiveNotEnoughInventoryTest()
	{
		IDrug drug = DomainFactory.CreateDrug(DrugType.COKE, 200, 100);
		IPlayer player = DomainFactory.CreatePlayer();
		IInventory inventory = DomainFactory.CreateInventory();
		inventory.Add(drug);

		ITransactionService transactionService =
			DomainFactory.CreateTransactionService(_providerManagerMock.Object, TransactionType.TAKE, player, inventory);

		bool success = transactionService.Commit(drug.Type, drug.Quantity, drug.CurrentPrice);

		Assert.IsFalse(success);
		Assert.AreEqual(200, inventory.TotalQuantity);
		Assert.AreEqual(200 * 100, inventory.TotalValue);
		Assert.AreEqual(default, player.TransactionCount);
	}

	[DataTestMethod]
	[DataRow(TransactionType.GIVE)]
	[DataRow(TransactionType.TAKE)]
	public void BustOrNoBustGiveTakeTest(TransactionType transactionType)
	{
		ITransactionService transactionService =
			DomainFactory.CreateTransactionService(_providerManagerMock.Object, transactionType, _playerMock.Object, _inventoryMock.Object);

		transactionService.BustOrNoBust();
	}

	[TestMethod]
	public void BustOrNoBustBelowBustChanceTest()
	{
		TransactionType transactionType = TransactionType.BUY;
		Mock<INotificationProvider> notificationProviderMock = MockHelper.GetNotificationProvider();
		Mock<IRandomProvider> mockRandomProvider = MockHelper.GetRandomProvider();
		_ = mockRandomProvider.Setup(x => x.GetFloat()).Returns(0.15f);
		Mock<IPlayerProvider> mockPlayerProvider = MockHelper.GetPlayerProvider();
		Mock<IProviderManager> providerManagerMock = new(MockBehavior.Loose);
		_ = providerManagerMock.Setup(x => x.PlayerProvider).Returns(mockPlayerProvider.Object);
		_ = providerManagerMock.Setup(x => x.RandomProvider).Returns(mockRandomProvider.Object);
		_ = providerManagerMock.Setup(x => x.NotificationProvider).Returns(notificationProviderMock.Object);
		ITransactionService transactionService =
			DomainFactory.CreateTransactionService(_providerManagerMock.Object, transactionType, _playerMock.Object, _inventoryMock.Object);

		transactionService.BustOrNoBust();
	}

	[TestMethod]
	public void BustOrNoBustAboveBustChanceTest()
	{
		TransactionType transactionType = TransactionType.BUY;
		Mock<INotificationProvider> notificationProviderMock = MockHelper.GetNotificationProvider();
		Mock<IRandomProvider> mockRandomProvider = MockHelper.GetRandomProvider();
		_ = mockRandomProvider.Setup(x => x.GetFloat()).Returns(0.05f);
		Mock<IPlayerProvider> playerProviderMock = MockHelper.GetPlayerProvider();
		Mock<IProviderManager> providerManagerMock = new(MockBehavior.Loose);
		_ = providerManagerMock.Setup(x => x.PlayerProvider).Returns(playerProviderMock.Object);
		_ = providerManagerMock.Setup(x => x.RandomProvider).Returns(mockRandomProvider.Object);
		_ = providerManagerMock.Setup(x => x.NotificationProvider).Returns(notificationProviderMock.Object);
		ITransactionService transactionService =
			DomainFactory.CreateTransactionService(providerManagerMock.Object, transactionType, _playerMock.Object, _inventoryMock.Object);

		transactionService.BustOrNoBust();

		notificationProviderMock.Verify(x => x.ShowSubtitle(Resources.Transaction_Message_Bust, 2500));
		Assert.AreEqual(Settings.Trafficking.WantedLevel, playerProviderMock.Object.WantedLevel);
	}
}
