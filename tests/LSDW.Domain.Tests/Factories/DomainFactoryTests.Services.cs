using LSDW.Abstractions.Domain.Models;
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
		IInventory source = DomainFactory.CreateInventory();
		IInventory target = DomainFactory.CreateInventory();
		ITransactionService? transactionService;

		transactionService = DomainFactory.CreateTransactionService(_providerManagerMock.Object, type, source, target);

		Assert.IsNotNull(transactionService);
	}
}
