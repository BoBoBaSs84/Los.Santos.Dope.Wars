using LSDW.Domain.Events.Base;

namespace LSDW.Domain.Events.Player;

/// <summary>
/// Represents event data for when a player's controllable state changes.
/// </summary>
/// <param name="isControllable">Indicates whether the player is now controllable.</param>
public sealed class ControllabilityChanged(bool isControllable) : EventBase
{
	/// <summary>
	/// Gets a value indicating whether the player is now controllable.
	/// </summary>
	public bool IsControllable => isControllable;
}
