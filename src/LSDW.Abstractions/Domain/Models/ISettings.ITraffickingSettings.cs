using LSDW.Abstractions.Domain.Models.Base;

namespace LSDW.Abstractions.Domain.Models;

public partial interface ISettings
{
	/// <summary>
	/// The trafficking settings interface.
	/// </summary>
	public interface ITraffickingSettings : INotificationBase
	{
		/// <summary>
		/// The bust chance property.
		/// </summary>
		float BustChance { get; set; }

		/// <summary>
		/// The discover dealer property.
		/// </summary>
		bool DiscoverDealer { get; set; }

		/// <summary>
		/// The wanted level property.
		/// </summary>
		int WantedLevel { get; set; }

		/// <summary>
		/// Returns the possible bust chance values.
		/// </summary>
		/// <returns>The list of possible values.</returns>
		float[] GetBustChanceValues();

		/// <summary>
		/// Returns the possible wanted level values.
		/// </summary>
		/// <returns>The list of possible values.</returns>
		int[] GetWantedLevelValues();
	}
}