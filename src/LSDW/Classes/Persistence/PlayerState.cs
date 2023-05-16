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
	}

	public PlayerState()
	{
	}

	[XmlElement(nameof(Inventory))]
	public InventoryState Inventory { get; set; }

	[XmlAttribute(nameof(Experience))]
	public int Experience { get; set; }
}
