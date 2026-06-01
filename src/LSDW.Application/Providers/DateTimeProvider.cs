using System.Diagnostics.CodeAnalysis;

using LSDW.Application.Abstractions.Application.Providers;

namespace LSDW.Application.Providers;

/// <summary>
/// The implementation for the date time provider contract.
/// </summary>
/// <inheritdoc cref="IDateTimeProvider"/>
[ExcludeFromCodeCoverage]
internal sealed class DateTimeProvider : IDateTimeProvider
{
	public DateTime Now => DateTime.Now;

	public TimeSpan TimeOfDay => DateTime.Now.TimeOfDay;

	public DateTime Today => DateTime.Today;

	public DateTime UtcNow => DateTime.UtcNow;

	public DateTime MaxValue => DateTime.MaxValue;

	public DateTime MinValue => DateTime.MinValue;
}
