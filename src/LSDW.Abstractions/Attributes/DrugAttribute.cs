namespace LSDW.Abstractions.Attributes;

/// <summary>
/// The drug attribute class.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
internal sealed class DrugAttribute : DescriptionAttribute
{
	private float probability;

	/// <summary>
	/// Initializes a instance of the <see cref="DrugAttribute"/> class.
	/// </summary>
	/// <param name="displayName">The display name of the drug.</param>
	/// <param name="description">The description of the drug.</param>
	/// <param name="averagePrice">The normal market value of the drug.</param>
	internal DrugAttribute(string displayName, string description, int averagePrice) : base(description)
	{
		DisplayName = displayName;
		AveragePrice = averagePrice;
	}

	/// <summary>
	/// The <see cref="DisplayName"/> property is the display name of the drug.
	/// </summary>
	public string DisplayName { get; }

	/// <summary>
	/// The <see cref="AveragePrice"/> property is the average price of the drug.
	/// </summary>
	public int AveragePrice { get; }

	/// <summary>
	/// The availability probability property of a drug.
	/// </summary>
	/// <remarks>
	/// A value between 0 and 1.
	/// </remarks>
	public float Probability
	{
		get => probability;
		set => SetProbability(value);
	}

	private void SetProbability(float value)
	{
		if (value is < 0 or > 1)
			throw new ArgumentOutOfRangeException(nameof(value));

		probability = value;
	}
}
