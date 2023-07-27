using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;

namespace LSDW.Domain.Tests.Extensions;

[TestClass, ExcludeFromCodeCoverage]
public class TransactionExtensionsTests
{
	[TestMethod]
	public void GetPositiveBuyValueTest()
	{
		ITransaction transaction =
			DomainFactory.CreateTransaction(DateTime.Now, TransactionType.BUY, DrugType.COKE, 10, 50);

		int value = transaction.GetValue();

		Assert.AreNotEqual(default, value);
	}

	[TestMethod]
	public void GetZeroBuyValueTest()
	{
		ITransaction transaction =
			DomainFactory.CreateTransaction(DateTime.Now, TransactionType.BUY, DrugType.COKE, 10, 500);

		int value = transaction.GetValue();

		Assert.AreEqual(default, value);
	}

	[TestMethod]
	public void GetPositiveSellValueTest()
	{
		ITransaction transaction =
			DomainFactory.CreateTransaction(DateTime.Now, TransactionType.SELL, DrugType.COKE, 10, 150);

		int value = transaction.GetValue();

		Assert.AreNotEqual(default, value);
	}

	[TestMethod]
	public void GetZeroSellValueTest()
	{
		ITransaction transaction =
			DomainFactory.CreateTransaction(DateTime.Now, TransactionType.SELL, DrugType.COKE, 10, 50);

		int value = transaction.GetValue();

		Assert.AreEqual(default, value);
	}
}