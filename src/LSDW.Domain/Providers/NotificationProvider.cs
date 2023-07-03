using GTA.UI;
using LSDW.Abstractions.Domain.Providers;

namespace LSDW.Domain.Providers;

/// <summary>
/// The notification provider class.
/// </summary>
/// <remarks>
/// Wrapper for the <see cref="Screen"/> and <see cref="Notification"/> methods.
/// </remarks>
internal sealed class NotificationProvider : INotificationProvider
{
	/// <summary>
	/// The notification provider singleton instance.
	/// </summary>
	public static NotificationProvider Instance => new();

	public void Hide(int handle)
		=> Notification.Hide(handle);

	public int Show(string message, bool blinking = false)
		=> Notification.Show(message, blinking);

	public int Show(NotificationIcon icon, string sender, string subject, string message, bool fadeIn = false, bool blinking = false)
		=> Notification.Show(icon, sender, subject, message, fadeIn, blinking);

	public void Show(string sender, string subject, string message, bool blinking = false)
		=> Show(NotificationIcon.Default, sender, subject, message, false, blinking);

	public void Show(string subject, string message, bool blinking = false)
		=> Show("Anonymous", subject, message, blinking);

	public void ShowHelpText(string helpText, int duration = -1, bool beep = true, bool looped = false)
		=> Screen.ShowHelpText(helpText, duration, beep, looped);

	public void ShowHelpTextThisFrame(string helpText)
		=> Screen.ShowHelpTextThisFrame(helpText);

	public void ShowHelpTextThisFrame(string helpText, bool beep)
		=> Screen.ShowHelpTextThisFrame(helpText, beep);

	public void ShowSubtitle(string message, int duration = 2500)
		=> Screen.ShowSubtitle(message, duration);

	public void ShowSubtitle(string message, int duration, bool drawImmediately = true)
		=> Screen.ShowSubtitle(message, duration, drawImmediately);
}
