using GTA;
using GTA.Math;
using Los.Santos.Dope.Wars.Contracts;
using Los.Santos.Dope.Wars.Extension;
using Los.Santos.Dope.Wars.Persistence.Settings;
using Los.Santos.Dope.Wars.Persistence.State;
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
		public bool IsDrugLord { get; private set; }
		/// <inheritdoc/>
		public bool ClosedforBusiness { get; set; }
		/// <inheritdoc/>
		public DateTime NextOpenBusinesTime { get; set; }
		/// <inheritdoc/>
		public DealerStash Stash { get; set; }
		#endregion

		#region ctor
		/// <summary>
		/// The <see cref="Dealer"/> standard constructor
		/// </summary>
		/// <param name="position"></param>
		/// <param name="heading"></param>
		/// <param name="isDrugLord"></param>
		public Dealer(Vector3 position, float heading, bool isDrugLord = false)
		{
			Position = position;
			Heading = heading;
			IsDrugLord = isDrugLord;
			Stash = new DealerStash();
		}
		#endregion

		#region IDealer members
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
		public void CreatePed(PedHash pedHash, WeaponHash weaponHash = WeaponHash.Pistol, float health = 100f,
			float armor = 50f, int money = 250, bool switchWeapons = true, bool blockEvents = false, bool dropWeapons = true)
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
		public void FleeFromBust()
		{
			if (Ped is not null && PedCreated)
			{
				DeleteBlip();
				ClosedforBusiness = true;
				NextOpenBusinesTime = ScriptHookUtils.GetGameDateTime().AddHours(24);
				Ped.Task.FleeFrom(Position);
			}
		}
		/// <inheritdoc/>
		public void UpdatePed(float health = 100f, float armor = 50f, int money = 250, bool switchWeapons = true,
			bool blockEvents = false, bool dropWeapons = true)
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
		/// <inheritdoc/>
		public void Refresh(GameSettings gameSettings, PlayerStats playerStats)
		{
			Stash.RefreshDrugMoney(playerStats, gameSettings, IsDrugLord);
			Stash.RefreshCurrentPrice(playerStats, gameSettings, IsDrugLord);
			(float health, float armor) = Utils.GetDealerHealthArmor(gameSettings.Dealer, playerStats.CurrentLevel);
			UpdatePed(
				health: health,
				armor: armor,
				money: Stash.DrugMoney,
				switchWeapons: gameSettings.Dealer.CanSwitchWeapons,
				blockEvents: gameSettings.Dealer.BlockPermanentEvents,
				dropWeapons: gameSettings.Dealer.DropsEquippedWeaponOnDeath
				);
		}
		/// <inheritdoc/>
		public void Restock(GameSettings gameSettings, PlayerStats playerStats)
		{
			Stash.RestockQuantity(playerStats, gameSettings, IsDrugLord);
			Refresh(gameSettings, playerStats);
		}
		#endregion
	}
}