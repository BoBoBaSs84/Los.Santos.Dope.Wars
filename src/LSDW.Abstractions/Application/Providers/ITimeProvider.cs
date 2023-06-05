namespace LSDW.Abstractions.Application.Providers;

/// <summary>
/// The date and time provider interface.
/// </summary>
public interface ITimeProvider
{
	/// <summary>
	/// The current date and time.
	/// </summary>
	DateTime Now { get; }

	/// <summary>
	/// The current time of the day.
	/// </summary>
	TimeSpan TimeOfDay { get; }
}
