﻿using System.Xml.Serialization;

namespace LSDW.Infrastructure.Constants;

/// <summary>
/// The xml constants class.
/// </summary>
internal static class XmlConstants
{
	internal const string NameSpace = "https://github.com/BoBoBaSs84/Los.Santos.Dope.Wars";
	internal const string NameSpacePreFix = "lsdw";
	internal const string DealerStateRootName = "Dealer";
	internal const string DrugStateRootName = "Drug";
	internal const string StateRootName = "State";
	internal const string InventoryStateRootName = "Inventory";
	internal const string PlayerStateRootName = "Player";
	internal const string TraffickingStateRootName = "Trafficking";
	internal const string TransactionStateRootName = "Transaction";

	/// <summary>
	/// Gets the necessary namespaces.
	/// </summary>
	internal static XmlSerializerNamespaces SerializerNamespaces => GetNameSpaces();

	private static XmlSerializerNamespaces GetNameSpaces()
	{
		XmlSerializerNamespaces namespaces = new();
		namespaces.Add(NameSpacePreFix, NameSpace);
		return namespaces;
	}
}
