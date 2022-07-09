using System.Xml.Serialization;

namespace Los.Santos.Dope.Wars.Classes
{
	/// <summary>
	/// The <see cref="Drug"/> class is the root element for the drug class
	/// </summary>
	[XmlRoot(ElementName = nameof(Drug), IsNullable = false)]
	public class Drug
	{
		#region fields
		private int _quantity = default;
		#endregion

		#region ctor
		/// <summary>
		/// Empty constructor for <see cref="Drug"/>
		/// </summary>
		public Drug()
		{
			Name = default!;
			Description = default!;
			CurrentPrice = default;
			AveragePrice = default;
			PurchasePrice = default;
			Quantity = default;
		}

		/// <summary>
		/// The <see cref="Enums.DrugTypes"/> constructor for <see cref="Drug"/>
		/// </summary>
		/// <param name="drugType"></param>
		/// <param name="description"></param>
		/// <param name="marketValue"></param>
		public Drug(Enums.DrugTypes drugType, string description, int marketValue)
		{
			Name = drugType.ToString();
			Description = description;
			CurrentPrice = default;
			AveragePrice = marketValue;
			PurchasePrice = default;
			Quantity = default;
		}

		/// <summary>
		/// The <see cref="string"/> constructor for <see cref="Drug"/>
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
		}
		#endregion

		#region properties
		/// <summary>
		/// The <see cref="Name"/> property
		/// </summary>
		[XmlText]
		public string Name { get; set; }

		/// <summary>
		/// The <see cref="Description"/> property
		/// </summary>
		[XmlIgnore]
		public string Description { get; set; }

		/// <summary>
		/// The <see cref="CurrentPrice"/> property
		/// </summary>
		[XmlIgnore]
		public int CurrentPrice { get; set; }

		/// <summary>
		/// The <see cref="AveragePrice"/> property
		/// </summary>
		[XmlIgnore]
		public int AveragePrice { get; set; }

		/// <summary>
		/// The <see cref="PurchasePrice"/> property
		/// </summary>
		[XmlAttribute(AttributeName = nameof(PurchasePrice))]
		public int PurchasePrice { get; set; }

		/// <summary>
		/// The <see cref="Quantity"/> property, if the quantity gets set to 0 the <see cref="PurchasePrice"/> property goes 0 too
		/// </summary>
		[XmlAttribute(AttributeName = nameof(Quantity))]
		public int Quantity
		{
			get { return _quantity; }
			set
			{
				_quantity = value;
				if (value.Equals(0))
				{
					PurchasePrice = value;
				}
			}
		}
		#endregion
	}
}
