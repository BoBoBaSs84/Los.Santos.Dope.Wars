using LSDW.Constants;

namespace LSDW.Tests.Constants
{
	[TestClass]
	public class PlayerConstantsTests
	{
		[TestMethod]
		public void CalculateExperienceNextLevelSuccessTest()
		{
			int level = 1;

			double exp = PlayerConstants.CalculateExperienceNextLevel(level);

			Assert.IsTrue(exp > 6727);
			Assert.IsTrue(exp < 6728);
		}

		[TestMethod]
		public void CalculateExperienceNextLevelFailedTest()
		{
			int level = 2;

			double exp = PlayerConstants.CalculateExperienceNextLevel(level);

			Assert.IsFalse(exp < 6727);
			Assert.IsTrue(exp > 6728);
		}

		[TestMethod]
		public void CalculateCurrentLevelSuccessTest()
		{
			double experience = 6728;

			int level = PlayerConstants.CalculateCurrentLevel(experience);

			Assert.AreEqual(2, level);
		}

		[TestMethod]
		public void CalculateCurrentLevelFailedTest()
		{
			double experience = 6727;

			int level = PlayerConstants.CalculateCurrentLevel(experience);

			Assert.AreNotEqual(2, level);
		}
	}
}