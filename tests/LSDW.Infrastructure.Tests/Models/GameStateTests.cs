using LSDW.Abstractions.Domain.Models;
using LSDW.Domain.Factories;
using LSDW.Infrastructure.Factories;
using LSDW.Infrastructure.Models;

namespace LSDW.Infrastructure.Tests.Models;

[TestClass]
public class GameStateTests
{
	[TestMethod]
	public void ShouldSerializeDealersTest()
	{
		ICollection<IDealer> dealers = DomainFactory.CreateDealers();
		IPlayer player = DomainFactory.CreatePlayer();

		State state =
			InfrastructureFactory.CreateGameState(dealers, player);

		Assert.IsFalse(state.ShouldSerializeDealers());
	}
}