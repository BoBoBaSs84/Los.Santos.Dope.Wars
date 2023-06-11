using LSDW.Domain.Constants;
using LSDW.Domain.Enumerators;
using LSDW.Domain.Interfaces.Models;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LSDW.Domain.Classes.Persistence;

[XmlRoot(XmlConstants.TransactionStateRootName, Namespace = XmlConstants.NameSpace)]
public sealed class TransactionState
{
	public TransactionState()
	{
	}

	internal TransactionState(ITransaction transaction)
	{
		DateTime = transaction.DateTime;
		Type = transaction.Type;
		DrugType = transaction.DrugType;
		Quantity = transaction.Quantity;
		Price = transaction.Price;
	}

	[XmlElement(nameof(DateTime), Form = XmlSchemaForm.Qualified)]
	public DateTime DateTime { get; set; }

	[XmlAttribute(nameof(Type), Form = XmlSchemaForm.Qualified)]
	public TransactionType Type { get; set; }

	[XmlElement(nameof(DrugType), Form = XmlSchemaForm.Qualified)]
	public DrugType DrugType { get; set; }

	[XmlElement(nameof(Quantity), Form = XmlSchemaForm.Qualified)]
	public int Quantity { get; set; }

	[XmlElement(nameof(Price), Form = XmlSchemaForm.Qualified)]
	public int Price { get; set; }

	public bool ShouldSerializePrice() => Price != default;
}
