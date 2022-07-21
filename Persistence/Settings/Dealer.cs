using System.Xml.Serialization;

namespace Los.Santos.Dope.Wars.Persistence.Settings
{
	/// <summary>
	/// The <see cref="Dealer"/> class is the <see cref="Dealer"/> element
	/// </summary>
	[XmlRoot(ElementName = nameof(Dealer), IsNullable = false)]
	public class Dealer
	{
		#region ctor
		/// <summary>
		/// Empty constructor with standard values
		/// </summary>
		public Dealer()
		{
			RestockIntervalHours = 24;
			RefreshIntervalHours = 6;
			ArmorBaseValue = 50f;
			HealthBaseValue = 100f;
			CanSwitchWeapons = true;
			BlockPermanentEvents = true;
			DropsEquippedWeaponOnDeath = false;
		}
		#endregion

		#region properties
		/// <summary>
		/// The <see cref="RestockIntervalHours"/> attribute for the restock interval in hours of the drug quantity
		/// </summary>
		[XmlAttribute(AttributeName = nameof(RestockIntervalHours))]
		public int RestockIntervalHours { get; set; }

		/// <summary>
		/// The <see cref="RefreshIntervalHours"/> attribute for the refresh interval in hours of the drug money and drug prices
		/// </summary>
		[XmlAttribute(AttributeName = nameof(RefreshIntervalHours))]
		public int RefreshIntervalHours { get; set; }

		/// <summary>
		/// The <see cref="ArmorBaseValue"/> attribute, how much money Armor a dealer is equiped with
		/// </summary>
		[XmlAttribute(AttributeName = nameof(ArmorBaseValue))]
		public float ArmorBaseValue { get; set; }

		/// <summary>
		/// The <see cref="HealthBaseValue"/> attribute, how much health a dealer has
		/// </summary>
		[XmlAttribute(AttributeName = nameof(HealthBaseValue))]
		public float HealthBaseValue { get; set; }

		/// <summary>
		/// The <see cref="CanSwitchWeapons"/> attribute, sets if the dealers can switch between different weapons
		/// </summary>
		[XmlAttribute(AttributeName = nameof(CanSwitchWeapons))]
		public bool CanSwitchWeapons { get; set; }

		/// <summary>
		/// The <see cref="BlockPermanentEvents"/> attribute, sets if permanent events are blocked
		/// the dealers will only do as told, and won't flee when shot at, etc.
		/// </summary>
		[XmlAttribute(AttributeName = nameof(BlockPermanentEvents))]
		public bool BlockPermanentEvents { get; set; }

		/// <summary>
		/// The <see cref="DropsEquippedWeaponOnDeath"/> attribute, sets if permanent dealer drops his equipment on death
		/// the dealers will only do as told, and won't flee when shot at, etc.
		/// </summary>
		[XmlAttribute(AttributeName = nameof(DropsEquippedWeaponOnDeath))]
		public bool DropsEquippedWeaponOnDeath { get; set; }
		#endregion
	}
}
