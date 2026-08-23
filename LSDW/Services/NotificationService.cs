using LSDW.Abstractions;

using GTA.UI;

namespace LSDW.Services;

/// <summary>
/// The three ways the mod talks to the player: a feed message with a contact picture, a
/// ticker line, and a subtitle.
/// </summary>
/// <remarks>
/// This is the seam the subsystems take, not <see cref="INotificationProvider"/> or
/// <see cref="IScreenProvider"/>. Those are the generated full surface of two GTA statics —
/// 12 and 14 members, of which the mod calls three. A test double for this interface is
/// three methods; a double for the generated pair is twenty-six, all but three of them
/// throwing.
/// </remarks>
internal interface INotificationService
{
  /// <summary>
  /// Posts a notification with a contact picture and a sender to the feed.
  /// </summary>
  /// <param name="sender">The character name shown as the sender.</param>
  /// <param name="subject">The subject line of the message.</param>
  /// <param name="message">The message body.</param>
  /// <param name="texture">The contact image, used as both the texture dictionary and the texture name.</param>
  void Picture(string sender, string subject, string message, string texture = "CHAR_DEFAULT");

  /// <summary>
  /// Posts a ticker notification with a specific <paramref name="message"/>.
  /// </summary>
  /// <param name="message">The message body.</param>
  /// <param name="isImportant">
  /// If set to true, the message will flash and may have a custom background color or vibrate the controller.
  /// </param>
  void Ticker(string message, bool isImportant = false);

  /// <summary>
  /// Shows a subtitle at the bottom of the screen for a given time.
  /// </summary>
  /// <param name="message">The message body.</param>
  /// <param name="duration">How long to display the subtitle, in milliseconds.</param>
  void Subtitle(string message, int duration = 2500);
}

/// <summary>
/// The live implementation, over the generated providers for <c>GTA.UI.Notification</c>
/// and <c>GTA.UI.Screen</c>.
/// </summary>
/// <remarks>
/// The optional arguments are spelled out at every call below because the generated
/// providers expand the statics' optional parameters into required ones. The defaults
/// restated here are the ones the GTA statics declare, so the behaviour is unchanged from
/// when these three lived on <c>ScriptHelper</c>.
/// </remarks>
internal sealed class NotificationService : INotificationService
{
  private readonly INotificationProvider _notifications;
  private readonly IScreenProvider _screen;

  /// <summary>
  /// Binds the service to the two generated providers.
  /// </summary>
  /// <param name="notifications">The feed and ticker surface.</param>
  /// <param name="screen">The subtitle surface.</param>
  public NotificationService(INotificationProvider notifications, IScreenProvider screen)
  {
    _notifications = notifications;
    _screen = screen;
  }

  /// <inheritdoc/>
  public void Picture(string sender, string subject, string message, string texture = "CHAR_DEFAULT")
    => _notifications.PostMessageText(message, new(texture, texture), true, FeedTextIcon.Blank, sender, subject);

  /// <inheritdoc/>
  public void Ticker(string message, bool isImportant = false)
    => _notifications.PostTicker(message, isImportant, false);

  /// <inheritdoc/>
  public void Subtitle(string message, int duration = 2500)
    => _screen.ShowSubtitle(message, duration);
}
