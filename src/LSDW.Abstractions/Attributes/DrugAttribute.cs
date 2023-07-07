namespace LSDW.Abstractions.Attributes;

/// <summary>
/// The drug attribute class.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
internal sealed class DrugAttribute : Attribute
{
	private float probability;
	private int averagePrice;

	/// <summary>
	/// Initializes a instance of the drug attribute class.
	/// </summary>
	/// <param name="name">The name of the drug.</param>
	/// <param name="description">The description of the drug.</param>
	internal DrugAttribute(string name, string description)
	{
		Name = name;
		Description = description;
	}

	/// <summary>
	/// The <see cref="Description"/> property is the description of the drug.
	/// </summary>
	public string Description { get; }

	/// <summary>
	/// The <see cref="Name"/> property is the name of the drug.
	/// </summary>
	public string Name { get; }

	/// <summary>
	/// The <see cref="AveragePrice"/> property is the average price of the drug.
	/// </summary>
	/// <remarks>
	/// A value greater 0.
	/// </remarks>
	public int AveragePrice
	{
		get => averagePrice;
		set => SetAveragePrice(value);
	}

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

	private void SetAveragePrice(int value)
	{
		if (value < 0)
			throw new ArgumentOutOfRangeException(nameof(value));

		averagePrice = value;
	}
}