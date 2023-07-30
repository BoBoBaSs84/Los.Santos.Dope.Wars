namespace LSDW.Abstractions.Domain.Models.Base;

/// <summary>
/// The bindable property interface.
/// </summary>
public interface IBindableProperty<T> where T : IEquatable<T>
{
	/// <summary>
	/// The actual value of the bindable property.
	/// </summary>
	T Value { get; set; }

	/// <summary>
	/// Occurs when the bindable property is changed.
	/// </summary>
	public event EventHandler<BindablePropertyChangedEventArgs<T>>? Changed;

	/// <summary>
	/// Occurs when the bindable property value is changing.
	/// </summary>
	public event EventHandler<BindablePropertyChangingEventArgs<T>>? Changing;
}

/// <summary>
/// The bindable property changed event args class.
/// </summary>
/// <typeparam name="T"></typeparam>
public class BindablePropertyChangedEventArgs<T> : EventArgs where T : IEquatable<T>
{
	/// <summary>
	/// Initializes a instance of the bindable property changed event args class.
	/// </summary>
	/// <param name="value">The value of the property that is changed.</param>
	public BindablePropertyChangedEventArgs(T value)
		=> Value = value;

	/// <summary>
	/// The value of the property that is changed.
	/// </summary>
	public T Value { get; }
}

/// <summary>
/// The bindable property changing event args class.
/// </summary>
/// <typeparam name="T"></typeparam>
public class BindablePropertyChangingEventArgs<T> : EventArgs where T : IEquatable<T>
{
	/// <summary>
	/// Initializes a instance of the bindable property changing event args class.
	/// </summary>
	/// <param name="value">The value of the property that is changing.</param>
	public BindablePropertyChangingEventArgs(T value)
		=> Value = value;

	/// <summary>
	/// The value of the property that is changing.
	/// </summary>
	public T Value { get; }
}
