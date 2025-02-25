using System.ComponentModel;

namespace LSDW.Abstractions.Domain.Models.Base;

/// <summary>
/// The notification base interface.
/// </summary>
/// <remarks>
///	Inherits from the following interfaces:
/// <list type="bullet">
/// <item>The <see cref="INotifyPropertyChanged"/> interface</item>
/// <item>The <see cref="INotifyPropertyChanging"/> interface</item>
/// </list>
/// </remarks>
public interface INotificationBase : INotifyPropertyChanged, INotifyPropertyChanging
{ }
