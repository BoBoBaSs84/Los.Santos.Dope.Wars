using System.Xml.Serialization;

namespace Los.Santos.Dope.Wars.Persistence
{
	/// <summary>
	/// The <see cref="SpecialReward"/> class is the root element for the special rewards each character can achieve
	/// </summary>
	[XmlRoot(ElementName = nameof(SpecialReward), IsNullable = false)]
	public class SpecialReward
	{
		#region ctor
		/// <summary>
		/// Empty constructor with default values
		/// </summary>
		public SpecialReward()
		{
			Warehouse = Enums.WarehouseStates.Locked;
			DrugLords = Enums.DrugLordStates.Locked;
		}
		#endregion

		#region properties
		/// <summary>
		/// The <see cref="Warehouse"/> property
		/// </summary>
		[XmlAttribute(AttributeName = nameof(Warehouse))]
		public Enums.WarehouseStates Warehouse { get; set; }

		/// <summary>
		/// The <see cref="DrugLords"/> property
		/// </summary>
		[XmlAttribute(AttributeName = nameof(DrugLords))]
		public Enums.DrugLordStates DrugLords { get; set; }
		#endregion
	}
}
