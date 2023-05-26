﻿using LSDW.Core.Extensions;

namespace LSDW.Core.Tests.Extensions;

[TestClass]
public class IntegerExtensionsTests
{
	[TestMethod]
	public void GetArrayTest()
	{
		int i = 10;

		int[] intArray = i.GetArray();

		Assert.AreEqual(11, intArray.Length);
	}
}