using LSDW.Abstractions.Domain.Providers;
using LSDW.Domain.Helpers;

namespace LSDW.Domain.Providers;

/// <summary>
/// The random provider class.
/// </summary>
/// <remarks>
/// Wrapper for the <see cref="RandomHelper"/> methods.
/// </remarks>
internal sealed class RandomProvider : IRandomProvider
{
	/// <summary>
	/// The singleton instance of the random provider.
	/// </summary>
	internal static readonly RandomProvider Instance = new();

	public double GetDouble()
		=> RandomHelper.GetDouble();

	public float GetFloat()
		=> RandomHelper.GetFloat();

	public int GetInt()
		=> RandomHelper.GetInt();

	public int GetInt(int max)
		=> RandomHelper.GetInt(max);

	public int GetInt(float max)
		=> RandomHelper.GetInt(max);

	public int GetInt(int min, int max)
		=> RandomHelper.GetInt(min, max);

	public int GetInt(float min, float max)
		=> RandomHelper.GetInt(min, max);
}
