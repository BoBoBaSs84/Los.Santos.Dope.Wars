using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LSDW.Classes.BaseClasses;

/// <summary>
///	The <see cref="NotificationBase"/> class.
/// <remarks>
///	Implements the following interfaces:
/// <list type="bullet">
/// <item>The members of the <see cref="INotifyPropertyChanged"/> interface.</item>
/// <item>The members of the <see cref="INotifyPropertyChanging"/> interface.</item>
/// </list>
/// </remarks>
/// </summary>
internal abstract class NotificationBase : INotifyPropertyChanged, INotifyPropertyChanging
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
