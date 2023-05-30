using LSDW.Core.Enumerators;
using LSDW.Core.Interfaces.Classes;
using System.Xml.Schema;
using System.Xml.Serialization;
using static LSDW.Constants.XmlConstants.NameSpaces;

namespace LSDW.Classes.Persistence;

[XmlRoot("LogEntry", Namespace = LogEntryStateNameSpace)]
public sealed class LogEntryState
{
	public LogEntryState()
	{
	}

	public LogEntryState(ILogEntry logEntry)
	{
		DateTime = logEntry.DateTime;
		TransactionType = logEntry.TransactionType;
		DrugType = logEntry.DrugType;
		Quantity = logEntry.Quantity;
		TotalValue = logEntry.TotalValue;
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
