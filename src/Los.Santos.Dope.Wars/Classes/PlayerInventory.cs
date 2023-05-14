using LSDW.Classes.BaseClasses;
using LSDW.Interfaces.Classes;

namespace LSDW.Classes;

/// <summary>
/// The player inventory class.
/// </summary>
internal sealed class PlayerInventory : InventoryBase
{
	/// <summary>
	/// Initializes a instance of the player inventory class.
	/// </summary>
	/// <param name="drugs"></param>
	internal PlayerInventory(List<IDrug> drugs) : base(drugs)
	{
	}
}
