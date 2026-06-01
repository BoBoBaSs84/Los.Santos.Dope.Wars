using LSDW.Application.Abstractions.Application.Services;
using LSDW.Domain.Abstractions.Events;

namespace LSDW.Application.Services;

/// <summary>
/// Represents a simple event service for publishing and subscribing to events.
/// </summary>
internal sealed class EventService : IEventService
{
	private readonly Dictionary<Type, List<Action<object>>> _subscribers = [];

	public void Subscribe<T>(Action<T> handler) where T : notnull, IEvent
	{
		if (!_subscribers.TryGetValue(typeof(T), out var handlers))
		{
			handlers = [];
			_subscribers[typeof(T)] = handlers;
		}

		handlers.Add(obj => handler((T)obj));
	}

	public void Publish<T>(T message) where T : notnull, IEvent
	{
		if (_subscribers.TryGetValue(typeof(T), out List<Action<object>>? handlers))
			handlers.ForEach(handler => handler(message));
	}
}
