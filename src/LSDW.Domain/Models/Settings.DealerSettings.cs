using LSDW.Domain.Models.Base;
using static LSDW.Abstractions.Domain.Models.ISettings;

namespace LSDW.Domain.Models;

internal sealed partial class Settings
{
	/// <summary>
	/// The dealer settings class.
	/// </summary>
	internal sealed class DealerSettings : NotificationBase, IDealerSettings
	{
		private static readonly Lazy<DealerSettings> _settings = new(() => new());
		private int downTimeInHours;
		private bool hasArmor;
		private bool hasWeapons;

		/// <summary>
		/// Initializes a instance of the dealer settings class.
		/// </summary>
		private DealerSettings()
		{
			downTimeInHours = 48;
			hasArmor = true;
			hasWeapons = true;
		}

		/// <summary>
		/// The singleton instance of the dealer settings.
		/// </summary>
		public static DealerSettings Instance
			=> _settings.Value;

		/// <inheritdoc/>
		public int DownTimeInHours
		{
			get => downTimeInHours;
			set => SetProperty(ref downTimeInHours, value);
		}

		/// <inheritdoc/>
		public bool HasArmor
		{
			get => hasArmor;
			set => SetProperty(ref hasArmor, value);
		}

		/// <inheritdoc/>
		public bool HasWeapons
		{
			get => hasWeapons;
			set => SetProperty(ref hasWeapons, value);
		}

		/// <inheritdoc/>
		public int[] GetDownTimeInHoursValues()
			=> new int[] { 24, 48, 72, 96, 120, 144, 168 };
	}
}
