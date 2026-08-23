#pragma warning disable IDE0130 // Namespace does not match folder structure
using System.ComponentModel;

namespace System.Runtime.CompilerServices;

/// <summary>
/// Enables <c>init</c> accessors and records on .NET Framework, whose BCL does not ship this type.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
internal static class IsExternalInit;
