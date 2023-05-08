using System.ComponentModel;

namespace LSDW.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
internal sealed class DrugTypeAttribute : DescriptionAttribute
{
	/// <summary>
	/// Initializes a instance of the <see cref="DrugTypeAttribute"/> class.
	/// </summary>
	/// <param name="displayName">The display name of the drug.</param>
	/// <param name="marketPrice">The normal market price of the drug.</param>
	/// <param name="description">The description of the drug.</param>
	public DrugTypeAttribute(string displayName, int marketPrice, string? description) : base(description)
	{
		DisplayName = displayName;
		MarketPrice = marketPrice;
	}

	/// <summary>
	/// The <see cref="DisplayName"/> property is the display name of the drug.
	/// </summary>
	public string DisplayName { get; }

	/// <summary>
	/// The <see cref="MarketPrice"/> property is the normal market price of the drug.
	/// </summary>
	public int MarketPrice { get; }
}
