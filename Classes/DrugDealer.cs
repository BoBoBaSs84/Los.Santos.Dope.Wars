using GTA;
using GTA.Math;
using GTA.Native;
using Los.Santos.Dope.Wars.Contracts;
using Los.Santos.Dope.Wars.Extension;
using System;

namespace Los.Santos.Dope.Wars.Classes
{
	/// <summary>
	/// The <see cref="DrugDealer"/> class, implements the <see cref="IDrugStash"/> interface members
	/// </summary>
	public class DrugDealer : IDrugDealer
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
		public IDrugStash DrugStash { get; }
		/// <inheritdoc/>
		public bool BlipCreated { get; set; }
		/// <inheritdoc/>
		public bool PedCreated { get; set; }
		/// <inheritdoc/>
		public bool ClosedforBusiness { get; set; }
		/// <inheritdoc/>
		public DateTime NextOpenBusinesTime { get; set; }
		#endregion

		#region ctor
		/// <summary>
		/// The <see cref="DrugDealer"/> standard constructor
		/// </summary>
		/// <param name="position"></param>
		/// <param name="heading"></param>
		public DrugDealer(Vector3 position, float heading)
		{
			Position = position;
			Heading = heading;
			DrugStash = new DrugStash();
		}
		#endregion

		#region public methods
		/// <inheritdoc/>
		public void CreateBlip(string blipName = "Drug Dealer", bool isFlashing = false)
		{
			if (!BlipCreated)
			{
				Blip = World.CreateBlip(Position);
				Blip.Sprite = BlipSprite.Drugs;
				Blip.Name = blipName;
				Blip.IsShortRange = true;
				Blip.IsFlashing = isFlashing;
				BlipCreated = !BlipCreated;
			}
		}

		/// <inheritdoc/>
		public void CreatePed(float health = 100f, float armor = 100f, int money = 250)
		{
			if (!PedCreated)
			{
				Model dealerModel = new(Constants.DrugDealerPedHashes[Constants.random.Next(Constants.DrugDealerPedHashes.Count)]);
				dealerModel.Request(250);

				if (dealerModel.IsValid && dealerModel.IsInCdImage)
				{
					Ped = World.CreatePed(dealerModel, Position, Heading);
					Ped.Task.StandStill(-1);
					Ped.Weapons.Give(Constants.DrugDealerWeaponHashes[Constants.random.Next(Constants.DrugDealerWeaponHashes.Count)], 500, true, true);
					Ped.HealthFloat = health;
					Ped.ArmorFloat = armor;
					Ped.Money = money;
					Ped.CanSwitchWeapons = true;
					Ped.BlockPermanentEvents = false;
					Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, new InputArgument(Ped), 46, 1);
					Function.Call(Hash.SET_PED_COMBAT_MOVEMENT, new InputArgument(Ped), 1);
					Function.Call(Hash.SET_PED_COMBAT_ABILITY, new InputArgument(Ped), 75);
					NextOpenBusinesTime = ScriptHookUtils.GetGameDate().AddHours(24);
					Ped.MarkAsNoLongerNeeded();
					PedCreated = !PedCreated;
				}
			}
		}

		/// <inheritdoc/>
		public void DeleteBlip()
		{
			if (BlipCreated)
				Blip!.Delete();
		}

		/// <inheritdoc/>
		public void DeletePed()
		{
			if (PedCreated)
				Ped!.Delete();
		}

		/// <inheritdoc/>
		public void RefreshArmorHealthMoney(float health, float armor, int money)
		{
			if (PedCreated)
			{
				Ped!.HealthFloat = health;
				Ped!.ArmorFloat = armor;
				Ped!.Money = money;
			}
		}
		#endregion
	}
}
