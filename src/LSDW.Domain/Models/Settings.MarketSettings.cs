using LSDW.Domain.Models.Base;
using static LSDW.Abstractions.Domain.Models.ISettings;

namespace LSDW.Domain.Models;

internal sealed partial class Settings
{
	/// <summary>
	/// The market settings class.
	/// </summary>
	internal sealed class MarketSettings : NotificationBase, IMarketSettings
	{
		private static readonly Lazy<MarketSettings> _settings = new(() => new());
		private float specialOfferChance;
		private int inventoryChangeInterval;
		private int priceChangeInterval;
		private float maximumDrugPrice;
		private float minimumDrugPrice;

		/// <summary>
		/// Initializes a instance of the market settings class.
		/// </summary>
		private MarketSettings()
		{
			specialOfferChance = 0.15f;
			inventoryChangeInterval = 24;
			priceChangeInterval = 6;
			maximumDrugPrice = 1.15f;
			minimumDrugPrice = 0.85f;
		}

		/// <summary>
		/// The singleton instance of the market settings.
		/// </summary>
		public static MarketSettings Instance
			=> _settings.Value;

		/// <inheritdoc/>
		public float SpecialOfferChance
		{
			get => specialOfferChance;
			set => SetProperty(ref specialOfferChance, value);
		}

		/// <inheritdoc/>
		public int InventoryChangeInterval
		{
			get => inventoryChangeInterval;
			set => SetProperty(ref inventoryChangeInterval, value);
		}

		/// <inheritdoc/>
		public int PriceChangeInterval
		{
			get => priceChangeInterval;
			set => SetProperty(ref priceChangeInterval, value);
		}

		/// <inheritdoc/>
		public float MaximumDrugPrice
		{
			get => maximumDrugPrice;
			set => SetProperty(ref maximumDrugPrice, value);
		}

		/// <inheritdoc/>
		public float MinimumDrugPrice
		{
			get => minimumDrugPrice;
			set => SetProperty(ref minimumDrugPrice, value);
		}

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
