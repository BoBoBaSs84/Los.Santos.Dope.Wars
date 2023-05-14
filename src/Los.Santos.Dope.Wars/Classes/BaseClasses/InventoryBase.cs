using LSDW.Interfaces.Classes;

namespace LSDW.Classes.BaseClasses;

internal abstract class InventoryBase : IInventory
{
	private readonly List<IDrug> _drugs;

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
			Remove(drugToAdd.Quantity * drugToAdd.Price);
		}
		else
		{
			existingDrug.Add(drugToAdd.Quantity, drugToAdd.Price);
			Remove(drugToAdd.Quantity * drugToAdd.Price);
		}
	}

	public void Add(int moneyToAdd)
		=> Money += moneyToAdd;

	public void Remove(IDrug drugToRemove)
	{
		IDrug? existingDrug = _drugs
			.Where(x => x.DrugType.Equals(drugToRemove.DrugType))
			.SingleOrDefault();

		existingDrug?.Remove(drugToRemove.Quantity);

		Add(drugToRemove.Quantity * drugToRemove.Price);
	}

	public void Remove(int moneyToRemove)
		=> Money -= moneyToRemove;

	IEnumerator IEnumerable.GetEnumerator() => _drugs.GetEnumerator();
}
