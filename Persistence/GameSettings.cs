using System;
using System.Xml.Serialization;

namespace Los.Santos.Dope.Wars.Persistence
{
	/// <summary>
	/// The <see cref="GameSettings"/> class is the root element of the configuration file
	/// </summary>
	[XmlRoot(ElementName = nameof(GameSettings), IsNullable = false, Namespace = Constants.XmlNamespace)]
	public class GameSettings
	{
		#region ctor
		/// <summary>
		/// Empty constructor with standard values
		/// </summary>
		public GameSettings()
		{
			Version = Constants.AssemblyVersion;
			DealerSettings = new();
			GamePlaySettings = new();
			PlayerSettings = new();
			LogLevel = Enums.LogLevels.Error | Enums.LogLevels.Warning | Enums.LogLevels.Status;
		}
		#endregion

		#region properties
		/// <summary>
		/// The <see cref="Version"/> attribute of type <see cref="string"/>
		/// </summary>
		[XmlAttribute(AttributeName = nameof(Version))]
		public string Version { get; set; }

		/// <summary>
		/// The <see cref="DealerSettings"/> class element
		/// </summary>
		[XmlElement(ElementName = nameof(DealerSettings), IsNullable = false)]
		public DealerSettings DealerSettings { get; set; }

		/// <summary>
		/// The <see cref="GamePlaySettings"/> class element
		/// </summary>
		[XmlElement(ElementName = nameof(GamePlaySettings), IsNullable = false)]
		public GamePlaySettings GamePlaySettings { get; set; }

		/// <summary>
		/// The <see cref="PlayerSettings"/> class element
		/// </summary>
		[XmlElement(ElementName = nameof(PlayerSettings), IsNullable = false)]
		public PlayerSettings PlayerSettings { get; set; }

		/// <summary>
		/// The <see cref="LogLevel"/> attribute of type <see cref="Enum"/>
		/// </summary>
		[XmlAttribute(AttributeName = nameof(LogLevel))]
		public Enums.LogLevels LogLevel { get; set; }
		#endregion
	}
}
