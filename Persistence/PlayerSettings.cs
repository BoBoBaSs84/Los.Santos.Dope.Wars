using System.Xml.Serialization;

namespace Los.Santos.Dope.Wars.Persistence
{
	/// <summary>
	/// The <see cref="PlayerSettings"/> class is the <see cref="PlayerSettings"/> element
	/// </summary>
	[XmlRoot(ElementName = nameof(PlayerSettings), IsNullable = false)]
	public class PlayerSettings
	{
		#region ctor
		/// <summary>
		/// Empty constructor with standard values
		/// </summary>
		public PlayerSettings()
		{
			BagSizeIncrease = 100;
			ExperienceMultiplier = 1000;
		}
		#endregion

		#region properties
		/// <summary>
		/// The <see cref="BagSizeIncrease"/> attribute, indicates by how much the bag size is increased per level
		/// </summary>
		[XmlAttribute(AttributeName = nameof(BagSizeIncrease))]
		public int BagSizeIncrease { get; set; }

		/// <summary>
		/// The <see cref="ExperienceMultiplier"/> attribute, indicates how much experience is needed to reach the next level, calculated binaerally
		/// </summary>
		[XmlAttribute(AttributeName = nameof(ExperienceMultiplier))]
		public int ExperienceMultiplier { get; set; }
		#endregion
	}
}
