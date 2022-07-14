using GTA;
using Los.Santos.Dope.Wars.Persistence.Settings;
using Los.Santos.Dope.Wars.Persistence.State;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Los.Santos.Dope.Wars.Extension
{
	/// <summary>
	/// The <see cref="Utils"/> class holds a lot of static utils
	/// </summary>
	public static class Utils
	{
		#region public methods
		/// <summary>
		/// Returns the drug type enums for the drug lords
		/// </summary>
		/// <param name="playerStats"></param>
		/// <returns><see cref="List{T}"/></returns>
		public static List<Enum> GetLordStashByLevel(PlayerStats playerStats)
		{
			if (playerStats.Reward.DrugLords.HasFlag(Enums.DrugLordStates.MaxedOut))
				return GetDrugEnumTypes(Constants.TradePackThree);
			else if (playerStats.Reward.DrugLords.HasFlag(Enums.DrugLordStates.Upgraded))
				return GetDrugEnumTypes(Constants.TradePackTwo);
			else
				return GetDrugEnumTypes(Constants.TradePackOne);
		}

		/// <summary>
		/// Returns <see cref="List{T}"/> of type <see cref="Enums.DrugTypes"/>
		/// </summary>
		/// <param name="drugTypes"></param>
		/// <returns><see cref="List{T}"/></returns>
		public static List<Enum> GetDrugEnumTypes(Enums.DrugTypes drugTypes)
		{
			var enumList = Enum.GetValues(drugTypes.GetType()).Cast<Enum>().Where(drugTypes.HasFlag).ToList();
			enumList.Remove(Enums.DrugTypes.None);
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
			try
			{
				float resultingHealth = dealerSettings.HealthBaseValue + playerLevel * Constants.DealerArmorHealthPerLevelFactor;
				float resultingArmor = dealerSettings.ArmorBaseValue + playerLevel * Constants.DealerArmorHealthPerLevelFactor;
				return (resultingHealth, resultingArmor);
			}
			catch (Exception ex)
			{
				Logger.Error($"{ex.Message} - {ex.InnerException} - {ex.StackTrace}");
			}
			return (100f, 100f);
		}

		/// <summary>
		/// Returns the difficult factor by the given <see cref="Enums.DifficultyTypes"/> enum
		/// </summary>
		/// <param name="difficulty"></param>
		/// <returns><see cref="double"/></returns>
		public static double GetDifficultFactor(Enums.DifficultyTypes difficulty)
		{
			return difficulty switch
			{
				Enums.DifficultyTypes.Easy => 1.1,
				Enums.DifficultyTypes.Normal => 1.0,
				Enums.DifficultyTypes.Hard => 0.9,
				_ => 1.0
			};
		}

		/// <summary>
		/// Returns the <see cref="Enums.Characters"/> of the currently played character
		/// </summary>
		/// <returns><see cref="Enums.Characters"/></returns>
		public static Enums.Characters GetCharacterFromModel()
		{
			return (PedHash)Game.Player.Character.Model switch
			{
				PedHash.Michael => Enums.Characters.Michael,
				PedHash.Franklin => Enums.Characters.Franklin,
				PedHash.Trevor => Enums.Characters.Trevor,
				_ => Enums.Characters.Unknown
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
				PedHash.Franklin => Color.FromArgb(171, 237, 171),
				PedHash.Michael => Color.FromArgb(101, 180, 212),
				PedHash.Trevor => Color.FromArgb(244, 164, 96),
				_ => Color.Black
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
		public static BlipColor GetCharacterBlipColor(Enums.Characters character)
		{
			return character switch
			{
				Enums.Characters.Michael => BlipColor.Michael,
				Enums.Characters.Franklin => BlipColor.Franklin,
				Enums.Characters.Trevor => BlipColor.Trevor,
				_ => BlipColor.White,
			};
		}

		/// <summary>
		/// Method loads the game settings, if no saved settings are found, new default settings are created and saved
		/// </summary>
		/// <returns><see cref="Tuple{T1, T2}"/></returns>
		public static (bool successs, GameSettings loadedGameSettings) LoadGameSettings()
		{
			GameSettings gameSettings = new();
			try
			{
				if (File.Exists(Constants.GameSettingsFileName))
				{
					string readText = File.ReadAllText(Constants.GameSettingsFileName);
					gameSettings = readText.DeserializeFromFile<GameSettings>();
					if (gameSettings.Version.Equals(Constants.AssemblyVersion))
					{
						gameSettings.Version = Constants.AssemblyVersion;
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
		/// Method saves the game settings
		/// </summary>
		/// <param name="settings"></param>
		public static void SaveGameSettings(GameSettings settings)
		{
			(bool success, string returnString) = SerializeObjectToString(settings);
			if (success)
				File.WriteAllText(
						path: Constants.GameSettingsFileName,
						contents: returnString,
						encoding: Encoding.UTF8
						);
		}

		/// <summary>
		/// Method loads the game state, if no saved game state is found, a new game state is created and saved
		/// </summary>
		/// <returns><see cref="Tuple{T1, T2}"/></returns>
		public static (bool success, GameState loadedGameState) LoadGameState()
		{
			GameState gameState = new();
			try
			{
				if (File.Exists(Constants.GameStateFileName))
				{
					string readText = File.ReadAllText(Constants.GameStateFileName);
					gameState = readText.DeserializeFromFile<GameState>();
					if (!gameState.Version.Equals(Constants.AssemblyVersion))
					{
						gameState.Version = Constants.AssemblyVersion;
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
		/// Method saves the game state
		/// </summary>
		/// <param name="gameState"></param>
		public static void SaveGameState(GameState gameState)
		{
			(bool success, string returnString) = SerializeObjectToString(gameState);
			if (success)
				File.WriteAllText(
						path: Constants.GameStateFileName,
						contents: returnString,
						encoding: Encoding.UTF8
						);
		}

		/// <summary>
		/// Method gets a random integer
		/// </summary>
		/// <returns><see cref="int"/></returns>
		public static int GetRandomInt() => Constants.random.Next();

		/// <summary>
		/// Method gets a random integer, within max range defined
		/// </summary>
		/// <param name="max"></param>
		/// <returns><see cref="int"/></returns>
		public static int GetRandomInt(int max) => Constants.random.Next(max);

		/// <summary>
		/// Method gets a random integer, within min and max range defined
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns><see cref="int"/></returns>
		public static int GetRandomInt(int min, int max) => Constants.random.Next(min, max);
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
