using LSDW.Abstractions.Helpers;
using static LSDW.Abstractions.Domain.Models.ISettings;

namespace LSDW.Domain.Models;

internal sealed partial class Settings
{
	/// <summary>
	/// The player settings class.
	/// </summary>
	internal sealed class PlayerSettings : IPlayerSettings
	{
		private static readonly Lazy<PlayerSettings> _settings = new(() => new());

		/// <summary>
		/// Initializes a instance of the player settings class.
		/// </summary>
		private PlayerSettings()
		{
			ExperienceMultiplier = new(1.0f);
			LooseDrugsOnDeath = new(true);
			LooseMoneyOnDeath = new(true);
			LooseDrugsWhenBusted = new(true);
			LooseMoneyWhenBusted = new(true);
			InventoryExpansionPerLevel = new(10);
			StartingInventory = new(100);
		}

		/// <summary>
		/// The singleton instance of the player settings.
		/// </summary>
		public static PlayerSettings Instance
			=> _settings.Value;

		/// <inheritdoc/>
		public BindableProperty<float> ExperienceMultiplier { get; set; }

		/// <inheritdoc/>
		public BindableProperty<bool> LooseDrugsOnDeath { get; set; }

		/// <inheritdoc/>
		public BindableProperty<bool> LooseMoneyOnDeath { get; set; }

		/// <inheritdoc/>
		public BindableProperty<bool> LooseDrugsWhenBusted { get; set; }

		/// <inheritdoc/>
		public BindableProperty<bool> LooseMoneyWhenBusted { get; set; }

		/// <inheritdoc/>
		public BindableProperty<int> InventoryExpansionPerLevel { get; set; }

		/// <inheritdoc/>
		public BindableProperty<int> StartingInventory { get; set; }

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
