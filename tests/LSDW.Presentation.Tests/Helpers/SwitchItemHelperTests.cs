using LSDW.Domain.Enumerators;
using LSDW.Presentation.Properties;

namespace LSDW.Presentation.Helpers.Tests;

[TestClass]
public class SwitchItemHelperTests
{
	[DataTestMethod]
	[DynamicData(nameof(GetTitles), DynamicDataSourceType.Method)]
	public void GetTitleTest(TransactionType type, string expectedTitle)
	{
		string returnTitle = SwitchItemHelper.GetTitle(type);

		Assert.AreEqual(expectedTitle, returnTitle);
	}

	[DataTestMethod]
	[DynamicData(nameof(GetDescriptions), DynamicDataSourceType.Method)]
	public void GetDescriptionTest(TransactionType type, string expectedDescription)
	{
		string returnDescription = SwitchItemHelper.GetDescription(type);

		Assert.AreEqual(expectedDescription, returnDescription);
	}

	private static IEnumerable<object[]> GetTitles()
	{
		yield return new object[]
		{
			TransactionType.BUY,
			Resources.UI_Switch_Item_Title_Buy
		};
		yield return new object[]
		{
			TransactionType.TAKE,
			Resources.UI_Switch_Item_Title_Take
		};
		yield return new object[]
		{
			TransactionType.SELL,
			Resources.UI_Switch_Item_Title_Sell
		};
		yield return new object[]
		{
			TransactionType.GIVE,
			Resources.UI_Switch_Item_Title_Give
		};
	}
	private static IEnumerable<object[]> GetDescriptions()
	{
		yield return new object[]
		{
			TransactionType.BUY,
			Resources.UI_Switch_Item_Description_Buy
		};
		yield return new object[]
		{
			TransactionType.TAKE,
			Resources.UI_Switch_Item_Description_Take
		};
		yield return new object[]
		{
			TransactionType.SELL,
			Resources.UI_Switch_Item_Description_Sell
		};
		yield return new object[]
		{
			TransactionType.GIVE,
			Resources.UI_Switch_Item_Description_Give
		};
	}
}