using Los.Santos.Dope.Wars.Classes.Base;
using Los.Santos.Dope.Wars.Contracts;
using Los.Santos.Dope.Wars.Extension;
using Los.Santos.Dope.Wars.Persistence;
using System.Linq;

namespace Los.Santos.Dope.Wars.Classes
{
	/// <summary>
	/// The <see cref="DealerStash"/> class for dealer types, inherits from <see cref="Stash"/> and
	/// implements the members of the <see cref="IDealerStash"/> interface
	/// </summary>
	public class DealerStash : Stash, IDealerStash
	{
		#region ctor
		/// <summary>
		/// The standard constructor for the <see cref="DealerStash"/> class
		/// </summary>
		public DealerStash() => Init();
		#endregion

		#region IDealerStash members
		/// <inheritdoc/>
		public void BuyDrug(string drugName, int drugQuantity, int drugPrice)
		{
			AddToStash(drugName, drugQuantity);
			RemoveDrugMoney(drugPrice * drugQuantity);
		}
		/// <inheritdoc/>
		public void RefreshCurrentPrice(PlayerStats playerStats, GameSettings gameSettings, bool isDrugLord = false)
		{
			double difficultyFactor = Utils.GetDifficultFactor(gameSettings.GamePlaySettings.Difficulty);
			double discountFactor = Constants.DiscountPerLevel * playerStats.CurrentLevel;
			double marketVolatility = Constants.MarketVolatility;

			if (isDrugLord)
			{
				foreach (Drug? drug in Drugs)
				{
					double minPrice = (1 - (marketVolatility + discountFactor)) * drug.AveragePrice * difficultyFactor;
					drug.CurrentPrice = (int)minPrice;
				}
				return;
			}

			foreach (Drug? drug in Drugs)
			{
				double minPrice = (1 - (marketVolatility + discountFactor)) * drug.AveragePrice * difficultyFactor;
				double maxPrice = (1 + (marketVolatility + discountFactor)) * drug.AveragePrice * difficultyFactor;
				drug.CurrentPrice = Utils.GetRandomInt((int)minPrice, (int)maxPrice);
			}
		}
		/// <inheritdoc/>
		public void RefreshDrugMoney(PlayerStats playerStats, GameSettings gameSettings, bool isDrugLord = false)
		{
			double difficultyFactor = Utils.GetDifficultFactor(gameSettings.GamePlaySettings.Difficulty);
			int playerLevel = playerStats.CurrentLevel;

			double minMoney = playerLevel * (isDrugLord ? 10000 : 1000) * difficultyFactor / 2;
			double maxMoney = playerLevel * (isDrugLord ? 10000 : 1000) * difficultyFactor;

			AddDrugMoney(Utils.GetRandomInt((int)minMoney, (int)maxMoney));
		}
		/// <inheritdoc/>
		public void RestockQuantity(PlayerStats playerStats, GameSettings gameSettings, bool isDrugLord = false)
		{
			double difficultyFactor = Utils.GetDifficultFactor(gameSettings.GamePlaySettings.Difficulty);
			int playerLevel = playerStats.CurrentLevel;

			if (isDrugLord)
			{
				Init();
				
				var specialStash = Utils.GetLordStashByLevel(playerStats);
				if (specialStash.Count.Equals(1))
					return;

				int index = Utils.GetRandomInt(0, specialStash.Count);
				Drug drug = Drugs.Where(x => x.Name.Equals(specialStash[index])).FirstOrDefault();
				drug.Quantity = (int)(playerStats.MaxBagSize * difficultyFactor);
				return;
			}

			Init();

			var tradeStash = Utils.GetTradeableDrugs(playerStats);
			// this is just how many drugs from the drug range are seeded
			int drugsToRestock = Utils.GetRandomInt(1, tradeStash.Count + 1);
			for (int i = 0; i < drugsToRestock; i++)
			{
				// now we are picking randomly a drug from the tradeable stash
				int index = Utils.GetRandomInt(0, tradeStash.Count);
				// looking it up in the list
				var drug = Drugs.Where(x => x.Name.Equals(tradeStash[index])).FirstOrDefault();
				// and setting the new amount
				drug.Quantity = Utils.GetRandomInt((int)(playerLevel * difficultyFactor), (int)(playerLevel * difficultyFactor * 10 / 2) + 1);
			}
		}
		/// <inheritdoc/>
		public void SellDrug(string drugName, int drugQuantity, int drugPrice)
		{
			RemoveFromStash(drugName, drugQuantity);
			AddDrugMoney(drugPrice * drugQuantity);
		}
		#endregion
	}
}
