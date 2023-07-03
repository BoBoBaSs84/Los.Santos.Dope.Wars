using LSDW.Domain.Constants;

namespace LSDW.Domain.Tests.Constants;

[TestClass]
public class PlayerConstantsTests
{
	[TestMethod]
	public void CalculateExperienceNextLevelSuccessTest()
	{
		int level = 1;

		int experience = PlayerConstants.CalculateExperienceNextLevel(level);

		Assert.AreEqual(8000, experience);
	}

	[TestMethod]
	public void CalculateExperienceNextLevelFailedTest()
	{
		int level = 2;

		int experience = PlayerConstants.CalculateExperienceNextLevel(level);

		Assert.AreNotEqual(8000, experience);
	}

	[TestMethod]
	public void CalculateCurrentLevelSuccessTest()
	{
		int experience = 26999;

		int level = PlayerConstants.CalculateCurrentLevel(experience);

		Assert.AreEqual(2, level);
	}

	[TestMethod]
	public void CalculateCurrentLevelFailedTest()
	{
		int experience = 6727;

		int level = PlayerConstants.CalculateCurrentLevel(experience);

		Assert.AreNotEqual(2, level);
	}

	[TestMethod]
	public void LevelLimitTest()
	{
		int maxLevel = PlayerConstants.MaximumLevel;

		for (int currentLevel = 0; currentLevel < maxLevel; currentLevel++)
		{
			int experience = PlayerConstants.CalculateExperienceNextLevel(currentLevel);
			Trace.WriteLine($"CurrentLevel:{currentLevel}\tExperienceNextLevel:{experience}");
		}
	}
}