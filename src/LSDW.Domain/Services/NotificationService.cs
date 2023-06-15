using GTA.UI;
using LSDW.Abstractions.Domain.Services;

namespace LSDW.Domain.Services;

/// <summary>
/// The notification service class.
/// </summary>
/// <remarks>
/// Wrapper for the <see cref="Screen"/> and <see cref="Notification"/> methods.
/// </remarks>
internal sealed class NotificationService : INotificationService
{
	public async Task Show(string message, bool blinking = false, int duration = 2500)
	{
		int handle = Notification.Show(message, blinking);
		await Task.Delay(duration);
		Notification.Hide(handle);
		await Task.CompletedTask;
	}

	public async Task Show(NotificationIcon icon, string sender, string subject, string message, bool fadeIn = false, bool blinking = false, int duration = 2500)
	{
		int hanle = Notification.Show(icon, sender, subject, message, fadeIn, blinking);
		await Task.Delay(duration);
		Notification.Hide(hanle);
		await Task.CompletedTask;
	}

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
