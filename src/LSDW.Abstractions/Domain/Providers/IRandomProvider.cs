namespace LSDW.Abstractions.Domain.Providers;

/// <summary>
/// The random provider interface.
/// </summary>
public interface IRandomProvider
{
	/// <summary>
	/// Returns a non-negative random integer.
	/// </summary>
	int GetInt();

	/// <summary>
	/// Returns a non-negative random integer that is less than the specified maximum.
	/// </summary>
	/// <param name="max">The exclusive upper bound of the random number to be generated.</param>
	int GetInt(int max);

	/// <summary>
	/// Returns a non-negative random integer that is less than the specified maximum.
	/// </summary>
	/// <param name="max">The exclusive upper bound of the random number to be generated.</param>
	int GetInt(float max);

	/// <summary>
	/// Returns a random integer that is within a specified range.
	/// </summary>
	/// <param name="min">The inclusive lower bound of the random number returned.</param>
	/// <param name="max">The exclusive upper bound of the random number returned.</param>
	int GetInt(int min, int max);

	/// <summary>
	/// Returns a random integer that is within a specified range.
	/// </summary>
	/// <param name="min">The inclusive lower bound of the random number returned.</param>
	/// <param name="max">The exclusive upper bound of the random number returned.</param>
	int GetInt(float min, float max);

	/// <summary>
	/// Returns a random floating-point number that is greater than or equal to 0.0 and less than 1.0.
	/// </summary>
	double GetDouble();

	/// <summary>
	/// Returns a random floating-point number that is greater than or equal to 0.0 and less than 1.0.
	/// </summary>
	float GetFloat();
}
