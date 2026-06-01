using LemonUI.Menus;
using LSDW.Presentation.Bindings;
using System.ComponentModel;

namespace LSDW.Presentation.Extensions;

/// <summary>
/// Represents extension methods for menu item bindings.
/// </summary>
internal static class NativeItemBindingExtensions
{
	/// <summary>
	/// Binds a <see cref="NativeCheckboxItem"/> to a property of a source object that implements <see cref="INotifyPropertyChanged"/>.
	/// </summary>
	/// <remarks>
	/// The binding is two-way, meaning that changes to the checkbox will update the source property, and changes to the source
	/// property will update the checkbox.
	/// </remarks>
	/// <typeparam name="TSource">The type of the source object.</typeparam>
	/// <param name="item">The checkbox item to bind.</param>
	/// <param name="source">The source object.</param>
	/// <param name="propertyName">The name of the property to bind to.</param>
	/// <returns>The binding object that can be used to manage the binding lifecycle.</returns>
	internal static IPropertyBinding BindTo<TSource>(this NativeCheckboxItem item, TSource source, string propertyName)
			where TSource : INotifyPropertyChanged
	{
		PropertyBinding<TSource, bool> binding = new(source, propertyName, value => item.Checked = value);

		item.CheckboxChanged += (sender, e) => binding.UpdateSource(item.Checked);

		return binding;
	}

	/// <summary>
	/// Binds a <see cref="NativeListItem{T}"/> to a property of a source object that implements <see cref="INotifyPropertyChanged"/>.
	/// </summary>
	/// <remarks>
	/// The binding is two-way, meaning that changes to the list item will update the source property, and changes to the source
	/// property will update the list item.
	/// </remarks>
	/// <typeparam name="TSource">The type of the source object.</typeparam>
	/// <typeparam name="T">The type of the list item values.</typeparam>
	/// <param name="item">The list item to bind.</param>
	/// <param name="source">The source object.</param>
	/// <param name="propertyName">The name of the property to bind to.</param>
	/// <returns>The binding object that can be used to manage the binding lifecycle.</returns>
	internal static IPropertyBinding BindTo<TSource, T>(this NativeListItem<T> item, TSource source, string propertyName)
			where TSource : INotifyPropertyChanged
	{
		PropertyBinding<TSource, T> binding = new(source, propertyName, value => item.SelectedItem = value);

		item.ItemChanged += (sender, e) => binding.UpdateSource(item.SelectedItem);

		return binding;
	}
}