using LSDW.Abstractions.Domain.Models.Base;
using System.Runtime.CompilerServices;

namespace LSDW.Domain.Models.Base;

/// <summary>
///	The notification base class.
/// </summary>
/// <remarks>
/// Implements the members of the <see cref="INotificationBase"/> interface.
/// </remarks>
public abstract class NotificationBase : INotificationBase
{
	/// <inheritdoc/>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <inheritdoc/>
	public event PropertyChangingEventHandler? PropertyChanging;

	/// <summary>
	/// Sets a new value for a property and notifies about the change.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="fieldValue">The referenced field.</param>
	/// <param name="newValue">The new value for the property.</param>
	/// <param name="propertyName">The name of the calling property.</param>
	protected void SetProperty<T>(ref T fieldValue, T newValue, [CallerMemberName] string propertyName = "") where T : IEquatable<T>
	{
		if (!EqualityComparer<T>.Default.Equals(fieldValue, newValue))
		{
			RaisePropertyChanging(propertyName);
			fieldValue = newValue;
			RaisePropertyChanged(propertyName);
		}
	}

	/// <summary>
	/// The method is used to raise the <see cref="PropertyChanged"/> event.
	/// </summary>
	/// <remarks>
	/// The calling member's name will be used as the parameter.
	/// </remarks>
	/// <param name="propertyName">The name of the calling property.</param>
	private void RaisePropertyChanged(string propertyName)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

	/// <summary>
	/// The method is used to raise the <see cref="PropertyChanging"/> event.
	/// </summary>
	/// <remarks>
	/// The calling member's name will be used as the parameter.
	/// </remarks>
	/// <param name="propertyName">The name of the calling property.</param>
	private void RaisePropertyChanging(string propertyName)
		=> PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
}
