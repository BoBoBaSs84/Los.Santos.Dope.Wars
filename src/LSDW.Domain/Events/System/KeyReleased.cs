using LSDW.Domain.Events.Base;

namespace LSDW.Domain.Events.System;

/// <summary>
/// Represents an event that is triggered when a key is released on the keyboard.
/// </summary>
/// <param name="keyData">The key data associated with the key release event.</param>
public sealed class KeyReleased(Keys keyData) : EventBase
{
	/// <summary>
	/// Gets the key data associated with the event.
	/// </summary>
	public Keys Keys => keyData;
}
