using LSDW.Core.Enumerators;

namespace LSDW.Core.Interfaces.Classes;

/// <summary>
/// The drug interface.
/// </summary>
public interface IDrug : INotifyPropertyChanged
{
	/// <summary>
	/// The type of the drug.
	/// </summary>
	DrugType DrugType { get; }

	/// <summary>
	/// The display name of the drug.
	/// </summary>
	string Name { get; }

	/// <summary>
	/// The market price of the drug.
	/// </summary>
	int MarketValue { get; }

	/// <summary>
	/// The quantity of the drug.
	/// </summary>
	int Quantity { get; }

	/// <summary>
	/// The price of the drug.
	/// </summary>
	int Price { get; }

	/// <summary>
	/// The profit of the drug compared to the nominal market price.
	/// </summary>
	int PossibleProfit { get; }

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
	/// Sets the current quantity for the drug.
	/// </summary>
	/// <param name="quantity">The new quantity of the drug.</param>
	void SetQuantity(int quantity);

	/// <summary>
	/// Sets the current price for the drug.
	/// </summary>
	/// <param name="price">The new price of the drug.</param>
	void SetPrice(int price);

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
}
