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
			WarehousePrice = 125000;
			WarehouseUpgradePrice = 875000;
		}
		#endregion

		#region properties
		/// <summary>
		/// The <see cref="WarehousePrice"/> property
		/// </summary>
		[XmlAttribute(AttributeName = nameof(WarehousePrice))]
		public int WarehousePrice { get; set; }

		/// <summary>
		/// The <see cref="WarehouseUpgradePrice"/> property
		/// </summary>
		[XmlAttribute(AttributeName = nameof(WarehouseUpgradePrice))]
		public int WarehouseUpgradePrice { get; set; }
		#endregion
	}
	#endregion
}