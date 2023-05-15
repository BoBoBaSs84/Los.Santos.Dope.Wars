using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;
using System.Xml.Serialization;

namespace LSDW.Core.Classes;

/// <summary>
/// The player state class.
/// </summary>
public sealed class PlayerState
{
	public PlayerState(IPlayer player)
	{
		Inventory = StateFactory.CreateInventoryState(player.Inventory);
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
