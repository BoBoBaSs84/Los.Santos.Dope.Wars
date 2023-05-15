using LSDW.Core.Interfaces.Classes;
using CF = LSDW.Core.Factories.CharacterFactory;
using IF = LSDW.Core.Factories.InventoryFactory;

namespace LSDW.Core.Tests.Classes;

[TestClass]
public class PlayerCharacterTests
{
	[TestMethod]
	public void AddExperienceTest()
	{
		IPlayerCharacter character = CF.CreatePlayer();
		int pointsToAdd = 500;

		character.AddExperience(pointsToAdd);

		Assert.IsTrue(Equals(character.CurrentExperience, pointsToAdd));
	}

	[TestMethod]
	public void CurrentLevelTest()
	{
		IPlayerCharacter character = CF.CreatePlayer();
		int pointsToAdd = 1500;

		character.AddExperience(pointsToAdd);

		Assert.IsTrue(Equals(character.CurrentLevel, 1));
	}

	[TestMethod]
	public void MaximumInventoryQuantityTest()
	{
		IPlayerCharacter character = CF.CreatePlayer();
		int pointsToAdd = 7500;

		character.AddExperience(pointsToAdd);

		Assert.IsTrue(Equals(character.MaximumInventoryQuantity, 120));
	}

	[TestMethod]
	public void NextLevelExperienceTest()
	{
		IInventoryCollection drugs = IF.CreateInventory();
		int experience = 1000;

		IPlayerCharacter player = CF.CreatePlayer(drugs, experience);

		Assert.IsTrue(Equals(player.NextLevelExperience, 6727));
	}
}