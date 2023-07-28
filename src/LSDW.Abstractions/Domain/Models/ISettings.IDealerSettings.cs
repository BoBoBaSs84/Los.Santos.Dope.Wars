using LSDW.Abstractions.Domain.Models.Base;

namespace LSDW.Abstractions.Domain.Models;

public partial interface ISettings
{
	/// <summary>
	/// The dealer settings interface.
	/// </summary>
	public interface IDealerSettings : INotificationBase
	{
		/// <summary>
		/// The down time in hours property.
		/// </summary>
		int DownTimeInHours { get; set; }

		/// <summary>
		/// The dealer has armor property.
		/// </summary>
		bool HasArmor { get; set; }

		/// <summary>
		/// The dealer has weapons property.
		/// </summary>
		bool HasWeapons { get; set; }

		/// <summary>
		/// Returns the possible dealer down time values.
		/// </summary>
		/// <returns>The list of possible values.</returns>
		int[] GetDownTimeInHoursValues();
	}
}