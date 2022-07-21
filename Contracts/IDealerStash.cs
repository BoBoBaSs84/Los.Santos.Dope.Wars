using Los.Santos.Dope.Wars.Persistence.Settings;
using Los.Santos.Dope.Wars.Persistence.State;

namespace Los.Santos.Dope.Wars.Contracts
{
	/// <summary>
	/// The <see cref="IDealerStash"/> interface, provides methods specifically for dealers
	/// </summary>
	public interface IDealerStash
	{
		/// <summary>
		/// The <see cref="DrugMoney"/> property, first choice to trade with which means, that the dealer can only buy a limited amount of drugs from the player
		/// </summary>
		public int DrugMoney { get; }
		/// <summary>
		/// Restock randomly the quantity of available drugs
		/// </summary>
		/// <param name="playerStats"></param>
		/// <param name="gameSettings"></param>
		/// <param name="isDrugLord"></param>
		void RestockQuantity(PlayerStats playerStats, GameSettings gameSettings, bool isDrugLord = false);
		/// <summary>
		/// Refreshes randomly the current prices
		/// </summary>
		/// <param name="playerStats"></param>
		/// <param name="gameSettings"></param>
		/// <param name="isDrugLord"></param>
		void RefreshCurrentPrice(PlayerStats playerStats, GameSettings gameSettings, bool isDrugLord = false);
		/// <summary>
		/// Refreshes randomly the current amount of money the dealer is carrying
		/// </summary>
		/// <param name="playerStats"></param>
		/// <param name="gameSettings"></param>
		/// <param name="isDrugLord"></param>
		void RefreshDrugMoney(PlayerStats playerStats, GameSettings gameSettings, bool isDrugLord = false);
		/// <summary>
		/// The <see cref="BuyDrug(string, int, int)"/> method is used to manipulate the dealer stash
		/// </summary>
		/// <param name="drugName"></param>
		/// <param name="drugQuantity"></param>
		/// <param name="drugPrice"></param>
		void BuyDrug(string drugName, int drugQuantity, int drugPrice);
		/// <summary>
		/// The <see cref="SellDrug(string, int, int)"/> method is used to manipulate the dealer stash
		/// </summary>
		/// <param name="drugName"></param>
		/// <param name="drugQuantity"></param>
		/// <param name="drugPrice"></param>
		void SellDrug(string drugName, int drugQuantity, int drugPrice);
		/// <summary>
		/// The <see cref="AddDrugMoney(int)"/> method adds the amount x to the <see cref="DrugMoney"/> property
		/// </summary>
		/// <param name="amount"></param>
		void AddDrugMoney(int amount);
		/// <summary>
		/// The <see cref="RemoveDrugMoney(int)"/> method removes the amount x from the <see cref="DrugMoney"/> property
		/// </summary>
		/// <param name="amount"></param>
		void RemoveDrugMoney(int amount);
	}
}
