using LSDW.Enumerators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LSDW.Extensions.Tests
{
	[TestClass]
	public class EnumeratorExtensionsTests
	{
		[TestMethod]
		public void GetDisplayNameSuccessTest()
		{
			DrugType drugType = DrugType.COKE;

			string displayName = drugType.GetDisplayName();

			Assert.AreNotEqual(displayName, drugType.ToString());
		}

		[TestMethod]
		public void GetDisplayNameFailedTest()
		{
			TestType testType = TestType.Test;

			string displayName = testType.GetDisplayName();

			Assert.AreEqual(displayName, testType.ToString());
		}

		[TestMethod]
		public void GetMarketPriceSuccessTest()
		{
			DrugType drugType = DrugType.COKE;

			int marketPrice = drugType.GetMarketPrice();

			Assert.AreNotEqual(marketPrice, 0);
		}

		[TestMethod]
		public void GetMarketPriceFailedTest()
		{
			TestType testType = TestType.Test;

			int marketPrice = testType.GetMarketPrice();

			Assert.AreEqual(marketPrice, 0);
		}

		[TestMethod]
		public void GetRankSuccessTest()
		{
			DrugType drugType = DrugType.COKE;

			int rank = drugType.GetRank();

			Assert.AreNotEqual(rank, 0);
		}

		[TestMethod]
		public void GetRankFailedTest()
		{
			TestType testType = TestType.Test;

			int rank = testType.GetRank();

			Assert.AreEqual(rank, 0);
		}

		internal enum TestType
		{
			Test
		}
	}
}