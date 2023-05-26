using LSDW.Core.Extensions;

namespace LSDW.Core.Tests.Extensions;

[TestClass]
public class StringExtensionsTests
{
	private readonly string _uncompressedString = @"<PlayerState Experience=""61155165""><Inventory Money=""1547467714""><Drugs><Drug Type=""COKE""><Quantity>0</Quantity><Price>0</Price></Drug><Drug Type=""SMACK""><Quantity>0</Quantity><Price>0</Price></Drug><Drug Type=""CANA""><Quantity>0</Quantity><Price>0</Price></Drug><Drug Type=""HASH""><Quantity>0</Quantity><Price>0</Price></Drug><Drug Type=""SHROOMS""><Quantity>0</Quantity><Price>0</Price></Drug><Drug Type=""SPEED""><Quantity>0</Quantity><Price>0</Price></Drug><Drug Type=""PCP""><Quantity>0</Quantity><Price>0</Price></Drug><Drug Type=""METH""><Quantity>0</Quantity><Price>0</Price></Drug><Drug Type=""KETA""><Quantity>0</Quantity><Price>0</Price></Drug><Drug Type=""PEYO""><Quantity>0</Quantity><Price>0</Price></Drug><Drug Type=""XTC""><Quantity>0</Quantity><Price>0</Price></Drug><Drug Type=""LSD""><Quantity>0</Quantity><Price>0</Price></Drug><Drug Type=""MDMA""><Quantity>0</Quantity><Price>0</Price></Drug><Drug Type=""CRACK""><Quantity>0</Quantity><Price>0</Price></Drug><Drug Type=""OXY""><Quantity>0</Quantity><Price>0</Price></Drug></Drugs></Inventory></PlayerState>";
	private readonly string _compressedString = @"pdRfC4MgEADwrxJ9gSb058WCMKHRnC57aI8RMoJh4WzMb7+iEdvjuCcPj594hycW984pI21nlUdfkzKD0r1K/RihKEJx5Gf4qJ9K29E4j41audRHUZiEcZKgcMkWZr49tsVr3LRQwiu6JC5zp+1gXXbAwR5jYYZerVtbgIMV/nDJclIBPMnPOYCXuSwhty9rzpmEnCAoLQBeEAHQjDaQ8ivaQJov6JUDeNsQgD5JSNtZwSCVkxr26nl7/VcHn9EN9gFf4q//IHsD";

	[TestMethod]
	public void FormatInvariantSuccessTest()
	{
		string unformatedString = "{0}+{1}={2}";
		int a = 1, b = 2, c = 3;

		string formatedString = unformatedString.FormatInvariant(a, b, c);

		Assert.AreEqual($"{a}+{b}={c}", formatedString);
	}

	[TestMethod]
	public void FormatInvariantFailedTest()
	{
		string unformatedString = "{0}+{1}={2}";
		int a = 1, b = 2, c = 3;

		string formatedString = unformatedString.FormatInvariant(a, b, c);

		Assert.AreNotEqual(unformatedString, formatedString);
	}

	[TestMethod]
	public void CompressTest()
	{
		string compressedString = _uncompressedString.Compress();
		Assert.AreEqual(_compressedString, compressedString);
	}

	[TestMethod]
	public void DecompressTest()
	{
		string uncompressedString = _compressedString.Decompress();
		Assert.AreEqual(_uncompressedString, uncompressedString);
	}
}