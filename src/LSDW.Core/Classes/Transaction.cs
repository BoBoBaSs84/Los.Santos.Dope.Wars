using LSDW.Core.Enumerators;
using LSDW.Core.Interfaces.Classes;
using RESX = LSDW.Core.Properties.Resources;

namespace LSDW.Core.Classes;

/// <summary>
/// The transaction class.
/// </summary>
internal sealed class Transaction : ITransaction
{
	private readonly IInventory _source;
	private readonly IInventory _target;
	private readonly ICollection<IDrug> _drugs = new List<IDrug>();

	/// <summary>
	/// Initializes a instance of the transaction class.
	/// </summary>
	/// <param name="type">The type of the transaction.</param>
	/// <param name="source">The source inventory.</param>
	/// <param name="target">The target inventory.</param>
	/// <param name="maximumQuantity">The targets maximum inventory quantity.</param>
	internal Transaction(TransactionType type, IInventory source, IInventory target, int maximumQuantity)
	{
		Type = type;
		_source = source;
		_target = target;
		MaximumTargetQuantity = maximumQuantity;
		Result = new();
	}

	public TransactionType Type { get; }

	public int MaximumTargetQuantity { get; }

	public TransactionResult Result { get; }

	public void Add(IDrug drug)
		=> _drugs.Add(drug);

	public void Add(IEnumerable<IDrug> drugs)
	{
		foreach (IDrug drug in drugs)
			_drugs.Add(drug);
	}

	public void Commit()
	{
		CheckDrugs();

		CheckTargetInventory();

		if (Type.Equals(TransactionType.TRAFFIC))
			CheckTargetMoney();

		if (Result.Messages.Any())
		{
			Result.Failed();
			return;
		}

		_source.Remove(_drugs);
		_target.Add(_drugs);

		if (Type.Equals(TransactionType.TRAFFIC))
		{
			int transactionValue = _drugs.Sum(drug => drug.Price * drug.Quantity);
			_source.Add(transactionValue);
			_target.Remove(transactionValue);
		}

		_drugs.Clear();
		Result.Success();
	}

	/// <summary>
	/// Checks if drugs for the transactionhave been added.
	/// </summary>
	private void CheckDrugs()
	{
		if (_drugs.Any())
			return;
		Result.Messages.Add(RESX.Transaction_Result_Message_NoDrugs);
	}

	/// <summary>
	/// Checks if the target has enough money for the transaction.
	/// </summary>
	private void CheckTargetMoney()
	{
		if (_target.Money >= _drugs.Sum(o => o.Price * o.Quantity))
			return;
		Result.Messages.Add(RESX.Transaction_Result_Message_NoMoney);
	}

	/// <summary>
	/// Checks if the target has enough room for the transaction.
	/// </summary>
	private void CheckTargetInventory()
	{
		if (_drugs.Sum(o => o.Quantity) + _target.Sum(d => d.Quantity) <= MaximumTargetQuantity)
			return;
		Result.Messages.Add(RESX.Transaction_Result_Message_NoInventory);
	}
}
