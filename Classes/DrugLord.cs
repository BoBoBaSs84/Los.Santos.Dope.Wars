using GTA.Math;
using Los.Santos.Dope.Wars.Classes.Base;

namespace Los.Santos.Dope.Wars.Classes
{
	/// <summary>
	/// The <see cref="DrugLord"/> class for the "Drug Lord", inherits from the base class <see cref="Dealer"/>
	/// </summary>
	public class DrugLord : Dealer
	{
		#region properties
		/// <summary>
		/// The <see cref="Stash"/> property
		/// </summary>
		public DealerStash Stash { get; set; }
		#endregion

		#region ctor
		/// <summary>
		/// The <see cref="DrugLord"/> standard constructor
		/// </summary>
		/// <param name="position"></param>
		/// <param name="heading"></param>
		public DrugLord(Vector3 position, float heading) : base(position, heading)
		{
			Stash = new DealerStash();
		}
		#endregion
	}
}
