using LSDW.Domain.Constants;
using LSDW.Domain.Enumerators;
using LSDW.Domain.Interfaces.Models;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LSDW.Domain.Classes.Persistence;

[XmlRoot("Drug", Namespace = XmlConstants.NameSpace)]
public sealed class DrugState
{
	public DrugState()
	{
	}

	internal DrugState(IDrug drug)
	{
		DrugType = drug.DrugType;
		Quantity = drug.Quantity;
		Price = drug.Price;
	}

	[XmlAttribute("Type", Form = XmlSchemaForm.Qualified)]
	public DrugType DrugType { get; set; }

	[XmlAttribute("Quantity", Form = XmlSchemaForm.Qualified)]
	public int Quantity { get; set; }

	[XmlAttribute("Price", Form = XmlSchemaForm.Qualified)]
	public int Price { get; set; }
}
