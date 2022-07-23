using GTA;
using GTA.Math;
using Los.Santos.Dope.Wars.Contracts.Base;
using Los.Santos.Dope.Wars.Extension;

namespace Los.Santos.Dope.Wars.Classes.Base
{
	/// <summary>
	/// The <see cref="Pedestrian"/> base class, implements the members of the <see cref="IPedestrian"/> interface
	/// </summary>
	public abstract class Pedestrian : IPedestrian
	{
		#region properties
		/// <inheritdoc/>
		public Blip? Blip { get; private set; }
		/// <inheritdoc/>
		public Vector3 Position { get; private set; }
		/// <inheritdoc/>
		public float Heading { get; private set; }
		/// <inheritdoc/>
		public Ped? Ped { get; private set; }
		/// <inheritdoc/>
		public bool BlipCreated { get; private set; }
		/// <inheritdoc/>
		public bool PedCreated { get; private set; }
		/// <inheritdoc/>
		#endregion

		#region ctor
		/// <summary>
		/// The <see cref="Pedestrian"/> standard constructor
		/// </summary>
		/// <param name="position"></param>
		/// <param name="heading"></param>
		public Pedestrian(Vector3 position, float heading)
		{
			Position = position;
			Heading = heading;
		}
		#endregion

		#region IPedestrian members
		/// <inheritdoc/>
		public void CreateBlip(BlipSprite blipSprite = BlipSprite.Standard, BlipColor blipColor = BlipColor.White, string blipName = "J.Doe", bool isFlashing = false, bool isShortRange = true)
		{
			if (!BlipCreated)
			{
				Blip = World.CreateBlip(Position);
				Blip.Sprite = blipSprite;
				Blip.Scale = 0.8f;
				Blip.Color = blipColor;
				Blip.Name = blipName;
				Blip.IsFlashing = isFlashing;
				Blip.IsShortRange = isShortRange;
				BlipCreated = !BlipCreated;
			}
		}
		/// <inheritdoc/>
		public void CreatePed(PedHash pedHash, WeaponHash weaponHash = WeaponHash.Knife, float health = 100, float armor = 50, int money = 250, bool switchWeapons = true, bool blockEvents = false, bool dropWeapons = true)
		{
			if (!PedCreated)
			{
				Model model = ScriptHookUtils.RequestModel(pedHash);
				Ped = World.CreatePed(model, Position, Heading);
				Ped.Task.StandStill(-1);
				Ped.Weapons.Give(weaponHash, 500, true, true);
				Ped.HealthFloat = health;
				Ped.ArmorFloat = armor;
				Ped.Money = money;
				Ped.CanSwitchWeapons = switchWeapons;
				Ped.BlockPermanentEvents = blockEvents;
				Ped.DropsEquippedWeaponOnDeath = dropWeapons;
				PedCreated = !PedCreated;
			}
		}
		/// <inheritdoc/>
		public void DeleteBlip()
		{
			if (Blip is not null && BlipCreated)
			{
				Blip.Delete();
				BlipCreated = !BlipCreated;
			}
		}
		/// <inheritdoc/>
		public void DeletePed()
		{
			if (Ped is not null && PedCreated)
			{
				Ped.Delete();
				PedCreated = !PedCreated;
			}
		}
		/// <inheritdoc/>
		public void UpdateBlip(BlipSprite blipSprite = BlipSprite.Standard, BlipColor blipColor = BlipColor.White, string blipName = "J.Doe", bool isFlashing = false, bool isShortRange = true)
		{
			if (Blip is not null && BlipCreated)
			{
				Blip.Sprite = blipSprite;
				Blip.Color = blipColor;
				Blip.Name = blipName;
				Blip.IsFlashing = isFlashing;
				Blip.IsShortRange = isShortRange;
			}
		}
		/// <inheritdoc/>
		public void UpdatePed(float health = 100, float armor = 50, int money = 250, bool switchWeapons = true, bool blockEvents = false, bool dropWeapons = true)
		{
			if (Ped is not null && PedCreated)
			{
				Ped.HealthFloat = health;
				Ped.ArmorFloat = armor;
				Ped.Money = money;
				Ped.CanSwitchWeapons = switchWeapons;
				Ped.BlockPermanentEvents = blockEvents;
				Ped.DropsEquippedWeaponOnDeath = dropWeapons;
			}
		}
		#endregion
	}
}