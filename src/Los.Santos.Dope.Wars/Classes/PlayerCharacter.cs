using LSDW.Classes.BaseClasses;
using LSDW.Constants;
using LSDW.Interfaces.Classes;
using LSDW.Properties;
using IF = LSDW.Factories.InventoryFactory;

namespace LSDW.Classes;

internal sealed class PlayerCharacter : NotificationBase, IPlayerCharacter
{
	private double currentExperience;

	/// <summary>
	/// Initializes a instance of the player character class.
	/// </summary>
	internal PlayerCharacter()
	{
		Inventory = IF.CreatePlayerInventory();
		SpentMoney = default;
		EarnedMoney = default;
		currentExperience = default;
	}

	/// <summary>
	/// Initializes a instance of the player character class.
	/// </summary>
	/// <param name="inventory">The player inventory.</param>
	/// <param name="spentMoney">The money spent on buying drugs.</param>
	/// <param name="earnedMoney">The money earned on selling drugs.</param>
	/// <param name="experience">The player experience points.</param>
	internal PlayerCharacter(IInventory inventory, int spentMoney, int earnedMoney, int experience)
	{
		Inventory = inventory;
		SpentMoney = spentMoney;
		EarnedMoney = earnedMoney;
		currentExperience = experience;
	}

	public IInventory Inventory { get; }

	public int SpentMoney
	{
		get;
		private set;
	}

	public int EarnedMoney
	{
		get;
		private set;
	}

	public int CurrentLevel
		=> GetCurrentLevel();

	public int CurrentExperience
		=> (int)currentExperience;

	public int NextLevelExperience
		=> GetNextLevelExpPoints();

	public int MaximumInventoryQuantity
		=> GetMaximumInventoryQuantity();

	public void AddExperience(int points)
		=> currentExperience += points;

	public void Buy(IDrug drug)
	{
		if (Inventory.TotalQuantity + drug.Quantity > MaximumInventoryQuantity)
			// TODO: Not enough space.
			throw new Exception("Not enough space.");

		if (drug.Quantity * drug.Price > Inventory.Money)
			// TODO: Not enough money.
			throw new Exception("Not enough money.");

		Inventory.Add(drug);
	}

	public void Buy(IEnumerable<IDrug> drugs) => throw new NotImplementedException();

	public void Sell(IDrug drug)
	{
		if (Inventory.Where(x => x.DrugType.Equals(drug.DrugType)).Sum(x => x.Quantity) < drug.Quantity)
			// TODO: Not enough money.
			throw new Exception("Not enough drugs.");

		Inventory.Remove(drug);
	}

	public void Sell(IEnumerable<IDrug> drugs) => throw new NotImplementedException();

	private int GetCurrentLevel()
		=> PlayerConstants.CalculateCurrentLevel(currentExperience);

	private int GetNextLevelExpPoints()
		=> (int)PlayerConstants.CalculateExperienceNextLevel(CurrentLevel);

	private int GetMaximumInventoryQuantity()
		=> Settings.Default.StartingInventoryCapacity + (CurrentLevel * Settings.Default.InventoryCapacityExpansionPerLevel);
}
