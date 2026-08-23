using LSDW.Services;

namespace LSDW.Tests.Fakes;

/// <summary>
/// Records what a subsystem told the player instead of showing it, which is what makes the
/// notifying paths testable at all — every one of them ends in a GTA native.
/// </summary>
/// <remarks>
/// An instance per test, never a static: <c>MSTestSettings</c> parallelises at method level,
/// and a shared recorder would interleave two tests' messages into one list.
/// </remarks>
internal sealed class FakeNotificationService : INotificationService
{
  /// <summary>The <see cref="Ticker"/> messages, in the order they were shown.</summary>
  internal List<string> Tickers { get; } = [];

  /// <summary>The <see cref="Subtitle"/> messages, in the order they were shown.</summary>
  internal List<string> Subtitles { get; } = [];

  /// <summary>The <see cref="Picture"/> messages, in the order they were shown.</summary>
  internal List<(string Sender, string Subject, string Message)> Pictures { get; } = [];

  /// <inheritdoc/>
  public void Picture(string sender, string subject, string message, string texture = "CHAR_DEFAULT")
    => Pictures.Add((sender, subject, message));

  /// <inheritdoc/>
  public void Ticker(string message, bool isImportant = false)
    => Tickers.Add(message);

  /// <inheritdoc/>
  public void Subtitle(string message, int duration = 2500)
    => Subtitles.Add(message);
}
