using System.ComponentModel;
using System.Reflection;

namespace LSDW.Presentation.Bindings;

/// <summary>
/// Represents a binding between a property of a source object that implements <c>INotifyPropertyChanged</c>
/// and a target setter action.
/// </summary>
/// <typeparam name="TSource">The type of the source object that implements <c>INotifyPropertyChanged</c>.</typeparam>
/// <typeparam name="TValue">The type of the value being bound.</typeparam>
internal sealed class PropertyBinding<TSource, TValue> : IPropertyBinding
	where TSource : INotifyPropertyChanged
{
	private readonly TSource _source;
	private readonly PropertyInfo _sourceProperty;
	private readonly string _sourcePropertyName;
	private readonly Action<TValue> _targetSetter;
	private readonly Func<object, TValue> _converter;
	private bool _isUpdatingTarget;
	private bool _isUpdatingSource;

	/// <summary>
	/// Initializes a new instance of the <see cref="PropertyBinding{TSource, TValue}"/> class.
	/// </summary>
	/// <param name="source">The source object that implements <c>INotifyPropertyChanged</c>.</param>
	/// <param name="sourcePropertyName">The name of the property on the source object to bind to.</param>
	/// <param name="targetSetter">The action to set the value on the target.</param>
	/// <param name="converter">An optional converter function to convert the source value to the target value.</param>
	public PropertyBinding(TSource source, string sourcePropertyName, Action<TValue> targetSetter, Func<object, TValue>? converter = null)
	{
		_source = source ?? throw new ArgumentNullException(nameof(source));
		_sourceProperty = _source.GetType().GetProperty(sourcePropertyName) ?? throw new ArgumentException($"Property '{sourcePropertyName}' not found on type '{typeof(TSource).FullName}'.", nameof(sourcePropertyName));
		_sourcePropertyName = sourcePropertyName ?? throw new ArgumentNullException(nameof(sourcePropertyName));
		_targetSetter = targetSetter ?? throw new ArgumentNullException(nameof(targetSetter));
		_converter = converter ?? (obj => (TValue)obj);

		Bind();
	}

	/// <inheritdoc/>
	public void Bind()
	{
		_source.PropertyChanged += (s, e) => OnSourcePropertyChanged(e.PropertyName);
		UpdateTarget();
	}

	/// <inheritdoc/>
	public void Unbind()
	{
		_source.PropertyChanged -= (s, e) => OnSourcePropertyChanged(e.PropertyName);
		UpdateTarget();
	}

	/// <summary>
	/// Updates the source property with a new value. This method is typically called when the target
	/// value changes and needs to be propagated back to the source.
	/// </summary>
	/// <param name="newValue">The new value to set on the source property.</param>
	public void UpdateSource(TValue newValue)
	{
		if (_isUpdatingTarget)
		{
			return;
		}

		_isUpdatingSource = true;
		try
		{
			if (_sourceProperty.CanWrite)
				_sourceProperty.SetValue(_source, newValue);
		}
		finally
		{
			_isUpdatingSource = false;
		}
	}

	private void OnSourcePropertyChanged(string propertyName)
	{
		if (propertyName == _sourcePropertyName && !_isUpdatingSource)
		{
			_isUpdatingTarget = true;
			try
			{
				UpdateTarget();
			}
			finally
			{
				_isUpdatingTarget = false;
			}
		}
	}

	private void UpdateTarget()
	{
		if (_sourceProperty.CanRead)
		{
			object value = _sourceProperty.GetValue(_source);
			_targetSetter(_converter(value));
		}
	}
}