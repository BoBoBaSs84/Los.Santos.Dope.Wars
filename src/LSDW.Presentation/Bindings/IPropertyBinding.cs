namespace LSDW.Presentation.Bindings;

/// <summary>
/// Represents a binding between a property of a source object that implements <c>INotifyPropertyChanged</c>.
/// </summary>
public interface IPropertyBinding
{
	/// <summary>
	/// Binds the property binding by subscribing to the source's <c>PropertyChanged</c> event and updating
	/// the target value accordingly.
	/// </summary>
	void Bind();

	/// <summary>
	/// Unbinds the property binding by unsubscribing from the source's <c>PropertyChanged</c> event. After
	/// calling this method, the binding will no longer update the target value when the source property changes.
	/// </summary>
	void Unbind();
}
