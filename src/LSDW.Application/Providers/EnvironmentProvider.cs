using BB84.SourceGenerators.Attributes;
using LSDW.Application.Abstractions.Application.Providers;

namespace LSDW.Application.Providers;

/// <summary>
/// Represents a provider for environment-related information and functionality within the application.
/// </summary>
[GenerateAbstraction(typeof(Environment), typeof(IEnvironmentProvider), typeof(EnvironmentProvider))]
internal sealed partial class EnvironmentProvider
{ }
