using LSDW.Domain.Models;
using LSDW.Presentation.Bindings;
using LSDW.Presentation.Extensions;
using LSDW.Presentation.Menus.Base;

namespace LSDW.Presentation.Menus;

/// <summary>
/// Represents the market settings menu, allowing users to configure settings related to the in-game market.
/// </summary>
internal sealed class MarketSettingsMenu : BaseMenu
{
	/// <summary>
	/// Initializes a new instance of the <see cref="MarketSettingsMenu"/> class.
	/// </summary>
	/// <param name="settings">The settings instance to be used by the market settings menu.</param>
	public MarketSettingsMenu(MarketSettings settings) : base("Market", "Market Settings", "Configure settings related to the in-game market.")
		=> AddDatabindings(settings);

	private void AddDatabindings(MarketSettings settings)
	{
		IPropertyBinding priceChangeIntervalBinding = AddListItem("Price Change Interval In Hours", "The amount of hours after which the market prices change.", MarketSettings.PriceChangeIntervalValues)
			.BindTo(settings, nameof(MarketSettings.PriceChangeIntervalInHours));
		Bindings.Add(priceChangeIntervalBinding);

		IPropertyBinding maximumDrugPriceFactorBinding = AddListItem("Maximum Drug Price Factor", "The maximum drug price multiplier for the market.", MarketSettings.MaximumDrugPriceValues)
			.BindTo(settings, nameof(MarketSettings.MaximumDrugPriceFactor));
		Bindings.Add(maximumDrugPriceFactorBinding);

		IPropertyBinding minimumDrugPriceFactorBinding = AddListItem("Minimum Drug Price Factor", "The minimum drug price multiplier for the market.", MarketSettings.MinimumDrugPriceValues)
			.BindTo(settings, nameof(MarketSettings.MinimumDrugPriceFactor));
		Bindings.Add(minimumDrugPriceFactorBinding);

		IPropertyBinding specialOfferChanceBinding = AddListItem("Special Offer Chance", "The chance for a special offer to appear in the market.", MarketSettings.SpecialOfferChanceValues)
			.BindTo(settings, nameof(MarketSettings.SpecialOfferChance));
		Bindings.Add(specialOfferChanceBinding);
	}
}
