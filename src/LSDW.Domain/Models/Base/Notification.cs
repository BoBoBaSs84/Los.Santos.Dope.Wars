using System.Runtime.CompilerServices;

namespace LSDW.Domain.Models.Base;

/// <summary>
///	The notification class.
/// <remarks>
///	Implements the following interface members:
/// <list type="bullet">
/// <item>The <see cref="INotifyPropertyChanged"/> interface members.</item>
/// <item>The <see cref="INotifyPropertyChanging"/> interface members.</item>
/// </list>
/// </remarks>
/// </summary>
public abstract class Notification : INotifyPropertyChanged, INotifyPropertyChanging
{
	/// <summary>
	/// Sets a new value for a property and notifies about the change.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="field">The referenced field.</param>
	/// <param name="newValue">The new value for the property.</param>
	/// <param name="propertyName">The name of the calling property.</param>
	protected void SetProperty<T>(ref T field, T newValue, [CallerMemberName] string? propertyName = null)
	{
		if (!Equals(field, newValue))
		{
			RaisePropertyChanging(propertyName);
			field = newValue;
			RaisePropertyChanged(propertyName);
		}
	}

	/// <inheritdoc/>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <inheritdoc/>
	public event PropertyChangingEventHandler? PropertyChanging;

	/// <summary>
	/// The method is used to raise the 'property changed' event.
	/// </summary>
	/// <remarks>
	/// The calling member's name will be used as the parameter.
	/// </remarks>
	/// <param name="propertyName">The name of the calling property.</param>
	protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null) =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

	/// <summary>
	/// The method is used to raise the 'property changing' event.
	/// </summary>
	/// <remarks>
	/// The calling member's name will be used as the parameter.
	/// </remarks>
	/// <param name="propertyName">The name of the calling property.</param>
	protected virtual void RaisePropertyChanging([CallerMemberName] string? propertyName = null) =>
		PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
}
