using Los.Santos.Dope.Wars.Interfaces.Base;
using System.Xml.Serialization;
using static Los.Santos.Dope.Wars.Extension.Utils;

namespace Los.Santos.Dope.Wars.Classes.Base;

/// <summary>
/// The <see cref="Stash"/> class is the base class for drug stashes.
/// Implements the members of the <see cref="IStash"/> interface
/// </summary>
[XmlRoot(ElementName = nameof(Stash), IsNullable = false)]
public abstract class Stash : IStash
{
	#region properties
	/// <inheritdoc/>
	[XmlArray(ElementName = nameof(Drugs), IsNullable = false)]
	[XmlArrayItem(ElementName = nameof(Drug), IsNullable = false)]
	public List<Drug> Drugs { get; set; }
	#endregion

	#region ctor
	/// <summary>
	/// The empty constructor for the <see cref="Stash"/> class
	/// </summary>
	public Stash() => Drugs = new();
	#endregion

	#region IStash members
	/// <inheritdoc/>
	public void Init()
	{
		Drugs.Clear();
		Drugs = GetAvailableDrugs();
	}
	/// <inheritdoc/>
	public void AddToStash(string drugName, int drugQuantity)
	{
		Drug drug = Drugs.Where(x => x.Name.Equals(drugName, StringComparison.Ordinal)).SingleOrDefault();
		if (drug is not null)
			drug.Quantity += drugQuantity;
	}
	/// <inheritdoc/>
	public void RemoveFromStash(string drugName, int drugQuantity)
	{
		Drug drug = Drugs.Where(x => x.Name.Equals(drugName, StringComparison.Ordinal)).SingleOrDefault();
		if (drug is not null)
			drug.Quantity -= drugQuantity;
	}
	#endregion
}
