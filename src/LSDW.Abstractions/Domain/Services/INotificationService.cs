using GTA.UI;

namespace LSDW.Abstractions.Domain.Services;

/// <summary>
/// The notification service interface.
/// </summary>
public interface INotificationService
{
	/// <summary>
	/// Shows a subtitle at the bottom of the screen for a given time.
	/// </summary>
	/// <param name="message">The message to display.</param>
	/// <param name="duration">The duration to display the subtitle in milliseconds.</param>
	void ShowSubtitle(string message, int duration = 2500);

	/// <summary>
	/// Shows a subtitle at the bottom of the screen for a given time.
	/// </summary>
	/// <param name="message">The message to display.</param>
	/// <param name="duration">The duration to display the subtitle in milliseconds.</param>
	/// <param name="drawImmediately">Whether to draw immediately or draw after all the queued subtitles have finished.</param>
	void ShowSubtitle(string message, int duration, bool drawImmediately = true);

	/// <summary>
	/// Displays a help message in the top corner of the screen this frame.
	/// Beeping sound will be played.
	/// </summary>
	/// <param name="helpText">The text to display.</param>
	void ShowHelpTextThisFrame(string helpText);

	/// <summary>
	/// Displays a help message in the top corner of the screen this frame.
	/// Specify whether beeping sound plays.
	/// </summary>
	/// <param name="helpText">The text to display.</param>
	/// <param name="beep">Whether to play beeping sound.</param>
	void ShowHelpTextThisFrame(string helpText, bool beep);

	/// <summary>
	/// Displays a help message in the top corner of the screen infinitely.
	/// </summary>
	/// <param name="helpText">The text to display.</param>
	/// <param name="duration">The duration how long the help text will be displayed in real time.</param>
	/// <param name="beep">Whether to play beeping sound.</param>
	/// <param name="looped">Whether to show this help message forever.</param>
	void ShowHelpText(string helpText, int duration = -1, bool beep = true, bool looped = false);

	/// <summary>
	/// Shows a notification above the minimap with the given message.
	/// </summary>
	/// <param name="message">The message in the notification.</param>
	/// <param name="blinking">If set to true the notification will blink.</param>
	/// <param name="duration">The duration to display the notification in milliseconds.</param>
	Task Show(string message, bool blinking = false, int duration = 2500);

	/// <summary>
	/// Creates a more advanced notification above the minimap showing a sender icon, subject and the message.
	/// </summary>
	/// <param name="icon">The notification icon.</param>
	/// <param name="sender">The sender name.</param>
	/// <param name="subject">The subject line.</param>
	/// <param name="message">The message itself.</param>
	/// <param name="fadeIn">If true the message will fade in.</param>
	/// <param name="blinking">if set to true the notification will blink.</param>
	/// <param name="duration">The duration to display the notification in milliseconds.</param>
	Task Show(NotificationIcon icon, string sender, string subject, string message, bool fadeIn = false, bool blinking = false, int duration = 2500);
}
