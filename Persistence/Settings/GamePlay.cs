using System.Xml.Serialization;

namespace Los.Santos.Dope.Wars.Persistence.Settings
{
	/// <summary>
	/// The <see cref="GamePlay"/> class is the <see cref="GamePlay"/> element
	/// </summary>
	[XmlRoot(ElementName = nameof(GamePlay), IsNullable = false)]
	public class GamePlay
	{
		#region ctor
		/// <summary>
		/// The empty <see cref="GamePlay"/> constructor with standard values
		/// </summary>
		public GamePlay()
		{
			LooseDrugsOnDeath = true;
			LooseDrugsWhenBusted = true;
			Difficulty = Enums.DifficultyTypes.Normal;
			Reward = new();
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
		/// The <see cref="Reward"/> property/attribute 
		/// </summary>
		[XmlElement(ElementName = nameof(Reward), IsNullable = false)]
		public Reward Reward { get; set; }
		#endregion
	}
}
