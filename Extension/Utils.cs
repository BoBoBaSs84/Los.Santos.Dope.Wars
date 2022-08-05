using GTA;
using GTA.Math;
using Los.Santos.Dope.Wars.Persistence.Settings;
using Los.Santos.Dope.Wars.Persistence.State;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using static Los.Santos.Dope.Wars.Constants;
using static Los.Santos.Dope.Wars.Enums;
using static Los.Santos.Dope.Wars.Statics;

namespace Los.Santos.Dope.Wars.Extension
{
	/// <summary>
	/// The <see cref="Utils"/> class holds a lot of static utils
	/// </summary>
	public static class Utils
	{
		#region fields
		private static readonly Random _random = new();
		#endregion

		#region public methods
		/// <summary>
		/// The <see cref="GetRandomPedHash(PedType)"/> method returns a random ped hash
		/// </summary>
		/// <param name="pedType"></param>
		/// <returns><see cref="PedHash"/></returns>
		public static PedHash GetRandomPedHash(PedType pedType)
		{
			return pedType switch
			{
				PedType.DrugLord => DrugLordPedHashes[GetRandomInt(DrugLordPedHashes.Count)],
				PedType.DrugDealer => DrugDealerPedHashes[GetRandomInt(DrugDealerPedHashes.Count)],
				PedType.Bodyguard => BodyguardPedHashes[GetRandomInt(BodyguardPedHashes.Count)],
				_ => PedHash.ArmGoon01GMM
			};
		}

		/// <summary>
		/// The <see cref="GetRandomWeaponHash(PedType)"/> method returns a random weapon hash
		/// </summary>
		/// <param name="pedType"></param>
		/// <returns><see cref="WeaponHash"/></returns>
		public static WeaponHash GetRandomWeaponHash(PedType pedType)
		{
			return pedType switch
			{
				PedType.DrugLord => DrugLordWeaponHashes[GetRandomInt(DrugLordWeaponHashes.Count)],
				PedType.DrugDealer => DrugDealerWeaponHashes[GetRandomInt(DrugDealerWeaponHashes.Count)],
				PedType.Bodyguard => BodyguardWeaponHashes[GetRandomInt(BodyguardWeaponHashes.Count)],
				_ => WeaponHash.Pistol
			};
		}

		/// <summary>
		/// The <see cref="GetWantedLevelByPlayerLevel(int)"/> method returns the wanted level in correlation to the player level
		/// </summary>
		/// <param name="playerLevel"></param>
		/// <returns><see cref="int"/></returns>
		public static int GetWantedLevelByPlayerLevel(int playerLevel)
		{
			if (playerLevel >= 40)
				return 4;
			if (playerLevel >= 25)
				return 3;
			if (playerLevel >= 10)
				return 2;
			if (playerLevel >= 0)
				return 1;
			return 1;
		}

		/// <summary>
		/// Returns the drug type enums for the drug lords
		/// </summary>
		/// <param name="playerStats"></param>
		/// <returns><see cref="List{T}"/></returns>
		public static List<DrugType> GetLordStashByLevel(PlayerStats playerStats)
		{
			if (playerStats.Reward.DrugLords.HasFlag(DrugLordStates.MaxedOut))
				return GetDrugEnumTypes(TradePackThree);
			else if (playerStats.Reward.DrugLords.HasFlag(DrugLordStates.Upgraded))
				return GetDrugEnumTypes(TradePackTwo);
			else
				return GetDrugEnumTypes(TradePackOne);
		}

		/// <summary>
		/// Returns <see cref="List{T}"/> of type <see cref="DrugType"/>
		/// </summary>
		/// <param name="drugTypes"></param>
		/// <returns><see cref="List{T}"/></returns>
		public static List<DrugType> GetDrugEnumTypes(DrugType drugTypes)
		{
			List<DrugType>? enumList = drugTypes.FlagsToList();
			enumList.Remove(DrugType.None);
			return enumList;
		}

		/// <summary>
		/// Get the current health and armor values for dealers
		/// </summary>
		/// <param name="dealerSettings">Can not be <see cref="Nullable"/></param>
		/// <param name="playerLevel"></param>
		/// <returns><see cref="Tuple{T1, T2}"/></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static (float health, float armor) GetDealerHealthArmor(Dealer dealerSettings, int playerLevel = 1)
		{
			float resultingHealth = dealerSettings.HealthBaseValue + playerLevel * DealerArmorHealthPerLevelFactor;
			float resultingArmor = dealerSettings.ArmorBaseValue + playerLevel * DealerArmorHealthPerLevelFactor;
			return (resultingHealth, resultingArmor);
		}

