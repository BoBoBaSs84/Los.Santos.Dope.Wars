using LSDW.Abstractions.Domain.Models.Base;

namespace LSDW.Abstractions.Domain.Models;

public partial interface ISettings
{
	/// <summary>
	/// The market settings interface.
	/// </summary>
	public interface IMarketSettings : INotificationBase
	{
		/// <summary>
		/// The inventory change interval property.
		/// </summary>
		int InventoryChangeInterval { get; set; }

		/// <summary>
		/// The maximum drug price factor.
		/// </summary>
		float MaximumDrugPrice { get; set; }

		/// <summary>
		/// The minimum drug price factor.
		/// </summary>
		float MinimumDrugPrice { get; set; }

		/// <summary>
		/// The price change interval property.
		/// </summary>
		int PriceChangeInterval { get; set; }

		/// <summary>
		/// The special offer chance property.
		/// </summary>
		float SpecialOfferChance { get; set; }

		/// <summary>
		/// Returns the possible inventory change interval values.
		/// </summary>
		/// <returns>The list of possible values.</returns>
		int[] GetInventoryChangeIntervalValues();

		/// <summary>
		/// Returns the possible maximum drug price factor values.
		/// </summary>
		/// <returns>The list of possible values.</returns>
		float[] GetMaximumDrugPriceValues();

		/// <summary>
		/// Returns the possible minimum drug price factor values.
		/// </summary>
		/// <returns>The list of possible values.</returns>
		float[] GetMinimumDrugPriceValues();

		/// <summary>
		/// Returns the possible price change interval values.
		/// </summary>
		/// <returns>The list of possible values.</returns>
		int[] GetPriceChangeIntervalValues();

		/// <summary>
		/// Returns the possible special offer chance values.
		/// </summary>
		/// <returns>The list of possible values.</returns>
		float[] GetSpecialOfferChanceValues();
	}
}