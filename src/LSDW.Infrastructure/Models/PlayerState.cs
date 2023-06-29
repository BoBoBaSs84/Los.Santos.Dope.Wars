using LSDW.Abstractions.Domain.Models;
using LSDW.Infrastructure.Constants;
using System.Xml.Schema;
using System.Xml.Serialization;
using static LSDW.Infrastructure.Factories.InfrastructureFactory;

namespace LSDW.Infrastructure.Models;

/// <summary>
/// The player state class.
/// </summary>
[XmlRoot(XmlConstants.PlayerStateRootName, Namespace = XmlConstants.NameSpace)]
public sealed class PlayerState
{
	/// <summary>
	/// Initializes a instance of the player state class.
	/// </summary>
	public PlayerState()
	{
		Inventory = new();
		Transactions = new();
	}

	/// <summary>
	/// Initializes a instance of the player state class.
	/// </summary>
	/// <param name="player">The player to use.</param>
	internal PlayerState(IPlayer player)
	{
		Inventory = CreateInventoryState(player.Inventory);
		Experience = player.Experience;
		Transactions = CreateTransactionStates(player.GetTransactions());
	}

	/// <summary>
	/// The inventory property of the player state.
	/// </summary>
	[XmlElement(nameof(Inventory), Form = XmlSchemaForm.Qualified)]
	public InventoryState Inventory { get; set; }

	/// <summary>
	/// The experience property of the player state.
	/// </summary>
	[XmlAttribute(nameof(Experience), Form = XmlSchemaForm.Qualified)]
	public int Experience { get; set; }

	/// <summary>
	/// The transactions property of the player state.
	/// </summary>
	[XmlArray(nameof(Transactions), Form = XmlSchemaForm.Qualified)]
	[XmlArrayItem(XmlConstants.TransactionStateRootName, Form = XmlSchemaForm.Qualified)]
	public List<TransactionState> Transactions { get; set; }

	/// <summary>
	/// Should the <see cref="Transactions"/> property be serialized?
	/// </summary>
	public bool ShouldSerializeTransactions() => Transactions.Count != default;
}
