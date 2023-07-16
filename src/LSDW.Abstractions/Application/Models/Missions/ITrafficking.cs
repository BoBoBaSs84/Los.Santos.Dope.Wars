using LSDW.Abstractions.Application.Models.Missions.Base;
using LSDW.Abstractions.Presentation.Menus;

namespace LSDW.Abstractions.Application.Models.Missions;

/// <summary>
/// The trafficking mission interface.
/// </summary>
public interface ITrafficking : IMission
{
	/// <summary>
	/// The side menu on the left of the screen.
	/// </summary>
	IDealMenu? LeftSideMenu { get; set; }

	/// <summary>
	/// The side menu on the right of the screen.
	/// </summary>
	IDealMenu? RightSideMenu { get; set; }
}
