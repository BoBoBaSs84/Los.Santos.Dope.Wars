using LSDW.Abstractions.Helpers;
using static LSDW.Abstractions.Domain.Models.ISettings;

namespace LSDW.Domain.Models;

internal sealed partial class Settings
{
	/// <summary>
	/// The dealer settings class.
	/// </summary>
	internal sealed class DealerSettings : IDealerSettings
	{
		private static readonly Lazy<DealerSettings> _settings = new(() => new());

		/// <summary>
		/// Initializes a instance of the dealer settings class.
		/// </summary>
		private DealerSettings()
		{
			DownTimeInHours = new(48);
			HasArmor = new(true);
			HasWeapons = new(true);
		}

		/// <summary>
		/// The singleton instance of the dealer settings.
		/// </summary>
		public static DealerSettings Instance
			=> _settings.Value;

		/// <inheritdoc/>
		public BindableProperty<int> DownTimeInHours { get; set; }

		/// <inheritdoc/>
		public BindableProperty<bool> HasArmor { get; set; }

		/// <inheritdoc/>
		public BindableProperty<bool> HasWeapons { get; set; }

		/// <inheritdoc/>
		public int[] GetDownTimeInHoursValues()
			=> new int[] { 24, 48, 72, 96, 120, 144, 168 };
	}
}
