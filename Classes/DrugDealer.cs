using GTA.Math;
using Los.Santos.Dope.Wars.Classes.Base;

namespace Los.Santos.Dope.Wars.Classes
{
	/// <summary>
	/// The <see cref="DrugDealer"/> class, inherits from the base class <see cref="Dealer"/>
	/// </summary>
	public class DrugDealer : Dealer
	{
		#region ctor
		/// <summary>
		/// The <see cref="DrugDealer"/> standard constructor
		/// </summary>
		/// <param name="position"></param>
		/// <param name="heading"></param>
		public DrugDealer(Vector3 position, float heading) : base(position, heading)
		{
		}
		#endregion
	}
}