		/// <summary>
		/// Returns the difficult factor by the given <see cref="DifficultyTypes"/> enum
		/// </summary>
		/// <param name="difficulty"></param>
		/// <returns><see cref="double"/></returns>
		public static double GetDifficultFactor(DifficultyTypes difficulty)
		{
			return difficulty switch
			{
				DifficultyTypes.Easy => 1.1,
				DifficultyTypes.Normal => 1.0,
				DifficultyTypes.Hard => 0.9,
				_ => 1.0
			};
		}

		/// <summary>
		/// Returns the <see cref="Characters"/> of the currently played character
		/// </summary>
		/// <returns><see cref="Characters"/></returns>
		public static Characters GetCharacterFromModel()
		{
			return (PedHash)Game.Player.Character.Model switch
			{
				PedHash.Michael => Characters.Michael,
				PedHash.Franklin => Characters.Franklin,
				PedHash.Trevor => Characters.Trevor,
				_ => Characters.Unknown
			};
		}

		/// <summary>
		/// The <see cref="GetCurrentPlayerColor"/> returns the associated player color of type <see cref="Color"/>
		/// </summary>
		/// <returns><see cref="Color"/></returns>
		public static Color GetCurrentPlayerColor()
		{
			return (PedHash)Game.Player.Character.Model switch
			{
				PedHash.Franklin => Color.LimeGreen,
				PedHash.Michael => Color.SkyBlue,
				PedHash.Trevor => Color.SandyBrown,
				_ => Color.Black
			};
		}

		/// <summary>
		/// The <see cref="GetWarehousePositions"/> method returns the associated player position for the warehouse
		/// </summary>
		/// <returns><see cref="Tuple{T1, T2, T3}"/></returns>
		public static (Vector3 location, Vector3 entrance, Vector3 mission) GetWarehousePositions()
		{
			return (PedHash)Game.Player.Character.Model switch
			{
				PedHash.Franklin => (WarehouseLocationFranklin, WarehouseEntranceFranklin, WarehouseMissionMarkerFranklin),
				PedHash.Michael => (WarehouseLocationMichael, WarehouseEntranceMichael, WarehouseMissionMarkerMichael),
				PedHash.Trevor => (WarehouseLocationTrevor, WarehouseEntranceTrevor, WarehouseMissionMarkerTrevor),
				_ => (Vector3.Zero, Vector3.Zero, Vector3.Zero)
			};
		}

		/// <summary>
		/// Returns the <see cref="PlayerStats"/> of the currently played character
		/// </summary>
		/// <param name="gameState"></param>
		/// <returns><see cref="PlayerStats"/></returns>
		public static PlayerStats GetPlayerStatsFromModel(GameState gameState)
		{
			return (PedHash)Game.Player.Character.Model switch
			{
				PedHash.Franklin => gameState.Franklin,
				PedHash.Michael => gameState.Michael,
				PedHash.Trevor => gameState.Trevor,
				_ => new PlayerStats()
			};
		}

		/// <summary>
		/// Returns the <see cref="BlipColor"/> of the currently played character
		/// </summary>
		/// <param name="character"></param>
		/// <returns><see cref="BlipColor"/></returns>
		public static BlipColor GetCharacterBlipColor(Characters character)
		{
			return character switch
			{
				Characters.Michael => BlipColor.Michael,
				Characters.Franklin => BlipColor.Franklin,
				Characters.Trevor => BlipColor.Trevor,
				_ => BlipColor.White,
			};
		}

		/// <summary>
		/// The <see cref="SaveGameSettings(GameSettings)"/> method loads the game settings, if no saved settings are found, new default settings are created and saved
		/// </summary>
		/// <returns><see cref="Tuple{T1, T2}"/></returns>
		public static (bool successs, GameSettings loadedGameSettings) LoadGameSettings()
		{
			GameSettings gameSettings = new();
			try
			{
				if (File.Exists(GameSettingsFileName))
				{
					string readText = File.ReadAllText(GameSettingsFileName);
					gameSettings = readText.DeserializeFromFile<GameSettings>();
					if (gameSettings.Version.Equals(AssemblyVersion, StringComparison.Ordinal))
					{
						gameSettings.Version = AssemblyVersion;
						SaveGameSettings(gameSettings);
					}
				}
				else
				{
					SaveGameSettings(gameSettings);
				}
			}
			catch (Exception ex)
			{
				Logger.Error($"{ex.Message} - {ex.InnerException} - {ex.StackTrace}");
				return (false, gameSettings);
			}
			return (true, gameSettings);
		}

		/// <summary>
		/// The <see cref="SaveGameSettings(GameSettings)"/> method saves the game settings
		/// </summary>
		/// <param name="settings"></param>
		public static void SaveGameSettings(GameSettings settings)
		{
			(bool success, string returnString) = SerializeObjectToString(settings);
			if (success)
				File.WriteAllText(path: GameSettingsFileName, contents: returnString, encoding: Encoding.UTF8);
		}

