using LSDW.Core.Classes;
using LSDW.Core.Enumerators;
using LSDW.Core.Extensions;
using LSDW.Core.Helpers;
using LSDW.Core.Interfaces.Classes;
using LSDW.Core.Properties;

namespace LSDW.Core.Factories;

/// <summary>
/// The drug fcactory class.
/// </summary>
public static class DrugFactory
{
	private static readonly double MinValue = Settings.Default.MinimumDrugValue;
	private static readonly double MaxValue = Settings.Default.MaximumDrugValue;

	/// <summary>
	/// Should create a drug instance.
	/// </summary>
	/// <param name="drugType">The type of the drug.</param>
	/// <param name="quantity">The quantity of the drug.</param>
	/// <param name="price">The price of the drug.</param>
	public static IDrug CreateDrug(DrugType drugType, int quantity, int price)
		=> new Drug(drugType, quantity, price);

	/// <summary>
	/// Should create a drug instance from saved drug state.
	/// </summary>
	/// <param name="drugState">The saved drug state.</param>
	public static IDrug CreateDrug(DrugState drugState)
		=> new Drug(drugState.DrugType, drugState.Quantity, drugState.Price);

	/// <summary>
	/// Should create a drug collection instance from a saved drug collection state.
	/// </summary>
	/// <param name="drugs">The saved drug collection state.</param>
	public static IEnumerable<IDrug> CreateDrugs(List<DrugState> drugs)
	{
		List<IDrug> drugList = new();
		foreach(DrugState drug in drugs)
			drugList.Add(CreateDrug(drug));
		return drugList;
	}

	/// <summary>
	/// Should create a random drug instance.
	/// </summary>
	public static IDrug CreateRandomDrug()
	{
		List<IDrug> drugList = CreateRandomDrugs().ToList();
		return drugList[RandomHelper.GetInt(0, drugList.Count)];
	}

	/// <summary>
	/// Should create a drug collection instance of all available drugs.
	/// </summary>
	/// <remarks>
	/// <see cref="IDrug.Quantity"/> and <see cref="IDrug.Price"/> are randomly choosen.
	/// </remarks>
	public static IEnumerable<IDrug> CreateRandomDrugs()
	{
		List<DrugType> drugTypes = DrugType.COKE.GetList();
		List<IDrug> drugs = new();
		foreach (DrugType drugType in drugTypes)
			drugs.Add(CreateDrug(drugType, RandomHelper.GetInt(5, 51), GetRandomValue(drugType)));
		return drugs;
	}

	private static int GetRandomValue(DrugType drugType)
	{
		double marketValue = drugType.GetMarketValue();
		return RandomHelper.GetInt(marketValue * MinValue, marketValue * MaxValue);
	}
}
