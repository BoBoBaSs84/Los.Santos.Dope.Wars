using LSDW.Drugs;

namespace LSDW.Tests.Drugs;

/// <summary>
/// The menu grid derived from the catalog, and the bounds the menus navigate by.
/// </summary>
/// <remarks>
/// <c>DrugMenu.ProcessKey</c> and <c>DealerTrading.CrossMenus</c> used to write these bounds
/// out as <c>1</c> and <c>2</c>. They now read <see cref="DrugCatalog.LastColumn"/> and
/// <see cref="DrugCatalog.LastRow"/>, so what is checked here is that the derivation still
/// produces what those literals asserted — and, if a seventh drug is ever added, that the
/// failure lands here rather than as a cursor that will not move.
/// </remarks>
[TestClass]
public sealed class DrugCatalogGridTests
{
  [TestMethod]
  public void LastColumn_ForTheCurrentCatalog_IsOne()
  {
    Assert.AreEqual(1, DrugCatalog.LastColumn);
  }

  [TestMethod]
  public void LastRow_ForTheCurrentCatalog_IsTwo()
  {
    Assert.AreEqual(2, DrugCatalog.LastRow);
  }

  [TestMethod]
  public void Grid_HasATileForEveryDrug_AndNoEmptySlots()
  {
    int slots = 0;

    foreach (string[] column in DrugCatalog.Grid)
    {
      foreach (string drug in column)
      {
        Assert.IsNotNull(drug, "Every grid slot must name a drug; a null is a hole the cursor can land in.");
        slots++;
      }
    }

    Assert.AreEqual(DrugCatalog.All.Count, slots);
  }

  [TestMethod]
  public void Grid_IsRectangular()
  {
    // DrugCatalog.LastRow reads column 0 and speaks for the rest, which only holds while
    // every column is the same length.
    foreach (string[] column in DrugCatalog.Grid)
    {
      Assert.AreEqual(DrugCatalog.Grid[0].Length, column.Length);
    }
  }

  [TestMethod]
  public void Grid_IndexesEachDrugAtItsOwnRowAndColumn()
  {
    foreach (Drug drug in DrugCatalog.All)
    {
      Assert.AreEqual(drug.Name, DrugCatalog.Grid[drug.MenuColumn][drug.MenuRow]);
    }
  }
}
