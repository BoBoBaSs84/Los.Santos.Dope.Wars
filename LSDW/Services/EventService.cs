using LSDW.Events.Base;

namespace LSDW.Services;

/// <summary>
/// A type-keyed pub/sub over <see cref="Event"/> subclasses. Publishing an event invokes
/// every handler subscribed to that type.
/// </summary>
/// <remarks>
/// Constructed once by <see cref="Core.DrugDeal"/> and handed to every subsystem that
/// subscribes or publishes. It used to be a <see langword="static"/> singleton; the bus is
/// the one thing every subsystem touches, so its lifetime decides theirs — an ambient one
/// outlives a script reload and keeps the previous run's handlers subscribed.
/// </remarks>
internal sealed class EventService
{
  private readonly Dictionary<Type, List<Action<object>>> _subscribers = [];

  /// <summary>
  /// Registers a handler to be invoked when a <typeparamref name="T"/> is published.
  /// </summary>
  /// <typeparam name="T">The event type to subscribe to.</typeparam>
  /// <param name="handler">The handler to invoke.</param>
  public void Subscribe<T>(Action<T> handler) where T : notnull, Event
  {
    if (!_subscribers.TryGetValue(typeof(T), out var handlers))
    {
      handlers = [];
      _subscribers[typeof(T)] = handlers;
    }

    handlers.Add(obj => handler((T)obj));
  }

  /// <summary>
  /// Publishes an event to every handler subscribed to <typeparamref name="T"/>.
  /// </summary>
  /// <typeparam name="T">The event type being published.</typeparam>
  /// <param name="message">The event to publish.</param>
  public void Publish<T>(T message) where T : notnull, Event
  {
    if (_subscribers.TryGetValue(typeof(T), out List<Action<object>> handlers))
      handlers.ForEach(handler => handler(message));
  }
}
