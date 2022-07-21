using GTA.Math;
using Los.Santos.Dope.Wars.Classes.Base;
using System.Collections.Generic;

namespace Los.Santos.Dope.Wars.Classes
{
	/// <summary>
	/// The <see cref="DrugLord"/> class, inherits from the base class <see cref="Dealer"/>
	/// </summary>
	public class DrugLord : Dealer
	{
		#region properties
		/// <summary>
		/// The <see cref="Bodyguards"/> property
		/// </summary>
		public List<Bodyguard> Bodyguards { get; set; }
		#endregion

		#region ctor
		/// <summary>
		/// The <see cref="DrugLord"/> standard constructor
		/// </summary>
		/// <param name="position"></param>
		/// <param name="heading"></param>
		public DrugLord(Vector3 position, float heading) : base(position, heading, true)
		{
			Bodyguards = new List<Bodyguard>()
			{
				new Bodyguard(position.Around(1.5f), heading),
				new Bodyguard(position.Around(3f), heading),
				new Bodyguard(position.Around(3f), heading),
				new Bodyguard(position.Around(5f), heading),
				new Bodyguard(position.Around(5f), heading),
			};
		}
		#endregion
	}
}
