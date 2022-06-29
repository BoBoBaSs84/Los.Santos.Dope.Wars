using Los.Santos.Dope.Wars.Classes;
using System;
using System.Xml.Serialization;

namespace Los.Santos.Dope.Wars.Persistence
{
	/// <summary>
	/// The <see cref="PlayerStats"/> class is the root element for each character
	/// </summary>
	[XmlRoot(ElementName = nameof(PlayerStats), IsNullable = false)]
	public class PlayerStats
	{
		#region ctor
		/// <summary>
		/// Empty constructor with default values
		/// </summary>
		public PlayerStats()
		{
			SpentMoney = default;
			EarnedMoney = default;
			CurrentLevel = 1;
			CurrentExperience = default;
			DrugStash = new()
			{
				Money = default,
				Drugs = new()
			};
			SpecialReward = new();
			Warehouse = new();
			DrugStash.Init();
		}
		#endregion

		#region properties
		/// <summary>
		/// The <see cref="MaxLevel"/> property, hard coded for now
		/// </summary>
		[XmlIgnore]
		public const int MaxLevel = 50;

		/// <summary>
		/// The <see cref="SpentMoney"/> property, money spend for buying drugs
		/// </summary>
		[XmlAttribute(AttributeName = nameof(SpentMoney))]
		public int SpentMoney { get; set; }

		/// <summary>
		/// The <see cref="EarnedMoney"/> property, money earned from selling drugs
		/// </summary>
		[XmlAttribute(AttributeName = nameof(EarnedMoney))]
		public int EarnedMoney { get; set; }

		/// <summary>
		/// The <see cref="CurrentLevel"/> property, increases depending on experience gain
		/// </summary>
		[XmlAttribute(AttributeName = nameof(CurrentLevel))]
		public int CurrentLevel { get; set; }

		/// <summary>
		/// The <see cref="CurrentExperience"/> property
		/// </summary>
		[XmlAttribute(AttributeName = nameof(CurrentExperience))]
		public int CurrentExperience { get; set; }

		/// <summary>
		/// The <see cref="NextLevelExperience"/> property, calculated not serialized
		/// </summary>
		[XmlIgnore]
		public int NextLevelExperience { get => GetNextLevelExpPoints(); }

		/// <summary>
		/// The <see cref="CurrentBagSize"/> property, increases/decreasing depending on the amount of drugs the player has
		/// </summary>
		[XmlAttribute(AttributeName = nameof(CurrentBagSize))]
		public int CurrentBagSize { get => GetCurrentBagSize(); }

		/// <summary>
		/// The <see cref="MaxBagSize"/> property, increases depending on level gain
		/// </summary>
		[XmlAttribute(AttributeName = nameof(MaxBagSize))]
		public int MaxBagSize { get => CurrentLevel * 50; }

		/// <summary>
		/// The <see cref="DrugStash"/> property, holds the bought player drugs and drug money for trading
		/// </summary>
		[XmlElement(ElementName = nameof(DrugStash), IsNullable = false)]
		public DrugStash DrugStash { get; set; }

		/// <summary>
		/// The <see cref="SpecialReward"/> property, holds the information about achieved rewards
		/// </summary>
		[XmlElement(ElementName = nameof(SpecialReward), IsNullable = false)]
		public SpecialReward SpecialReward { get; set; }

		/// <summary>
		/// The <see cref="SpecialReward"/> property, holds the warehouse information
		/// </summary>
		[XmlElement(ElementName = nameof(Warehouse), IsNullable = false)]
		public DrugWarehouse Warehouse { get; set; }
		#endregion

		#region public members
		/// <summary>
		/// bad implementation for now ... 
		/// </summary>
		/// <param name="points"></param>
		public void AddExperiencePoints(int points)
		{
			CurrentExperience += points;
			while (GetNextLevelExpPoints() < CurrentExperience)
				CurrentLevel++;
		}
		#endregion

		/// <summary>
		/// Returns the need experience points fo the next level
		/// </summary>
		/// <returns><see cref="int"/></returns>
		private int GetNextLevelExpPoints() => (int)Math.Pow(CurrentLevel + 1, 2.5) * 1000;

		/// <summary>
		/// Returns the current bag size
		/// </summary>
		/// <returns></returns>
		private int GetCurrentBagSize()
		{
			int bagSize = 0;

			if (DrugStash.Drugs.Count.Equals(0))
				return bagSize;

			foreach (Drug? drug in DrugStash.Drugs)
				bagSize += drug.Quantity;
			return bagSize;
		}
	}
}
