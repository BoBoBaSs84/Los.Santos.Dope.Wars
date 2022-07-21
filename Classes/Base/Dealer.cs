using GTA;
using GTA.Math;
using Los.Santos.Dope.Wars.Contracts;
using Los.Santos.Dope.Wars.Extension;
using System;

namespace Los.Santos.Dope.Wars.Classes.Base
{
	/// <summary>
	/// The <see cref="Dealer"/> base class, implements the members of the <see cref="IDealer"/> interface
	/// </summary>
	public abstract class Dealer : IDealer
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
		public bool ClosedforBusiness { get; set; }
		/// <inheritdoc/>
		public DateTime NextOpenBusinesTime { get; set; }
		#endregion

		#region ctor
		/// <summary>
		/// The <see cref="Dealer"/> standard constructor
		/// </summary>
		/// <param name="position"></param>
		/// <param name="heading"></param>
		public Dealer(Vector3 position, float heading)
		{
			Position = position;
			Heading = heading;
		}
		#endregion

		#region public methods
		/// <inheritdoc/>
		public void CreateBlip(string blipName = "Drug Dealer", bool isFlashing = false, bool isShortRange = true)
		{
			if (!BlipCreated)
			{
				Blip = World.CreateBlip(Position);
				Blip.Sprite = BlipSprite.Drugs;
				Blip.Name = blipName;
				Blip.IsShortRange = isShortRange;
				Blip.IsFlashing = isFlashing;
				BlipCreated = !BlipCreated;
			}
		}
		/// <inheritdoc/>
		public void CreatePed(float health = 100f, float armor = 50f, int money = 250, bool switchWeapons = true, bool blockEvents = false, bool dropWeapons = true)
		{
			if (!PedCreated)
			{
				Model dealerModel = new(Constants.DrugDealerPedHashes[Constants.random.Next(Constants.DrugDealerPedHashes.Count)]);
				dealerModel.Request(250);

				if (dealerModel.IsValid && dealerModel.IsInCdImage)
				{
					Ped = World.CreatePed(dealerModel, Position, Heading);
					Ped.MarkAsNoLongerNeeded();
					Ped.Task.StandStill(-1);
					Ped.Weapons.Give(Constants.DrugDealerWeaponHashes[Constants.random.Next(Constants.DrugDealerWeaponHashes.Count)], 500, true, true);
					Ped.HealthFloat = health;
					Ped.ArmorFloat = armor;
					Ped.Money = money;
					Ped.CanSwitchWeapons = switchWeapons;
					Ped.BlockPermanentEvents = blockEvents;
					Ped.DropsEquippedWeaponOnDeath = dropWeapons;
				}
				PedCreated = !PedCreated;
			}
		}
		/// <inheritdoc/>
		public void DeleteBlip()
		{
			if (BlipCreated)
			{
				Blip!.Delete();
				BlipCreated = !BlipCreated;
			}
		}
		/// <inheritdoc/>
		public void DeletePed()
		{
			if (PedCreated)
			{
				Ped!.Delete();
				PedCreated = !PedCreated;
			}
		}
		/// <inheritdoc/>
		public void UpdatePed(float health = 100f, float armor = 50f, int money = 250, bool switchWeapons = true, bool blockEvents = false, bool dropWeapons = true)
		{
			if (PedCreated)
			{
				Ped!.HealthFloat = health;
				Ped!.ArmorFloat = armor;
				Ped!.Money = money;
				Ped!.CanSwitchWeapons = switchWeapons;
				Ped!.BlockPermanentEvents = blockEvents;
				Ped!.DropsEquippedWeaponOnDeath = dropWeapons;
			}
		}
		/// <inheritdoc/>
		public void FleeFromBust()
		{
			if (PedCreated)
			{
				DeleteBlip();
				ClosedforBusiness = true;
				NextOpenBusinesTime = ScriptHookUtils.GetGameDateTime().AddHours(24);
				Ped!.Task.FleeFrom(Position);
			}
		}
		#endregion
	}
}