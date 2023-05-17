using GTA.Math;
using LSDW.Factories;
using LSDW.Interfaces.Actors;
using System.Xml.Serialization;

namespace LSDW.Classes.Persistence;

public sealed class DealerState
{
	public DealerState(IDealer dealer)
	{
		ClosedUntil = dealer.ClosedUntil;
		Discovered = dealer.Discovered;
		Inventory = PersistenceFactory.CreateInventoryState(dealer.Inventory);
		Name = dealer.Name;
		Position = dealer.Position;
	}

	public DealerState()
	{
	}

	[XmlElement(nameof(ClosedUntil))]
	public DateTime? ClosedUntil { get; set; } = default!;

	[XmlAttribute(nameof(Discovered))]
	public bool Discovered { get; set; } = default!;

	[XmlElement(nameof(Inventory))]
	public InventoryState Inventory { get; set; } = default!;

	[XmlAttribute(nameof(Name))]
	public string Name { get; set; } = default!;

	[XmlElement(nameof(Position))]
	public Vector3 Position { get; set; } = default!;

	public bool ShouldSerializeClosedUntil() => ClosedUntil.HasValue;
}
