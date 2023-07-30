using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Models.Base;
using LSDW.Domain.Models.Base;

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
			ExperienceMultiplier = new BindableProperty<float>(1);
			InventoryExpansionPerLevel = new BindableProperty<int>(10);
			LooseDrugsOnDeath = new BindableProperty<bool>(true);
			LooseDrugsWhenBusted = new BindableProperty<bool>(true);
			LooseMoneyOnDeath = new BindableProperty<bool>(true);
			LooseMoneyWhenBusted = new BindableProperty<bool>(true);
			StartingInventory = new BindableProperty<int>(100);
		}

		/// <summary>
		/// The singleton instance of the player settings.
		/// </summary>
		public static PlayerSettings Instance
			=> _settings.Value;

		public IBindableProperty<float> ExperienceMultiplier { get; }
		public IBindableProperty<int> InventoryExpansionPerLevel { get; }
		public IBindableProperty<bool> LooseDrugsOnDeath { get; }
		public IBindableProperty<bool> LooseDrugsWhenBusted { get; }
		public IBindableProperty<bool> LooseMoneyOnDeath { get; }
		public IBindableProperty<bool> LooseMoneyWhenBusted { get; }
		public IBindableProperty<int> StartingInventory { get; }

		public float[] GetExperienceMultiplierValues()
			=> new float[] { 0.75f, 0.8f, 0.85f, 0.9f, 0.95f, 1f, 1.05f, 1.1f, 1.15f, 1.2f, 1.25f };

		public int[] GetInventoryExpansionPerLevelValues()
			=> new int[] { 0, 5, 10, 15, 25, 30, 35, 40, 45, 50 };

		public int[] GetStartingInventoryValues()
			=> new int[] { 50, 75, 100, 125, 150 };
	}
}
