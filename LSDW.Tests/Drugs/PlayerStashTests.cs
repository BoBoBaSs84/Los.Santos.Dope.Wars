using LSDW.Drugs;

namespace LSDW.Tests.Drugs;

/// <summary>
/// What the player carries: the two maps, what each mutator touches, and what it leaves alone.
/// </summary>
/// <remarks>
/// This was static state read during other types' static initialisation, which is why it was
/// never covered — a test could not get a clean one. It is an instance now, built per test
/// below. <c>MarketValue</c> and <c>DrugNames</c> stay static because they are catalog
/// projections, identical for every player and never written; the last two tests hold the
/// property the market passes rely on.
/// </remarks>
[TestClass]
public sealed class PlayerStashTests
{
  private const string Cocaine = "Cocaine";
  private const string Weed = "Weed";

  [TestMethod]
  public void NewStash_CarriesNothing()
  {
    PlayerStash stash = new();

    Assert.AreEqual(0, stash.Bag);
    Assert.IsEmpty(stash.TipsShown);
    Assert.HasCount(DrugCatalog.All.Count, stash.Drugs);

    foreach (string drug in PlayerStash.DrugNames)
    {
      Assert.AreEqual(0, stash.Drugs[drug], drug + " started with stock");
    }
  }

  [TestMethod]
  public void NewStash_HasTheStartingBagSize()
  {
    // Raised from here by RewardSystem on every level; 100 is the level-1 size.
    Assert.AreEqual(100, new PlayerStash().BagSize);
  }

  [TestMethod]
  public void NewStash_SeedsTheCostBasisFromTheCatalog()
  {
    // Not zero: a drug the player was given rather than bought still has to price a sale, and
    // the market value is the honest stand-in until they pay for one.
    PlayerStash stash = new();

    foreach (Drug drug in DrugCatalog.All)
    {
      Assert.AreEqual(drug.MarketValue, stash.BoughtPrice[drug.Name], drug.Name);
    }
  }

  [TestMethod]
  public void Add_RaisesTheAmount_AndLeavesTheCostBasis()
  {
    // The path for drugs the player did not pay for — a stolen van's haul.
    PlayerStash stash = new();
    int basis = stash.BoughtPrice[Cocaine];

    stash.Add(Cocaine, 4);
    stash.Add(Cocaine, 3);

    Assert.AreEqual(7, stash.Drugs[Cocaine]);
    Assert.AreEqual(basis, stash.BoughtPrice[Cocaine]);
  }

  [TestMethod]
  public void AddPurchased_RaisesTheAmount_AndRecordsWhatWasPaid()
  {
    PlayerStash stash = new();

    stash.AddPurchased(Cocaine, 5, pricePaid: 920);

    Assert.AreEqual(5, stash.Drugs[Cocaine]);
    Assert.AreEqual(920, stash.BoughtPrice[Cocaine]);
  }

  [TestMethod]
  public void AddPurchased_Twice_KeepsTheLatestPriceAsTheBasis()
  {
    // No averaging: the last price paid is the basis, which is the behaviour the sell-side
    // profit figure is written against.
    PlayerStash stash = new();

    stash.AddPurchased(Cocaine, 5, 920);
    stash.AddPurchased(Cocaine, 5, 700);

    Assert.AreEqual(10, stash.Drugs[Cocaine]);
    Assert.AreEqual(700, stash.BoughtPrice[Cocaine]);
  }

  [TestMethod]
  public void Remove_LowersTheAmount_AndLeavesTheCostBasisBehind()
  {
    // Deliberate: the sale that prompted the removal still has to read the basis afterwards to
    // work out its own margin.
    PlayerStash stash = new();
    stash.AddPurchased(Cocaine, 10, 920);

    stash.Remove(Cocaine, 4);

    Assert.AreEqual(6, stash.Drugs[Cocaine]);
    Assert.AreEqual(920, stash.BoughtPrice[Cocaine]);
  }

  [TestMethod]
  public void ClearCarried_EmptiesEveryDrug_AndLeavesEveryPrice()
  {
    // A death or an arrest. The prices survive because the player's dealings have not been
    // undone, only their bag.
    PlayerStash stash = new();
    stash.AddPurchased(Cocaine, 10, 920);
    stash.AddPurchased(Weed, 40, 12);

    stash.ClearCarried();

    Assert.AreEqual(0, stash.Bag);
    Assert.AreEqual(920, stash.BoughtPrice[Cocaine]);
    Assert.AreEqual(12, stash.BoughtPrice[Weed]);
  }

  [TestMethod]
  public void Restore_SetsBothOutright()
  {
    // The save loader's path: it does not add to what is there, it replaces it.
    PlayerStash stash = new();
    stash.AddPurchased(Cocaine, 10, 920);

    stash.Restore(Cocaine, 3, 500);

    Assert.AreEqual(3, stash.Drugs[Cocaine]);
    Assert.AreEqual(500, stash.BoughtPrice[Cocaine]);
  }

  [TestMethod]
  public void Bag_IsTheSumAcrossEveryDrug()
  {
    // Derived, not counted — the reason there is no stored count to fall out of step.
    PlayerStash stash = new();

    stash.Add(Cocaine, 4);
    stash.Add(Weed, 40);
    stash.Add("Meth", 6);
    stash.Remove(Weed, 10);

    Assert.AreEqual(40, stash.Bag);
  }

  [TestMethod]
  public void TwoStashes_DoNotShareTheirMaps()
  {
    // What DrugCatalog.NewAmountMap being a fresh map each call buys: the state is per
    // instance, so a script reload starts a player with an empty bag.
    PlayerStash first = new();
    PlayerStash second = new();

    first.AddPurchased(Cocaine, 10, 920);

    Assert.AreEqual(0, second.Drugs[Cocaine]);
    Assert.AreEqual(DrugCatalog.Get(Cocaine).MarketValue, second.BoughtPrice[Cocaine]);
  }

  [TestMethod]
  public void DrugNames_IsInCatalogOrder()
  {
    // Load-bearing: the supply distribution takes consecutive draws off the shared Random while
    // walking this list, so reordering it re-deals every market.
    Assert.AreSequenceEqual([.. DrugCatalog.All.Select(drug => drug.Name)], PlayerStash.DrugNames);
  }

  [TestMethod]
  public void MarketValue_IsTheCatalogsReferencePrice()
  {
    Assert.HasCount(DrugCatalog.All.Count, PlayerStash.MarketValue);

    foreach (Drug drug in DrugCatalog.All)
    {
      Assert.AreEqual(drug.MarketValue, PlayerStash.MarketValue[drug.Name], drug.Name);
    }
  }
}
