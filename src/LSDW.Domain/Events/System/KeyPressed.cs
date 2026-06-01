using LSDW.Domain.Events.Base;

namespace LSDW.Domain.Events.System;

/// <summary>
/// Represents an event that is triggered when a key is pressed on the keyboard.
/// </summary>
/// <param name="keyData">The key data associated with the key press event</param>
public sealed class KeyPressed(Keys keyData) : EventBase
{
	/// <summary>
	/// Gets the key data associated with the event.
	/// </summary>
	public Keys Keys => keyData;
}
