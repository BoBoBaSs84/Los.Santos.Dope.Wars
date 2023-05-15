using LSDW.Core.Classes;
using LSDW.Core.Enumerators;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Factories;

/// <summary>
/// The drug fcactory class.
/// </summary>
public static class DrugFactory
{
	/// <summary>
	/// Should create a random drug instance.
	/// </summary>
	public static IDrug CreateRandomDrug()
		=> new Drug(DrugType.COKE, 1, 1);

	/// <summary>
	/// Should create a drug instance.
	/// </summary>
	/// <param name="drugType">The type of the drug.</param>
	/// <param name="quantity">The quantity of the drug.</param>
	/// <param name="price">The price of the drug.</param>
	public static IDrug CreateDrug(DrugType drugType, int quantity, int price)
		=> new Drug(drugType, quantity, price);
}
