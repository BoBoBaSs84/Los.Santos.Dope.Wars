using LSDW.Core.Interfaces.Classes;
using LSDW.Factories;
using System.Xml.Serialization;

namespace LSDW.Classes.Persistence;

/// <summary>
/// The inventory state class.
/// </summary>
public sealed class InventoryState
{
	public InventoryState()
	{
	}

	public InventoryState(IInventory inventory)
	{
		Drugs = PersistenceFactory.CreateDrugStates(inventory);
		Money = inventory.Money;
	}

	[XmlArray(nameof(Drugs))]
	[XmlArrayItem("Drug")]
	public List<DrugState> Drugs { get; set; }

	[XmlAttribute(nameof(Money))]
	public int Money { get; set; }
}
