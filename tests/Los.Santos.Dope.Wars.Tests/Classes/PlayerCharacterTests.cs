using LSDW.Interfaces.Classes;
using CF = LSDW.Factories.CharacterFactory;

namespace LSDW.Tests.Classes
{
	[TestClass]
	public class PlayerCharacterTests
	{
		[TestMethod]
		public void AddExperienceTest()
		{
			IPlayerCharacter character = CF.CreatePlayerCharacter();
			double pointsToAdd = 500;

			character.AddExperience(pointsToAdd);

			Assert.IsTrue(Equals(character.CurrentExperience, pointsToAdd));
		}


	}
}