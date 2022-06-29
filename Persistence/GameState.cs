using System;
using System.Xml.Serialization;

namespace Los.Santos.Dope.Wars.Persistence
{
	/// <summary>
	/// The <see cref="GameState"/> class is the root element of the savegame file
	/// </summary>
	[XmlRoot(ElementName = nameof(GameState), IsNullable = false, Namespace = Constants.XmlNamespace)]
	public class GameState
	{
		#region ctor
		/// <summary>
		/// Empty constructor, default values
		/// </summary>
		public GameState()
		{
			Version = Constants.AssemblyVersion;
			LastRestock = DateTime.MinValue;
			LastRefresh = DateTime.MinValue;
			Franklin = new PlayerStats();
			Trevor = new PlayerStats();
			Michael = new PlayerStats();
		}
		#endregion

		#region properties
		/// <summary>
		/// The <see cref="Version"/> attribute of type <see cref="string"/>
		/// </summary>
		[XmlAttribute(AttributeName = nameof(Version))]
		public string Version { get; set; }

		/// <summary>
		/// The <see cref="LastRestock"/> attribute for the last in game date time restock of the drug quantity
		/// </summary>
		[XmlAttribute(AttributeName = nameof(LastRestock))]
		public DateTime LastRestock { get; set; }

		/// <summary>
		/// The <see cref="LastRefresh"/> attribute for the last in game date time refresh of the drug money and drug prices
		/// </summary>
		[XmlAttribute(AttributeName = nameof(LastRefresh))]
		public DateTime LastRefresh { get; set; }

		/// <summary>
		/// The <see cref="Franklin"/> property for the statistics and progress of the player character <see cref="Franklin"/>
		/// </summary>
		[XmlElement(ElementName = nameof(Franklin), IsNullable = false)]
		public PlayerStats Franklin { get; set; }

		/// <summary>
		/// The <see cref="Trevor"/> property for the statistics and progress of the player character <see cref="Trevor"/>
		/// </summary>
		[XmlElement(ElementName = nameof(Trevor), IsNullable = false)]
		public PlayerStats Trevor { get; set; }

		/// <summary>
		/// The <see cref="Michael"/> property for the statistics and progress of the player character <see cref="Michael"/>
		/// </summary>
		[XmlElement(ElementName = nameof(Michael), IsNullable = false)]
		public PlayerStats Michael { get; set; }
		#endregion
	}
}
