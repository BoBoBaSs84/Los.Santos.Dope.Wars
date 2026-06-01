using System.Diagnostics.CodeAnalysis;

using BB84.SourceGenerators.Attributes;

using LSDW.Application.Abstractions.Application.Providers;

namespace LSDW.Application.Providers;

/// <summary>
/// Represents a provider for file-related operations. This class serves as
/// a wrapper around the <see cref="File"/> class, providing an abstraction
/// layer for file operations within the application.
/// </summary>
[ExcludeFromCodeCoverage]
[GenerateAbstraction(typeof(File), typeof(IFileProvider), typeof(FileProvider))]
internal sealed partial class FileProvider : IFileProvider
{ }
