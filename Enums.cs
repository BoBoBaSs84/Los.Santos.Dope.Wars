using System.ComponentModel;
using System.Reflection;

namespace Los.Santos.Dope.Wars;

/// <summary>
/// The <see cref="Enums"/> class holds all the required enums
/// </summary>
public static class Enums
{
	/// <summary>
	/// The <see cref="LogLevels"/> enum flags.
	/// </summary>
	[Flags]
	public enum LogLevels
	{
		/// <summary>
		/// The <see cref="None"/> log level.
		/// </summary>
		[Description("Not used for writing log messages. Specifies that a logging category should not write any messages.")]
		None = 0,
		/// <summary>
		/// The <see cref="Trace"/> log level.
		/// </summary>
		[Description("Logs that contain the most detailed messages. These messages may contain sensitive application data. These messages are disabled by default and should never be enabled in a production environment.")]
		Trace = 1,
		/// <summary>
		/// The <see cref="Debug"/> log level.
		/// </summary>
		[Description("Logs that are used for interactive investigation during development. These logs should primarily contain information useful for debugging and have no long-term value.")]
		Debug = 2,
		/// <summary>
		/// The <see cref="Information"/> log level.
		/// </summary>
		[Description("Logs that track the general flow of the application. These logs should have long-term value.")]
		Information = 4,
		/// <summary>
		/// The <see cref="Warning"/> log level.
		/// </summary>
		[Description("Logs that highlight an abnormal or unexpected event in the application flow, but do not otherwise cause the application execution to stop.")]
		Warning = 8,
		/// <summary>
		/// The <see cref="Error"/> log level.
		/// </summary>
		[Description("Logs that highlight when the current flow of execution is stopped due to a failure. These should indicate a failure in the current activity, not an application-wide failure.")]
		Error = 16,
		/// <summary>
		/// The <see cref="Critical"/> log level.
		/// </summary>
		[Description("Logs that describe an unrecoverable application or system crash, or a catastrophic failure that requires immediate attention.")]
		Critical = 32
	}

	/// <summary>
	/// The <see cref="Character"/> enum.
	/// </summary>
	public enum Character
	{
		/// <summary>
		/// The <see cref="Unknown"/> character enum.
		/// </summary>
		Unknown = -1,
		/// <summary>
		/// The <see cref="Michael"/> character enum for Michael.
		/// </summary>
		Michael = 0,
		/// <summary>
		/// The <see cref="Franklin"/> character enum for Franklin.
		/// </summary>			
		Franklin = 1,
		/// <summary>
		/// The <see cref="Trevor"/> character enum for Trevor.
		/// </summary>
		Trevor = 2
	}

	/// <summary>
	/// The <see cref="DifficultyType"/> enum.
	/// </summary>
	public enum DifficultyType
	{
		/// <summary>
		/// The <see cref="Easy"/> difficult level enum.
		/// </summary>
		Easy = -1,
		/// <summary>
		/// The <see cref="Normal"/> difficult level enum.
		/// </summary>
		Normal = 0,
		/// <summary>
		/// The <see cref="Hard"/> difficult level enum.
		/// </summary>
		Hard = 1
	}

