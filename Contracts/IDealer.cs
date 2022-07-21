using GTA;
using GTA.Math;
using System;

namespace Los.Santos.Dope.Wars.Contracts
{
	/// <summary>
	/// The <see cref="IDealer"/> interface for the necessary properties and methods
	/// </summary>
	public interface IDealer
	{
		/// <summary>
		/// The<see cref="Blip"/> property, the blip on the map
		/// </summary>
		Blip? Blip { get; }

		/// <summary>
		/// The<see cref="Position"/> property, the position of the dealer and the blip on the map
		/// </summary>
		Vector3 Position { get; }

		/// <summary>
		/// The<see cref="Heading"/> property, the heading of the dealer (facing towards to)
		/// </summary>
		float Heading { get; }

		/// <summary>
		/// The<see cref="Ped"/> property, the ped and ped settings of the dealer
		/// </summary>		
		Ped? Ped { get; }

		/// <summary>
		/// The <see cref="BlipCreated"/> property, is the blip created
		/// </summary>
		bool BlipCreated { get; }

		/// <summary>
		/// The <see cref="PedCreated"/> property, is the ped created
		/// </summary>
		bool PedCreated { get; }

		/// <summary>
		/// The <see cref="ClosedforBusiness"/> property, is the dealer currently closed for buisness
		/// </summary>
		bool ClosedforBusiness { get; set; }

		/// <summary>
		/// The <see cref="NextOpenBusinesTime"/> property, when will the dealer be open for buisness again
		/// </summary>
		DateTime NextOpenBusinesTime { get; set; }

		/// <summary>
		/// The <see cref="CreateBlip(string, bool, bool)"/> method for creating the blip on the map
		/// </summary>
		/// <param name="blipName"></param>
		/// <param name="isFlashing"></param>
		/// <param name="isShortRange"></param>
		void CreateBlip(string blipName = "Drug Dealer", bool isFlashing = false, bool isShortRange = true);

		/// <summary>
		/// The <see cref="CreatePed(float, float, int, bool, bool, bool)"/> method for creating the ped
		/// </summary>
		void CreatePed(float health = 100f, float armor = 50f, int money = 250, bool switchWeapons = true, bool blockEvents = false, bool dropWeapons = true);

		/// <summary>
		/// The <see cref="DeleteBlip"/> method for deleting the blip on the map
		/// </summary>
		void DeleteBlip();

		/// <summary>
		/// The <see cref="DeletePed"/> method for deleting the ped
		/// </summary>
		void DeletePed();

		/// <summary>
		/// The <see cref="UpdatePed(float, float, int, bool, bool, bool)"/> method for applying the dealer ped settings
		/// </summary>
		void UpdatePed(float health = 100f, float armor = 50f, int money = 250, bool switchWeapons = true, bool blockEvents = false, bool dropWeapons = true);

		/// <summary>
		/// The <see cref="FleeFromBust"/> method gets called, when a dea bust is initiated
		/// </summary>
		void FleeFromBust();
	}
}