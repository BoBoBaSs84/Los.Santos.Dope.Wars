using System.Xml.Serialization;

namespace Los.Santos.Dope.Wars.Persistence.Settings;

/// <summary>
/// The <see cref="Reward"/> class is the root element for the special rewards settings 
/// </summary>
[XmlRoot(ElementName = nameof(Reward), IsNullable = false)]
public class Reward
{
	#region ctor
	/// <summary>
	/// The <see cref="Reward"/> standard constructor with standard values
	/// </summary>
	public Reward()
	{
		Warehouse = new();
	}
	#endregion

	#region properties
	/// <summary>
	/// The <see cref="Warehouse"/> property
	/// </summary>
	[XmlElement(ElementName = nameof(WarehouseSettings), IsNullable = false)]
	public WarehouseSettings Warehouse { get; set; }
	#endregion

	#region sub classes
	/// <summary>
	/// The <see cref="WarehouseSettings"/> class is the <see cref="WarehouseSettings"/> element for the special rewards settings
	/// </summary>
	[XmlRoot(ElementName = nameof(WarehouseSettings), IsNullable = false)]
	public class WarehouseSettings
	{
		#region ctor
		/// <summary>
		/// The <see cref="Warehouse"/> standard constructor with standard values
		/// </summary>
		public WarehouseSettings()
		{
			Price = 125000;
			UpgradePrice = 875000;
			MissionSettings = new();
		}
		#endregion

		#region properties
		/// <summary>
		/// The <see cref="Price"/> property
		/// </summary>
		[XmlAttribute(AttributeName = nameof(Price))]
		public int Price { get; set; }

		/// <summary>
		/// The <see cref="UpgradePrice"/> property
		/// </summary>
		[XmlAttribute(AttributeName = nameof(UpgradePrice))]
		public int UpgradePrice { get; set; }

		/// <summary>
		/// The <see cref="MissionSettings"/> property
		/// </summary>
		[XmlElement(ElementName = nameof(MissionSettings), IsNullable = false)]
		public WarehouseMissionSettings MissionSettings { get; set; }
		#endregion
	}

	/// <summary>
	/// The <see cref="WarehouseMissionSettings"/> class is the <see cref="WarehouseMissionSettings"/> element for the special rewards settings.
	/// </summary>
	[XmlRoot(ElementName = nameof(WarehouseMissionSettings), IsNullable = false)]
	public class WarehouseMissionSettings
	{
		/// <summary>
		/// The <see cref="WarehouseMissionSettings"/> class constructor.
		/// </summary>
		public WarehouseMissionSettings()
		{
			MissionInterval = 168;
		}

		/// <summary>
		/// The <see cref="MissionInterval"/> property
		/// </summary>
		[XmlAttribute(AttributeName = nameof(MissionInterval))]
		public int MissionInterval { get; set; }
	}
	#endregion
}