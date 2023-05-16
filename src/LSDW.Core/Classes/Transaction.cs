using LSDW.Core.Enumerators;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;
using RESX = LSDW.Core.Properties.Resources;

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
		Result = new();
	}

	public TransactionType Type { get; }

	public int MaximumTargetQuantity { get; }

	public IEnumerable<TransactionObject> Objects { get; }

	public TransactionResult Result { get; }

	public void Commit(IInventory source, IInventory target)
	{
		if (Result.IsCompleted)
			return;

		if (Type.Equals(TransactionType.TRAFFIC))
			CheckTargetMoney(target);

		CheckTargetInventory(target);

		if (Result.Messages.Any())
		{
			Result.Failed();
			return;
		}

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

		Result.Success();
	}

	/// <summary>
	/// Checks if the target has enough money for the transaction.
	/// </summary>
	/// <param name="target">The target inventory.</param>
	private void CheckTargetMoney(IInventory target)
	{
		if (target.Money > Objects.Sum(o => o.Price * o.Quantity))
			return;
		Result.Messages.Add(RESX.Transaction_Result_Message_Money);
	}

	/// <summary>
	/// Checks if the target has enough room for the transaction.
	/// </summary>
	/// <param name="target">The target inventory.</param>
	private void CheckTargetInventory(IInventory target)
	{
		if (Objects.Sum(o => o.Quantity) + target.Sum(d => d.Quantity) < MaximumTargetQuantity)
			return;
		Result.Messages.Add(RESX.Transaction_Result_Message_Inventory);
	}

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
