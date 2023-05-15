namespace LSDW.Core.Constants;

/// <summary>
/// The player constants class.
/// </summary>
public static class PlayerConstants
{
	private const double LevelFactor = 2.75;
	private const int LevelMultiplicator = 1000;

	/// <summary>
	/// The maximum player level.
	/// </summary>
	internal const int MaximumLevel = 100;

	/// <summary>
	/// Should return the experience points needed for the next level up.
	/// </summary>
	/// <param name="level">The current player level.</param>
	public static double CalculateExperienceNextLevel(int level)
		=> Math.Pow(level + 1, LevelFactor) * LevelMultiplicator;

	/// <summary>
	/// Should return the level based on the experience points.
	/// </summary>
	/// <param name="experiencePoints">The current experience points.</param>
	public static int CalculateCurrentLevel(double experiencePoints)
		=> (int)Math.Floor(Math.Pow(experiencePoints / LevelMultiplicator, 1.0 / LevelFactor));
}
