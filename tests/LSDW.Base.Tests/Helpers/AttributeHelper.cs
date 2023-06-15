using System.Reflection;

namespace LSDW.Base.Tests.Helpers;

public static class AttributeHelper
{
	public static bool TypeHasAttribute<TAttribute>(Type type)
		where TAttribute : Attribute =>
		type.GetCustomAttributes<TAttribute>().Any();

	public static bool MethodHasAttribute<TAttribute>(MethodInfo info)
		where TAttribute : Attribute =>
		info.GetCustomAttributes<TAttribute>().Any();

	public static bool FieldHasAttribute<TAttribute>(FieldInfo info)
		where TAttribute : Attribute =>
		info.GetCustomAttributes<TAttribute>().Any();

	public static bool PropertyHasAttribute<TAttribute>(PropertyInfo info)
		where TAttribute : Attribute =>
		info.GetCustomAttributes<TAttribute>().Any();
}
