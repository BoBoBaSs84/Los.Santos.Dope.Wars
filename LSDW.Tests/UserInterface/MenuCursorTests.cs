using LSDW.Drugs;
using LSDW.UserInterface;

namespace LSDW.Tests.UserInterface;

/// <summary>
/// Moving the selection around the drug grid: where it starts, where it refuses to go, and
/// whether it admits to having moved.
/// </summary>
/// <remarks>
/// The binds are passed in rather than read from <c>SettingsService</c>, so these tests use keys
/// no player has and cannot be broken by a change to the defaults. That is also the arrangement
/// that keeps the real capture at construction time, where <c>DrugDeal</c>'s first ordering rule
/// needs it.
/// </remarks>
[TestClass]
public sealed class MenuCursorTests
{
  private const Keys Up = Keys.F13;
  private const Keys Down = Keys.F14;
  private const Keys Left = Keys.F15;
  private const Keys Right = Keys.F16;

  private static MenuCursor NewCursor() => new(Up, Down, Left, Right);

  [TestMethod]
  public void NewCursor_StartsAtTheTopLeft()
  {
    MenuCursor cursor = NewCursor();

    Assert.AreEqual(0, cursor.Row);
    Assert.AreEqual(0, cursor.Column);
    Assert.AreEqual(DrugCatalog.Grid[0][0], cursor.CurrentItem);
  }

  [TestMethod]
  public void Down_FromTheTop_Moves()
  {
    MenuCursor cursor = NewCursor();

    Assert.IsTrue(cursor.TryMove(Down));
    Assert.AreEqual(1, cursor.Row);
    Assert.AreEqual(0, cursor.Column);
  }

  [TestMethod]
  public void Right_FromTheLeft_Moves()
  {
    MenuCursor cursor = NewCursor();

    Assert.IsTrue(cursor.TryMove(Right));
    Assert.AreEqual(1, cursor.Column);
    Assert.AreEqual(0, cursor.Row);
  }

  [TestMethod]
  public void Up_AtTheTop_ReportsNoMove()
  {
    // The return value is what gates the click. A wall has to stay silent.
    MenuCursor cursor = NewCursor();

    Assert.IsFalse(cursor.TryMove(Up));
    Assert.AreEqual(0, cursor.Row);
  }

  [TestMethod]
  public void Left_AtTheLeftEdge_ReportsNoMove()
  {
    MenuCursor cursor = NewCursor();

    Assert.IsFalse(cursor.TryMove(Left));
    Assert.AreEqual(0, cursor.Column);
  }

  [TestMethod]
  public void Down_AtTheBottom_ReportsNoMove()
  {
    MenuCursor cursor = NewCursor();

    for (int row = 0; row < DrugCatalog.LastRow; row++)
    {
      Assert.IsTrue(cursor.TryMove(Down), "could not reach the bottom row");
    }

    Assert.AreEqual(DrugCatalog.LastRow, cursor.Row);
    Assert.IsFalse(cursor.TryMove(Down));
    Assert.AreEqual(DrugCatalog.LastRow, cursor.Row);
  }

  [TestMethod]
  public void Right_AtTheRightEdge_ReportsNoMove()
  {
    // The edge DealerTrading.CrossMenus watches for: pushing right off the last column is
    // what hands the key to the other side's menu, and it can only mean that because the
    // cursor itself refuses to go further.
    MenuCursor cursor = NewCursor();

    for (int column = 0; column < DrugCatalog.LastColumn; column++)
    {
      Assert.IsTrue(cursor.TryMove(Right), "could not reach the last column");
    }

    Assert.AreEqual(DrugCatalog.LastColumn, cursor.Column);
    Assert.IsFalse(cursor.TryMove(Right));
    Assert.AreEqual(DrugCatalog.LastColumn, cursor.Column);
  }

  [TestMethod]
  public void AnUnboundKey_ReportsNoMove()
  {
    MenuCursor cursor = NewCursor();

    Assert.IsFalse(cursor.TryMove(Keys.Space));
    Assert.AreEqual(0, cursor.Row);
    Assert.AreEqual(0, cursor.Column);
  }

  [TestMethod]
  public void TheBinds_AreCapturedPerCursor()
  {
    // Proves the keys are captured rather than read from the settings: this cursor answers to
    // A and D and ignores the ones every other test here uses.
    MenuCursor cursor = new(Keys.W, Keys.S, Keys.A, Keys.D);

    Assert.IsFalse(cursor.TryMove(Down));
    Assert.IsTrue(cursor.TryMove(Keys.S));
    Assert.AreEqual(1, cursor.Row);
  }

  [TestMethod]
  public void CurrentItem_TracksTheGrid()
  {
    MenuCursor cursor = NewCursor();

    _ = cursor.TryMove(Right);
    _ = cursor.TryMove(Down);

    Assert.AreEqual(DrugCatalog.Grid[1][1], cursor.CurrentItem);
  }

  [TestMethod]
  public void WalkingTheGrid_ReachesEveryDrugExactlyOnce()
  {
    // Catalog-driven rather than written out: adding a drug should extend this walk on its
    // own. A tile the cursor cannot reach is a drug the player cannot trade.
    MenuCursor cursor = NewCursor();
    List<string> visited = [];

    while (true)
    {
      visited.Add(cursor.CurrentItem);

      while (cursor.TryMove(Down))
      {
        visited.Add(cursor.CurrentItem);
      }

      if (!cursor.TryMove(Right))
      {
        break;
      }

      // Back to the top of the next column, the way a player would walk it.
      while (cursor.TryMove(Up))
      {
      }
    }

    Assert.HasCount(DrugCatalog.All.Count, visited);
    Assert.AreSequenceEqual(DrugCatalog.All.Select(drug => drug.Name).OrderBy(name => name).ToList(), visited.OrderBy(name => name).ToList());
  }

  [TestMethod]
  public void MovingBackAndForth_ReturnsToWhereItStarted()
  {
    MenuCursor cursor = NewCursor();

    _ = cursor.TryMove(Down);
    _ = cursor.TryMove(Right);
    _ = cursor.TryMove(Up);
    _ = cursor.TryMove(Left);

    Assert.AreEqual(0, cursor.Row);
    Assert.AreEqual(0, cursor.Column);
    Assert.AreEqual(DrugCatalog.Grid[0][0], cursor.CurrentItem);
  }
}
