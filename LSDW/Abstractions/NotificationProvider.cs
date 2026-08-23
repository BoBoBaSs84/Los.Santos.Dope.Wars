using BB84.SourceGenerators.Attributes;

namespace LSDW.Abstractions;

internal partial interface INotificationProvider;

[GenerateAbstraction(typeof(GTA.UI.Notification), typeof(INotificationProvider), typeof(NotificationProvider))]
internal sealed partial class NotificationProvider;
