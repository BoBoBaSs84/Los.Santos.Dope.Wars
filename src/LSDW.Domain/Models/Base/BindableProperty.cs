using LSDW.Abstractions.Domain.Models.Base;

namespace LSDW.Domain.Models.Base;

/// <summary>
/// The bindable property class.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class BindableProperty<T> : IBindableProperty<T> where T : IEquatable<T>
{
	private T value;

	/// <inheritdoc/>
	public T Value
	{
		get => value;
		set
		{
			if (EqualityComparer<T>.Default.Equals(this.value, value))
				return;

			Changing?.Invoke(this, new BindablePropertyChangingEventArgs<T>(this.value));
			this.value = value;
			Changed?.Invoke(this, new BindablePropertyChangedEventArgs<T>(value));
		}
	}

	/// <summary>
	/// Initializes a instance of the bindable property class.
	/// </summary>
	/// <param name="value">The value of the bindable property.</param>
	public BindableProperty(T value)
		=> this.value = value;

	/// <inheritdoc/>	
	public event EventHandler<BindablePropertyChangedEventArgs<T>>? Changed;

	/// <inheritdoc/>
	public event EventHandler<BindablePropertyChangingEventArgs<T>>? Changing;
}
