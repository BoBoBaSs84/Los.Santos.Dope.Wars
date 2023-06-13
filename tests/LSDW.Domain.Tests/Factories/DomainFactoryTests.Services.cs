using LSDW.Domain.Enumerators;
using LSDW.Domain.Factories;
using LSDW.Domain.Interfaces.Models;
using LSDW.Domain.Interfaces.Services;

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

		transactionService = DomainFactory.CreateTransactionService(type, source, target);

		Assert.IsNotNull(transactionService);
	}
}
