using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Models.Base;
using LSDW.Domain.Models.Base;

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
			DownTimeInHours = new BindableProperty<int>(48);
			HasArmor = new BindableProperty<bool>(true);
			HasWeapons = new BindableProperty<bool>(true);
		}

		/// <summary>
		/// The singleton instance of the dealer settings.
		/// </summary>
		public static DealerSettings Instance
			=> _settings.Value;

		public IBindableProperty<int> DownTimeInHours { get; }
		public IBindableProperty<bool> HasArmor { get; }
		public IBindableProperty<bool> HasWeapons { get; }

		public int[] GetDownTimeInHoursValues()
			=> new int[] { 24, 48, 72, 96, 120, 144, 168 };
	}
}
