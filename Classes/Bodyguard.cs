using GTA;
using GTA.Math;
using Los.Santos.Dope.Wars.Extension;

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
			if (Blip is not null && BlipCreated)
			{
				Blip.Delete();
				BlipCreated = !BlipCreated;
			}
		}

		/// <summary>
		/// The <see cref="CreatePed(PedHash, WeaponHash, Ped)"/> method for creating the bodyguard ped
		/// </summary>
		public void CreatePed(PedHash pedHash, WeaponHash weaponHash, Ped pedToProtect)
		{
			if (!PedCreated)
			{
				Model model = ScriptHookUtils.RequestModel(pedHash);
				Ped = World.CreatePed(model, Position, Heading);
				Ped.Task.GuardCurrentPosition();
				Ped.Weapons.Give(weaponHash, 1000, true, true);
				Ped.HealthFloat = 150f;
				Ped.ArmorFloat = 150f;
				Ped.Accuracy = 100;
				Ped.AttachTo(pedToProtect);
				PedCreated = !PedCreated;
			}
		}

		/// <summary>
		/// The <see cref="DeletePed"/> method for deleting the ped
		/// </summary>
		public void DeletePed()
		{
			if (Ped is not null && PedCreated)
			{
				Ped.Delete();
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
