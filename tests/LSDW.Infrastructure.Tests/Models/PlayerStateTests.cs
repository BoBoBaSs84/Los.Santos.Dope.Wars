using LSDW.Abstractions.Domain.Models;
using LSDW.Domain.Factories;
using LSDW.Infrastructure.Factories;
using LSDW.Infrastructure.Models;

namespace LSDW.Infrastructure.Tests.Models;

[TestClass]
public class PlayerStateTests
{
	[TestMethod]
	public void ShouldSerializeTransactionsTest()
	{
		IPlayer player = DomainFactory.CreatePlayer();

		PlayerState state =
			InfrastructureFactory.CreatePlayerState(player);

		Assert.IsFalse(state.ShouldSerializeTransactions());
	}
}