using LSDW.Domain.Enumerators;
using LSDW.Domain.Interfaces.Models;
using LSDW.Domain.Interfaces.Services;
using RESX = LSDW.Domain.Properties.Resources;

namespace LSDW.Domain.Classes.Services;

/// <summary>
/// The transaction service class.
/// </summary>
internal sealed class TransactionService : ITransactionService
{
	private readonly TransactionType _transactionType;
	private readonly IInventory _source;
	private readonly IInventory _target;
	private readonly int _maximumQuantity;

	/// <summary>
	/// Initializes a instance of the transaction service class.
	/// </summary>
	/// <param name="type">The type of the transaction.</param>
	/// <param name="source">The transaction source.</param>
	/// <param name="target">The transaction target.</param>
	/// <param name="maximumQuantity">The maximum target quantity.</param>
	internal TransactionService(TransactionType type, IInventory source, IInventory target, int maximumQuantity)
	{
		_transactionType = type;
		_source = source;
		_target = target;
		_maximumQuantity = maximumQuantity;

		Errors = new List<string>();
	}

	public ICollection<string> Errors { get; }

	public bool Commit(DrugType type, int quantity, int price)
	{
		CheckTargetInventory(quantity);

		CheckTargetMoney(quantity, price);

		if (Errors.Any())
			return false;

		_source.Remove(type, quantity);
		_target.Add(type, quantity, price);

		TransferMoney(quantity, price);

		return true;
	}

	/// <summary>
	/// Checks if the target has enough room for the transaction.
	/// </summary>
	private void CheckTargetInventory(int quantity)
	{
		if (quantity + _target.Sum(d => d.Quantity) <= _maximumQuantity)
			return;

		Errors.Add(RESX.Transaction_Result_Message_NoInventory);
	}

	/// <summary>
	/// Checks if the target has enough money for the transaction.
	/// </summary>
	private void CheckTargetMoney(int quantity, int price)
	{
		if (!Equals(_transactionType, TransactionType.TRAFFIC))
			return;

		if (_target.Money >= price * quantity)
			return;

		Errors.Add(RESX.Transaction_Result_Message_NoMoney);
	}

	/// <summary>
	/// Transfer the money from the target inventory to the source inventory.
	/// </summary>
	private void TransferMoney(int quantity, int price)
	{
		if (!Equals(_transactionType, TransactionType.TRAFFIC))
			return;

		int transactionValue = price * quantity;

		_source.Add(transactionValue);
		_target.Remove(transactionValue);
	}
}
