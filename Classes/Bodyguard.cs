using GTA;
using GTA.Math;

namespace Los.Santos.Dope.Wars.Classes
{
	/// <summary>
	/// The <see cref="Bodyguard"/> class, 
	/// </summary>
	public class Bodyguard
	{
		#region constructor
		/// <summary>
		/// The <see cref="Bodyguard"/> standard constructor
		/// </summary>
		/// <param name="position"></param>
		/// <param name="heading"></param>
		public Bodyguard(Vector3 position, float heading)
		{
			Position = position;
			Heading = heading;
		}
		#endregion

		#region properties
		/// <summary>
		/// The<see cref="Blip"/> property, the blip on the map
		/// </summary>
		public Blip? Blip { get; private set; }

		/// <summary>
		/// The<see cref="Position"/> property, the position of the dealer and the blip on the map
		/// </summary>
		public Vector3 Position { get; private set; }

		/// <summary>
		/// The<see cref="Heading"/> property, the heading of the dealer (facing towards to)
		/// </summary>
		public float Heading { get; private set; }

		/// <summary>
		/// The<see cref="Ped"/> property, the ped and ped settings of the dealer
		/// </summary>		
		public Ped? Ped { get; private set; }

		/// <summary>
		/// The <see cref="BlipCreated"/> property, is the blip created
		/// </summary>
		public bool BlipCreated { get; private set; }

		/// <summary>
		/// The <see cref="PedCreated"/> property, is the ped created
		/// </summary>
		public bool PedCreated { get; private set; }
		#endregion

		#region public methods
		/// <summary>
		/// The <see cref="CreateBlip"/> method for creating the bodyguard blip on the map
		/// </summary>
		public void CreateBlip()
		{
			if (!BlipCreated)
			{
				Blip = World.CreateBlip(Position);
				Blip.Sprite = BlipSprite.Enemy;
				Blip.IsShortRange = true;
				BlipCreated = !BlipCreated;
			}
		}

		/// <summary>
		/// The <see cref="DeleteBlip"/> method for deleting the blip on the map
		/// </summary>
		public void DeleteBlip()
		{
			if (BlipCreated)
			{
				Blip!.Delete();
				BlipCreated = !BlipCreated;
			}
		}

		/// <summary>
		/// The <see cref="CreatePed(Ped)"/> method for creating the bodyguard ped
		/// </summary>
		public void CreatePed(Ped pedToProtect)
		{
			if (!PedCreated)
			{
				Model dealerModel = new(Constants.DrugDealerPedHashes[Constants.random.Next(Constants.DrugDealerPedHashes.Count)]);
				dealerModel.Request(250);

				if (dealerModel.IsValid && dealerModel.IsInCdImage)
				{
					Ped = World.CreatePed(dealerModel, Position, Heading);
					Ped.Task.GuardCurrentPosition();
					Ped.Weapons.Give(Constants.DrugLordWeaponHashes[Constants.random.Next(Constants.DrugLordWeaponHashes.Count)], 500, true, true);
					Ped.HealthFloat = 150f;
					Ped.ArmorFloat = 150f;
					Ped.Accuracy = 100;
					Ped.AttachTo(pedToProtect);
					Ped.MarkAsNoLongerNeeded();
				}
				PedCreated = !PedCreated;
			}
		}

		/// <summary>
		/// The <see cref="DeletePed"/> method for deleting the ped
		/// </summary>
		public void DeletePed()
		{
			if (PedCreated)
			{
				Ped!.Delete();
				PedCreated = !PedCreated;
			}
		}

		/// <summary>
		/// The <see cref="AimAtPed(Ped)"/> method for protection related stuff
		/// </summary>
		/// <param name="targetPed"></param>
		public void AimAtPed(Ped targetPed)
		{
			if (PedCreated && targetPed.Exists())
				Ped!.Task.AimAt(targetPed, -1);
		}

		/// <summary>
		/// The <see cref="AttackTarget(Ped)"/> method for protection related stuff
		/// </summary>
		/// <param name="targetPed"></param>
		public void AttackTarget(Ped targetPed)
		{
			if (PedCreated && targetPed.Exists())
				Ped!.Task.FightAgainst(targetPed);
		}
		#endregion
	}
}
