namespace LSDW.Domain.Helpers;

/// <summary>
/// The random helper class.
/// </summary>
public static class RandomHelper
{
	private static readonly Random _random = new(Guid.NewGuid().GetHashCode());

	/// <summary>
	/// Returns a non-negative random integer.
	/// </summary>
	public static int GetInt() => _random.Next();

	/// <summary>
	/// Returns a non-negative random integer that is less than the specified maximum.
	/// </summary>
	/// <param name="max">The exclusive upper bound of the random number to be generated.</param>
	public static int GetInt(int max) => _random.Next(max);

	/// <summary>
	/// Returns a random integer that is within a specified range.
	/// </summary>
	/// <param name="min">The inclusive lower bound of the random number returned.</param>
	/// <param name="max">The exclusive upper bound of the random number returned.</param>
	public static int GetInt(int min, int max) => _random.Next(min, max);

	/// <summary>
	/// Returns a random integer that is within a specified range.
	/// </summary>
	/// <param name="min">The inclusive lower bound of the random number returned.</param>
	/// <param name="max">The exclusive upper bound of the random number returned.</param>
	public static int GetInt(float min, float max) => _random.Next((int)min, (int)max);

	/// <summary>
	/// Returns a random floating-point number that is greater than or equal to 0.0 and less than 1.0.
	/// </summary>
	/// <returns></returns>
	public static double GetDouble() => _random.NextDouble();
}