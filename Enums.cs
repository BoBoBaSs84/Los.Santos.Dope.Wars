using System;

namespace Los.Santos.Dope.Wars
{
	/// <summary>
	/// The <see cref="Enums"/> class holds all the required enums
	/// </summary>
	public static class Enums
	{
        /// <summary>
        /// The <see cref="LogLevels"/> enums
        /// </summary>
        [Flags]
		public enum LogLevels
		{
			/// <summary>
			/// The <see cref="Trace"/> level
			/// </summary>
			Trace = 0,
			/// <summary>
			/// The <see cref="Status"/> level
			/// </summary>
			Status = 1,
			/// <summary>
			/// The <see cref="Warning"/> level
			/// </summary>			
			Warning = 2,
			/// <summary>
			/// The <see cref="Error"/> level
			/// </summary>
			Error = 4,
			/// <summary>
			/// The <see cref="Panic"/> level
			/// </summary>
			Panic = 8
		}
		
		/// <summary>
		/// The <see cref="Characters"/> enums
		/// </summary>
		public enum Characters
		{
			/// <summary>
			/// The <see cref="Unknown"/> enum
			/// </summary>
			Unknown = -1,
			/// <summary>
			/// The <see cref="Michael"/> enum for Michael
			/// </summary>
			Michael = 0,
			/// <summary>
			/// The <see cref="Franklin"/> enum for Franklin
			/// </summary>			
			Franklin = 1,
			/// <summary>
			/// The <see cref="Trevor"/> enum for Trevor
			/// </summary>
			Trevor = 2
		}

		/// <summary>
		/// The <see cref="DifficultyTypes"/> enums
		/// </summary>
		public enum DifficultyTypes
		{
			/// <summary>
			/// The <see cref="Easy"/> difficult level
			/// </summary>
			Easy = -1,
			/// <summary>
			/// The <see cref="Normal"/> difficult level
			/// </summary>
			Normal = 0,
			/// <summary>
			/// The <see cref="Hard"/> difficult level
			/// </summary>
			Hard = 1
		}

		/// <summary>
		/// The <see cref="DrugTypes"/> enums
		/// </summary>
		[Flags]
		public enum DrugTypes
		{
			/// <summary>
			/// The <see cref="None"/> type
			/// </summary>
			None = 0,
			/// <summary>
			/// The <see cref="Cocaine"/> type
			/// </summary>
			Cocaine = 1,
			/// <summary>
			/// The <see cref="Heroin"/> type
			/// </summary>
			Heroin = 2,
			/// <summary>
			/// The <see cref="Marijuana"/> type
			/// </summary>			
			Marijuana = 4,
			/// <summary>
			/// The <see cref="Hashish"/> type
			/// </summary>
			Hashish = 8,
			/// <summary>
			/// The <see cref="Mushrooms"/> type
			/// </summary>
			Mushrooms = 16,
			/// <summary>
			/// The <see cref="Amphetamine"/> type
			/// </summary>
			Amphetamine = 32,
			/// <summary>
			/// The <see cref="PCP"/> type
			/// </summary>
			PCP = 64,
			/// <summary>
			/// The <see cref="Methamphetamine"/> type
			/// </summary>
			Methamphetamine = 128,
			/// <summary>
			/// The <see cref="Ketamine"/> type
			/// </summary>
			Ketamine = 256,
			/// <summary>
			/// The <see cref="Mescaline"/> type
			/// </summary>
			Mescaline = 512,
			/// <summary>
			/// The <see cref="Ecstasy"/> type
			/// </summary>
			Ecstasy = 1024,
			/// <summary>
			/// The <see cref="Acid"/> type
			/// </summary>
			Acid = 2048,
			/// <summary>
			/// The <see cref="MDMA"/> type
			/// </summary>
			MDMA = 4096,
			/// <summary>
			/// The <see cref="Crack"/> type
			/// </summary>
			Crack = 8192,
			/// <summary>
			/// The <see cref="Oxycodone"/> type
			/// </summary>
			Oxycodone = 16384,
			/// <summary>
			/// This is the level one collection of a drug lord
			/// </summary>
			LordStashLevelOne = Mushrooms | Amphetamine | Oxycodone | Marijuana | Hashish,
			/// <summary>
			/// This is the level two collection of a drug lord
			/// </summary>
			LordStashLevelTwo = Mescaline | MDMA | Ecstasy | Acid | PCP,
			/// <summary>
			/// This is the level three collection of a drug lord
			/// </summary>
			LordStashLevelThree = Heroin | Cocaine | Methamphetamine | Crack | Ketamine,
			/// <summary>
			/// This drug collection is the first the player is able to trade with from level 1 on
			/// </summary>
			TradePackOne = LordStashLevelOne,
			/// <summary>
			/// This drug collection is the second the player is able to trade with from level 11 on
			/// </summary>
			TradePackTwo = LordStashLevelTwo,
			/// <summary>
			/// This drug collection is the third the player is able to trade with from level 20 on
			/// </summary>
			TradePackThree = LordStashLevelThree,
		}

		/// <summary>
		/// The <see cref="WarehouseStates"/> enums
		/// </summary>
		[Flags]
		public enum WarehouseStates
		{
			/// <summary>
			/// The <see cref="Locked"/> state
			/// </summary>
			Locked = 0,
			/// <summary>
			/// The <see cref="Unlocked"/> state
			/// </summary>
			Unlocked = 1,
			/// <summary>
			/// The <see cref="Bought"/> state
			/// </summary>
			Bought = 2,
			/// <summary>
			/// The <see cref="UpgradeUnlocked"/> state
			/// </summary>
			UpgradeUnlocked = 4,
			/// <summary>
			/// The <see cref="Upgraded"/> state
			/// </summary>
			Upgraded = 8
		}

		/// <summary>
		/// The <see cref="DrugLordStates"/> enums
		/// </summary>
		[Flags]
		public enum DrugLordStates
		{
			/// <summary>
			/// The <see cref="Locked"/> state
			/// </summary>
			Locked = 0,
			/// <summary>
			/// The <see cref="Unlocked"/> state
			/// </summary>
			Unlocked = 1,
			/// <summary>
			/// The <see cref="Upgraded"/> state
			/// </summary>
			Upgraded = 2,
			/// <summary>
			/// The <see cref="MaxedOut"/> state
			/// </summary>
			MaxedOut = 4
		}
	}
}