using LSDW.Abstractions.Domain.Providers;
using LSDW.Domain.Factories;

namespace LSDW.Domain.Tests.Providers;

[TestClass, ExcludeFromCodeCoverage]
public class RandomProviderTests
{
	private readonly IRandomProvider _randomProvider = DomainFactory.GetRandomProvider();

	[TestMethod]
	public void GetDoubleTest()
	{
		double d = 0d;

		double r = _randomProvider.GetDouble();

		Assert.AreNotEqual(d, r);
	}

	[TestMethod]
	public void GetFloatTest()
	{
		double f = 0f;

		double r = _randomProvider.GetFloat();

		Assert.AreNotEqual(f, r);
	}

	[TestMethod]
	public void GetIntTest()
	{
		int i = 0;

		int r = _randomProvider.GetInt();

		Assert.AreNotEqual(i, r);
	}

	[TestMethod]
	public void GetIntWithMaxIntTest()
	{
		int iMax = 10;

		int r = _randomProvider.GetInt(iMax);

		Assert.AreNotEqual(iMax, r);
	}

	[TestMethod]
	public void GetIntWithMaxFloatTest()
	{
		float fMax = 10f;

		int r = _randomProvider.GetInt(fMax);

		Assert.AreNotEqual(fMax, r);
	}

	[TestMethod]
	public void GetIntWithMinMaxIntTest()
	{
		int iMin = 1;
		int iMax = 10;

		int r = _randomProvider.GetInt(iMin, iMax);

		Assert.AreNotEqual(iMax, r);
		Assert.AreNotEqual(default, r);
	}

	[TestMethod]
	public void GetIntWithMinMaxFloatTest()
	{
		float fMin = 1f;
		float fMax = 10f;

		int r = _randomProvider.GetInt(fMin, fMax);

		Assert.AreNotEqual(fMax, r);
		Assert.AreNotEqual(default, r);
	}
}