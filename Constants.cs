using static Los.Santos.Dope.Wars.Enums;

namespace Los.Santos.Dope.Wars;

/// <summary>
/// The <see cref="Constants"/> class holds all the needed constants
/// </summary>
public static class Constants
{
	/// <summary>
	/// The xml namespace for save file and settings file
	/// </summary>
	public const string XmlNamespace = "http://www.los.santos.dope.wars.org";

	/// <summary>
	/// The volatility of market, so drug prices can reach from -10% to +10%
	/// </summary>
	public const double MarketVolatility = 0.10;

	/// <summary>
	/// The chance if a car contains drugs, see warehouse mission
	/// </summary>
	public const double VehicleContainsDrugsChance = 0.5;

	/// <summary>
	/// For every player level the dealers get additional health and armor values.
	/// This would leed to lvl.1 -> 105 - lvl.50 -> 250
	/// </summary>
	public const float DealerArmorHealthPerLevelFactor = 5f;

	/// <summary>
	/// the additional discount per level factor level 1 means a plus of +0.5% and -0.5%
	/// </summary>
	public const double DiscountPerLevel = 0.005;

	/// <summary>
	/// The <see cref="TradePackOne"/> are the drugs the player can peddle from level 1 on
	/// </summary>
	public const DrugType TradePackOne = DrugType.Mushrooms | DrugType.Amphetamine | DrugType.Oxycodone | DrugType.Marijuana | DrugType.Hashish;

	/// <summary>
	/// The <see cref="TradePackTwo"/> are the drugs the player can additional peddle from level 10 on
	/// </summary>
	public const DrugType TradePackTwo = DrugType.Mescaline | DrugType.MDMA | DrugType.Ecstasy | DrugType.Acid | DrugType.PCP;

	/// <summary>
	/// The <see cref="TradePackThree"/> are the drugs the player can additional peddle from level 20 on
	/// </summary>
	public const DrugType TradePackThree = DrugType.Heroin | DrugType.Cocaine | DrugType.Methamphetamine | DrugType.Crack | DrugType.Ketamine;

	public const float MarkerDrawDistance = 100f;

	public const float DealerCreateDistance = 100f;

	public const float DealInteractionDistance = 2f;

	public const float MarkerInteractionDistance = 1.5f;
}