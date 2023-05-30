using GTA;
using GTA.Math;
using LSDW.Factories;
using LSDW.Interfaces.Actors;
using System.Xml.Schema;
using System.Xml.Serialization;
using static LSDW.Constants.XmlConstants.NameSpaces;

namespace LSDW.Classes.Persistence;

[XmlRoot("Dealer", Namespace = DealerStateNameSpace)]
public sealed class DealerState
{
	public DealerState(IDealer dealer)
	{
		ClosedUntil = dealer.ClosedUntil;
		Discovered = dealer.Discovered;
		Inventory = PersistenceFactory.CreateInventoryState(dealer.Inventory);
		Name = dealer.Name;
		Position = dealer.Position;
		Hash = dealer.Hash;
	}

	public DealerState()
	{
		Inventory = new();
		Name = string.Empty;
	}

	[XmlElement(nameof(ClosedUntil), Form = XmlSchemaForm.Qualified)]
	public DateTime? ClosedUntil { get; set; }

	[XmlAttribute(nameof(Discovered), Form = XmlSchemaForm.Qualified)]
	public bool Discovered { get; set; }

	[XmlElement(nameof(Inventory), Form = XmlSchemaForm.Qualified)]
	public InventoryState Inventory { get; set; }

	[XmlAttribute(nameof(Name), Form = XmlSchemaForm.Qualified)]
	public string Name { get; set; }

	[XmlElement(nameof(Position), Form = XmlSchemaForm.Qualified)]
	public Vector3 Position { get; set; }

	[XmlAttribute(nameof(Position), Form = XmlSchemaForm.Qualified)]
	public PedHash Hash { get; set; }

	public bool ShouldSerializeClosedUntil() => ClosedUntil.HasValue;
	// TODO: Fix this...
	public bool ShouldSerializeHash() => false;
	public bool ShouldSerializeName() => Discovered && !ClosedUntil.HasValue;
	public bool ShouldSerializeInventory() => Discovered && !ClosedUntil.HasValue;
}
