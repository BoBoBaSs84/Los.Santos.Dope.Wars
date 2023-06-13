using LSDW.Domain.Classes.Persistence;
using LSDW.Domain.Constants;
using LSDW.Domain.Enumerators;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
using LSDW.Domain.Helpers;
using LSDW.Domain.Interfaces.Actors;
using LSDW.Domain.Interfaces.Models;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace LSDW.Domain.Tests.Extensions;

[TestClass]
public class ClassExtensionsTests
{
	private readonly XmlWriterSettings _writerSettings = new();
	private readonly XmlReaderSettings _readerSettings = new();

	[DataTestMethod]
	[DataRow("utf-8"), DataRow("utf-16"), DataRow("utf-32")]
	public void ToXmlStringSuccessTest(string encodingString)
	{
		_writerSettings.Encoding = Encoding.GetEncoding(encodingString);

		FancyXmlTestClass testClass = new();

		string xmlString = testClass.ToXmlString(settings: _writerSettings);

		Assert.IsTrue(xmlString.Contains($"encoding=\"{encodingString}\""));
		Assert.IsTrue(xmlString.Contains($@"{nameof(testClass.Id)}=""{testClass.Id}"""));
		Assert.IsTrue(xmlString.Contains($@"<{nameof(testClass.Name)}>{testClass.Name}"));
		Assert.IsTrue(xmlString.Contains($@"<{nameof(testClass.Description)}>{testClass.Description}"));
	}

	[TestMethod]
	public void ToXmlStringFailedTest()
	{
		FancyXmlTestClass testClass = new();

		string xmlString = testClass.ToXmlString();

		Assert.IsFalse(xmlString.Contains($@"<{nameof(testClass.Id)}>""{testClass.Id}"""));
		Assert.IsFalse(xmlString.Contains($@"{nameof(testClass.Name)}=""{testClass.Name}"""));
		Assert.IsFalse(xmlString.Contains($@"{nameof(testClass.Description)}=""{testClass.Description}"""));
	}

	[TestMethod]
	public void FromXmlStringSuccessTest()
	{
		FancyXmlTestClass fancy = new();

		fancy = fancy.FromXmlString(XmlTextString, _readerSettings);

		Assert.AreEqual(Guid.Parse("348798ee-12f2-4a20-b030-756bb6a4134d"), fancy.Id);
		Assert.AreEqual("UnitTest", fancy.Name);
		Assert.AreEqual("UnitTestDescription", fancy.Description);
	}

	[TestMethod]
	public void FromXmlStringFailedTest()
	{
		FancyXmlTestClass fancy = new();

		fancy = fancy.FromXmlString(XmlTextString);

		Assert.AreNotEqual(Guid.Parse("348798ae-12f2-4a20-b030-756bb6a4134d"), fancy.Id);
		Assert.AreNotEqual("UnitTes", fancy.Name);
		Assert.AreNotEqual("UnitTestDescriptio", fancy.Description);
	}

	[TestMethod]
	public void HugeGameStateTest()
	{
		IPlayer player = DomainFactory.CreatePlayer();
		for (int i = 0; i < 1000; i++)
		{
			IDrug drug = DomainFactory.CreateDrug();
			ITransaction transaction =
				DomainFactory.CreateTransaction(DateTime.Now, TransactionType.BUY, drug.Type, RandomHelper.GetInt(10, 25), RandomHelper.GetInt(drug.AveragePrice - 10, drug.AveragePrice + 10));
			player.AddTransaction(transaction);
		}
		_ = player.Inventory.Randomize(player.Level);
		ICollection<IDealer> dealers = new HashSet<IDealer>();
		for (int i = 0; i < 80; i++)
		{
			IDealer dealer = DomainFactory.CreateDealer(new GTA.Math.Vector3(0, 0, 0));
			_ = dealer.Inventory.Randomize(player.Level);
			dealer.SetDiscovered(true);
			dealers.Add(dealer);
		}
		GameState gameState = DomainFactory.CreateGameState(player, dealers);

		string gameStateXml = gameState.ToXmlString(XmlConstants.SerializerNamespaces);
		string compressedGameStateXml = gameStateXml.Compress();

		File.WriteAllText($"{nameof(gameStateXml)}.xml", gameStateXml);
		File.WriteAllText($"{nameof(compressedGameStateXml)}.xml", compressedGameStateXml);

		Assert.IsFalse(string.IsNullOrWhiteSpace(gameStateXml));
		Assert.IsFalse(string.IsNullOrWhiteSpace(compressedGameStateXml));
	}

	[XmlRoot("Fancy")]
	public class FancyXmlTestClass
	{
		[XmlAttribute]
		public Guid Id { get; set; } = Guid.NewGuid();
		[XmlElement]
		public string Name { get; set; } = "Super fancy name";
		[XmlElement]
		public string Description { get; set; } = "Super fancy description";
	}

	private const string XmlTextString = @"<Fancy Id=""348798ee-12f2-4a20-b030-756bb6a4134d""><Name>UnitTest</Name><Description>UnitTestDescription</Description></Fancy>";
}