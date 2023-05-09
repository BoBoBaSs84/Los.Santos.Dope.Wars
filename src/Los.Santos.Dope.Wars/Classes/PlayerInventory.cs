using LSDW.Interfaces.Classes;

namespace LSDW.Classes;

/// <summary>
/// The player inventory class.
/// </summary>
internal sealed class PlayerInventory : Inventory
{
	/// <summary>
	/// Initializes a instance of the player inventory class.
	/// </summary>
	/// <param name="drugs"></param>
	public PlayerInventory(List<IDrug> drugs) : base(drugs)
	{ }
}
