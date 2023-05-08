using LSDW.Interfaces.Classes;
using System.Collections;

namespace LSDW.Classes;

internal abstract class InventoryBase : IInventory
{
	private readonly List<IDrug> _drugs;

	public InventoryBase(List<IDrug> drugs) => _drugs = drugs;

	public int Count => _drugs.Count;

	public int TotalQuantity => _drugs.Sum(drug => drug.Quantity);

	public int TotalValue => _drugs.Sum(drug => drug.Quantity * drug.Price);

	public int TotalProfit => _drugs.Sum(drug => drug.Profit);

	public IEnumerator<IDrug> GetEnumerator() => _drugs.GetEnumerator();

	public void Add(Drug drugToAdd)
	{
		IDrug? existingDrug = _drugs.Where(x => x.DrugType.Equals(drugToAdd.DrugType))
			.SingleOrDefault();

		if (existingDrug is null)
			_drugs.Add(drugToAdd);
		else
			existingDrug.Add(drugToAdd.Quantity, drugToAdd.Price);
	}

	public void Remove(Drug drugToRemove)
	{
		IDrug? existingDrug = _drugs.Where(x => x.DrugType.Equals(drugToRemove.DrugType))
			.SingleOrDefault();

		existingDrug?.Remove(drugToRemove.Quantity);
	}

	IEnumerator IEnumerable.GetEnumerator() => _drugs.GetEnumerator();
}