		/// <summary>
		/// The <see cref="LoadGameState"/> method loads the game state, if no saved game state is found, a new game state is created and saved
		/// </summary>
		/// <returns><see cref="Tuple{T1, T2}"/></returns>
		public static (bool success, GameState loadedGameState) LoadGameState()
		{
			GameState gameState = new();
			try
			{
				if (File.Exists(GameStateFileName))
				{
					string readText = File.ReadAllText(GameStateFileName);
					gameState = readText.DeserializeFromFile<GameState>();
					if (!gameState.Version.Equals(AssemblyVersion, StringComparison.Ordinal))
					{
						gameState.Version = AssemblyVersion;
						SaveGameState(gameState);
					}
				}
				else
				{
					SaveGameState(gameState);
				}
			}
			catch (Exception ex)
			{
				Logger.Error($"{ex.Message} - {ex.InnerException} - {ex.StackTrace}");
				return (false, gameState);
			}
			return (true, gameState);
		}

		/// <summary>
		/// The <see cref="SaveGameState(GameState)"/> method saves the current game state also calls <see cref="Game.DoAutoSave"/>
		/// </summary>
		/// <param name="gameState"></param>
		public static void SaveGameState(GameState gameState)
		{
			(bool success, string returnString) = SerializeObjectToString(gameState);
			if (success)
			{
				File.WriteAllText(path: GameStateFileName, contents: returnString, encoding: Encoding.UTF8);
				Game.DoAutoSave();
			}
		}

		/// <summary>
		/// The <see cref="GetRandomDouble"/> method Returns a random floating-point number that is greater than or equal to 0.0 and less than 1.0.
		/// </summary>
		/// <returns><see cref="double"/></returns>
		public static double GetRandomDouble() => _random.NextDouble();

		/// <summary>
		/// Method gets a random integer
		/// </summary>
		/// <returns><see cref="int"/></returns>
		public static int GetRandomInt() => _random.Next();

		/// <summary>
		/// Method gets a random integer, within max range defined
		/// </summary>
		/// <param name="max"></param>
		/// <returns><see cref="int"/></returns>
		public static int GetRandomInt(int max) => _random.Next(max);

		/// <summary>
		/// Method gets a random integer, within min and max range defined
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns><see cref="int"/></returns>
		public static int GetRandomInt(int min, int max) => _random.Next(min, max);
		#endregion

		#region private methods
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="toDeserialize"></param>
		/// <param name="decompress"></param>
		/// <returns></returns>
		private static T DeserializeFromFile<T>(this string toDeserialize, bool decompress = false)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(toDeserialize))
					throw new ArgumentNullException(nameof(toDeserialize));

				XmlSerializer xmlSerializer = new(typeof(T));
				XmlReaderSettings xmlReaderSettings = new()
				{
					CheckCharacters = true
				};

				if (decompress)
					toDeserialize = Compressor.DecompressString(toDeserialize);

				using StringReader stringReader = new(toDeserialize);
				using XmlReader xmlReader = XmlReader.Create(stringReader, xmlReaderSettings);

				if (!xmlSerializer.CanDeserialize(xmlReader))
					return default!;

				return (T)xmlSerializer.Deserialize(xmlReader);
			}
			catch (Exception ex)
			{
				Logger.Error($"{ex.Message} - {ex.InnerException} - {ex.StackTrace}");
			}
			return default!;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="toSerialize"></param>
		/// <param name="compress"></param>
		/// <returns><see cref="Tuple{T1, T2}"/></returns>
		private static (bool success, string returnString) SerializeObjectToString<T>(this T toSerialize, bool compress = false)
				where T : class
		{
			string stringToReturn = string.Empty;

			XmlSerializer xmlSerializer = new(typeof(T));
			XmlWriterSettings xmlWriterSettings = new()
			{
				CheckCharacters = true,
				Encoding = Encoding.UTF8,
				Indent = false,
				OmitXmlDeclaration = true,
				NewLineHandling = NewLineHandling.None
			};
			try
			{
				using StringWriter stringWriter = new();
				using XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings);

				xmlSerializer.Serialize(xmlWriter, toSerialize);
				stringToReturn = stringWriter.ToString();

				if (compress)
					stringToReturn = Compressor.CompressString(stringToReturn);
			}
			catch (Exception ex)
			{
				Logger.Error($"{ex.Message} - {ex.InnerException} - {ex.StackTrace}");
				return (false, stringToReturn);
			}
			return (true, stringToReturn);
		}
		#endregion
	}
}
