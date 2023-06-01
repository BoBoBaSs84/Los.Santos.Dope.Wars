using LSDW.Core.Enumerators;
using LSDW.Core.Interfaces.Models;
using System.Xml.Schema;
using System.Xml.Serialization;
using static LSDW.Constants.XmlConstants.NameSpaces;

namespace LSDW.Classes.Persistence;

[XmlRoot("Drug", Namespace = DrugStateNameSpace)]
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

	[XmlAttribute("Type", Form = XmlSchemaForm.Qualified)]
	public DrugType DrugType { get; set; }

	[XmlElement("Quantity", Form = XmlSchemaForm.Qualified)]
	public int Quantity { get; set; }

	[XmlElement("Price", Form = XmlSchemaForm.Qualified)]
	public int Price { get; set; }
}
