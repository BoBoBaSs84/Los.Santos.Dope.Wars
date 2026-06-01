using BB84.SourceGenerators.Attributes;

namespace LSDW.Domain.Models.Base;

/// <summary>
/// Represents a base class for notifiable objects that implement the
/// <c>INotifyPropertyChanging</c> and <c>INotifyPropertyChanged</c> interfaces.
/// </summary>
[GenerateNotifications]
public abstract partial class NotifiableBase
{
	/// <summary>
	/// Sets the property value and raises the PropertyChanging and PropertyChanged events
	/// </summary>
	/// <typeparam name="T">The type of the property value.</typeparam>
	/// <param name="field">The backing field for the property.</param>
	/// <param name="value">The new value for the property.</param>
	/// <param name="propertyName">The name of the property.</param>
	protected virtual void SetProperty<T>(ref T field, T value, string propertyName)
	{
		if (!EqualityComparer<T>.Default.Equals(field, value))
		{
			RaisePropertyChanging(propertyName);
			field = value;
			RaisePropertyChanged(propertyName);
		}
	}
}
