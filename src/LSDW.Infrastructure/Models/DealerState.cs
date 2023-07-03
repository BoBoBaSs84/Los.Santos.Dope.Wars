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
		NextPriceChange = dealer.NextPriceChange;
		NextInventoryChange = dealer.NextInventoryChange;
		Discovered = dealer.Discovered;
		Inventory = CreateInventoryState(dealer.Inventory);
		Name = dealer.Name;
		SpawnPosition = dealer.SpawnPosition;
		Hash = dealer.Hash;
	}

	/// <summary>
	/// The closed until property of the dealer state.
	/// </summary>
	[XmlElement(nameof(ClosedUntil), Form = XmlSchemaForm.Qualified)]
	public DateTime? ClosedUntil { get; set; }

	/// <summary>
	/// The next price change property of the dealer state.
	/// </summary>
	[XmlElement(nameof(NextPriceChange), Form = XmlSchemaForm.Qualified)]
	public DateTime NextPriceChange { get; set; }

	/// <summary>
	/// The next inventory change property of the dealer state.
	/// </summary>
	[XmlElement(nameof(NextInventoryChange), Form = XmlSchemaForm.Qualified)]
	public DateTime NextInventoryChange { get; set; }

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
	/// The spawn position property of the dealer state.
	/// </summary>
	[XmlElement(nameof(SpawnPosition), Form = XmlSchemaForm.Qualified)]
	public Vector3 SpawnPosition { get; set; }

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
	/// Should the <see cref="NextPriceChange"/> property be serialized?
	/// </summary>
	public bool ShouldSerializeNextPriceChange() => Discovered && !ClosedUntil.HasValue;
	/// <summary>
	/// Should the <see cref="NextInventoryChange"/> property be serialized?
	/// </summary>
	public bool ShouldSerializeNextInventoryChange() => Discovered && !ClosedUntil.HasValue;
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
