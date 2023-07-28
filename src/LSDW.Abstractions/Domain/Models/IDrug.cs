using LSDW.Abstractions.Domain.Models.Base;
using LSDW.Abstractions.Enumerators;

namespace LSDW.Abstractions.Domain.Models;

/// <summary>
/// The drug interface.
/// </summary>
public interface IDrug : INotificationBase
{
	/// <summary>
	/// The type of the drug.
	/// </summary>
	DrugType Type { get; }

	/// <summary>
	/// The quantity of the drug.
	/// </summary>
	int Quantity { get; }

	/// <summary>
	/// The price of the drug.
	/// </summary>
	int Price { get; }

	/// <summary>
	/// Should add the quantity and price to the drug.
	/// </summary>
	/// <param name="quantity">The quantity of the drug to add.</param>
	/// <param name="price">The price of the drug to add.</param>
	void Add(int quantity, int price);

	/// <summary>
	/// Should remove the quantity from the drug.
	/// </summary>
	/// <param name="quantity">The quantity of the drug to remove.</param>
	void Remove(int quantity);

	/// <summary>
	/// Randomizes the price for the provided drug.
	/// </summary>
	/// <remarks>
	/// The upper and lower price limits depend on the player
	/// level <b>(maximum ±10%)</b> and the user settings.
	/// </remarks>
	/// <param name="playerLevel">The current player level.</param>
	void RandomizePrice(int playerLevel);

	/// <summary>
	/// Randomizes the quantity for the provided drug.
	/// </summary>
	/// <remarks>
	/// The zero quantity chance depends on the rank of the drug,
	/// the higher the rank the higher the no quantity chance.
	/// </remarks>
	/// <param name="playerLevel">The current player level.</param>
	void RandomizeQuantity(int playerLevel);

	/// <summary>
	/// Set the drug <see cref="Quantity"/> to the lowest possible value
	/// and the <see cref="Price"/> to the highest possible value.
	/// </summary>
	/// <param name="playerLevel">The current player level.</param>
	void SpecialBuyOffer(int playerLevel);

	/// <summary>
	/// Set the drug <see cref="Quantity"/> to the highest possible value
	/// and the <see cref="Price"/> to the lowest possible value.
	/// </summary>
	/// <param name="playerLevel">The current player level.</param>
	void SpecialSellOffer(int playerLevel);
}
