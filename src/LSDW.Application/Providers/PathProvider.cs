using System.Diagnostics.CodeAnalysis;

using BB84.SourceGenerators.Attributes;

using LSDW.Application.Abstractions.Application.Providers;

namespace LSDW.Application.Providers;

/// <summary>
/// Represents a provider for path-related operations. This class serves as
/// a wrapper around the <see cref="Path"/> class, providing an abstraction
/// layer for path operations within the application.
/// </summary>
[ExcludeFromCodeCoverage]
[GenerateAbstraction(typeof(Path), typeof(IPathProvider), typeof(PathProvider))]
internal sealed partial class PathProvider : IPathProvider
{ }
