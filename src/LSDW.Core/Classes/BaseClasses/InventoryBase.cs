using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Classes.BaseClasses;

internal abstract class InventoryBase : IInventoryCollection
{
	private readonly List<IDrug> _drugs;

	/// <summary>
	/// Initializes a instance of the inventory base class.
	/// </summary>
	/// <param name="drugs">The drugs to add to the inventory.</param>
	public InventoryBase(List<IDrug> drugs)
		=> _drugs = drugs;

	public int Count
		=> _drugs.Count;

	public int Money { get; private set; }

	public int TotalQuantity
		=> _drugs.Sum(drug => drug.Quantity);

	public int TotalMarketValue
		=> _drugs.Sum(drug => drug.MarketValue * drug.Quantity);

	public int TotalProfit
		=> _drugs.Sum(drug => drug.Profit);

	public bool IsReadOnly => false;

	public void Clear()
		=> _drugs.Clear();

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

	public void Add(int moneyToAdd)
	{
		if (moneyToAdd < 1)
			throw new ArgumentOutOfRangeException(nameof(moneyToAdd));
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

		if (Equals(existingDrug.Quantity, 0))
			return _drugs.Remove(existingDrug);

		return true;
	}

	public void Remove(int moneyToRemove)
	{
		if (moneyToRemove < 1)
			throw new ArgumentOutOfRangeException(nameof(moneyToRemove));
		Money -= moneyToRemove;
	}

	IEnumerator IEnumerable.GetEnumerator() => _drugs.GetEnumerator();
}
