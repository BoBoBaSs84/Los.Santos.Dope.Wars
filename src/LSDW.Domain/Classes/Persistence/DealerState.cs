using GTA;
using GTA.Math;
using LSDW.Domain.Constants;
using LSDW.Domain.Factories;
using LSDW.Domain.Interfaces.Actors;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LSDW.Domain.Classes.Persistence;

[XmlRoot(XmlConstants.DealerStateRootName, Namespace = XmlConstants.NameSpace)]
public sealed class DealerState
{
	public DealerState()
	{
		Inventory = new();
		Name = string.Empty;
	}

	internal DealerState(IDealer dealer)
	{
		ClosedUntil = dealer.ClosedUntil;
		Discovered = dealer.Discovered;
		Inventory = DomainFactory.CreateInventoryState(dealer.Inventory);
		Name = dealer.Name;
		Position = dealer.Position;
		Hash = dealer.Hash;
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

	[XmlAttribute(nameof(Hash), Form = XmlSchemaForm.Qualified)]
	public PedHash Hash { get; set; }

	public bool ShouldSerializeClosedUntil() => ClosedUntil.HasValue;
	public bool ShouldSerializeHash() => Discovered && !ClosedUntil.HasValue;
	public bool ShouldSerializeName() => Discovered && !ClosedUntil.HasValue;
	public bool ShouldSerializeInventory() => Discovered && !ClosedUntil.HasValue;
}
