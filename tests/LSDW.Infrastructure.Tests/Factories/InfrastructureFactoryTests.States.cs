using GTA;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Domain.Constants;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
using LSDW.Domain.Helpers;
using LSDW.Infrastructure.Constants;
using LSDW.Infrastructure.Models;
using static LSDW.Infrastructure.Factories.InfrastructureFactory;

namespace LSDW.Infrastructure.Tests.Factories;

[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public partial class InfrastructureFactoryTests
{
	[TestMethod]
	public void CreateDrugFromStateTest()
	{
		DrugState state = GetDrugState();

		IDrug drug = CreateDrug(state);

		Assert.IsNotNull(drug);
		Assert.AreEqual(state.Type, drug.Type);
		Assert.AreEqual(state.Quantity, drug.Quantity);
		Assert.AreEqual(state.CurrentPrice, drug.CurrentPrice);
	}

	[TestMethod]
	public void CreateDrugsFromStatesTest()
	{
		List<DrugState> states = new() { GetDrugState(), GetDrugState() };

		ICollection<IDrug> drugs = CreateDrugs(states);

		Assert.IsNotNull(drugs);
		Assert.AreEqual(states.Count, drugs.Count);
	}

	[TestMethod]
	public void CreateDrugStateTest()
	{
		IDrug drug = DomainFactory.CreateDrug();

		DrugState state = CreateDrugState(drug);

		Assert.IsNotNull(state);
		Assert.AreEqual(drug.Type, state.Type);
		Assert.AreEqual(drug.Quantity, state.Quantity);
		Assert.AreEqual(drug.CurrentPrice, state.CurrentPrice);
	}

	[TestMethod]
	public void CreateDrugStatesTest()
	{
		ICollection<IDrug> drugs = DomainFactory.CreateAllDrugs();

		List<DrugState> states = CreateDrugStates(drugs);

		Assert.IsNotNull(states);
		Assert.AreEqual(drugs.Count, states.Count);
	}

	[TestMethod]
	public void CreateInventoryStateTest()
	{
		int money = 1000;
		IInventory inventory = DomainFactory.CreateInventory(money);

		InventoryState state = CreateInventoryState(inventory);

		Assert.IsNotNull(state);
		Assert.IsNotNull(state.Drugs);
		Assert.AreEqual(money, state.Money);
	}

	[TestMethod]
	public void CreatePlayerStateTest()
	{
		IInventory inventory = DomainFactory.CreateInventory();
		int experience = RandomHelper.GetInt(123456789, 987654321);
		ICollection<ITransaction> transactions = new List<ITransaction>()
		{
			DomainFactory.CreateTransaction(DateTime.Now, TransactionType.BUY, DrugType.COKE, 10, 75),
			DomainFactory.CreateTransaction(DateTime.Now.AddDays(-1), TransactionType.SELL, DrugType.METH, 15, 125),
		};
		IPlayer player = DomainFactory.CreatePlayer(inventory, experience, transactions);
		player.Inventory.Restock(player.Level);

		PlayerState state = CreatePlayerState(player);

		Assert.IsNotNull(state);
		Assert.AreEqual(player.Experience, state.Experience);
		Assert.AreEqual(player.Inventory.Count, state.Inventory.Drugs.Count);
		Assert.AreEqual(player.Inventory.Money, state.Inventory.Money);
		Assert.AreEqual(player.TransactionCount, state.Transactions.Count);
	}

	[TestMethod]
	public void CreateDealerStatesTest()
	{
		ICollection<IDealer> dealers = new List<IDealer>()
		{
			DomainFactory.CreateDealer(_zeroVector, _pedHash),
			DomainFactory.CreateDealer(_zeroVector, _pedHash),
		};

		List<DealerState> states = CreateDealerStates(dealers);

		Assert.IsNotNull(dealers);
		Assert.AreEqual(dealers.Count, states.Count);
	}

	[TestMethod]
	public void CreateDealerStateTest()
	{
		DateTime date = DateTime.Now;
		string name = NameConstants.GetFullName();
		IInventory drugs = DomainFactory.CreateInventory(10000);
		IDealer dealer = DomainFactory.CreateDealer(_zeroVector, _pedHash, name, date, true, drugs, date, date);

		DealerState state = CreateDealerState(dealer);

		Assert.IsNotNull(state);
		Assert.IsNotNull(state.Inventory);
		Assert.IsNotNull(state.Inventory.Drugs);
		Assert.IsFalse(Equals(state.Inventory.Drugs.Count, 0));
		Assert.AreEqual(drugs.Money, state.Inventory.Money);
		Assert.AreEqual(dealer.SpawnPosition, state.SpawnPosition);
		Assert.AreEqual(dealer.Hash, state.Hash);
		Assert.AreEqual(dealer.ClosedUntil, state.ClosedUntil);
		Assert.AreEqual(dealer.Discovered, state.Discovered);
		Assert.AreEqual(dealer.Name, state.Name);
		Assert.AreEqual(dealer.NextPriceChange, state.NextPriceChange);
		Assert.AreEqual(dealer.NextInventoryChange, state.NextInventoryChange);
	}

	[TestMethod]
	public void CreateInventoryFromStateTest()
	{
		InventoryState state = GetInventoryState();

		IInventory inventory = CreateInventory(state);

		Assert.IsNotNull(inventory);
		Assert.AreEqual(state.Money, inventory.Money);
		Assert.AreNotEqual(state.Drugs.Count, inventory.Count);
	}

	[TestMethod]
	public void CreateTransactionFromStateTest()
	{
		TransactionState state = GetTransactionState();

		ITransaction transaction = CreateTransaction(state);

		Assert.IsNotNull(transaction);
		Assert.AreEqual(state.DateTime, transaction.DateTime);
		Assert.AreEqual(state.DrugType, transaction.DrugType);
		Assert.AreEqual(state.Type, transaction.Type);
		Assert.AreEqual(state.Price, transaction.Price);
		Assert.AreEqual(state.Quantity, transaction.Quantity);
	}

	[TestMethod]
	public void CreateTransactionsFromStatesTest()
	{
		List<TransactionState> states =
			new() { GetTransactionState(), GetTransactionState() };

		ICollection<ITransaction> transactions = CreateTransactions(states);

		Assert.IsNotNull(transactions);
		Assert.AreEqual(states.Count, transactions.Count);
	}

	[TestMethod]
	public void CreateTransactionStateTest()
	{
		ITransaction transaction = DomainFactory.CreateTransaction(DateTime.MinValue, TransactionType.TAKE, DrugType.COKE, 10, 100);

		TransactionState state = CreateTransactionState(transaction);

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
		ICollection<ITransaction> transactions = new List<ITransaction>()
		{
			DomainFactory.CreateTransaction(DateTime.MinValue, TransactionType.SELL, DrugType.COKE, 10, 100),
			DomainFactory.CreateTransaction(DateTime.MinValue, TransactionType.SELL, DrugType.METH, 10, 125),
		};

		List<TransactionState> states = CreateTransactionStates(transactions);

		Assert.IsNotNull(states);
		Assert.AreEqual(transactions.Count, states.Count);
	}

	[TestMethod]
	public void CreateGameStateTest()
	{
		IPlayer player = DomainFactory.CreatePlayer(RandomHelper.GetInt(123456789, 987654321));
		player.Inventory.Restock(player.Level);
		List<IDealer> dealers = new()
		{
			DomainFactory.CreateDealer(_zeroVector),
			DomainFactory.CreateDealer(_zeroVector),
		};
		foreach (IDealer dealer in dealers)
			_ = dealer.Inventory.Restock(player.Level);

		State state = CreateGameState(dealers, player);

		Assert.IsNotNull(state);
		Assert.AreEqual(player.Experience, state.Player.Experience);
		Assert.AreEqual(player.Inventory.Money, state.Player.Inventory.Money);
		Assert.AreEqual(player.Inventory.Count, state.Player.Inventory.Drugs.Count);
		Assert.AreEqual(dealers.Count, state.Dealers.Count);
		Assert.AreEqual(dealers.Count, state.Dealers.Count);
	}

	[TestMethod]
	public void CreateDealerFromStateTest()
	{
		DealerState state = GetDealerState();

		IDealer dealer = CreateDealer(state);

		Assert.IsNotNull(dealer);
		Assert.IsNotNull(dealer.Inventory);
		Assert.IsTrue(dealer.Closed);
		Assert.AreEqual(TaskType.NOTASK, dealer.CurrentTask);
		Assert.AreEqual(state.ClosedUntil, dealer.ClosedUntil);
		Assert.AreEqual(state.Discovered, dealer.Discovered);
		Assert.AreEqual(state.Name, dealer.Name);
		Assert.AreEqual(state.NextInventoryChange, dealer.NextInventoryChange);
		Assert.AreEqual(state.NextPriceChange, dealer.NextPriceChange);
		Assert.AreEqual(state.SpawnPosition, dealer.SpawnPosition);
		Assert.AreEqual(state.Hash, dealer.Hash);
	}

	[TestMethod]
	public void CreateDealersFromStatesTest()
	{
		List<DealerState> states = new() { GetDealerState() };

		ICollection<IDealer> dealers = CreateDealers(states);

		Assert.IsNotNull(dealers);
		Assert.AreEqual(states.Count, dealers.Count);
	}

	[TestMethod]
	public void CreatePLayerFromState()
	{
		PlayerState state = GetPlayerState();

		IPlayer player = CreatePlayer(state);

		Assert.IsNotNull(player);
		Assert.AreEqual(state.Experience, player.Experience);
	}

	[TestMethod]
	public void CreatePLayerFromGameState()
	{
		State state = GetGameState();

		IPlayer player = CreatePlayer(state);

		Assert.IsNotNull(player);
	}

	[TestMethod]
	public void CreateDealersFromGameState()
	{
		State gameState = new();

		ICollection<IDealer> dealers = CreateDealers(gameState);

		Assert.IsNotNull(dealers);
	}

	[TestMethod]
	public void HugeGameStateTest()
	{
		IPlayer player = DomainFactory.CreatePlayer();
		for (int i = 0; i < 1000; i++)
		{
			IDrug drug = DomainFactory.CreateDrug();
			ITransaction transaction =
				DomainFactory.CreateTransaction(DateTime.Now, TransactionType.BUY, drug.Type, RandomHelper.GetInt(10, 25), RandomHelper.GetInt(drug.AveragePrice - 10, drug.AveragePrice + 10));
			player.AddTransaction(transaction);
		}
		_ = player.Inventory.Restock(player.Level);
		ICollection<IDealer> dealers = new HashSet<IDealer>();
		for (int i = 0; i < 80; i++)
		{
			IDealer dealer = DomainFactory.CreateDealer(_zeroVector);
			_ = dealer.Inventory.Restock(player.Level);
			dealer.Discovered = true;
			dealers.Add(dealer);
		}
		State gameState = CreateGameState(dealers, player);

		string gameStateXml = gameState.ToXmlString(XmlConstants.SerializerNamespaces);
		string compressedGameStateXml = gameStateXml.Compress();

		File.WriteAllText($"{nameof(gameStateXml)}.xml", gameStateXml);
		File.WriteAllText($"{nameof(compressedGameStateXml)}.xml", compressedGameStateXml);

		Assert.IsFalse(string.IsNullOrWhiteSpace(gameStateXml));
		Assert.IsFalse(string.IsNullOrWhiteSpace(compressedGameStateXml));
	}

	private static DealerState GetDealerState()
		=> new()
		{
			
			ClosedUntil = DateTime.MinValue,
			Discovered = true,
			Name = "Dealer",
			NextInventoryChange = DateTime.MinValue,
			NextPriceChange = DateTime.MinValue,
			SpawnPosition = new(0, 0, 0),
			Hash = PedHash.AcidLabCook,
			Inventory = new()
		};

	private static DrugState GetDrugState()
		=> new()
		{
			Type = DrugType.COKE,
			Quantity = 10,
			CurrentPrice = 100,
		};

	private static InventoryState GetInventoryState()
		=> new()
		{
			Drugs = new(),
			Money = 100
		};

	private static PlayerState GetPlayerState()
		=> new()
		{
			Experience = 10000,
			Inventory = new(),
			Transactions = new()
		};

	private static TransactionState GetTransactionState()
		=> new()
		{
			DateTime = DateTime.MinValue,
			DrugType = DrugType.COKE,
			Quantity = 10,
			Price = 100,
			Type = TransactionType.TAKE
		};

	private static State GetGameState()
		=> new()
		{
			Player = new(),
			Dealers = new()
		};
}
