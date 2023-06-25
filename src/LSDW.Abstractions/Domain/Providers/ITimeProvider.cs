using GTA;

namespace LSDW.Abstractions.Domain.Providers;

/// <summary>
/// The date and time provider interface.
/// </summary>
public interface ITimeProvider
{
	/// <summary>
	/// Gets or sets the current date and time in the <see cref="World"/>.
	/// </summary>
	DateTime Now { get; set; }

	/// <summary>
	/// Gets or sets the current time of day in the <see cref="World"/>.
	/// </summary>
	TimeSpan TimeOfDay { get; set; }
}
