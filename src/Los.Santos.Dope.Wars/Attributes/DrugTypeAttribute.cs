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
	/// <param name="rank">The rank or level of the drug.</param>
	/// <param name="description">The description of the drug.</param>
	public DrugTypeAttribute(string displayName, int marketPrice, int rank, string? description) : base(description)
	{
		DisplayName = displayName;
		MarketPrice = marketPrice;
		Rank = rank;
	}

	/// <summary>
	/// The <see cref="DisplayName"/> property is the display name of the drug.
	/// </summary>
	public string DisplayName { get; }

	/// <summary>
	/// The <see cref="MarketPrice"/> property is the normal market price of the drug.
	/// </summary>
	public int MarketPrice { get; }

	/// <summary>
	/// The <see cref="Rank"/> property is the rank or level of the drug.
	/// </summary>
	public int Rank { get; }
}
