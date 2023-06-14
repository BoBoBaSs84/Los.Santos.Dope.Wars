using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Infrastructure.Constants;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LSDW.Infrastructure.Classes.States;

/// <summary>
/// The transaction state class.
/// </summary>
[XmlRoot(XmlConstants.TransactionStateRootName, Namespace = XmlConstants.NameSpace)]
public sealed class TransactionState
{
	/// <summary>
	/// Initializes a instance of the transaction state class.
	/// </summary>
	public TransactionState()
	{
	}

	/// <summary>
	/// Initializes a instance of the transaction state class.
	/// </summary>
	/// <param name="transaction">The transaction to use.</param>
	internal TransactionState(ITransaction transaction)
	{
		DateTime = transaction.DateTime;
		Type = transaction.Type;
		DrugType = transaction.DrugType;
		Quantity = transaction.Quantity;
		Price = transaction.Price;
	}

	/// <summary>
	/// The date time property of the transaction state.
	/// </summary>
	[XmlElement(nameof(DateTime), Form = XmlSchemaForm.Qualified)]
	public DateTime DateTime { get; set; }

	/// <summary>
	/// The transaction type property of the transaction state.
	/// </summary>
	[XmlAttribute(nameof(Type), Form = XmlSchemaForm.Qualified)]
	public TransactionType Type { get; set; }

	/// <summary>
	/// The drug type property of the transaction state.
	/// </summary>
	[XmlElement(nameof(DrugType), Form = XmlSchemaForm.Qualified)]
	public DrugType DrugType { get; set; }

	/// <summary>
	/// The transaction quantity property of the transaction state.
	/// </summary>
	[XmlElement(nameof(Quantity), Form = XmlSchemaForm.Qualified)]
	public int Quantity { get; set; }

	/// <summary>
	/// The transaction price property of the transaction state.
	/// </summary>
	[XmlElement(nameof(Price), Form = XmlSchemaForm.Qualified)]
	public int Price { get; set; }

	/// <summary>
	/// Should the price property be serialized?
	/// </summary>
	public bool ShouldSerializePrice() => Price != default;
}
