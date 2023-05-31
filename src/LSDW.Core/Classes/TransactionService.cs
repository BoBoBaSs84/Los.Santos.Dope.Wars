using LSDW.Core.Enumerators;
using LSDW.Core.Interfaces.Classes;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using RESX = LSDW.Core.Properties.Resources;

namespace LSDW.Core.Classes;

/// <summary>
/// The transaction service class.
/// </summary>
internal sealed class TransactionService : ITransactionService
{
	private readonly TransactionType _transactionType;
	private readonly IDrug _drug;
	private readonly IInventory _source;
	private readonly IInventory _target;
	private readonly int _maximumQuantity;
	private readonly ObservableCollection<ILogEntry> logEntries = new();
	
	public event NotifyCollectionChangedEventHandler? LogEntriesChanged;

	/// <summary>
	/// Initializes a instance of the transaction service class.
	/// </summary>
	/// <param name="parameter">The transaction parameter.</param>
	public TransactionService(TransactionParameter parameter)
	{
		_transactionType = parameter.Type;
		_drug = parameter.Drug;
		_source = parameter.Source;
		_target = parameter.Target;
		_maximumQuantity = parameter.MaximumQuantity;

		Errors = new List<string>();

		logEntries.CollectionChanged += LogEntriesChanged;
	}

	public ICollection<string> Errors { get; }

	public void Commit()
	{
		CheckTargetInventory();

		CheckTargetMoney();

		if (Errors.Any())
			return;

		_source.Remove(_drug);
		_target.Add(_drug);

		TransferMoney();

		CreateLogEntry();
	}

	private void CreateLogEntry()
		=> throw new NotImplementedException();

	/// <summary>
	/// Checks if the target has enough money for the transaction.
	/// </summary>
	private void CheckTargetMoney()
	{
		if (!Equals(_transactionType, TransactionType.TRAFFIC))
			return;

		if (_target.Money >= _drug.Price * _drug.Quantity)
			return;

		Errors.Add(RESX.Transaction_Result_Message_NoMoney);
	}

	/// <summary>
	/// Checks if the target has enough room for the transaction.
	/// </summary>
	private void CheckTargetInventory()
	{
		if (_drug.Quantity + _target.Sum(d => d.Quantity) <= _maximumQuantity)
			return;

		Errors.Add(RESX.Transaction_Result_Message_NoInventory);
	}

	/// <summary>
	/// Transfer the money from the target inventory to the source inventory.
	/// </summary>
	private void TransferMoney()
	{
		if (!Equals(_transactionType, TransactionType.TRAFFIC))
			return;

		int transactionValue = _drug.Price * _drug.Quantity;

		_source.Add(transactionValue);
		_target.Remove(transactionValue);
	}
}
