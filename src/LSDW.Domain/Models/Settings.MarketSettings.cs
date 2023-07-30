using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Models.Base;
using LSDW.Domain.Models.Base;

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
			InventoryChangeInterval = new BindableProperty<int>(24);
			MaximumDrugPrice = new BindableProperty<float>(1.15f);
			MinimumDrugPrice = new BindableProperty<float>(0.85f);
			PriceChangeInterval = new BindableProperty<int>(6);
			SpecialOfferChance = new BindableProperty<float>(0.15f);
		}

		/// <summary>
		/// The singleton instance of the market settings.
		/// </summary>
		public static MarketSettings Instance
			=> _settings.Value;

		public IBindableProperty<int> InventoryChangeInterval { get; }
		public IBindableProperty<float> MaximumDrugPrice { get; }
		public IBindableProperty<float> MinimumDrugPrice { get; }
		public IBindableProperty<int> PriceChangeInterval { get; }
		public IBindableProperty<float> SpecialOfferChance { get; }
		
		public float[] GetSpecialOfferChanceValues()
			=> new float[] { 0.5f, 0.10f, 0.15f, 0.20f, 0.25f };

		public int[] GetInventoryChangeIntervalValues()
			=> new int[] { 24, 48, 72, 96, 120, 144, 168 };

		public int[] GetPriceChangeIntervalValues()
			=> new int[] { 3, 6, 8, 12, 24 };

		public float[] GetMaximumDrugPriceValues()
			=> new float[] { 1.05f, 1.1f, 1.15f, 1.2f, 1.25f };

		public float[] GetMinimumDrugPriceValues()
			=> new float[] { 0.75f, 0.8f, 0.85f, 0.9f, 0.95f };
	}
}
