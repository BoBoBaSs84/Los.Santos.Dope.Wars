using Los.Santos.Dope.Wars.Persistence;

namespace Los.Santos.Dope.Wars.Contracts
{
	/// <summary>
	/// The <see cref="IDealerStash"/> interface, provides methods specifically for dealers
	/// </summary>
	public interface IDealerStash
	{
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
		/// <param name="name"></param>
		/// <param name="quantity"></param>
		/// <param name="price"></param>
		void BuyDrug(string name, int quantity, int price);

		/// <summary>
		/// The <see cref="SellDrug(string, int, int)"/> method is used to manipulate the dealer stash
		/// </summary>
		/// <param name="name"></param>
		/// <param name="quantity"></param>
		/// <param name="price"></param>
		void SellDrug(string name, int quantity, int price);
	}
}
