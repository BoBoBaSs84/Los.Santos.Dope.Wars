using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Domain.Factories;
using LSDW.Infrastructure.Factories;
using LSDW.Infrastructure.Models;

namespace LSDW.Infrastructure.Tests.Models;

[TestClass, ExcludeFromCodeCoverage]
public class PlayerStateTests
{
	[TestMethod]
	public void ShouldNotSerializeTransactionsTest()
	{
		IPlayer player = DomainFactory.CreatePlayer();

		PlayerState state =
			InfrastructureFactory.CreatePlayerState(player);

		Assert.IsFalse(state.ShouldSerializeTransactions());
	}

	[TestMethod]
	public void ShouldSerializeTransactionsTest()
	{
		ITransaction transaction = DomainFactory.CreateTransaction(DateTime.Now, TransactionType.BUY, DrugType.COKE, 10, 100);
		IPlayer player = DomainFactory.CreatePlayer();
		player.AddTransaction(transaction);

		PlayerState state =
			InfrastructureFactory.CreatePlayerState(player);

		Assert.IsTrue(state.ShouldSerializeTransactions());
	}

	[TestMethod()]
	public void ShouldNotSerializeExperienceTest()
	{
		IPlayer player = DomainFactory.CreatePlayer();

		PlayerState state =
			InfrastructureFactory.CreatePlayerState(player);

		Assert.IsFalse(state.ShouldSerializeExperience());
	}

	[TestMethod()]
	public void ShouldSerializeExperienceTest()
	{
		IPlayer player = DomainFactory.CreatePlayer(1000);

		PlayerState state =
			InfrastructureFactory.CreatePlayerState(player);

		Assert.IsTrue(state.ShouldSerializeExperience());
	}
}