namespace Los.Santos.Dope.Wars.Interfaces;

/// <summary>
/// The <see cref="IPlayerStash"/> interface, provides methods specifically for the player
/// </summary>
public interface IPlayerStash
{
	/// <summary>
	/// The <see cref="BuyDrug(string, int, int)"/> method is used to manipulate the player stash
	/// </summary>
	/// <param name="drugName"></param>
	/// <param name="drugQuantity"></param>
	/// <param name="drugPrice"></param>
	void BuyDrug(string drugName, int drugQuantity, int drugPrice);

	/// <summary>
	/// The <see cref="SellDrug(string, int, int)"/> method is used to manipulate the player stash
	/// </summary>
	/// <param name="drugName"></param>
	/// <param name="drugQuantity"></param>
	/// <param name="drugPrice"></param>
	void SellDrug(string drugName, int drugQuantity, int drugPrice);

	/// <summary>
	/// The <see cref="MoveIntoInventory(string, int, int)"/> method is used to make transfers into player inventories
	/// </summary>
	/// <param name="drugName"></param>
	/// <param name="drugQuantity"></param>
	/// <param name="drugPrice"></param>
	void MoveIntoInventory(string drugName, int drugQuantity, int drugPrice);

	/// <summary>
	/// The <see cref="TakeFromInventory(string, int)"/> method is used to make transfers from player inventories
	/// </summary>
	/// <param name="drugName"></param>
	/// <param name="drugQuantity"></param>
	void TakeFromInventory(string drugName, int drugQuantity);
}
