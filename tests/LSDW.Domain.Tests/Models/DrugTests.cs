using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Extensions;
using LSDW.Domain.Factories;

namespace LSDW.Domain.Tests.Models;

[TestClass, ExcludeFromCodeCoverage]
[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public class DrugTests
{
	[TestMethod]
	public void AddSuccessTest()
	{
		IDrug drug = DomainFactory.CreateDrug(DrugType.COKE, 10, 100);

		drug.Add(10, 50);

		Assert.IsTrue(drug.Quantity.Equals(20));
		Assert.IsTrue(drug.Price.Equals(75));
	}

	[TestMethod]
	public void AddPriceLessThanZeroExceptionTest()
	{
		IDrug drug = DomainFactory.CreateDrug(DrugType.COKE, 10, 1000);

		Assert.Throws<ArgumentOutOfRangeException>(() => drug.Add(1, -1));
	}

	[TestMethod]
	public void AddFailureTest()
	{
		IDrug drug = DomainFactory.CreateDrug(DrugType.COKE, 10, 1000);

		drug.Add(5, default);

		Assert.IsFalse(drug.Quantity.Equals(20));
		Assert.IsFalse(drug.Price.Equals(750));
	}

	[TestMethod()]
	public void AddEarlyExitTest()
	{
		IDrug drug = DomainFactory.CreateDrug(DrugType.METH, 5);

		drug.Add(-5, default);

		Assert.AreEqual(5, drug.Quantity);
	}

	[TestMethod]
	public void RemoveSuccessTest()
	{
		IDrug drug = DomainFactory.CreateDrug(DrugType.COKE, 10, 1000);

		drug.Remove(5);

		Assert.IsTrue(drug.Quantity.Equals(5));
	}

	[TestMethod]
	public void RemoveFailureTest()
	{
		IDrug drug = DomainFactory.CreateDrug(DrugType.COKE, 10, 1000);

		drug.Remove(1);

		Assert.IsFalse(drug.Quantity.Equals(5));
	}

	[TestMethod]
	public void RemoveEarlyExitTest()
	{
		IDrug drug = DomainFactory.CreateDrug(DrugType.HASH);

		drug.Remove(-1);

		Assert.AreEqual(default, drug.Quantity);
	}

	[TestMethod]
	public void RemovePriceSetZeroTest()
	{
		IDrug drug = DomainFactory.CreateDrug(DrugType.LSD, 100, 20);

		drug.Remove(100);

		Assert.AreEqual(default, drug.Quantity);
		Assert.AreEqual(default, drug.Price);
	}

	[TestMethod]
	public void RandomizeQuantityTest()
	{
		IDrug drug = DomainFactory.CreateDrug(DrugType.CANA, -1);

		drug.RandomizeQuantity(default);

		Assert.AreNotEqual(-1, drug.Quantity);
	}

	[TestMethod]
	public void RandomizePriceTest()
	{
		IDrug drug = DomainFactory.CreateDrug(DrugType.CANA, default, -1);

		drug.RandomizePrice(default);

		Assert.AreNotEqual(-1, drug.Price);
	}

	[TestMethod]
	public void SpecialBuyOfferTest()
	{
		IDrug drug = DomainFactory.CreateDrug();

		drug.SpecialBuyOffer(50);

		Assert.AreEqual(0, drug.Quantity);
		Assert.AreNotEqual(drug.Type.GetAveragePrice(), drug.Price);
	}

	[TestMethod]
	public void SpecialSellOfferTest()
	{
		IDrug drug = DomainFactory.CreateDrug();

		drug.SpecialSellOffer(default);

		Assert.AreNotEqual(0, drug.Quantity);
		Assert.AreNotEqual(drug.Type.GetAveragePrice(), drug.Price);
	}
}
