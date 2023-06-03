using LSDW.Domain.Constants;
using LSDW.Domain.Factories;
using LSDW.Domain.Interfaces.Models;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LSDW.Domain.Classes.Persistence;

/// <summary>
/// The player state class.
/// </summary>
[XmlRoot("Player", Namespace = XmlConstants.NameSpace)]
public sealed class PlayerState
{
	public PlayerState()
	{
		Inventory = new();
		Transactions = new();
	}

	internal PlayerState(IPlayer player)
	{
		Inventory = DomainFactory.CreateInventoryState(player.Inventory);
		Experience = player.Experience;
		Transactions = DomainFactory.CreateTransactionStates(player.Transactions);
	}

	[XmlElement("Inventory", Form = XmlSchemaForm.Qualified)]
	public InventoryState Inventory { get; set; }

	[XmlAttribute("Experience", Form = XmlSchemaForm.Qualified)]
	public int Experience { get; set; }

	[XmlArray("Transactions", Form = XmlSchemaForm.Qualified)]
	[XmlArrayItem("Transaction", Form = XmlSchemaForm.Qualified)]
	public List<TransactionState> Transactions { get; set; }
}
