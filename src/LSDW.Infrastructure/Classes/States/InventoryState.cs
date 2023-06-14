using LSDW.Abstractions.Domain.Models;
using LSDW.Infrastructure.Constants;
using System.Xml.Schema;
using System.Xml.Serialization;
using static LSDW.Infrastructure.Factories.InfrastructureFactory;

namespace LSDW.Infrastructure.Classes.States;

/// <summary>
/// The inventory state class.
/// </summary>
[XmlRoot(XmlConstants.InventoryStateRootName, Namespace = XmlConstants.NameSpace)]
public sealed class InventoryState
{
	/// <summary>
	/// Initializes a instance of the inventoty state class.
	/// </summary>
	public InventoryState()
		=> Drugs = new();

	/// <summary>
	/// Initializes a instance of the inventoty state class.
	/// </summary>
	/// <param name="inventory">The inventory to use.</param>
	internal InventoryState(IInventory inventory)
	{
		Drugs = CreateDrugStates(inventory);
		Money = inventory.Money;
	}

	/// <summary>
	/// The drugs property of the inventoty state.
	/// </summary>
	[XmlArray(nameof(Drugs), Form = XmlSchemaForm.Qualified)]
	[XmlArrayItem(XmlConstants.DrugStateRootName)]
	public List<DrugState> Drugs { get; set; }

	/// <summary>
	/// The money property of the inventoty state.
	/// </summary>
	[XmlAttribute(nameof(Money), Form = XmlSchemaForm.Qualified)]
	public int Money { get; set; }
}
