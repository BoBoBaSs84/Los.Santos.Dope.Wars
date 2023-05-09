namespace LSDW.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
internal sealed class DrugAttribute : Attribute
{
	/// <summary>
	/// Initializes a instance of the <see cref="DrugAttribute"/> class.
	/// </summary>
	/// <param name="displayName">The display name of the drug.</param>
	/// <param name="marketValue">The normal market value of the drug.</param>
	/// <param name="rank">The rank or level of the drug.</param>
	public DrugAttribute(string displayName, int marketValue, int rank)
	{
		DisplayName = displayName;
		MarketValue = marketValue;
		Rank = rank;
	}

	/// <summary>
	/// The <see cref="DisplayName"/> property is the display name of the drug.
	/// </summary>
	public string DisplayName { get; }

	/// <summary>
	/// The <see cref="MarketValue"/> property is the normal market value of the drug.
	/// </summary>
	public int MarketValue { get; }

	/// <summary>
	/// The <see cref="Rank"/> property is the rank or level of the drug.
	/// </summary>
	public int Rank { get; }
}
