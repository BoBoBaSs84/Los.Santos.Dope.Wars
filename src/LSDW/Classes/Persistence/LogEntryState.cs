using LSDW.Core.Enumerators;
using LSDW.Core.Interfaces.Classes;
using System.Xml.Serialization;

namespace LSDW.Classes.Persistence;

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

	[XmlElement("Date")]
	public DateTime DateTime { get; set; }

	[XmlAttribute("Type")]
	public TransactionType TransactionType { get; set; }

	[XmlElement(nameof(DrugType))]
	public DrugType DrugType { get; set; }

	[XmlElement(nameof(Quantity))]
	public int Quantity { get; set; }

	[XmlElement(nameof(TotalValue))]
	public int TotalValue { get; set; }
}
