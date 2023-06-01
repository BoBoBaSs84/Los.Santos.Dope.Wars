using LSDW.Core.Enumerators;
using LSDW.Core.Interfaces.Models;
using System.Xml.Schema;
using System.Xml.Serialization;
using static LSDW.Constants.XmlConstants.NameSpaces;

namespace LSDW.Classes.Persistence;

[XmlRoot("Transaction", Namespace = TransactionStateNameSpace)]
public sealed class TransactionState
{
	public TransactionState()
	{
	}

	public TransactionState(ITransaction transaction)
	{
		DateTime = transaction.DateTime;
		TransactionType = transaction.TransactionType;
		DrugType = transaction.DrugType;
		Quantity = transaction.Quantity;
		TotalValue = transaction.Price;
	}

	[XmlElement("Date", Form = XmlSchemaForm.Qualified)]
	public DateTime DateTime { get; set; }

	[XmlAttribute("Type", Form = XmlSchemaForm.Qualified)]
	public TransactionType TransactionType { get; set; }

	[XmlElement("Drug", Form = XmlSchemaForm.Qualified)]
	public DrugType DrugType { get; set; }

	[XmlElement("Quantity", Form = XmlSchemaForm.Qualified)]
	public int Quantity { get; set; }

	[XmlElement("TotalValue", Form = XmlSchemaForm.Qualified)]
	public int TotalValue { get; set; }
}