	/// <summary>
	/// The <see cref="DrugType"/> enum.
	/// </summary>
	[Flags]
	public enum DrugType
	{
		/// <summary>
		/// The <see cref="None"/> drug type enum.
		/// </summary>		
		[Description("The none flag.")]
		[DrugPrice(0)]
		None = 0,
		/// <summary>
		/// The <see cref="Cocaine"/> drug type enum.
		/// </summary>
		[Description("Cocaine is a powerful stimulant and narcotic.")]
		[DrugPrice(865)]
		Cocaine = 1,
		/// <summary>
		/// The <see cref="Heroin"/> drug type enum.
		/// </summary>
		[Description("Heroin is a semi-synthetic, strongly analgesic opioid.")]
		[DrugPrice(895)]
		Heroin = 2,
		/// <summary>
		/// The <see cref="Marijuana"/> drug type enum.
		/// </summary>
		[Description("Marijuana is a psychoactive drug from the Cannabis plant")]
		[DrugPrice(165)]
		Marijuana = 4,
		/// <summary>
		/// The <see cref="Hashish"/> drug type enum.
		/// </summary>
		[Description("Hashish refers to the resin extracted from the cannabis plant.")]
		[DrugPrice(125)]
		Hashish = 8,
		/// <summary>
		/// The <see cref="Mushrooms"/> drug type enum.
		/// </summary>
		[Description("Psychoactive mushrooms, also known as magic mushrooms.")]
		[DrugPrice(245)]
		Mushrooms = 16,
		/// <summary>
		/// The <see cref="Amphetamine"/> drug type enum.
		/// </summary>
		[Description("Amphetamine has a strong stimulating and uplifting effect.")]
		[DrugPrice(215)]
		Amphetamine = 32,
		/// <summary>
		/// The <see cref="PCP"/> drug type enum.
		/// </summary>
		[Description("Also known as Angel Dust or Peace Pill in the drug scene.")]
		[DrugPrice(255)]
		PCP = 64,
		/// <summary>
		/// The <see cref="Methamphetamine"/> drug type enum.
		/// </summary>
		[Description("Methamphetamine is a powerful psychostimulant.")]
		[DrugPrice(785)]
		Methamphetamine = 128,
		/// <summary>
		/// The <see cref="Ketamine"/> drug type enum.
		/// </summary>
		[Description("Ketamine is a dissociative anaesthetic used in human medicine.")]
		[DrugPrice(545)]
		Ketamine = 256,
		/// <summary>
		/// The <see cref="Mescaline"/> drug type enum.
		/// </summary>
		[Description("Mescaline or mescaline is a psychedelic and hallucinogenic alkaloid.")]
		[DrugPrice(470)]
		Mescaline = 512,
		/// <summary>
		/// The <see cref="Ecstasy"/> drug type enum.
		/// </summary>
		[Description("Ecstasy, also XTC, is a term for so-called 'party pills'.")]
		[DrugPrice(275)]
		Ecstasy = 1024,
		/// <summary>
		/// The <see cref="Acid"/> drug type enum.
		/// </summary>
		[Description("Acid, also known as LSD, is one of the strongest known hallucinogens.")]
		[DrugPrice(265)]
		Acid = 2048,
		/// <summary>
		/// The <see cref="MDMA"/> drug type enum.
		/// </summary>
		[Description("MDMA is particularly known as a party drug that is widely used worldwide.")]
		[DrugPrice(315)]
		MDMA = 4096,
		/// <summary>
		/// The <see cref="Crack"/> drug type enum.
		/// </summary>
		[Description("Crack is a drug made from cocaine salt and sodium bicarbonate.")]
		[DrugPrice(615)]
		Crack = 8192,
		/// <summary>
		/// The <see cref="Oxycodone"/> drug type enum.
		/// </summary>
		[DrugPrice(185)]
		[Description("A semi-synthetic opioid, highly addictive and a common drug of abuse.")]
		Oxycodone = 16384
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
	/// <summary>
	/// The <see cref="PedType"/> enums
	/// </summary>
	public enum PedType
	{
		/// <summary>
		/// The <see cref="DrugDealer"/> ped type
		/// </summary>
		DrugDealer = 1,
		/// <summary>
		/// The <see cref="DrugLord"/> ped type
		/// </summary>
		DrugLord = 2,
		/// <summary>
		/// The <see cref="Bodyguard"/> ped type
		/// </summary>
		Bodyguard = 3
	}
	/// <summary>
	/// The <see cref="WarehouseMissionStates"/> enums
	/// </summary>
	public enum WarehouseMissionStates
	{
		/// <summary>
		/// The <see cref="NotStarted"/> enum
		/// </summary>
		NotStarted = 0,
		/// <summary>
		/// The <see cref="Started"/> enum
		/// </summary>
		Started = 1,
		/// <summary>
		/// The <see cref="VanDelivered"/> enum
		/// </summary>			
		VanStolen = 2,
		/// <summary>
		/// The <see cref="NotStarted"/> enum
		/// </summary>
		VanDelivered = 3,
		/// <summary>
		/// The <see cref="Aborted"/> enum
		/// </summary>
		Aborted = 4
	}

