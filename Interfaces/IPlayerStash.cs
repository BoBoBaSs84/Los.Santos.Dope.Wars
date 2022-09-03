namespace Los.Santos.Dope.Wars.Interfaces;

/// <summary>
/// The <see cref="IPlayerStash"/> interface, provides th epublic methods for the player stash.
/// </summary>
public interface IPlayerStash
{
	/// <summary>
	/// The method is used to manipulate the player stash by adding drugs through a buy transaction.
	/// </summary>
	/// <param name="drugName">The name of the drug.</param>
	/// <param name="drugQuantity">The quantity / amount of the drug.</param>
	/// <param name="drugPrice">The current price of the drug.</param>
	void BuyDrug(string drugName, int drugQuantity, int drugPrice);

	/// <summary>
	/// The method is used to manipulate the player stash by adding drugs through a sell transaction.
	/// </summary>
	/// <param name="drugName">The name of the drug.</param>
	/// <param name="drugQuantity">The quantity / amount of the drug.</param>
	/// <param name="drugPrice">The current price of the drug.</param>
	void SellDrug(string drugName, int drugQuantity, int drugPrice);

	/// <summary>
	/// The method is used to make transfers into player inventories by adding drug into a inventory.
	/// </summary>
	/// <param name="drugName">The name of the drug.</param>
	/// <param name="drugQuantity">The quantity / amount of the drug.</param>
	/// <param name="drugPrice">The current price of the drug.</param>
	void MoveIntoInventory(string drugName, int drugQuantity, int drugPrice);

	/// <summary>
	/// The method is used to make transfers into player inventories by removing drug from a inventory.
	/// </summary>
	/// <param name="drugName">The name of the drug.</param>
	/// <param name="drugQuantity">The quantity / amount of the drug.</param>
	void TakeFromInventory(string drugName, int drugQuantity);
}
