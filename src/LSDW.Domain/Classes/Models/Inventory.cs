﻿using LSDW.Domain.Classes.Models.Base;
using LSDW.Domain.Enumerators;
using LSDW.Domain.Factories;
using LSDW.Domain.Interfaces.Models;
using System.Collections;

namespace LSDW.Domain.Classes.Models;

/// <summary>
/// The inventory class.
/// </summary>
/// <remarks>
/// Inherits from the <see cref="Notification"/> class and
/// implements the members of the <see cref="IInventory"/>
/// </remarks>
internal sealed class Inventory : Notification, IInventory
{
	private readonly List<IDrug> _drugs;
	private int money;

	/// <summary>
	/// Initializes a instance of the inventory class.
	/// </summary>
	/// <param name="drugs">The collection of drugs to add to the inventory.</param>
	/// <param name="money">The money to add to the inventory.</param>
	public Inventory(IEnumerable<IDrug> drugs, int money)
	{
		_drugs = drugs.ToList();
		Money = money;
	}

	public int Count
		=> _drugs.Count;

	public int Money
	{
		get => money;
		private set => SetProperty(ref money, value);
	}

	public int TotalQuantity
		=> _drugs.Sum(drug => drug.Quantity);

	public int TotalValue
		=> _drugs.Sum(drug => drug.Price * drug.Quantity);

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

	public void Add(DrugType drugType, int quantity, int price)
		=> Add(DomainFactory.CreateDrug(drugType, quantity, price));

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

		return true;
	}

	public void Remove(IEnumerable<IDrug> drugsToRemove)
	{
		foreach (IDrug drug in drugsToRemove)
			_ = Remove(drug);
	}

	public void Remove(DrugType drugType, int quantity)
		=> Remove(DomainFactory.CreateDrug(drugType, quantity));

	public void Remove(int moneyToRemove)
	{
		if (moneyToRemove < 1)
			return;

		Money -= moneyToRemove;
	}

	IEnumerator IEnumerable.GetEnumerator() => _drugs.GetEnumerator();
}