using System.Xml.Serialization;

namespace Los.Santos.Dope.Wars.Persistence.State
{
	/// <summary>
	/// The <see cref="Reward"/> class is the root element for the special rewards each character can achieve
	/// </summary>
	[XmlRoot(ElementName = nameof(Reward), IsNullable = false)]
	public class Reward
	{
		#region ctor
		/// <summary>
		/// Empty constructor with default values
		/// </summary>
		public Reward()
		{
			Warehouse = Enums.WarehouseStates.Locked;
			DrugLords = Enums.DrugLordStates.Locked;
			DrugTypes = Constants.TradePackOne;
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

		/// <summary>
		/// The <see cref="DrugTypes"/> property
		/// </summary>
		[XmlAttribute(AttributeName = nameof(DrugTypes))]
		public Enums.DrugTypes DrugTypes { get; set; }
		#endregion
	}
}
