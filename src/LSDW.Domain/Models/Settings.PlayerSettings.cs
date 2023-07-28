using LSDW.Domain.Models.Base;
using static LSDW.Abstractions.Domain.Models.ISettings;

namespace LSDW.Domain.Models;

internal sealed partial class Settings
{
	/// <summary>
	/// The player settings class.
	/// </summary>
	internal sealed class PlayerSettings : NotificationBase, IPlayerSettings
	{
		private static readonly Lazy<PlayerSettings> _settings = new(() => new());
		private float experienceMultiplier;
		private bool looseDrugsOnDeath;
		private bool looseMoneyOnDeath;
		private bool looseDrugsWhenBusted;
		private bool looseMoneyWhenBusted;
		private int inventoryExpansionPerLevel;
		private int startingInventory;

		/// <summary>
		/// Initializes a instance of the player settings class.
		/// </summary>
		private PlayerSettings()
		{
			experienceMultiplier = 1;
			looseDrugsOnDeath = true;
			looseMoneyOnDeath = true;
			looseDrugsWhenBusted = true;
			looseMoneyWhenBusted = true;
			inventoryExpansionPerLevel = 10;
			startingInventory = 100;
		}

		/// <summary>
		/// The singleton instance of the player settings.
		/// </summary>
		public static PlayerSettings Instance
			=> _settings.Value;

		/// <inheritdoc/>
		public float ExperienceMultiplier
		{
			get => experienceMultiplier;
			set => SetProperty(ref experienceMultiplier, value);
		}

		/// <inheritdoc/>
		public bool LooseDrugsOnDeath
		{
			get => looseDrugsOnDeath;
			set => SetProperty(ref looseDrugsOnDeath, value);
		}

		/// <inheritdoc/>
		public bool LooseMoneyOnDeath
		{
			get => looseMoneyOnDeath;
			set => SetProperty(ref looseMoneyOnDeath, value);
		}

		/// <inheritdoc/>
		public bool LooseDrugsWhenBusted
		{
			get => looseDrugsWhenBusted;
			set => SetProperty(ref looseDrugsWhenBusted, value);
		}

		/// <inheritdoc/>
		public bool LooseMoneyWhenBusted
		{
			get => looseMoneyWhenBusted;
			set => SetProperty(ref looseMoneyWhenBusted, value);
		}

		/// <inheritdoc/>
		public int InventoryExpansionPerLevel
		{
			get => inventoryExpansionPerLevel;
			set => SetProperty(ref inventoryExpansionPerLevel, value);
		}

		/// <inheritdoc/>
		public int StartingInventory
		{
			get => startingInventory;
			set => SetProperty(ref startingInventory, value);
		}

		/// <inheritdoc/>
		public float[] GetExperienceMultiplierValues()
			=> new float[] { 0.75f, 0.8f, 0.85f, 0.9f, 0.95f, 1f, 1.05f, 1.1f, 1.15f, 1.2f, 1.25f };

		/// <inheritdoc/>
		public int[] GetInventoryExpansionPerLevelValues()
			=> new int[] { 0, 5, 10, 15, 25, 30, 35, 40, 45, 50 };

		/// <inheritdoc/>
		public int[] GetStartingInventoryValues()
			=> new int[] { 50, 75, 100, 125, 150 };
	}
}
