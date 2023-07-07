using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Infrastructure.Constants;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LSDW.Infrastructure.Models;

/// <summary>
/// 
/// </summary>
[XmlRoot(XmlConstants.DrugStateRootName, Namespace = XmlConstants.NameSpace)]
public sealed class DrugState
{
	/// <summary>
	/// Initializes a instance of the drug state class.
	/// </summary>
	public DrugState()
	{
	}

	/// <summary>
	/// Initializes a instance of the drug state class.
	/// </summary>
	/// <param name="drug">The drug to use.</param>
	internal DrugState(IDrug drug)
	{
		Type = drug.Type;
		Quantity = drug.Quantity;
		CurrentPrice = drug.Price;
	}

	/// <summary>
	/// The type property of the drug state.
	/// </summary>
	[XmlAttribute(nameof(Type), Form = XmlSchemaForm.Qualified)]
	public DrugType Type { get; set; }

	/// <summary>
	/// The quantity property of the drug state.
	/// </summary>
	[XmlAttribute(nameof(Quantity), Form = XmlSchemaForm.Qualified)]
	public int Quantity { get; set; }

	/// <summary>
	/// The current price property of the drug state.
	/// </summary>
	[XmlAttribute(nameof(CurrentPrice), Form = XmlSchemaForm.Qualified)]
	public int CurrentPrice { get; set; }
}
