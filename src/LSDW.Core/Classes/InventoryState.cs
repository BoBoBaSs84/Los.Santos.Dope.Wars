using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;
using System.Xml.Serialization;

namespace LSDW.Core.Classes;

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
		Drugs = StateFactory.CreateDrugStates(inventory);
		Money = inventory.Money;
	}

	[XmlArray(nameof(Drugs))]
	[XmlArrayItem(nameof(Drug))]
	public List<DrugState> Drugs { get; set; }

	[XmlAttribute(nameof(Money))]
	public int Money { get; set; }
}
