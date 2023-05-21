using LSDW.Core.Enumerators;
using LSDW.Core.Interfaces.Classes;
using System.Xml.Serialization;

namespace LSDW.Classes.Persistence;

public sealed class DrugState
{
	public DrugState()
	{
	}

	public DrugState(IDrug drug)
	{
		DrugType = drug.DrugType;
		Quantity = drug.Quantity;
		Price = drug.Price;
	}

	[XmlAttribute("Type")]
	public DrugType DrugType { get; set; }

	[XmlElement(nameof(Quantity))]
	public int Quantity { get; set; }

	[XmlElement(nameof(Price))]
	public int Price { get; set; }
}
