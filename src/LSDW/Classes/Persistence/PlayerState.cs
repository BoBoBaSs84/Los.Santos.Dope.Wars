using LSDW.Core.Interfaces.Models;
using LSDW.Factories;
using System.Xml.Schema;
using System.Xml.Serialization;
using static LSDW.Constants.XmlConstants.NameSpaces;

namespace LSDW.Classes.Persistence;

/// <summary>
/// The player state class.
/// </summary>
[XmlRoot("Player", Namespace = PlayerStateNameSpace)]
public sealed class PlayerState
{
	public PlayerState(IPlayer player)
	{
		Inventory = PersistenceFactory.CreateInventoryState(player.Inventory);
		Experience = player.Experience;
		Transactions = PersistenceFactory.CreateTransactionStates(player.Transactions);
	}

	public PlayerState()
	{
		Inventory = new();
		Transactions = new();
	}

	[XmlElement("Inventory", Form = XmlSchemaForm.Qualified)]
	public InventoryState Inventory { get; set; }

	[XmlAttribute("Experience", Form = XmlSchemaForm.Qualified)]
	public int Experience { get; set; }

	[XmlArray("Transactions", Form = XmlSchemaForm.Qualified)]
	[XmlArrayItem("Transaction", Form = XmlSchemaForm.Qualified)]
	public List<TransactionState> Transactions { get; set; }
}
