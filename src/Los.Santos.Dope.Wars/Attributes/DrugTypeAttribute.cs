using System.ComponentModel;

namespace LSDW.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
internal sealed class DrugTypeAttribute : DescriptionAttribute
{
	/// <summary>
	/// Initializes a instance of the <see cref="DrugTypeAttribute"/> class.
	/// </summary>
	/// <param name="name">The display name of the drug.</param>
	/// <param name="price">The normal market price of the drug.</param>
	/// <param name="description">The description of the drug.</param>
	public DrugTypeAttribute(string name, int price, string? description) : base(description)
	{
		Name = name;
		Price = price;
	}

	/// <summary>
	/// The <see cref="Name"/> property is the display name of the drug.
	/// </summary>
	public string Name { get; }

	/// <summary>
	/// The <see cref="Price"/> property is the normal market price of the drug.
	/// </summary>
	public int Price { get; }
}
