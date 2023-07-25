using LSDW.Abstractions.Helpers;
using static LSDW.Abstractions.Domain.Models.ISettings;

namespace LSDW.Domain.Models;

internal sealed partial class Settings
{
	/// <summary>
	/// The market settings class.
	/// </summary>
	internal sealed class MarketSettings : IMarketSettings
	{
		private static readonly Lazy<MarketSettings> _settings = new(() => new());

		/// <summary>
		/// Initializes a instance of the market settings class.
		/// </summary>
		private MarketSettings()
		{
			SpecialOfferChance = new(0.15f);
			InventoryChangeInterval = new(24);
			PriceChangeInterval = new(6);
			MaximumDrugPrice = new(1.15f);
			MinimumDrugPrice = new(0.85f);
		}

		/// <summary>
		/// The singleton instance of the market settings.
		/// </summary>
		public static MarketSettings Instance
			=> _settings.Value;

		/// <inheritdoc/>
		public BindableProperty<float> SpecialOfferChance { get; set; }

		/// <inheritdoc/>
		public BindableProperty<int> InventoryChangeInterval { get; set; }

		/// <inheritdoc/>
		public BindableProperty<int> PriceChangeInterval { get; set; }

		/// <inheritdoc/>
		public BindableProperty<float> MaximumDrugPrice { get; set; }

		/// <inheritdoc/>
		public BindableProperty<float> MinimumDrugPrice { get; set; }

		/// <inheritdoc/>
		public float[] GetSpecialOfferChanceValues()
			=> new float[] { 0.5f, 0.10f, 0.15f, 0.20f, 0.25f };

		/// <inheritdoc/>
		public int[] GetInventoryChangeIntervalValues()
			=> new int[] { 24, 48, 72, 96, 120, 144, 168 };

		/// <inheritdoc/>
		public int[] GetPriceChangeIntervalValues()
			=> new int[] { 3, 6, 8, 12, 24 };

		/// <inheritdoc/>
		public float[] GetMaximumDrugPriceValues()
			=> new float[] { 1.05f, 1.1f, 1.15f, 1.2f, 1.25f };

		/// <inheritdoc/>
		public float[] GetMinimumDrugPriceValues()
			=> new float[] { 0.75f, 0.8f, 0.85f, 0.9f, 0.95f };
	}
}
