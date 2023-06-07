using Microsoft.VisualStudio.TestTools.UnitTesting;
using LSDW.Presentation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSDW.Domain.Enumerators;
using LSDW.Presentation.Properties;

namespace LSDW.Presentation.Helpers.Tests;

[TestClass]
public class SwitchItemHelperTests
{
	[DataTestMethod]
	[DynamicData(nameof(GetTitles), DynamicDataSourceType.Method)]
	public void GetTitleTest(MenuType menuType, string expectedTitle)
	{
		string returnTitle = SwitchItemHelper.GetTitle(menuType);

		Assert.AreEqual(expectedTitle, returnTitle);
	}

	[DataTestMethod]
	[DynamicData(nameof(GetDescriptions), DynamicDataSourceType.Method)]
	public void GetDescriptionTest(MenuType menuType, string expectedDescription)
	{
		string returnDescription = SwitchItemHelper.GetDescription(menuType);

		Assert.AreEqual(expectedDescription, returnDescription);
	}

	private static IEnumerable<object[]> GetTitles()
	{
		yield return new object[]
		{
			MenuType.BUY,
			Resources.UI_Switch_Item_Title_Buy
		};
		yield return new object[]
		{
			MenuType.RETRIEVE,
			Resources.UI_Switch_Item_Title_Retrieve
		};
		yield return new object[]
		{
			MenuType.TAKE,
			Resources.UI_Switch_Item_Title_Take
		};
		yield return new object[]
		{
			MenuType.SELL,
			Resources.UI_Switch_Item_Title_Sell
		};
		yield return new object[]
		{
			MenuType.STORE,
			Resources.UI_Switch_Item_Title_Store
		};
		yield return new object[]
		{
			MenuType.GIVE,
			Resources.UI_Switch_Item_Title_Give
		};
	}
	private static IEnumerable<object[]> GetDescriptions()
	{
		yield return new object[]
		{
			MenuType.BUY,
			Resources.UI_Switch_Item_Description_Buy
		};
		yield return new object[]
		{
			MenuType.RETRIEVE,
			Resources.UI_Switch_Item_Description_Retrieve
		};
		yield return new object[]
		{
			MenuType.TAKE,
			Resources.UI_Switch_Item_Description_Take
		};
		yield return new object[]
		{
			MenuType.SELL,
			Resources.UI_Switch_Item_Description_Sell
		};
		yield return new object[]
		{
			MenuType.STORE,
			Resources.UI_Switch_Item_Description_Store
		};
		yield return new object[]
		{
			MenuType.GIVE,
			Resources.UI_Switch_Item_Description_Give
		};
	}
}