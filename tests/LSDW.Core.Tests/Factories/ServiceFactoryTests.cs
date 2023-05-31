using LSDW.Core.Classes;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSDW.Core.Tests.Factories;

[TestClass()]
public class ServiceFactoryTests
{
	[TestMethod()]
	public void CreateTransactionServiceTest()
	{
		ITransactionService? transactionService;
		var parameter = new TransactionParameter()

		transactionService = ServiceFactory.CreateTransactionService(null);

		Assert.IsNotNull(transactionService);
	}
}