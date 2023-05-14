using LSDW.Interfaces.Classes;

namespace LSDW.Classes.BaseClasses;

internal abstract class InventoryBase : IInventory
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

	public IEnumerator<IDrug> GetEnumerator()
		=> _drugs.GetEnumerator();

	public void Add(IDrug drugToAdd)
	{
		IDrug? existingDrug = _drugs
			.Where(x => x.DrugType.Equals(drugToAdd.DrugType))
			.SingleOrDefault();

		if (existingDrug is null)
		{
			_drugs.Add(drugToAdd);
			int moneyToRemove = drugToAdd.Quantity * drugToAdd.Price;
			Remove(moneyToRemove);
		}
		else
		{
			existingDrug.Add(drugToAdd.Quantity, drugToAdd.Price);
			int moneyToRemove = drugToAdd.Quantity * drugToAdd.Price;
			Remove(moneyToRemove);
		}
	}

	public void Add(int moneyToAdd)
	{
		if (moneyToAdd < 1)
			throw new ArgumentOutOfRangeException(nameof(moneyToAdd));
		Money += moneyToAdd;
	}
		

	public void Remove(IDrug drugToRemove)
	{
		IDrug? existingDrug = _drugs
			.Where(x => x.DrugType.Equals(drugToRemove.DrugType))
			.SingleOrDefault();

		existingDrug?.Remove(drugToRemove.Quantity);
		int moneyToAdd = drugToRemove.Quantity * drugToRemove.Price;
		Add(moneyToAdd);
	}

	public void Remove(int moneyToRemove)
	{
		if (moneyToRemove < 1)
			throw new ArgumentOutOfRangeException(nameof(moneyToRemove));
		Money -= moneyToRemove;
	}
		

	IEnumerator IEnumerable.GetEnumerator() => _drugs.GetEnumerator();
}
