using LSDW.Abstractions.Domain.Models;
using LSDW.Domain.Constants;
using LSDW.Domain.Extensions;
using LSDW.Domain.Models.Base;

namespace LSDW.Domain.Models;

/// <summary>
/// The player class.
/// </summary>
internal sealed class Player : NotificationBase, IPlayer
{
	private readonly ICollection<ITransaction> _transactions;
	private int experience = -1;
	private int level;
	private int experienceNextLevel;
	private int maximumInventoryQuantity;

	/// <summary>
	/// Initializes a instance of the player character class.
	/// </summary>
	/// <param name="inventory">The player inventory.</param>
	/// <param name="experience">The player experience points.</param>
	/// <param name="transactions">The transactions for the player.</param>
	internal Player(IInventory inventory, int experience, ICollection<ITransaction> transactions)
	{
		PropertyChanged += (sender, args) => OnPropertyChanged(args);

		Inventory = inventory;
		Experience = experience;
		_transactions = transactions;
	}

	public IInventory Inventory { get; }

	public int Level
	{
		get => level;
		private set => SetProperty(ref level, value);
	}

	public int Experience
	{
		get => experience;
		private set
		{
			if (value < 0)
				return;

			SetProperty(ref experience, value);
		}
	}

	public int ExperienceNextLevel
	{
		get => experienceNextLevel;
		private set => SetProperty(ref experienceNextLevel, value);
	}

	public int MaximumInventoryQuantity
	{
		get => maximumInventoryQuantity;
		private set => SetProperty(ref maximumInventoryQuantity, value);
	}

	public int TransactionCount
		=> _transactions.Count;

	public void AddExperience(int points)
		=> Experience += (int)(points * Settings.Instance.Player.ExperienceMultiplier.Value);

	public void AddTransaction(ITransaction transaction)
	{
		AddExperience(transaction.GetValue());
		_transactions.Add(transaction);
	}

	public ICollection<ITransaction> GetTransactions()
		=> _transactions;

	private void OnPropertyChanged(PropertyChangedEventArgs args)
	{
		if (!args.PropertyName.Equals(nameof(Experience), StringComparison.Ordinal))
			return;

		Level = PlayerConstants.CalculateCurrentLevel(Experience);
		ExperienceNextLevel = PlayerConstants.CalculateExperienceNextLevel(Level);
		MaximumInventoryQuantity = Settings.Instance.Player.StartingInventory.Value + (Level * Settings.Instance.Player.InventoryExpansionPerLevel.Value);
	}
}
