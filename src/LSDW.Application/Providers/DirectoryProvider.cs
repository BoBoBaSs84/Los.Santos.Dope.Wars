using System.Diagnostics.CodeAnalysis;

using BB84.SourceGenerators.Attributes;

using LSDW.Application.Abstractions.Application.Providers;

namespace LSDW.Application.Providers;

/// <summary>
/// Represents a provider for directory-related operations. This class serves as
/// a wrapper around the <see cref="Directory"/> class, providing an abstraction
/// layer for directory operations within the application.
/// </summary>
[ExcludeFromCodeCoverage]
[GenerateAbstraction(typeof(Directory), typeof(IDirectoryProvider), typeof(DirectoryProvider))]
internal sealed partial class DirectoryProvider : IDirectoryProvider
{ }
