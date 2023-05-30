using LSDW.Core.Interfaces.Classes;
using LSDW.Factories;
using System.Xml.Serialization;

namespace LSDW.Classes.Persistence;

/// <summary>
/// The player state class.
/// </summary>
public sealed class PlayerState
{
	public PlayerState(IPlayer player)
	{
		Inventory = PersistenceFactory.CreateInventoryState(player.Inventory);
		Experience = player.Experience;
		Transactions = PersistenceFactory.CreateLogEntryStates(player.Transactions);
	}

	public PlayerState()
	{
		Inventory = new();
		Transactions = new();
	}

	[XmlElement(nameof(Inventory))]
	public InventoryState Inventory { get; set; }

	[XmlAttribute(nameof(Experience))]
	public int Experience { get; set; }

	[XmlArray("Transactions")]
	[XmlArrayItem("Transaction")]
	public List<LogEntryState> Transactions { get; set; }
}
