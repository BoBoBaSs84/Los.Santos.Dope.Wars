using GTA;
using LSDW.Abstractions.Interfaces.Application.Providers;

namespace LSDW.Application.Providers;

/// <summary>
/// The game time provider.
/// </summary>
internal sealed class GameTimeProvider : ITimeProvider
{
	/// <summary>
	/// Initializes a instance of the game time provider class.
	/// </summary>
	internal GameTimeProvider()
	{
		Now = World.CurrentDate;
		TimeOfDay = World.CurrentTimeOfDay;
	}

	public DateTime Now { get; }
	public TimeSpan TimeOfDay { get; }
}
