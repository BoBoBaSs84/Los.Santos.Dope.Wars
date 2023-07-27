using LSDW.Domain.Helpers;
using System.Text;

namespace LSDW.Domain.Tests.Helpers;

[TestClass, ExcludeFromCodeCoverage]
public class StringWriterWithEncodingTests
{
	[TestMethod]
	public void StringWriterWithoutEncodingTest()
	{
		StringWriterWithEncoding stringWriter = new();

		Assert.IsNotNull(stringWriter);
		Assert.AreNotEqual(Encoding.UTF8, stringWriter.Encoding);
	}

	[TestMethod]
	public void StringWriterWithEncodingTest()
	{
		StringWriterWithEncoding stringWriter = new(Encoding.UTF8);

		Assert.IsNotNull(stringWriter);
		Assert.AreEqual(Encoding.UTF8, stringWriter.Encoding);
	}
}