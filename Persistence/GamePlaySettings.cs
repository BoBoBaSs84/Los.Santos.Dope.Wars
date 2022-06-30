using System.Xml.Serialization;

namespace Los.Santos.Dope.Wars.Persistence
{
	/// <summary>
	/// The <see cref="GamePlaySettings"/> class is the <see cref="GamePlaySettings"/> element
	/// </summary>
	[XmlRoot(ElementName = nameof(GamePlaySettings), IsNullable = false)]
	public class GamePlaySettings
	{
		#region ctor
		/// <summary>
		/// Empty constructor with standard values
		/// </summary>
		public GamePlaySettings()
		{
			LooseDrugsOnDeath = true;
			LooseDrugsWhenBusted = true;
			Difficulty = Enums.DifficultyTypes.Normal;
			SpecialRewardSettings = new();
		}
		#endregion

		#region properties
		/// <summary>
		/// The <see cref="LooseDrugsOnDeath"/> attribute indicates whether the player loses the drugs when he dies.
		/// </summary>
		[XmlAttribute(AttributeName = nameof(LooseDrugsOnDeath))]
		public bool LooseDrugsOnDeath { get; set; }

		/// <summary>
		/// The <see cref="LooseDrugsWhenBusted"/> attribute indicates whether the player loses the drugs when arrested.
		/// </summary>
		[XmlAttribute(AttributeName = nameof(LooseDrugsWhenBusted))]
		public bool LooseDrugsWhenBusted { get; set; }

		/// <summary>
		/// The <see cref="Difficulty"/> property/attribute 
		/// </summary>
		[XmlAttribute(AttributeName = nameof(Difficulty))]
		public Enums.DifficultyTypes Difficulty { get; set; }

		/// <summary>
		/// The <see cref="SpecialRewardSettings"/> property/attribute 
		/// </summary>
		[XmlElement(ElementName = nameof(SpecialRewardSettings), IsNullable = false)]
		public SpecialRewardSettings SpecialRewardSettings { get; set; }
		#endregion
	}
}
