using GTA;
using GTA.Math;

namespace Los.Santos.Dope.Wars.Contracts.Base
{
	/// <summary>
	/// The <see cref="IPedestrian"/> interface for the necessary properties and methods for a pedestrian
	/// </summary>
	public interface IPedestrian
	{
		#region properties
		/// <summary>
		/// The<see cref="Blip"/> property, the blip on the map for the pedestrian
		/// </summary>
		Blip? Blip { get; }
		/// <summary>
		/// The<see cref="Position"/> property, the position of the pedestrian in the world and the blip on the map
		/// </summary>
		Vector3 Position { get; }
		/// <summary>
		/// The<see cref="Heading"/> property, the heading of the pedestrian (facing towards to)
		/// </summary>
		float Heading { get; }
		/// <summary>
		/// The<see cref="Ped"/> property, the ped settings of the pedestrian
		/// </summary>		
		Ped? Ped { get; }
		/// <summary>
		/// The <see cref="BlipCreated"/> property, is the blip created for the pedestrian
		/// </summary>
		bool BlipCreated { get; }
		/// <summary>
		/// The <see cref="PedCreated"/> property, is the ped created for the pedestrian
		/// </summary>
		bool PedCreated { get; }
		#endregion

		#region public methods
		/// <summary>
		/// The <see cref="CreateBlip(BlipSprite, BlipColor, string, bool, bool)"/> method for creating the blip on the map
		/// </summary>
		/// <param name="blipSprite"></param>
		/// <param name="blipColor"></param>
		/// <param name="blipName"></param>
		/// <param name="isFlashing"></param>
		/// <param name="isShortRange"></param>
		void CreateBlip(BlipSprite blipSprite = BlipSprite.Standard, BlipColor blipColor = BlipColor.White, string blipName = "J.Doe", bool isFlashing = false, bool isShortRange = true);
		/// <summary>
		/// The <see cref="CreatePed(PedHash, WeaponHash, float, float, int, bool, bool, bool)"/> method for creating the pedestrian itself
		/// </summary>
		/// <param name="pedHash"></param>
		/// <param name="weaponHash"></param>
		/// <param name="health"></param>
		/// <param name="armor"></param>
		/// <param name="money"></param>
		/// <param name="switchWeapons"></param>
		/// <param name="blockEvents"></param>
		/// <param name="dropWeapons"></param>
		void CreatePed(PedHash pedHash, WeaponHash weaponHash = WeaponHash.Knife, float health = 100f, float armor = 50f, int money = 250, bool switchWeapons = true, bool blockEvents = false, bool dropWeapons = true);
		/// <summary>
		/// The <see cref="DeleteBlip"/> method for deleting the blip on the map
		/// </summary>
		void DeleteBlip();
		/// <summary>
		/// The <see cref="DeletePed"/> method for deleting the pedestrian
		/// </summary>
		void DeletePed();
		/// <summary>
		/// The <see cref="UpdateBlip(BlipSprite, BlipColor, string, bool, bool)"/> method for updateing the pedestrian blip
		/// </summary>
		/// <param name="blipSprite"></param>
		/// <param name="blipColor"></param>
		/// <param name="blipName"></param>
		/// <param name="isFlashing"></param>
		/// <param name="isShortRange"></param>
		void UpdateBlip(BlipSprite blipSprite = BlipSprite.Standard, BlipColor blipColor = BlipColor.White, string blipName = "J.Doe", bool isFlashing = false, bool isShortRange = true);
		/// <summary>
		/// The <see cref="UpdatePed(float, float, int, bool, bool, bool)"/> method for applying the pedestrian settings
		/// </summary>
		/// <param name="health"></param>
		/// <param name="armor"></param>
		/// <param name="money"></param>
		/// <param name="switchWeapons"></param>
		/// <param name="blockEvents"></param>
		/// <param name="dropWeapons"></param>
		void UpdatePed(float health = 100f, float armor = 50f, int money = 250, bool switchWeapons = true, bool blockEvents = false, bool dropWeapons = true);
		#endregion
	}
}