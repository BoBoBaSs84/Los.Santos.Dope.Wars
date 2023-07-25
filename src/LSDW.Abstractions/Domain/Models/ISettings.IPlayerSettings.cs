using LSDW.Abstractions.Helpers;

namespace LSDW.Abstractions.Domain.Models;

public partial interface ISettings
{
	/// <summary>
	/// The player settings interface.
	/// </summary>
	public interface IPlayerSettings
	{
		/// <summary>
		/// The experience multiplier property.
		/// </summary>
		BindableProperty<float> ExperienceMultiplier { get; set; }

		/// <summary>
		/// The inventory expansion per level property.
		/// </summary>
		BindableProperty<int> InventoryExpansionPerLevel { get; set; }

		/// <summary>
		/// The loose drugs on death property.
		/// </summary>
		BindableProperty<bool> LooseDrugsOnDeath { get; set; }

		/// <summary>
		/// The loose drugs when busted property.
		/// </summary>
		BindableProperty<bool> LooseDrugsWhenBusted { get; set; }

		/// <summary>
		/// The loose money on death property.
		/// </summary>
		BindableProperty<bool> LooseMoneyOnDeath { get; set; }

		/// <summary>
		/// The loose money when busted property.
		/// </summary>
		BindableProperty<bool> LooseMoneyWhenBusted { get; set; }

		/// <summary>
		/// The starting inventory property.
		/// </summary>
		BindableProperty<int> StartingInventory { get; set; }

		/// <summary>
		/// Returns the possible experience multiplier factor values.
		/// </summary>
		/// <returns>The list of possible values.</returns>
		float[] GetExperienceMultiplierValues();

		/// <summary>
		/// Returns the possible inventory expansion per level values.
		/// </summary>
		/// <returns>The list of possible values.</returns>
		int[] GetInventoryExpansionPerLevelValues();

		/// <summary>
		/// Returns the possible starting inventory values.
		/// </summary>
		/// <returns>The list of possible values.</returns>
		int[] GetStartingInventoryValues();
	}
}