namespace LSDW.Abstractions.Helpers;

/// <summary>
/// The bindable property class.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class BindableProperty<T> where T : IComparable
{
	private T _value = default!;

	/// <summary>
	/// Initializes a instance of the bindable property class.
	/// </summary>
	/// <param name="value">The value to set.</param>
	public BindableProperty(T value)
		=> _value = value;

	/// <summary>
	/// The value of the property.
	/// </summary>
	public T Value
	{
		get => _value;
		set
		{
			if (!EqualityComparer<T>.Default.Equals(_value, value))
			{
				ValueChanging?.Invoke(this, new(_value, value));
				_value = value;
				ValueChanged?.Invoke(this, new(value));
			}
		}
	}

	/// <summary>
	/// The event that is triggered if the value is changing.
	/// </summary>
	public event EventHandler<ValueEventArgs<T>>? ValueChanging;

	/// <summary>
	/// The event that is triggered if the value has changed.
	/// </summary>
	public event EventHandler<ValueEventArgs<T>>? ValueChanged;
}

/// <summary>
/// The value event args class.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class ValueEventArgs<T> : EventArgs
{
	/// <summary>
	/// Initializes a instance of the value event args class.
	/// </summary>
	/// <param name="newValue">The new value.</param>
	internal ValueEventArgs(T newValue) : this(default!, newValue)
	{ }

	/// <summary>
	/// Initializes a instance of the value event args class.
	/// </summary>
	/// <param name="oldValue">The old value.</param>
	/// <param name="newValue">The new value.</param>
	internal ValueEventArgs(T oldValue, T newValue)
	{
		OldValue = oldValue;
		NewValue = newValue;
	}

	/// <summary>
	/// The new value property.
	/// </summary>
	public T NewValue { get; }

	/// <summary>
	/// The old value property.
	/// </summary>
	public T OldValue { get; }
}
