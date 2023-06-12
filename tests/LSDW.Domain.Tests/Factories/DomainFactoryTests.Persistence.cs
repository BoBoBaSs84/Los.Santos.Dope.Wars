using LSDW.Domain.Classes.Persistence;
using LSDW.Domain.Constants;
using LSDW.Domain.Enumerators;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
using LSDW.Domain.Helpers;
using LSDW.Domain.Interfaces.Actors;
using LSDW.Domain.Interfaces.Models;

namespace LSDW.Domain.Tests.Factories;

[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public partial class DomainFactoryTests
{
	[TestMethod]
	public void CreateDrugStateTest()
	{
		IDrug drug = DomainFactory.CreateDrug();

		DrugState state = DomainFactory.CreateDrugState(drug);

		Assert.IsNotNull(state);
		Assert.AreEqual(drug.Type, state.Type);
		Assert.AreEqual(drug.Quantity, state.Quantity);
		Assert.AreEqual(drug.Price, state.Price);
	}

	[TestMethod]
	public void CreateDrugStatesTest()
	{
		IEnumerable<IDrug> drugs = DomainFactory.CreateAllDrugs();

		List<DrugState> states = DomainFactory.CreateDrugStates(drugs);

		Assert.IsNotNull(states);
		Assert.AreEqual(drugs.Count(), states.Count);
	}

	[TestMethod]
	public void CreateInventoryStateTest()
	{
		int money = 1000;
		IInventory inventory = DomainFactory.CreateInventory(money);

		InventoryState state = DomainFactory.CreateInventoryState(inventory);

		Assert.IsNotNull(state);
		Assert.IsNotNull(state.Drugs);
		Assert.AreEqual(money, state.Money);
	}

	[TestMethod]
	public void CreatePlayerStateTest()
	{
		IInventory inventory = DomainFactory.CreateInventory();
		int experience = RandomHelper.GetInt(123456789, 987654321);
		IEnumerable<ITransaction> transactions = new List<ITransaction>()
		{
			DomainFactory.CreateTransaction(DateTime.Now, TransactionType.TRAFFIC, DrugType.COKE, 10, 1000),
			DomainFactory.CreateTransaction(DateTime.Now.AddDays(-1), TransactionType.DEPOSIT, DrugType.METH, 15, 2500),
		};
		IPlayer player = DomainFactory.CreatePlayer(inventory, experience, transactions);
		player.Inventory.Randomize(player.Level);

		PlayerState state = DomainFactory.CreatePlayerState(player);

		Assert.IsNotNull(state);
		Assert.AreEqual(player.Experience, state.Experience);
		Assert.AreEqual(player.Inventory.Count, state.Inventory.Drugs.Count);
		Assert.AreEqual(player.Inventory.Money, state.Inventory.Money);
		Assert.AreEqual(player.Transactions.Count, state.Transactions.Count);
	}

	[TestMethod]
	public void CreateDealerStatesTest()
	{
		ICollection<IDealer> dealers = new List<IDealer>()
		{
			DomainFactory.CreateDealer(zeroVector, pedHash),
			DomainFactory.CreateDealer(zeroVector, pedHash),
		};

		List<DealerState> states = DomainFactory.CreateDealerStates(dealers);

		Assert.IsNotNull(dealers);
		Assert.AreEqual(dealers.Count(), states.Count);
	}

	[TestMethod]
	public void CreateDealerStateTest()
	{
		string name = NameConstants.GetFullName();
		IInventory drugs = DomainFactory.CreateInventory(10000);
		IDealer dealer = DomainFactory.CreateDealer(zeroVector, pedHash, DateTime.MinValue, true, drugs, name);

		DealerState state = DomainFactory.CreateDealerState(dealer);

		Assert.IsNotNull(state);
		Assert.IsNotNull(state.Inventory);
		Assert.IsNotNull(state.Inventory.Drugs);
		Assert.IsTrue(state.Inventory.Drugs.Any());
		Assert.AreEqual(drugs.Money, state.Inventory.Money);
		Assert.AreEqual(dealer.Position, state.Position);
		Assert.AreEqual(dealer.Hash, state.Hash);
		Assert.AreEqual(dealer.ClosedUntil, state.ClosedUntil);
		Assert.AreEqual(dealer.Discovered, state.Discovered);
		Assert.AreEqual(dealer.Name, state.Name);
	}

	[TestMethod]
	public void CreateTransactionStateTest()
	{
		ITransaction transaction = DomainFactory.CreateTransaction(DateTime.MinValue, TransactionType.TRAFFIC, DrugType.COKE, 10, 1000);

		TransactionState state = DomainFactory.CreateTransactionState(transaction);

		Assert.IsNotNull(state);
		Assert.AreEqual(transaction.DateTime, state.DateTime);
		Assert.AreEqual(transaction.Type, state.Type);
		Assert.AreEqual(transaction.DrugType, state.DrugType);
		Assert.AreEqual(transaction.Quantity, state.Quantity);
		Assert.AreEqual(transaction.Price, state.Price);
	}

	[TestMethod]
	public void CreateTransactionStatesTest()
	{
		IEnumerable<ITransaction> transactions = new List<ITransaction>()
		{
			DomainFactory.CreateTransaction(DateTime.MinValue, TransactionType.TRAFFIC, DrugType.COKE, 10, 1000),
			DomainFactory.CreateTransaction(DateTime.MinValue, TransactionType.DEPOSIT, DrugType.METH, 10, 0),
		};

		List<TransactionState> states = DomainFactory.CreateTransactionStates(transactions);

		Assert.IsNotNull(states);
		Assert.AreEqual(transactions.Count(), states.Count);
	}

	[TestMethod]
	public void CreateGameStateTest()
	{
		IPlayer player = DomainFactory.CreatePlayer(RandomHelper.GetInt(123456789, 987654321));
		player.Inventory.Randomize(player.Level);
		List<IDealer> dealers = new()
		{
			DomainFactory.CreateDealer(zeroVector),
			DomainFactory.CreateDealer(zeroVector),
		};
		foreach (IDealer dealer in dealers)
			_ = dealer.Inventory.Randomize(player.Level);

		GameState state = DomainFactory.CreateGameState(player, dealers);

		Assert.IsNotNull(state);
		Assert.AreEqual(player.Experience, state.Player.Experience);
		Assert.AreEqual(player.Inventory.Money, state.Player.Inventory.Money);
		Assert.AreEqual(player.Inventory.Count, state.Player.Inventory.Drugs.Count);
		Assert.AreEqual(dealers.Count, state.Dealers.Count);
		Assert.AreEqual(dealers.Count, state.Dealers.Count);
	}
}
