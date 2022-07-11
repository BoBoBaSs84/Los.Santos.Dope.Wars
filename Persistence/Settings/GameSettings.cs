using System;
using System.Xml.Serialization;

namespace Los.Santos.Dope.Wars.Persistence.Settings
{
	/// <summary>
	/// The <see cref="GameSettings"/> class is the root element of the configuration file
	/// </summary>
	[XmlRoot(ElementName = nameof(GameSettings), IsNullable = false, Namespace = Constants.XmlNamespace)]
	public class GameSettings
	{
		#region ctor
		/// <summary>
		/// The empty <see cref="GameSettings"/> constructor with standard values
		/// </summary>
		public GameSettings()
		{
			Version = Constants.AssemblyVersion;
			Dealer = new();
			GamePlay = new();
			Player = new();
			LogLevel = Enums.LogLevels.Error;
		}
		#endregion

		#region properties
		/// <summary>
		/// The <see cref="Version"/> attribute of type <see cref="string"/>
		/// </summary>
		[XmlAttribute(AttributeName = nameof(Version))]
		public string Version { get; set; }

		/// <summary>
		/// The <see cref="Dealer"/> class element
		/// </summary>
		[XmlElement(ElementName = nameof(Dealer), IsNullable = false)]
		public Dealer Dealer { get; set; }

		/// <summary>
		/// The <see cref="GamePlay"/> class element
		/// </summary>
		[XmlElement(ElementName = nameof(GamePlay), IsNullable = false)]
		public GamePlay GamePlay { get; set; }

		/// <summary>
		/// The <see cref="Player"/> class element
		/// </summary>
		[XmlElement(ElementName = nameof(Player), IsNullable = false)]
		public Player Player { get; set; }

		/// <summary>
		/// The <see cref="LogLevel"/> attribute of type <see cref="Enum"/>
		/// </summary>
		[XmlAttribute(AttributeName = nameof(LogLevel))]
		public Enums.LogLevels LogLevel { get; set; }
		#endregion
	}
}
