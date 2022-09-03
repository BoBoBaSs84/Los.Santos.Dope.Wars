using Los.Santos.Dope.Wars.Classes.BaseTypes;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Los.Santos.Dope.Wars.Classes;

/// <summary>
/// The <see cref="Drug"/> class is the root element for the drug class.
/// </summary>
[XmlRoot(ElementName = nameof(Drug), IsNullable = false)]
public class Drug : NotifyBase
{
	#region fields
	private int quantity;
	#endregion

	#region ctor
	/// <summary>
	/// Initializes a new instance of the <see cref="Drug"/> class.
	/// </summary>
	/// <param name="drugName"></param>
	/// <param name="description"></param>
	/// <param name="marketValue"></param>
	public Drug(string drugName, string description, int marketValue)
	{
		Name = drugName;
		Description = description;
		CurrentPrice = default;
		AveragePrice = marketValue;
		PurchasePrice = default;
		Quantity = default;
		PropertyChanged += OnDrugPropertyChanged;
	}
	#endregion

	#region properties
	/// <summary>
	/// The <see cref="Name"/> property is the name of the drug.
	/// </summary>
	[XmlText]
	public string Name { get; set; }

	/// <summary>
	/// The <see cref="Description"/> property is the description of the drug.
	/// </summary>
	[XmlIgnore]
	public string Description { get; set; }

	/// <summary>
	/// The <see cref="CurrentPrice"/> property is the current price of the drug.
	/// </summary>
	[XmlIgnore]
	public int CurrentPrice { get; set; }

	/// <summary>
	/// The <see cref="AveragePrice"/> property is the normal market price of the drug.
	/// </summary>
	[XmlIgnore]
	public int AveragePrice { get; set; }

	/// <summary>
	/// The <see cref="PurchasePrice"/> property is the price the drug has been purchased.
	/// </summary>
	[XmlAttribute(AttributeName = nameof(PurchasePrice))]
	public int PurchasePrice { get; set; }

	/// <summary>
	/// The <see cref="Quantity"/> property is the amount of the drug.
	/// </summary>
	[XmlAttribute(AttributeName = nameof(Quantity))]
	public int Quantity { get => quantity; set => SetProperty(ref quantity, value); }
	#endregion

	#region private methods
	/// <summary>
	/// The event trigger method if a property that notifies has changed.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void OnDrugPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (sender is not Drug drug)
			return;
		if (e is null)
			return;

		if (Equals(e.PropertyName, nameof(drug.Quantity)))
		{
			if (Equals(drug.Quantity, 0))
				drug.PurchasePrice = Quantity;
		}
	}
	#endregion
}
