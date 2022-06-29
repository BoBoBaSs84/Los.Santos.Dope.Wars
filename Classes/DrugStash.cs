using Los.Santos.Dope.Wars.Contracts;
using Los.Santos.Dope.Wars.Extension;
using Los.Santos.Dope.Wars.Persistence;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Los.Santos.Dope.Wars.Classes
{
	/// <summary>
	/// The <see cref="DrugStash"/> class is the root element for the drug stash of the player or dealer or, or, or
	/// </summary>
	[XmlRoot(ElementName = nameof(DrugStash), IsNullable = false)]
	public class DrugStash : IDrugStash
	{
		#region ctor
		/// <summary>
		/// Empty constructor, default values
		/// </summary>
		public DrugStash()
		{
			Money = default;
			Drugs = new();
		}
		#endregion

		#region properties
		/// <inheritdoc/>
		[XmlElement(ElementName = "Drug", IsNullable = false)]
		public List<Drug> Drugs { get; set; }

		/// <inheritdoc/>
		[XmlAttribute(AttributeName = nameof(Money))]
		public int Money { get; set; }
		#endregion

		#region IDrugStash members
		/// <inheritdoc/>
		public void Init()
		{
			if (!Drugs.Count.Equals(0))
				Drugs.Clear();

			List<Drug>? drugList = AvailableDrugs;

			foreach (Drug? drug in drugList)
				Drugs.Add(new Drug(drug.Name, drug.Description, drug.AveragePrice));
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

			Money = Utils.GetRandomInt((int)minMoney, (int)maxMoney);
		}

        /// <inheritdoc/>
        public void RestockQuantity(PlayerStats playerStats, GameSettings gameSettings, bool isDrugLord = false)
        {
            double difficultyFactor = Utils.GetDifficultFactor(gameSettings.GamePlaySettings.Difficulty);
            int playerLevel = playerStats.CurrentLevel;

			if (isDrugLord)
			{
				foreach (Drug? drug in Drugs)
					drug.Quantity = 0;

				int index = Utils.GetRandomInt(0, Drugs.Count);
				Drugs[index].Quantity = (int)(playerStats.MaxBagSize * difficultyFactor);

				return;
			}

			foreach (Drug? drug in Drugs)
				drug.Quantity = 0;

			int drugsToRestock = Utils.GetRandomInt(1, Drugs.Count + 1);

			for (int i = 0; i < drugsToRestock; i++)
			{
				int index = Utils.GetRandomInt(0, Drugs.Count);
				Drugs[index].Quantity = Utils.GetRandomInt((int)(playerLevel * difficultyFactor), (int)(playerLevel * difficultyFactor * 10 / 2) + 1);
			}
		}
		#endregion

		private static readonly List<Drug> AvailableDrugs = new()
		{
			new Drug(Enums.DrugTypes.Cocaine, "Cocaine is a powerful stimulant and narcotic.", 865),
			new Drug(Enums.DrugTypes.Heroin, "Heroin is a semi-synthetic, strongly analgesic opioid.", 895),
			new Drug(Enums.DrugTypes.Marijuana, "Marijuana is a psychoactive drug from the Cannabis plant", 165),
			new Drug(Enums.DrugTypes.Hashish, "Hashish refers to the resin extracted from the cannabis plant.", 125),
			new Drug(Enums.DrugTypes.Mushrooms, "Psychoactive mushrooms, also known as magic mushrooms.", 245),
			new Drug(Enums.DrugTypes.Amphetamine, "Amphetamine has a strong stimulating and uplifting effect.", 215),
			new Drug(Enums.DrugTypes.PCP, "Also known as Angel Dust or Peace Pill in the drug scene.", 255),
			new Drug(Enums.DrugTypes.Methamphetamine, "Methamphetamine is a powerful psychostimulant.", 835),
			new Drug(Enums.DrugTypes.Ketamine, "Ketamine is a dissociative anaesthetic used in human medicine.", 545),
			new Drug(Enums.DrugTypes.Mescaline, "Mescaline or mescaline is a psychedelic and hallucinogenic alkaloid.", 470),
			new Drug(Enums.DrugTypes.Ecstasy, "Ecstasy, also XTC, is a term for so-called 'party pills'.", 275),
			new Drug(Enums.DrugTypes.Acid, "Acid, also known as LSD, is one of the strongest known hallucinogens.", 265),
			new Drug(Enums.DrugTypes.MDMA, "MDMA is particularly known as a party drug that is widely used worldwide.", 315),
			new Drug(Enums.DrugTypes.Crack, "Crack is a drug made from cocaine salt and sodium bicarbonate.", 615),
			new Drug(Enums.DrugTypes.Oxycodone, "A semi-synthetic opioid, highly addictive and a common drug of abuse.", 185)
		};
	}
}
