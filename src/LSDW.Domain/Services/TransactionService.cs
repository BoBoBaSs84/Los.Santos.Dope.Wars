using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Services;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Extensions;
using LSDW.Abstractions.Models;
using LSDW.Domain.Extensions;
using LSDW.Domain.Properties;

namespace LSDW.Domain.Services;

/// <summary>
/// The transaction service class.
/// </summary>
internal sealed class TransactionService : ITransactionService
{
	private readonly IProviderManager _providerManager;
	private readonly TransactionType _type;
	private readonly IInventory _source;
	private readonly IInventory _target;
	private readonly int _maxQuantity;

	/// <summary>
	/// Initializes a instance of the transaction service class.
	/// </summary>
	/// <param name="providerManager">The provider manager instance to use.</param>
	/// <param name="type">The type of the transaction.</param>
	/// <param name="source">The transaction source.</param>
	/// <param name="target">The transaction target.</param>
	/// <param name="maxQuantity">The maximum target quantity.</param>
	internal TransactionService(IProviderManager providerManager, TransactionType type, IInventory source, IInventory target, int maxQuantity)
	{
		_providerManager = providerManager;
		_type = type;
		_source = source;
		_target = target;
		_maxQuantity = maxQuantity;
	}

	public void BustOrNoBust()
	{
		if (_type is TransactionType.TAKE or TransactionType.GIVE)
			return;

		float random = _providerManager.RandomProvider.GetFloat();

		if (random >= Settings.Trafficking.BustChance)
			return;

		_providerManager.NotificationProvider.ShowSubtitle(Resources.Transaction_Message_Bust);
		_providerManager.PlayerProvider.WantedLevel = Settings.Trafficking.WantedLevel;
		_providerManager.PlayerProvider.DispatchsCops = true;
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
		IndicateSuccess(type, transactionValue);
		return true;
	}

	/// <summary>
	/// Checks if the target has enough room for the transaction.
	/// </summary>
	/// <param name="quantity">The quantity to add.</param>
	/// <returns><see langword="true"/> or <see langword="false"/></returns>
	private bool CheckInventory(int quantity)
	{
		if (quantity + _target.Sum(d => d.Quantity) >= _maxQuantity)
		{
			_providerManager.NotificationProvider.ShowSubtitle(Resources.Transaction_Message_NoInventory);
			return false;
		}

		return true;
	}

	/// <summary>
	/// Checks if the target has enough money for the transaction.
	/// </summary>
	/// <param name="transactionValue">The transaction value to check.</param>
	/// <returns><see langword="true"/> or <see langword="false"/></returns>
	private bool CheckMoney(int transactionValue)
	{
		if (_type is TransactionType.BUY)
		{
			int playerMoney = _providerManager.PlayerProvider.Money;
			if (playerMoney < transactionValue)
			{
				string message = Resources.Transaction_Message_Player_NoMoney.FormatInvariant(transactionValue, playerMoney);
				_providerManager.NotificationProvider.ShowSubtitle(message);
				return false;
			}
		}

		if (_type is TransactionType.SELL)
		{
			int dealerMoney = _target.Money;
			if (dealerMoney < transactionValue)
			{
				string message = Resources.Transaction_Message_Dealer_NoMoney.FormatInvariant(transactionValue, dealerMoney);
				_providerManager.NotificationProvider.ShowSubtitle(message);
				return false;
			}
		}

		return true;
	}

	/// <summary>
	/// Depending on the transaction type, the money is either transferred from the
	/// player to the dealer or the other way around.
	/// </summary>
	/// <param name="transactionValue">The transaction value to transfer.</param>
	private void TransferMoney(int transactionValue)
	{
		if (_type is TransactionType.BUY)
		{
			_providerManager.PlayerProvider.Money -= transactionValue;
			_source.Add(transactionValue);
		}

		if (_type is TransactionType.SELL)
		{
			_target.Remove(transactionValue);
			_providerManager.PlayerProvider.Money += transactionValue;
		}
	}

	private void IndicateSuccess(DrugType type, int transactionValue)
	{
		string soundFile = "PURCHASE";
		string soundSet = "HUD_LIQUOR_STORE_SOUNDSET";
		string drugName = type.GetDisplayName();

		if (_type is TransactionType.BUY)
		{
			_providerManager.AudioProvider.PlaySoundFrontend(soundFile, soundSet);
			string message = Resources.Transaction_Message_Buy_Sucess.FormatInvariant(drugName, transactionValue);
			_providerManager.NotificationProvider.ShowSubtitle(message);
		}

		if (_type is TransactionType.SELL)
		{
			_providerManager.AudioProvider.PlaySoundFrontend(soundFile, soundSet);
			string message = Resources.Transaction_Message_Sell_Sucess.FormatInvariant(drugName, transactionValue);
			_providerManager.NotificationProvider.ShowSubtitle(message);
		}
	}
}
