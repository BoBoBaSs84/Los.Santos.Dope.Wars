using LSDW.Abstractions.Domain.Services;
using LSDW.Abstractions.Enumerators;
using LSDW.Domain.Factories;

namespace LSDW.Domain.Tests.Factories;

public partial class DomainFactoryTests
{
	[TestMethod]
	public void CreateTransactionServiceTest()
	{
		TransactionType type = TransactionType.BUY;
		ITransactionService? transactionService;

		transactionService = DomainFactory.CreateTransactionService(_providerManagerMock.Object, type, _inventoryMock.Object, _inventoryMock.Object);

		Assert.IsNotNull(transactionService);
	}

	[TestMethod]
	public void CreateTransactionServiceTestTwo()
	{
		TransactionType type = TransactionType.BUY;
		ITransactionService? service;

		service = DomainFactory.CreateTransactionService(_providerManagerMock.Object, type, _playerMock.Object, _inventoryMock.Object);

		Assert.IsNotNull(service);
	}
}
