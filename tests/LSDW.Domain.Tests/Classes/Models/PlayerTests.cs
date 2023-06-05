using LSDW.Domain.Classes.Models;
using LSDW.Domain.Factories;
using LSDW.Domain.Interfaces.Models;

namespace LSDW.Domain.Tests.Classes.Models;

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
		IPlayer player = DomainFactory.CreatePlayer();
		int pointsToAdd = 500;

		player.AddExperience(pointsToAdd);

		Assert.AreEqual(pointsToAdd, player.Experience);
	}

	[TestMethod]
	public void CurrentLevelTest()
	{
		IPlayer player = DomainFactory.CreatePlayer();
		int pointsToAdd = 1500;

		player.AddExperience(pointsToAdd);

		Assert.AreEqual(1, player.Level);
	}

	[TestMethod]
	public void MaximumInventoryQuantityTest()
	{

		IPlayer player = DomainFactory.CreatePlayer();
		int pointsToAdd = 10000;

		player.AddExperience(pointsToAdd);

		Assert.AreEqual(120, player.MaximumInventoryQuantity);
	}

	[TestMethod]
	public void NextLevelExperienceTest()
	{
		int experience = 1000;

		IPlayer player = DomainFactory.CreatePlayer(experience);

		Assert.AreEqual(8000, player.ExperienceNextLevel);
	}
}