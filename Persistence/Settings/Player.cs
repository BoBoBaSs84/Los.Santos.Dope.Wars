using System.Xml.Serialization;

namespace Los.Santos.Dope.Wars.Persistence.Settings;

/// <summary>
/// The <see cref="Player"/> class is the <see cref="Player"/> element
/// </summary>
[XmlRoot(ElementName = nameof(Player), IsNullable = false)]
public class Player
{
	#region ctor
	/// <summary>
	/// Empty constructor with standard values
	/// </summary>
	public Player()
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
