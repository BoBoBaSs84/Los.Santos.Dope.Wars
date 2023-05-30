using System.Xml.Serialization;

namespace LSDW.Constants;

/// <summary>
/// The xml constants class.
/// </summary>
internal static class XmlConstants
{
	/// <summary>
	/// The name spaces class.
	/// </summary>
	internal static class NameSpaces
	{
		internal const string GameStateNameSpace = "https://github.com/BoBoBaSs84/Los.Santos.Dope.Wars";
		internal const string GameStateNameSpacePreFix = "lsdw";
		internal const string DealerStateNameSpace = "https://github.com/BoBoBaSs84/Los.Santos.Dope.Wars/Dealer";
		internal const string DealerStateNameSpacePreFix = "lsdw-de";
		internal const string PlayerStateNameSpace = "https://github.com/BoBoBaSs84/Los.Santos.Dope.Wars/Player";
		internal const string PlayerStateNameSpacePreFix = "lsdw-pl";
		internal const string InventoryStateNameSpace = "https://github.com/BoBoBaSs84/Los.Santos.Dope.Wars/Inventory";
		internal const string InventoryStateNameSpacePreFix = "lsdw-in";
		internal const string LogEntryStateNameSpace = "https://github.com/BoBoBaSs84/Los.Santos.Dope.Wars/LogEntry";
		internal const string LogEntryStateNameSpacePreFix = "lsdw-le";
		internal const string DrugStateNameSpace = "https://github.com/BoBoBaSs84/Los.Santos.Dope.Wars/Drug";
		internal const string DrugStateNameSpacePreFix = "lsdw-dr";

		/// <summary>
		/// Gets all the necessary xml namespaces.
		/// </summary>
		internal static XmlSerializerNamespaces SerializerNamespaces => GetNameSpaces();

		private static XmlSerializerNamespaces GetNameSpaces()
		{
			XmlSerializerNamespaces namespaces = new();
			namespaces.Add(GameStateNameSpacePreFix, GameStateNameSpace);
			namespaces.Add(DrugStateNameSpacePreFix, DrugStateNameSpace);
			namespaces.Add(DealerStateNameSpacePreFix, DealerStateNameSpace);
			namespaces.Add(LogEntryStateNameSpacePreFix, LogEntryStateNameSpace);
			namespaces.Add(InventoryStateNameSpacePreFix, InventoryStateNameSpace);
			namespaces.Add(PlayerStateNameSpacePreFix, PlayerStateNameSpace);

			return namespaces;
		}
	}
}
