using LSDW.Domain.Classes.Persistence;
using LSDW.Domain.Interfaces.Actors;
using LSDW.Domain.Interfaces.Models;

namespace LSDW.Domain.Factories;

/// <summary>
/// The domain factory class.
/// </summary>
public static partial class DomainFactory
{
	/// <summary>
	/// Creates a new saveable drug state from a drug instance.
	/// </summary>
	/// <param name="drug">The drug instance to save.</param>
	public static DrugState CreateDrugState(IDrug drug)
		=> new(drug);

	/// <summary>
	/// Creates a new saveable drug state collection from a drug instance collection.
	/// </summary>
	/// <param name="drugs">The drug instance collection to save.</param>
	public static List<DrugState> CreateDrugStates(IEnumerable<IDrug> drugs)
	{
		List<DrugState> drugStates = new();
		foreach (IDrug drug in drugs)
			drugStates.Add(CreateDrugState(drug));
		return drugStates;
	}

	/// <summary>
	/// Creates a new saveable inventory state from a inventory instance.
	/// </summary>
	/// <param name="inventory">The inventory instance to save.</param>
	public static InventoryState CreateInventoryState(IInventory inventory)
		=> new(inventory);

	/// <summary>
	/// Creates a new saveable dealer state from a dealer instance.
	/// </summary>
	/// <param name="dealer">The dealer instance to save.</param>
	public static DealerState CreateDealerState(IDealer dealer)
		=> new(dealer);

	/// <summary>
	/// Creates a new saveable dealer state collection from a dealer instance collection.
	/// </summary>
	/// <param name="dealers">The dealer instance colection to save.</param>
	public static List<DealerState> CreateDealerStates(IEnumerable<IDealer> dealers)
	{
		List<DealerState> states = new();
		foreach (IDealer dealer in dealers)
			states.Add(CreateDealerState(dealer));
		return states;
	}

	/// <summary>
	/// Creates a new saveable transaction state from a transaction instance.
	/// </summary>
	/// <param name="transaction">The transaction instance to save.</param>
	public static TransactionState CreateTransactionState(ITransaction transaction)
		=> new(transaction);

	/// <summary>
	/// Creates a new saveable transaction state collection from a transaction instance collection.
	/// </summary>
	/// <param name="transactions">The transaction instance collection to save.</param>
	public static List<TransactionState> CreateTransactionStates(ICollection<ITransaction> transactions)
	{
		List<TransactionState> states = new();
		foreach (ITransaction transaction in transactions)
			states.Add(CreateTransactionState(transaction));
		return states;
	}

	/// <summary>
	/// Creates a new saveable player state from a player instance.
	/// </summary>
	/// <param name="player">The player instance to save.</param>
	public static PlayerState CreatePlayerState(IPlayer player)
		=> new(player);

	/// <summary>
	/// Creates a new saveable game state.
	/// </summary>
	/// <param name="player">The player instance to save.</param>
	/// <param name="dealers">The dealer instance colection to save.</param>
	public static GameState CreateGameState(IPlayer player, IEnumerable<IDealer> dealers)
		=> new(player, dealers);
}
