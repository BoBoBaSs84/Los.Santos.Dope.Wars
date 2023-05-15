﻿namespace LSDW.Core.Constants;

/// <summary>
/// The player constants class.
/// </summary>
public static class PlayerConstants
{
	private const int LevelFactor = 3;
	private const int LevelMultiplicator = 1000;

	/// <summary>
	/// The maximum player level.
	/// </summary>
	internal const int MaximumLevel = 100;

	/// <summary>
	/// Should return the experience points needed for the next level up.
	/// </summary>
	/// <param name="level">The current player level.</param>
	public static int CalculateExperienceNextLevel(int level)
		=> (int)(Math.Pow(level + 1, LevelFactor) * LevelMultiplicator);

	/// <summary>
	/// Should return the level based on the experience points.
	/// </summary>
	/// <param name="experience">The current experience points.</param>
	public static int CalculateCurrentLevel(int experience)
		=> (int)Math.Floor(Math.Pow(experience / LevelMultiplicator, 1.0 / LevelFactor));
}