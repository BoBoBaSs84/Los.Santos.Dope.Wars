using LSDW.Core.Interfaces.Classes;
using System.Collections;

namespace LSDW.Core.Classes;

/// <summary>
/// The inventory class.
/// </summary>
internal sealed class Inventory : IInventory
{
	private readonly List<IDrug> _drugs;

	/// <summary>
	/// Initializes a instance of the inventory base class.
	/// </summary>
	/// <param name="drugs">The drugs to add to the inventory.</param>
	/// <param name="money">The money to add to the inventory.</param>
	public Inventory(IEnumerable<IDrug> drugs, int money)
	{
		_drugs = drugs.ToList();
		Money = money;
	}

	public int Count
		=> _drugs.Count;

	public int Money { get; private set; }

	public int TotalQuantity
		=> _drugs.Sum(drug => drug.Quantity);

	public int TotalMarketValue
		=> _drugs.Sum(drug => drug.MarketValue * drug.Quantity);

	public int TotalProfit
		=> _drugs.Sum(drug => drug.PossibleProfit);

	public bool IsReadOnly => false;

	public void Clear()
	{
		Money = 0;
		_drugs.Clear();
	}

	public bool Contains(IDrug item)
		=> _drugs.Contains(item);

	public void CopyTo(IDrug[] array, int arrayIndex)
		=> _drugs.CopyTo(array, arrayIndex);

	public IEnumerator<IDrug> GetEnumerator()
		=> _drugs.GetEnumerator();

	public void Add(IDrug drugToAdd)
	{
		IDrug? existingDrug = _drugs
			.Where(x => x.DrugType.Equals(drugToAdd.DrugType))
			.SingleOrDefault();

		if (existingDrug is null)
			_drugs.Add(drugToAdd);
		else
			existingDrug.Add(drugToAdd.Quantity, drugToAdd.Price);
	}

	public void Add(IEnumerable<IDrug> drugsToAdd)
	{
		foreach (IDrug drug in drugsToAdd)
			Add(drug);
	}

	public void Add(int moneyToAdd)
	{
		if (moneyToAdd < 1)
			return;

		Money += moneyToAdd;
	}

	public bool Remove(IDrug drugToRemove)
	{
		IDrug? existingDrug = _drugs
			.Where(x => x.DrugType.Equals(drugToRemove.DrugType))
			.SingleOrDefault();

		if (existingDrug is null)
			return false;

		existingDrug.Remove(drugToRemove.Quantity);

		return !Equals(existingDrug.Quantity, 0) || _drugs.Remove(existingDrug);
	}

	public void Remove(IEnumerable<IDrug> drugsToRemove)
	{
		foreach (IDrug drug in drugsToRemove)
			_ = Remove(drug);
	}

	public void Remove(int moneyToRemove)
	{
		if (moneyToRemove < 1)
			return;

		Money -= moneyToRemove;
	}

	IEnumerator IEnumerable.GetEnumerator() => _drugs.GetEnumerator();
}
