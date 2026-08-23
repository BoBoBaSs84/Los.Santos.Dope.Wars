using LSDW.Drugs;

namespace LSDW.Tests.Drugs;

/// <summary>
/// The catalog itself: lookup, the maps every per-drug dictionary is built from, and the
/// relationship between a drug's price and its scarcity.
/// </summary>
/// <remarks>
/// The menu grid derived from the same list has its own class, <c>DrugCatalogGridTests</c> —
/// the split is deliberate, because a grid failure and a tuning failure want to be read
/// separately. The last test here is the one that speaks to a drug being <i>added</i>: price and
/// supply run inverse to each other, and getting that backwards makes the new drug either
/// worthless or a money printer, which nothing else in the suite would catch.
/// </remarks>
[TestClass]
public sealed class DrugCatalogTests
{
  [TestMethod]
  public void Get_ByName_ReturnsThatDrug()
  {
    Drug drug = DrugCatalog.Get("Cocaine");

    Assert.AreEqual("Cocaine", drug.Name);
    Assert.AreSame(DrugCatalog.All.First(entry => entry.Name == "Cocaine"), drug);
  }

  [TestMethod]
  public void Get_ForEveryCatalogEntry_Succeeds()
  {
    foreach (Drug drug in DrugCatalog.All)
    {
      Assert.AreSame(drug, DrugCatalog.Get(drug.Name));
    }
  }

  [TestMethod]
  public void Get_AnUnknownName_Throws()
  {
    // Not a null and not a default entry: a name the catalog does not know means the save
    // loader or a settings key is out of step with the build, and the callers that tolerate
    // that check first — see SaveService.LoadDrugs.
    _ = Assert.ThrowsExactly<KeyNotFoundException>(() => DrugCatalog.Get("Mandrake"));
  }

  [TestMethod]
  public void Get_IsCaseSensitive()
  {
    // The name is the key in the save document and in the settings file, so it is matched
    // exactly rather than folded.
    _ = Assert.ThrowsExactly<KeyNotFoundException>(() => DrugCatalog.Get("cocaine"));
  }

  [TestMethod]
  public void Names_AreUnique()
  {
    // The name keys every per-drug dictionary in the mod, both save sections and the settings
    // file. A duplicate would already throw out of the Lookup initialiser; asserted here so the
    // failure reads as what it is.
    Assert.AreEqual(DrugCatalog.All.Count, DrugCatalog.All.Select(drug => drug.Name).Distinct().Count());
  }

  [TestMethod]
  public void NewAmountMap_IsEveryDrugAtNothing()
  {
    Dictionary<string, int> map = DrugCatalog.NewAmountMap();

    Assert.HasCount(DrugCatalog.All.Count, map);
    Assert.AreSequenceEqual([.. DrugCatalog.All.Select(drug => drug.Name)], map.Keys);

    foreach (int amount in map.Values)
    {
      Assert.AreEqual(0, amount);
    }
  }

  [TestMethod]
  public void NewPriceMap_IsEveryDrugAtItsMarketValue()
  {
    Dictionary<string, int> map = DrugCatalog.NewPriceMap();

    Assert.HasCount(DrugCatalog.All.Count, map);

    foreach (Drug drug in DrugCatalog.All)
    {
      Assert.AreEqual(drug.MarketValue, map[drug.Name], drug.Name);
    }
  }

  [TestMethod]
  public void NewAmountMap_ReturnsAFreshMapEachCall()
  {
    // The whole reason these are methods rather than properties: PlayerStash and every dealer
    // hold their own, and a shared one would make one dealer's stock every dealer's.
    Dictionary<string, int> first = DrugCatalog.NewAmountMap();
    Dictionary<string, int> second = DrugCatalog.NewAmountMap();

    first["Cocaine"] = 99;

    Assert.AreNotSame(first, second);
    Assert.AreEqual(0, second["Cocaine"]);
  }

  [TestMethod]
  public void NewPriceMap_ReturnsAFreshMapEachCall()
  {
    Dictionary<string, int> first = DrugCatalog.NewPriceMap();
    Dictionary<string, int> second = DrugCatalog.NewPriceMap();

    first["Cocaine"] = 1;

    Assert.AreNotSame(first, second);
    Assert.AreEqual(DrugCatalog.Get("Cocaine").MarketValue, second["Cocaine"]);
  }

  [TestMethod]
  public void EveryDrug_HasAPriceAndASupplyWorthHaving()
  {
    Assert.IsNotEmpty(DrugCatalog.All);

    foreach (Drug drug in DrugCatalog.All)
    {
      Assert.IsPositive(drug.MarketValue, drug.Name + " is worthless");
      Assert.IsPositive(drug.SupplyBase, drug.Name + " never reaches the streets");
      Assert.IsNotEmpty(drug.SpriteDictionary, drug.Name + " has no texture dictionary");
      Assert.IsNotEmpty(drug.SpriteName, drug.Name + " has no icon");
    }
  }

  [TestMethod]
  public void MarketValueAndSupplyBase_RunInverseToEachOther()
  {
    // The whole economy in one property: cocaine is dear because it is scarce. Checked pairwise
    // rather than by sorting, so a drug added at the same price as an existing one does not fail
    // this for a tie it is allowed to have.
    foreach (Drug drug in DrugCatalog.All)
    {
      foreach (Drug other in DrugCatalog.All)
      {
        if (drug.MarketValue <= other.MarketValue)
        {
          continue;
        }

        Assert.IsLessThan(other.SupplyBase, drug.SupplyBase,
          $"{drug.Name} is dearer than {other.Name} but no scarcer");
      }
    }
  }
}
