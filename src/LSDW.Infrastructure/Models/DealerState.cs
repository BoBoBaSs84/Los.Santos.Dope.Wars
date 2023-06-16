using GTA;
using GTA.Math;
using LSDW.Abstractions.Domain.Models;
using LSDW.Infrastructure.Constants;
using System.Xml.Schema;
using System.Xml.Serialization;
using static LSDW.Infrastructure.Factories.InfrastructureFactory;

namespace LSDW.Infrastructure.Models;

/// <summary>
/// The dealer state class.
/// </summary>
[XmlRoot(XmlConstants.DealerStateRootName, Namespace = XmlConstants.NameSpace)]
public sealed class DealerState
{
	/// <summary>
	/// Initializes a instance of the dealer state class.
	/// </summary>
	public DealerState()
	{
		Inventory = new();
		Name = string.Empty;
	}

	/// <summary>
	/// Initializes a instance of the dealer state class.
	/// </summary>
	/// <param name="dealer">The dealer to use.</param>
	internal DealerState(IDealer dealer)
	{
		ClosedUntil = dealer.ClosedUntil;
		LastRefresh = dealer.LastRefresh;
		LastRestock = dealer.LastRestock;
		Discovered = dealer.Discovered;
		Inventory = CreateInventoryState(dealer.Inventory);
		Name = dealer.Name;
		Position = dealer.Position;
		Hash = dealer.Hash;
	}

	/// <summary>
	/// The closed until property of the dealer state.
	/// </summary>
	[XmlElement(nameof(ClosedUntil), Form = XmlSchemaForm.Qualified)]
	public DateTime? ClosedUntil { get; set; }

	/// <summary>
	/// The last refresh property of the dealer state.
	/// </summary>
	[XmlElement(nameof(LastRefresh), Form = XmlSchemaForm.Qualified)]
	public DateTime LastRefresh { get; set; }

	/// <summary>
	/// The last restock property of the dealer state.
	/// </summary>
	[XmlElement(nameof(LastRestock), Form = XmlSchemaForm.Qualified)]
	public DateTime LastRestock { get; set; }

	/// <summary>
	/// The discovered property of the dealer state.
	/// </summary>
	[XmlAttribute(nameof(Discovered), Form = XmlSchemaForm.Qualified)]
	public bool Discovered { get; set; }

	/// <summary>
	/// The inventory property of the dealer state.
	/// </summary>
	[XmlElement(nameof(Inventory), Form = XmlSchemaForm.Qualified)]
	public InventoryState Inventory { get; set; }

	/// <summary>
	/// The name property of the dealer state.
	/// </summary>
	[XmlAttribute(nameof(Name), Form = XmlSchemaForm.Qualified)]
	public string Name { get; set; }

	/// <summary>
	/// The position property of the dealer state.
	/// </summary>
	[XmlElement(nameof(Position), Form = XmlSchemaForm.Qualified)]
	public Vector3 Position { get; set; }

	/// <summary>
	/// The hash property of the dealer state.
	/// </summary>
	[XmlAttribute(nameof(Hash), Form = XmlSchemaForm.Qualified)]
	public PedHash Hash { get; set; }

	/// <summary>
	/// Should the <see cref="ClosedUntil"/> property be serialized?
	/// </summary>
	public bool ShouldSerializeClosedUntil() => ClosedUntil.HasValue;
	/// <summary>
	/// Should the <see cref="LastRefresh"/> property be serialized?
	/// </summary>
	public bool ShouldSerializeLastRefresh() => Discovered && !ClosedUntil.HasValue;
	/// <summary>
	/// Should the <see cref="LastRestock"/> property be serialized?
	/// </summary>
	public bool ShouldSerializeLastRestock() => Discovered && !ClosedUntil.HasValue;
	/// <summary>
	/// Should the <see cref="Hash"/> property be serialized?
	/// </summary>
	public bool ShouldSerializeHash() => Discovered && !ClosedUntil.HasValue;
	/// <summary>
	/// Should the <see cref="Name"/> property be serialized?
	/// </summary>
	public bool ShouldSerializeName() => Discovered && !ClosedUntil.HasValue;
	/// <summary>
	/// Should the <see cref="Inventory"/> property be serialized?
	/// </summary>
	public bool ShouldSerializeInventory() => Discovered && !ClosedUntil.HasValue;
}
