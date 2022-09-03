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
		/// The <see cref="NONE"/> drug type enum.
		/// </summary>		
		[DrugType("None", 0, "The none flag.")]
		NONE = 0,
		/// <summary>
		/// The <see cref="COKE"/> drug type enum.
		/// </summary>
		[DrugType("Cocaine", 2029, "Cocaine is a powerful stimulant and narcotic.")]
		COKE = 1,
		/// <summary>
		/// The <see cref="SMACK"/> drug type enum.
		/// </summary>
		[DrugType("Heroin", 1207, "Heroin is a semi-synthetic, strongly analgesic opioid.")]
		SMACK = 2,
		/// <summary>
		/// The <see cref="CANA"/> drug type enum.
		/// </summary>
		[DrugType("Marijuana", 283, "Marijuana is a psychoactive drug from the Cannabis plant")]
		CANA = 4,
		/// <summary>
		/// The <see cref="HASH"/> drug type enum.
		/// </summary>
		[DrugType("Hashish", 266, "Hashish refers to the resin extracted from the cannabis plant.")]
		HASH = 8,
		/// <summary>
		/// The <see cref="SHROOMS"/> drug type enum.
		/// </summary>
		[DrugType("Mushrooms", 245, "Psychoactive mushrooms, also known as magic mushrooms.")]
		SHROOMS = 16,
		/// <summary>
		/// The <see cref="SPEED"/> drug type enum.
		/// </summary>
		[DrugType("Amphetamine", 337, "Illegally trafficked amphetamine is also known as 'speed' or 'pep'.")]
		SPEED = 32,
		/// <summary>
		/// The <see cref="PCP"/> drug type enum.
		/// </summary>
		[DrugType("Angel Dust", 255, "Also known as PCP or Peace Pill in the drug scene.")]
		PCP = 64,
		/// <summary>
		/// The <see cref="METH"/> drug type enum.
		/// </summary>
		[DrugType("Methamphetamine", 2211, "On the black market, methamphetamine is also known as meth or crystal.")]
		METH = 128,
		/// <summary>
		/// The <see cref="KETA"/> drug type enum.
		/// </summary>
		[DrugType("Ketamine", 545, "Ketamine can greatly reduce the sensation of pain and cause unconsciousness.")]
		KETA = 256,
		/// <summary>
		/// The <see cref="PEYO"/> drug type enum.
		/// </summary>
		[DrugType("Mescaline", 470, "In terms of effect, mescaline is a typical hallucinogen, also called peyotl.")]
		PEYO = 512,
		/// <summary>
		/// The <see cref="XTC"/> drug type enum.
		/// </summary>
		[DrugType("Ecstasy", 231, "On the illegal market, Ecstasy, also XTC, is a term for so-called 'party pills'.")]
		XTC = 1024,
		/// <summary>
		/// The <see cref="LSD"/> drug type enum.
		/// </summary>
		[DrugType("Acid", 285, "LSD, or 'acid', is sold in the form of small 'cardboards' printed with various designs.")]
		LSD = 2048,
		/// <summary>
		/// The <see cref="MDMA"/> drug type enum.
		/// </summary>
		[DrugType("Molly", 315, "MDMA, also known as 'Molly', is a white or off-white powder or crystal, purer than XTC.")]
		MDMA = 4096,
		/// <summary>
		/// The <see cref="CRACK"/> drug type enum.
		/// </summary>
		[DrugType("Crack", 2078, "Crack and also known as 'rock', is a free base form of cocaine that can be smoked.")]
		CRACK = 8192,
		/// <summary>
		/// The <see cref="OXY"/> drug type enum.
		/// </summary>
		[DrugType("Oxycodone", 185, "A semi-synthetic opioid, highly addictive and a common drug of abuse.")]
		OXY = 16384
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
	/// The <see cref="DrugTypeAttribute"/> class.
	/// </summary>
	/// <remarks>
	/// Should only be used with the <see cref="DrugType"/> enum.
	/// Inherits from the <see cref="DescriptionAttribute"/> class.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	private sealed class DrugTypeAttribute : DescriptionAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DrugTypeAttribute"/> class.
		/// </summary>
		/// <remarks>
		/// Will throw an exception if <paramref name="name"/> is <see langword="null"/>.
		/// </remarks>
		/// <param name="name">The normal display name of the drug, cannot be <see langword="null"/>.</param>
		/// <param name="price">The normal market price of the drug.</param>
		/// <param name="description">The description of the drug, can be <see langword="null"/>.</param>
		public DrugTypeAttribute(string name, int price, string? description) : base(description)
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Price = price;
		}

		/// <summary>
		/// The <see cref="Name"/> property is the display name of the drug.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// The <see cref="Price"/> property is the normal market price of the drug.
		/// </summary>
		public int Price { get; private set; }
	}

	/// <summary>
	/// The extension method will try to get the <see cref="DescriptionAttribute.Description"/> of an enumerator of type <see cref="DrugType"/>.
	/// </summary>
	/// <remarks>
	/// If the enumerator has no <see cref="DrugTypeAttribute"/> or the <see cref="DescriptionAttribute.Description"/>
	/// is <see langword="null"/>, the name of the enumerator will be returned.
	/// </remarks>
	/// <typeparam name="T">The enumerator itself.</typeparam>
	/// <param name="enumValue">The enumerator itself.</param>
	/// <returns>The drug description or the name of the enumerator.</returns>
	public static string GetDescription<T>(this T enumValue) where T : Enum
	{
		FieldInfo? fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
		if (fieldInfo is not null)
		{
			DescriptionAttribute? attribute = fieldInfo.GetCustomAttribute(typeof(DrugTypeAttribute), true) as DescriptionAttribute;
			if (attribute is not null && attribute.Description is not null)
				return attribute.Description;
		}
		return enumValue.ToString();
	}

	/// <summary>
	/// The extension method will try to get the <see cref="DrugTypeAttribute.Price"/> of an enumerator of type <see cref="DrugType"/>.
	/// </summary>
	/// <remarks>
	/// If the enumerator has no <see cref="DrugTypeAttribute"/>, the value of 0 is returned.
	/// </remarks>
	/// <typeparam name="T">The enumerator itself.</typeparam>
	/// <param name="enumValue">The enumerator itself.</param>
	/// <returns>The drug price or the the value of 0.</returns>
	public static int GetPrice<T>(this T enumValue) where T : Enum
	{
		FieldInfo? fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
		if (fieldInfo is not null)
		{
			DrugTypeAttribute? attribute = fieldInfo.GetCustomAttribute(typeof(DrugTypeAttribute), false) as DrugTypeAttribute;
			if (attribute is not null)
				return attribute.Price;
		}
		return default;
	}

	/// <summary>
	/// The extension method will try to get the <see cref="DrugTypeAttribute.Name"/> of an enumerator of type <see cref="DrugType"/>.
	/// </summary>
	/// <remarks>
	/// If the enumerator has no <see cref="DrugTypeAttribute"/>, the name of the enumerator will be returned.
	/// </remarks>
	/// <typeparam name="T">The enumerator itself.</typeparam>
	/// <param name="enumValue">The enumerator itself.</param>
	/// <returns>The drug name or the name of the enumerator.</returns>
	public static string GetName<T>(this T enumValue) where T : Enum
	{
		FieldInfo? fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
		if (fieldInfo is not null)
		{
			DrugTypeAttribute? attribute = fieldInfo.GetCustomAttribute(typeof(DrugTypeAttribute), false) as DrugTypeAttribute;
			if (attribute is not null)
				return attribute.Name;
		}
		return enumValue.ToString();
	}

	/// <summary>
	/// The extension method should return a list of enumerators which are included in the enumerator flags.
	/// </summary>
	/// <typeparam name="T">The enumerator itself.</typeparam>
	/// <param name="enumFlags">The enumerator itself.</param>
	/// <returns>A list of enums out of the provided enum flags.</returns>
	public static List<T> FlagsToList<T>(this T enumFlags) where T : Enum
	{
		List<T> list = new();
		foreach (T flagToCheck in Enum.GetValues(typeof(T)))
			if (enumFlags.HasFlag(flagToCheck))
				list.Add(flagToCheck);
		return list;
	}

	/// <summary>
	/// The extension method should return a list of all enumerators of the given type of enumerator.
	/// </summary>
	/// <typeparam name="T">The enumerator itself.</typeparam>
	/// <param name="enumValue">The enumerator itself.</param>
	/// <returns>A list of all enumerators out of the provided enumerator.</returns>
	public static List<T> GetListFromEnum<T>(this T enumValue) where T : Enum
		=> Enum.GetValues(enumValue.GetType()).Cast<T>().ToList();
}
