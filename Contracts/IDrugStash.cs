using Los.Santos.Dope.Wars.Classes;
using Los.Santos.Dope.Wars.Persistence;
using System.Collections.Generic;

namespace Los.Santos.Dope.Wars.Contracts
{
	/// <summary>
	/// The <see cref="IDrugStash"/> interface, needed for all who want to do something with drugs
	/// </summary>
	public interface IDrugStash
	{
		/// <summary>
		/// The <see cref="Drugs"/> property, well illegal goods
		/// </summary>
		List<Drug> Drugs { get; }

		/// <summary>
		/// The <see cref="Money"/> property, first choice to trade with
		/// </summary>
		public int Money { get; }

		/// <summary>
		/// Initializes the <see cref="List{T}"/> of <see cref="Drug"/> named <see cref="Drugs"/>
		/// </summary>
		void Init();

		/// <summary>
		/// Restock randomly the quantity of available drugs
		/// </summary>
		void RestockQuantity(PlayerStats playerStats, GameSettings gameSettings, bool isDrugLord = false);

		/// <summary>
		/// Refreshes randomly the current prices 
		/// </summary>
		void RefreshCurrentPrice(PlayerStats playerStats, GameSettings gameSettings, bool isDrugLord = false);

		/// <summary>
		/// Refreshes randomly the current amount of money the dealer is carrying
		/// </summary>
		void RefreshDrugMoney(PlayerStats playerStats, GameSettings gameSettings, bool isDrugLord = false);
	}
}
