using LSDW.Abstractions.Application.Missions.Base;
using LSDW.Abstractions.Domain.Services;

namespace LSDW.Abstractions.Application.Missions;

/// <summary>
/// The trafficking mission interface.
/// </summary>
public interface ITrafficking : IMission
{
	/// <summary>
	/// The notification service to use for the trafficking mission.
	/// </summary>
	INotificationService NotificationService { get; }
}
