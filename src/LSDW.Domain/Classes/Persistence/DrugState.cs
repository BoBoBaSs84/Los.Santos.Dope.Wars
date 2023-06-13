using LSDW.Domain.Constants;
using LSDW.Domain.Enumerators;
using LSDW.Domain.Interfaces.Models;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LSDW.Domain.Classes.Persistence;

/// <summary>
/// 
/// </summary>
[XmlRoot(XmlConstants.DrugStateRootName, Namespace = XmlConstants.NameSpace)]
public sealed class DrugState
{
	public DrugState()
	{
	}

	internal DrugState(IDrug drug)
	{
		Type = drug.Type;
		Quantity = drug.Quantity;
		CurrentPrice = drug.CurrentPrice;
	}

	[XmlAttribute(nameof(Type), Form = XmlSchemaForm.Qualified)]
	public DrugType Type { get; set; }

	[XmlAttribute(nameof(Quantity), Form = XmlSchemaForm.Qualified)]
	public int Quantity { get; set; }

	[XmlAttribute(nameof(CurrentPrice), Form = XmlSchemaForm.Qualified)]
	public int CurrentPrice { get; set; }
}
