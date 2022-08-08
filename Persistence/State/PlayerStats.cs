using Los.Santos.Dope.Wars.Classes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Los.Santos.Dope.Wars.Persistence.State;

/// <summary>
/// The <see cref="PlayerStats"/> class is the root element for each character
/// </summary>
[XmlRoot(ElementName = nameof(PlayerStats), IsNullable = false)]
public class PlayerStats : INotifyPropertyChanged
{
	#region fields
	private int currentLevel;
	/// <inheritdoc/>
	public event PropertyChangedEventHandler? PropertyChanged;
	#endregion

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
		Stash = new()
		{
			Drugs = new()
		};
		Reward = new();
		Warehouse = new();
		Stash.Init();
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
	public int CurrentLevel
	{
		get { return currentLevel; }
		set
		{
			if (value > currentLevel)
			{
				currentLevel = value;
				OnPropertyChanged(nameof(CurrentLevel));
			}
		}
	}

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
	/// The <see cref="Stash"/> property, holds the bought player drugs and drug money for trading
	/// </summary>
	[XmlElement(ElementName = nameof(Stash), IsNullable = false)]
	public PlayerStash Stash { get; set; }

	/// <summary>
	/// The <see cref="Reward"/> property, holds the information about achieved rewards
	/// </summary>
	[XmlElement(ElementName = nameof(Reward), IsNullable = false)]
	public Reward Reward { get; set; }

	/// <summary>
	/// The <see cref="Warehouse"/> property, holds the warehouse information
	/// </summary>
	[XmlElement(ElementName = nameof(Warehouse), IsNullable = false)]
	public Warehouse Warehouse { get; set; }
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

		if (Stash.Drugs.Count.Equals(0))
			return bagSize;

		foreach (Drug? drug in Stash.Drugs)
			bagSize += drug.Quantity;
		return bagSize;
	}

	/// <summary>
	/// The <see cref="OnPropertyChanged(string?)"/> method for the event trigger
	/// </summary>
	/// <param name="name"></param>
	private protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
