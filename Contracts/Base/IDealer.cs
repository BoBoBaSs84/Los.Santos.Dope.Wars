using Los.Santos.Dope.Wars.Classes;
using Los.Santos.Dope.Wars.Classes.Base;
using Los.Santos.Dope.Wars.Persistence.Settings;
using Los.Santos.Dope.Wars.Persistence.State;
using System;

namespace Los.Santos.Dope.Wars.Contracts.Base
{
	/// <summary>
	/// The <see cref="IDealer"/> interface for the necessary properties and methods
	/// </summary>
	public interface IDealer
	{
		#region properties
		/// <summary>
		/// The <see cref="IsDrugLord"/> property, is the dealer a drug lord?
		/// </summary>
		bool IsDrugLord { get; }
		/// <summary>
		/// The <see cref="ClosedforBusiness"/> property, is the dealer currently closed for buisness
		/// </summary>
		bool ClosedforBusiness { get; set; }
		/// <summary>
		/// The <see cref="NextOpenBusinesTime"/> property, when will the dealer be open for buisness again
		/// </summary>
		DateTime NextOpenBusinesTime { get; set; }
		/// <summary>
		/// The <see cref="Stash"/> property
		/// </summary>
		DealerStash Stash { get; set; }
		#endregion

		#region methods
		/// <summary>
		/// The <see cref="FleeFromBust"/> method gets called, when a dea bust is initiated
		/// </summary>
		void FleeFromBust();
		/// <summary>
		/// The <see cref="Refresh(GameSettings, PlayerStats)"/> method refreshes the drug dealers prices,
		/// the drug money for trading and updates the "dealer" pedestrian via the 
		/// <see cref="Pedestrian.UpdatePed(float, float, int, bool, bool, bool)"/> method
		/// </summary>
		/// <param name="gameSettings">Needed, not <see cref="Nullable"/>!</param>
		/// <param name="playerStats">Needed, not <see cref="Nullable"/>!</param>
		void Refresh(GameSettings gameSettings, PlayerStats playerStats);
		/// <summary>
		/// The <see cref="Restock(GameSettings, PlayerStats)"/> method restocks the dealers drug amount,
		/// refreshes the drug prices, the drug money for trading and updates the "dealer" pedestrian
		/// via the <see cref="Pedestrian.UpdatePed(float, float, int, bool, bool, bool)"/> method
		/// </summary>
		/// <param name="gameSettings">Needed, not <see cref="Nullable"/>!</param>
		/// <param name="playerStats">Needed, not <see cref="Nullable"/>!</param>
		void Restock(GameSettings gameSettings, PlayerStats playerStats);
		#endregion
	}
}