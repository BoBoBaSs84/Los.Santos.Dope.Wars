using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Models;
using LSDW.Core.Models;

namespace LSDW.Core.Tests.Classes;

[TestClass]
public class PlayerTests
{
	[ClassInitialize]
	public static void ClassInitialize(TestContext context)
	{
		Settings.Player.ExperienceMultiplier = 1;
		Settings.Player.StartingInventory = 100;
		Settings.Player.InventoryExpansionPerLevel = 10;
	}

	[TestMethod]
	public void AddExperienceTest()
	{
		IPlayer player = ModelFactory.CreatePlayer();
		int pointsToAdd = 500;

		player.AddExperience(pointsToAdd);

		Assert.AreEqual(pointsToAdd, player.Experience);
	}

	[TestMethod]
	public void CurrentLevelTest()
	{
		IPlayer player = ModelFactory.CreatePlayer();
		int pointsToAdd = 1500;

		player.AddExperience(pointsToAdd);

		Assert.AreEqual(1, player.Level);
	}

	[TestMethod]
	public void MaximumInventoryQuantityTest()
	{

		IPlayer player = ModelFactory.CreatePlayer();
		int pointsToAdd = 10000;

		player.AddExperience(pointsToAdd);

		Assert.AreEqual(120, player.MaximumInventoryQuantity);
	}

	[TestMethod]
	public void NextLevelExperienceTest()
	{
		int experience = 1000;

		IPlayer player = ModelFactory.CreatePlayer(experience);

		Assert.AreEqual(8000, player.ExperienceNextLevel);
	}
}