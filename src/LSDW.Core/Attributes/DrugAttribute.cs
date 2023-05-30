﻿namespace LSDW.Core.Attributes;

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
	/// <param name="marketValue">The normal market value of the drug.</param>
	/// <param name="rank">The rank or level of the drug.</param>
	public DrugAttribute(string displayName, string description, int marketValue) : base(description)
	{
		DisplayName = displayName;
		MarketValue = marketValue;
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
