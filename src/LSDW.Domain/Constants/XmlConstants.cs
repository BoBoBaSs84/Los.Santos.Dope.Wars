using System.Xml.Serialization;

namespace LSDW.Domain.Constants;

/// <summary>
/// The xml constants class.
/// </summary>
public static class XmlConstants
{
	internal const string NameSpace = "https://github.com/BoBoBaSs84/Los.Santos.Dope.Wars";
	internal const string NameSpacePreFix = "lsdw";

	/// <summary>
	/// Gets the necessary namespaces.
	/// </summary>
	public static XmlSerializerNamespaces SerializerNamespaces => GetNameSpaces();

	private static XmlSerializerNamespaces GetNameSpaces()
	{
		XmlSerializerNamespaces namespaces = new();
		namespaces.Add(NameSpacePreFix, NameSpace);
		return namespaces;
	}
}
