using LSDW.Core.Interfaces.Classes;
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
		LogEntries = PersistenceFactory.CreateLogEntryStates(player.Transactions);
	}

	public PlayerState()
	{
		Inventory = new();
		LogEntries = new();
	}

	[XmlElement("Inventory", Form = XmlSchemaForm.Qualified)]
	public InventoryState Inventory { get; set; }

	[XmlAttribute("Experience", Form = XmlSchemaForm.Qualified)]
	public int Experience { get; set; }

	[XmlArray("LogEntries", Form = XmlSchemaForm.Qualified)]
	[XmlArrayItem("LogEntry", Form = XmlSchemaForm.Qualified)]
	public List<LogEntryState> LogEntries { get; set; }
}
