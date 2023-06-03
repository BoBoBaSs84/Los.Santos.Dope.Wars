using LSDW.Domain.Constants;
using LSDW.Domain.Factories;
using LSDW.Domain.Interfaces.Models;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LSDW.Domain.Classes.Persistence;

/// <summary>
/// The inventory state class.
/// </summary>
[XmlRoot("Inventory", Namespace = XmlConstants.NameSpace)]
public sealed class InventoryState
{
	public InventoryState()
		=> Drugs = new();

	internal InventoryState(IInventory inventory)
	{
		Drugs = DomainFactory.CreateDrugStates(inventory);
		Money = inventory.Money;
	}

	[XmlArray("Drugs", Form = XmlSchemaForm.Qualified)]
	[XmlArrayItem("Drug")]
	public List<DrugState> Drugs { get; set; }

	[XmlAttribute("Money", Form = XmlSchemaForm.Qualified)]
	public int Money { get; set; }
}
