namespace LSDW.Interfaces.Services;

/// <summary>
/// The date time service interface.
/// </summary>
public interface IDateTimeService
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
