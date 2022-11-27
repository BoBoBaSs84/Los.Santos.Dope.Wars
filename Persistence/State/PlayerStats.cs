using Los.Santos.Dope.Wars.Classes;
using Los.Santos.Dope.Wars.Classes.BaseTypes;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Los.Santos.Dope.Wars.Persistence.State;

/// <summary>
/// The <see cref="PlayerStats"/> class is the root element for each character.
/// </summary>
[XmlRoot(ElementName = nameof(PlayerStats), IsNullable = false)]
public class PlayerStats : NotifyBase
{
	#region fields
	private int currentBagSize;
	private int currentExperience;
	private int currentLevel;
	#endregion

	#region ctor
	/// <summary>
	/// The <see cref="PlayerStats"/> class constructor with default values.
	/// </summary>
	public PlayerStats()
	{
		SpentMoney = default;
		EarnedMoney = default;
		CurrentLevel = 1;
		CurrentExperience = default;
		Stash = new();
		Reward = new();
		Warehouse = new();
		PropertyChanged += OnPlayerStatsPropertyChanged;
		InitPlayerStash();
	}
	#endregion

	#region properties
	/// <summary>
	/// The <see cref="SpentMoney"/> property, money spend for buying drugs.
	/// </summary>
	[XmlAttribute(AttributeName = nameof(SpentMoney))]
	public int SpentMoney { get; set; }

	/// <summary>
	/// The <see cref="EarnedMoney"/> property, money earned from selling drugs.
	/// </summary>
	[XmlAttribute(AttributeName = nameof(EarnedMoney))]
	public int EarnedMoney { get; set; }

	/// <summary>
	/// The <see cref="CurrentLevel"/> property, increases depending on experience gain.
	/// </summary>
	[XmlAttribute(AttributeName = nameof(CurrentLevel))]
	public int CurrentLevel { get => currentLevel; set => SetProperty(ref currentLevel, value); }

	/// <summary>
	/// The <see cref="CurrentExperience"/> property
	/// </summary>
	[XmlAttribute(AttributeName = nameof(CurrentExperience))]
	public int CurrentExperience { get => currentExperience; set => SetProperty(ref currentExperience, value); }

	/// <summary>
	/// The <see cref="NextLevelExperience"/> property, calculated depending on the current level, not serialized.
	/// </summary>
	[XmlIgnore]
	public int NextLevelExperience => GetNextLevelExpPoints();

	/// <summary>
	/// The <see cref="CurrentBagSize"/> property, increases/decreasing depending on the amount of drugs the player has.
	/// </summary>
	[XmlAttribute(AttributeName = nameof(CurrentBagSize))]
	public int CurrentBagSize { get => currentBagSize; set => SetProperty(ref currentBagSize, value); }

	/// <summary>
	/// The <see cref="MaxBagSize"/> property, increases depending on level gain.
	/// </summary>	
	[XmlAttribute(AttributeName = nameof(MaxBagSize))]
	// TODO: The maximum player bag size is still hard coded here ...
	public int MaxBagSize => CurrentLevel * 50;

	/// <summary>
	/// The <see cref="Stash"/> property, holds the bought and otherwise acquired drugs for trading.
	/// </summary>
	[XmlElement(ElementName = nameof(Stash), IsNullable = false)]
	public PlayerStash Stash { get; set; }

	/// <summary>
	/// The <see cref="Reward"/> property, holds the information about achieved rewards.
	/// </summary>
	[XmlElement(ElementName = nameof(Reward), IsNullable = false)]
	public Reward Reward { get; set; }

	/// <summary>
	/// The <see cref="Warehouse"/> property, holds the warehouse information.
	/// </summary>
	[XmlElement(ElementName = nameof(Warehouse), IsNullable = false)]
	public Warehouse Warehouse { get; set; }
	#endregion

	#region public members
	/// <summary>
	/// The Method adds the desired experience points to the current experience.
	/// </summary>
	/// <param name="points">The experience points to add to the current experience.</param>
	public void AddExperiencePoints(int points) => CurrentExperience += points;
	#endregion

	#region private members
	/// <summary>
	/// The player stash init method.
	/// </summary>
	/// <remarks>
	/// Needed here so that the PropertyChanged event can be subscribed.
	/// </remarks>
	private void InitPlayerStash()
	{
		if (Stash is null)
			return;

		if (Stash.Drugs is null)
			return;

		List<Drug> druglist = Extension.Utils.GetAvailableDrugs();

		foreach (Drug drug in druglist)
		{
			Drug newDrug = new(drug.Name, drug.Description, drug.AveragePrice);
			newDrug.PropertyChanged += OnDrugItemQuantityChanged;
			Stash.Drugs.Add(newDrug);
		}
	}

	/// <summary>
	/// The event trigger method if a drug item quantity has changed.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void OnDrugItemQuantityChanged(object sender, PropertyChangedEventArgs e)
	{
		if (sender is not Drug drug)
			return;

		if (e is null)
			return;

		if (Equals(e.PropertyName, nameof(drug.Quantity)))
			CurrentBagSize = Stash.Drugs.Sum(x => x.Quantity);
	}

	/// <summary>
	/// The method calculates the experience points needed for the next level up.
	/// </summary>
	/// <returns>The needed experience points.</returns>
	private int GetNextLevelExpPoints() => (int)Math.Pow(CurrentLevel + 1, 2.5) * 1000;

	/// <summary>
	/// The event trigger method if a property that notifies has changed.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void OnPlayerStatsPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e is null)
			return;

		if (Equals(e.PropertyName, nameof(CurrentExperience)))
		{
			if (GetNextLevelExpPoints() < CurrentExperience)
				CurrentLevel++;
		}
	}
	#endregion
}
