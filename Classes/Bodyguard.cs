using GTA;
using GTA.Math;
using Los.Santos.Dope.Wars.Classes.Base;

namespace Los.Santos.Dope.Wars.Classes
{
	/// <summary>
	/// The <see cref="Bodyguard"/> class inherits from the <see cref="Pedestrian"/> base class
	/// </summary>
	public class Bodyguard : Pedestrian
	{
		#region constructor
		/// <summary>
		/// The <see cref="Bodyguard"/> standard constructor
		/// </summary>
		/// <param name="position"></param>
		/// <param name="heading"></param>
		public Bodyguard(Vector3 position, float heading) : base(position, heading)
		{
		}
		#endregion

		#region public methods
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

		/// <summary>
		/// The <see cref="ProtectTarget(Ped)"/> method for protection related stuff
		/// </summary>
		/// <param name="targetPed"></param>
		public void ProtectTarget(Ped targetPed)
		{
			if (PedCreated && targetPed.Exists())
			{
				Ped!.AttachTo(targetPed);
				Ped!.Task.GuardCurrentPosition();
			}
		}
		#endregion
	}
}
