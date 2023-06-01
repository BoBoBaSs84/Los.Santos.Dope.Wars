using LSDW.Core.Interfaces.Models;
using LSDW.Factories;
using System.Xml.Schema;
using System.Xml.Serialization;
using static LSDW.Constants.XmlConstants.NameSpaces;

namespace LSDW.Classes.Persistence;

/// <summary>
/// The inventory state class.
/// </summary>
[XmlRoot("Inventory", Namespace = InventoryStateNameSpace)]
public sealed class InventoryState
{
	public InventoryState()
		=> Drugs = new();

	public InventoryState(IInventory inventory)
	{
		Drugs = PersistenceFactory.CreateDrugStates(inventory);
		Money = inventory.Money;
	}

	[XmlArray("Drugs", Form = XmlSchemaForm.Qualified)]
	[XmlArrayItem("Drug")]
	public List<DrugState> Drugs { get; set; }

	[XmlAttribute("Money", Form = XmlSchemaForm.Qualified)]
	public int Money { get; set; }
}
