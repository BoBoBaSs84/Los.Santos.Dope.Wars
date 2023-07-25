using LSDW.Abstractions.Helpers;

namespace LSDW.Abstractions.Domain.Models;

public partial interface ISettings
{
	/// <summary>
	/// The dealer settings interface.
	/// </summary>
	public interface IDealerSettings
	{
		/// <summary>
		/// The down time in hours property.
		/// </summary>
		BindableProperty<int> DownTimeInHours { get; set; }

		/// <summary>
		/// The dealer has armor property.
		/// </summary>
		BindableProperty<bool> HasArmor { get; set; }

		/// <summary>
		/// The dealer has weapons property.
		/// </summary>
		BindableProperty<bool> HasWeapons { get; set; }

		/// <summary>
		/// Returns the possible dealer down time values.
		/// </summary>
		/// <returns>The list of possible values.</returns>
		int[] GetDownTimeInHoursValues();
	}
}