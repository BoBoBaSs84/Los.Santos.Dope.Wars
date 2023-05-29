using LSDW.Core.Classes;
using LSDW.Core.Interfaces.Classes;
using LSDW.Core.Factories;

namespace LSDW.Core.Tests.Classes;

[TestClass]
public class PlayerTests
{
	[ClassInitialize]
	public static void ClassInitialize(TestContext context)
	{
		Settings.PlayerSettings.ExperienceMultiplier = 1;
		Settings.PlayerSettings.StartingInventory = 100;
		Settings.PlayerSettings.InventoryExpansionPerLevel = 10;
	}

	[TestMethod]
	public void AddExperienceTest()
	{
		IPlayer player = PlayerFactory.CreatePlayer();
		int pointsToAdd = 500;

		player.AddExperience(pointsToAdd);

		Assert.AreEqual(pointsToAdd, player.Experience);
	}

	[TestMethod]
	public void CurrentLevelTest()
	{
		IPlayer player = PlayerFactory.CreatePlayer();
		int pointsToAdd = 1500;

		player.AddExperience(pointsToAdd);

		Assert.AreEqual(1, player.Level);
	}

	[TestMethod]
	public void MaximumInventoryQuantityTest()
	{

		IPlayer player = PlayerFactory.CreatePlayer();
		int pointsToAdd = 10000;

		player.AddExperience(pointsToAdd);

		Assert.AreEqual(120, player.MaximumInventoryQuantity);
	}

	[TestMethod]
	public void NextLevelExperienceTest()
	{
		int experience = 1000;

		IPlayer player = PlayerFactory.CreatePlayer(experience);

		Assert.AreEqual(8000, player.ExperienceNextLevel);
	}
}