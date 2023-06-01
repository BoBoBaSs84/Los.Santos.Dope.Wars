using LSDW.Core.Enumerators;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Models;
using LSDW.Core.Interfaces.Services;

namespace LSDW.Core.Tests.Factories;

[TestClass]
public class ServiceFactoryTests
{
	[TestMethod]
	public void CreateTransactionServiceTest()
	{
		IPlayer player = ModelFactory.CreatePlayer();
		IInventory drugs = ModelFactory.CreateInventory();

		ITransactionService? transactionService;

		transactionService = ServiceFactory.CreateTransactionService(TransactionType.TRAFFIC, drugs, player.Inventory, player.MaximumInventoryQuantity);

		Assert.IsNotNull(transactionService);
	}
}