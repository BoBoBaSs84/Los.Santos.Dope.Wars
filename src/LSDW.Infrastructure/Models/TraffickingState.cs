using LSDW.Abstractions.Application.Missions;
using LSDW.Infrastructure.Constants;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LSDW.Infrastructure.Models;

/// <summary>
/// The trafficking state class.
/// </summary>
[XmlRoot(XmlConstants.TraffickingStateRootName, Namespace = XmlConstants.NameSpace)]
public sealed class TraffickingState
{
	/// <summary>
	/// Initializes a instance of the trafficking state class.
	/// </summary>
	public TraffickingState()
	{
	}

	/// <summary>
	/// Initializes a instance of the trafficking state class.
	/// </summary>
	/// <param name="trafficking">The trafficking instance to use.</param>
	public TraffickingState(ITrafficking trafficking)
	{
		LastChange = trafficking.LastChange;
		LastRenew = trafficking.LastRenew;
	}

	/// <summary>
	/// The last change property of the trafficking state.
	/// </summary>
	[XmlElement(nameof(LastChange), Form = XmlSchemaForm.Qualified)]
	public DateTime LastChange { get; set; }

	/// <summary>
	/// The last renew property of the trafficking state.
	/// </summary>
	[XmlElement(nameof(LastRenew), Form = XmlSchemaForm.Qualified)]
	public DateTime LastRenew { get; set; }
}
