using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Services;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Extensions;
using LSDW.Abstractions.Models;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
using LSDW.Domain.Properties;

namespace LSDW.Domain.Services;

/// <summary>
/// The transaction service class.
/// </summary>
internal sealed class TransactionService : ITransactionService
{
	private readonly IProviderManager _providerManager;
	private readonly TransactionType _transactionType;
	private readonly IPlayer _player;
	private readonly IInventory _source;
	private readonly IInventory _target;
	private readonly int _maxQuantity;

	/// <summary>
	/// Initializes a instance of the transaction service class.
	/// </summary>
	/// <param name="providerManager">The provider manager instance to use.</param>
	/// <param name="transactionType">The type of the transaction.</param>
	/// <param name="player">The player and his inventory.</param>
	/// <param name="inventory">The opposing inventory.</param>
	internal TransactionService(IProviderManager providerManager, TransactionType transactionType, IPlayer player, IInventory inventory)
	{
		_providerManager = providerManager;
		_transactionType = transactionType;
		_player = player;
		(_source, _target) = GetInventories(transactionType, player, inventory);
		_maxQuantity = GetMaximumQuantity(transactionType, player);
	}

	public void BustOrNoBust()
	{
		if (_transactionType is TransactionType.TAKE or TransactionType.GIVE)
			return;

		float random = _providerManager.RandomProvider.GetFloat();

		if (random >= Settings.Trafficking.BustChance)
			return;

		_providerManager.NotificationProvider.ShowSubtitle(Resources.Transaction_Message_Bust);
		_providerManager.PlayerProvider.WantedLevel = Settings.Trafficking.WantedLevel;
		_providerManager.PlayerProvider.DispatchsCops = true;
	}

	public bool Commit(DrugType drugType, int quantity, int price)
	{
		int transactionValue = price * quantity;

		if (!CheckInventory(quantity))
			return false;

		if (!CheckMoney(transactionValue))
			return false;

		TransferGoods(drugType, quantity, price);

		TransferMoney(transactionValue);

		AddTransaction(drugType, quantity, price);

		ReportSuccess(drugType, transactionValue);

		return true;
	}

	/// <summary>
	/// Returns the source and target inventory based on the transaction type.
	/// </summary>
	/// <param name="transactionType">The transaction type for the menu.</param>
	/// <param name="player">The player and his inventory.</param>
	/// <param name="inventory">The opposing inventory.</param>
	private static (IInventory source, IInventory target) GetInventories(TransactionType transactionType, IPlayer player, IInventory inventory)
		=> transactionType is TransactionType.SELL or TransactionType.GIVE
		? ((IInventory source, IInventory target))(player.Inventory, inventory)
		: ((IInventory source, IInventory target))(inventory, player.Inventory);

	/// <summary>
	/// Returns the maximum quantity for the transaction based on the menu type.
	/// </summary>
	/// <param name="transactionType">The transaction type for the menu.</param>
	/// <param name="player">The player and his inventory.</param>
	internal static int GetMaximumQuantity(TransactionType transactionType, IPlayer player)
		=> transactionType is TransactionType.SELL or TransactionType.GIVE
		? int.MaxValue
		: player.MaximumInventoryQuantity;

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
		if (_transactionType is TransactionType.BUY)
		{
			int playerMoney = _providerManager.PlayerProvider.Money;
			if (playerMoney < transactionValue)
			{
				string message = Resources.Transaction_Message_Player_NoMoney.FormatInvariant(transactionValue, playerMoney);
				_providerManager.NotificationProvider.ShowSubtitle(message);
				return false;
			}
		}

		if (_transactionType is TransactionType.SELL)
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
		if (_transactionType is TransactionType.BUY)
		{
			_providerManager.PlayerProvider.Money -= transactionValue;
			_source.Add(transactionValue);
		}

		if (_transactionType is TransactionType.SELL)
		{
			_target.Remove(transactionValue);
			_providerManager.PlayerProvider.Money += transactionValue;
		}
	}

	/// <summary>
	/// Transfers the goods from source to target inventory.
	/// </summary>
	/// <param name="drugType">The type of the drug to transact.</param>
	/// <param name="quantity">The transaction quantity of the drug.</param>
	/// <param name="price">The transaction price of the drug.</param>
	private void TransferGoods(DrugType drugType, int quantity, int price)
	{
		_source.Remove(drugType, quantity);
		_target.Add(drugType, quantity, price);
	}

	/// <summary>
	/// Adds the transaction to the players transactions.
	/// </summary>
	/// <param name="drugType">The type of the drug to transact.</param>
	/// <param name="quantity">The transaction quantity of the drug.</param>
	/// <param name="price">The transaction price of the drug.</param>
	private void AddTransaction(DrugType drugType, int quantity, int price)
	{
		ITransaction transaction =
			DomainFactory.CreateTransaction(_providerManager.WorldProvider.Now, _transactionType, drugType, quantity, price);
		_player.AddTransaction(transaction);
	}

	/// <summary>
	/// Reports the success of the transaction by sound and message.
	/// </summary>
	/// <param name="type">The type of the drug to transact.</param>
	/// <param name="transactionValue">The transaction value to transfer.</param>
	private void ReportSuccess(DrugType type, int transactionValue)
	{
		string soundFile = "PURCHASE";
		string soundSet = "HUD_LIQUOR_STORE_SOUNDSET";
		string drugName = type.GetDisplayName();

		if (_transactionType is TransactionType.BUY)
		{
			_ = _providerManager.AudioProvider.PlaySoundFrontend(soundFile, soundSet);
			string message = Resources.Transaction_Message_Buy_Sucess.FormatInvariant(drugName, transactionValue);
			_providerManager.NotificationProvider.ShowSubtitle(message);
		}

		if (_transactionType is TransactionType.SELL)
		{
			_ = _providerManager.AudioProvider.PlaySoundFrontend(soundFile, soundSet);
			string message = Resources.Transaction_Message_Sell_Sucess.FormatInvariant(drugName, transactionValue);
			_providerManager.NotificationProvider.ShowSubtitle(message);
		}
	}
}
