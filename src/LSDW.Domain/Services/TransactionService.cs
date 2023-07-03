using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Abstractions.Domain.Services;
using LSDW.Abstractions.Enumerators;
using RESX = LSDW.Domain.Properties.Resources;

namespace LSDW.Domain.Services;

/// <summary>
/// The transaction service class.
/// </summary>
internal sealed class TransactionService : ITransactionService
{
	private readonly INotificationProvider _notificationProvider;
	private readonly TransactionType _type;
	private readonly IInventory _source;
	private readonly IInventory _target;
	private readonly int _maxQuantity;

	/// <summary>
	/// Initializes a instance of the transaction service class.
	/// </summary>
	/// <param name="notificationProvider">The notification provider instance to use.</param>
	/// <param name="type">The type of the transaction.</param>
	/// <param name="source">The transaction source.</param>
	/// <param name="target">The transaction target.</param>
	/// <param name="maxQuantity">The maximum target quantity.</param>
	internal TransactionService(INotificationProvider notificationProvider, TransactionType type, IInventory source, IInventory target, int maxQuantity)
	{
		_notificationProvider = notificationProvider;
		_type = type;
		_source = source;
		_target = target;
		_maxQuantity = maxQuantity;
	}

	public bool Commit(DrugType type, int quantity, int price)
	{
		int transactionValue = price * quantity;

		if (!CheckInventory(quantity))
			return false;

		if (!CheckMoney(transactionValue))
			return false;

		_source.Remove(type, quantity);
		_target.Add(type, quantity, price);

		TransferMoney(transactionValue);
		return true;
	}

	/// <summary>
	/// Checks if the target has enough room for the transaction.
	/// </summary>
	/// <param name="quantity">The quantity to add.</param>
	/// <returns></returns>
	private bool CheckInventory(int quantity)
	{
		if (quantity + _target.Sum(d => d.Quantity) >= _maxQuantity)
		{
			_notificationProvider.ShowSubtitle(RESX.Transaction_Result_Message_NoInventory);
			return false;
		}

		return true;
	}

	/// <summary>
	/// Checks if the target has enough money for the transaction.
	/// </summary>
	/// <param name="transactionValue">The transaction value to check.</param>
	/// <returns></returns>
	private bool CheckMoney(int transactionValue)
	{
		if (_type is not TransactionType.BUY or TransactionType.SELL)
			return true;

		if (_target.Money <= transactionValue)
		{
			_notificationProvider.ShowSubtitle(RESX.Transaction_Result_Message_NoMoney);
			return false;
		}

		return true;
	}

	/// <summary>
	/// Transfer the money from the target inventory to the source inventory.
	/// </summary>
	private void TransferMoney(int transactionValue)
	{
		if (_type is not TransactionType.BUY or TransactionType.SELL)
			return;

		_source.Add(transactionValue);
		_target.Remove(transactionValue);
	}
}
