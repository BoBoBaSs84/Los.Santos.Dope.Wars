using GTA.Math;

namespace Los.Santos.Dope.Wars.Classes
{
	/// <summary>
	/// The <see cref="DrugLord"/> class for the "Drug Lord", inhertits from <see cref="DrugDealer"/>
	/// </summary>
	public class DrugLord : DrugDealer
	{
		/// <summary>
		/// The <see cref="DrugLord"/> standard constructor
		/// </summary>
		/// <param name="position"></param>
		/// <param name="heading"></param>
		public DrugLord(Vector3 position, float heading) : base(position, heading)
		{
		}
	}
}
