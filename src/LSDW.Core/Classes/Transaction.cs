using LSDW.Core.Enumerators;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Classes;

/// <summary>
/// The transaction class.
/// </summary>
internal sealed class Transaction : ITransaction
{
	/// <summary>
	/// Initializes a instance of the transaction class.
	/// </summary>
	/// <param name="type">The type of the transaction.</param>
	/// <param name="objects">The transaction objects to process.</param>
	/// <param name="maximumQuantity">The targets maximum inventory quantity.</param>
	internal Transaction(TransactionType type, IEnumerable<TransactionObject> objects, int maximumQuantity)
	{
		Type = type;
		Objects = objects;
		MaximumTargetQuantity = maximumQuantity;
	}

	public TransactionType Type { get; }

	public int MaximumTargetQuantity { get; }

	public IEnumerable<TransactionObject> Objects { get; }

	public TransactionResult Commit(IInventory source, IInventory target)
	{
		TransactionResult result = new();

		if (Type.Equals(TransactionType.TRAFFIC))
			if (!CheckTargetMoney(target))
				result.Messages.Add("Not enough money for transaction.");

		if (GetResultingTargetQuantity(target) > MaximumTargetQuantity)
			result.Messages.Add("Not enough space in inventory for transaction.");

		if (result.Messages.Any())
			return result;

		IEnumerable<IDrug> drugs = GetDrugsFromObjects();

		foreach (IDrug drug in drugs)
		{
			_ = source.Remove(drug);
			target.Add(drug);
		}

		if (Type.Equals(TransactionType.TRAFFIC))
		{
			int transactionValue = drugs.Sum(drug => drug.Price * drug.Quantity);
			source.Add(transactionValue);
			target.Remove(transactionValue);
		}

		result.Successful = true;
		return result;
	}

	/// <summary>
	/// Returns <see langword="true"/> or <see langword="true"/> if the target has enough money for the transaction.
	/// </summary>
	/// <param name="target">The target inventory.</param>
	private bool CheckTargetMoney(IInventory target)
		=> target.Money > Objects.Sum(o => o.Price * o.Quantity);

	/// <summary>
	/// Returns the resulting target inventory quantity.
	/// </summary>
	private int GetResultingTargetQuantity(IInventory target)
		=> Objects.Sum(o => o.Quantity) + target.Sum(d => d.Quantity);

	/// <summary>
	/// Returns a drug list from the transaction objects.
	/// </summary>
	private IEnumerable<IDrug> GetDrugsFromObjects()
	{
		List<IDrug> drugsToReturn = new();
		foreach (TransactionObject obj in Objects)
			drugsToReturn.Add(DrugFactory.CreateDrug(obj.DrugType, obj.Quantity, obj.Price));
		return drugsToReturn;
	}
}
