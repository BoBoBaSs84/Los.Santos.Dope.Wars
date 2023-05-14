using LSDW.Enumerators;
using LSDW.Extensions;

namespace LSDW.Tests.Extensions
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

			int marketPrice = drugType.GetMarketValue();

			Assert.AreNotEqual(marketPrice, 0);
		}

		[TestMethod]
		public void GetMarketPriceFailedTest()
		{
			TestType testType = TestType.Test;

			int marketPrice = testType.GetMarketValue();

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

		[TestMethod]
		public void GetDescriptionSuccessTest()
		{
			DrugType drugType = DrugType.COKE;

			string description = drugType.GetDescription();

			Assert.AreNotEqual(description, drugType.ToString());
		}

		[TestMethod]
		public void GetDescriptionFailedTest()
		{
			TestType testType = TestType.Test;

			string description = testType.GetDescription();

			Assert.AreEqual(description, testType.ToString());
		}

		internal enum TestType
		{
			Test
		}
	}
}