	/// <summary>
	/// The <see cref="DrugPriceAttribute"/> class.
	/// </summary>
	/// <remarks>
	/// Should only be used with the <see cref="DrugType"/> enum.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Field)]
	private class DrugPriceAttribute : Attribute
	{
		public int Price { get; private set; }

		public DrugPriceAttribute(int price) => Price = price;
	}

	/// <summary>
	/// The <see cref="GetDescription{T}(T)"/> extension method will try to get the <see cref="DescriptionAttribute"/> of an enum if used
	/// </summary>
	/// <remarks>
	/// If the enum has no description attribute, the enum name will be returned.
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	/// <param name="enumValue"></param>
	/// <returns>The Description of type <see cref="string"/> or the name of the enum.</returns>
	public static string GetDescription<T>(this T enumValue) where T : struct, IConvertible
	{
		FieldInfo? fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
		if (fieldInfo is not null)
		{
			DescriptionAttribute[]? attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
			if (attributes is not null && attributes.Length > 0)
				return attributes[0].Description;
		}
		return enumValue.ToString();
	}

	/// <summary>
	/// The <see cref="GetPrice{T}(T)"/> method will try to get the <see cref="DrugPriceAttribute"/> of an enum if used.
	/// </summary>
	/// <remarks>
	/// If the enum has no drug price attribute, the value of 0 will be returned.
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	/// <param name="enumValue"></param>
	/// <returns>The drug price of type <see cref="int"/> or the the value of 0.</returns>
	public static int GetPrice<T>(this T enumValue) where T : struct, IConvertible
	{
		FieldInfo? fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
		if (fieldInfo is not null)
		{
			DrugPriceAttribute[]? attributes = fieldInfo.GetCustomAttributes(typeof(DrugPriceAttribute), false) as DrugPriceAttribute[];
			if (attributes is not null && attributes.Length > 0)
				return attributes[0].Price;
		}
		return default;
	}

	/// <summary>
	/// The <see cref="FlagsToList{T}(T)"/> extension method returns a <see cref="List{T}"/>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="enumFlags"></param>
	/// <returns>A <see cref="List{T}"/> of the provided enum flags.</returns>
	public static List<T> FlagsToList<T>(this T enumFlags) where T : Enum, IConvertible
	{
		List<T> list = new();
		foreach (T flagToCheck in Enum.GetValues(typeof(T)))
			if (enumFlags.HasFlag(flagToCheck))
				list.Add(flagToCheck);
		return list;
	}

	/// <summary>
	/// The <see cref="GetListFromEnum{T}(T)"/> method should return a list of all enumerators of the given type of enum.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns>A <see cref="List{T}"/> of the provided enum.</returns>
	public static List<T> GetListFromEnum<T>(this T @enum) where T : Enum
		=> Enum.GetValues(@enum.GetType()).Cast<T>().ToList();

	/// <summary>
	/// The <see cref="GetEnumsWithDescription{T}(T)"/> method should return a dictornary with enums and their description.
	/// </summary>
	/// <remarks>
	/// If the enum has no description property, the enum name will be returned.
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	/// <param name="enum"></param>
	/// <returns>A dictionary with enums and their description.</returns>
	public static Dictionary<T, string> GetEnumsWithDescription<T>(this T @enum) where T : struct, IConvertible
	{
		List<T> enumList = Enum.GetValues(@enum.GetType()).Cast<T>().ToList();
		Dictionary<T, string> dictToReturn = new();
		foreach (T e in enumList)
			dictToReturn.Add(e, e.GetDescription());
		return dictToReturn;
	}
}