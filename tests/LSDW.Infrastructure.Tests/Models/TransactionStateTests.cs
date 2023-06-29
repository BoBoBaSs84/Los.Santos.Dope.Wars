using LSDW.Abstractions.Domain.Models;
using LSDW.Infrastructure.Factories;
using LSDW.Infrastructure.Models;
using Moq;

namespace LSDW.Infrastructure.Tests.Models;

[TestClass]
public class TransactionStateTests
{
	[TestMethod]
	public void ShouldSerializePriceTest()
	{
		Mock<ITransaction> mock = new();		

		TransactionState state =
			InfrastructureFactory.CreateTransactionState(mock.Object);

		Assert.IsFalse(state.ShouldSerializePrice());
	}
}