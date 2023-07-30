using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Models.Base;
using LSDW.Domain.Models.Base;

namespace LSDW.Domain.Models;

internal sealed partial class Settings
{
	/// <summary>
	/// The trafficking settings class.
	/// </summary>

	internal sealed class TraffickingSettings : ITraffickingSettings
	{
		private static readonly Lazy<TraffickingSettings> _settings = new(() => new());

		/// <summary>
		/// Initializes a instance of the trafficking settings class.
		/// </summary>
		private TraffickingSettings()
		{
			BustChance = new BindableProperty<float>(0.1f);
			DiscoverDealer = new BindableProperty<bool>(false);
			WantedLevel = new BindableProperty<int>(2);
		}

		/// <summary>
		/// The singleton instance of the trafficking settings.
		/// </summary>
		public static TraffickingSettings Instance
			=> _settings.Value;

		public IBindableProperty<float> BustChance { get; }
		public IBindableProperty<bool> DiscoverDealer { get; }
		public IBindableProperty<int> WantedLevel { get; }

		public float[] GetBustChanceValues()
			=> new float[] { 0.05f, 0.1f, 0.15f, 0.2f, 0.25f, 0.3f, 0.35f, 0.4f, 0.45f, 0.5f };

		public int[] GetWantedLevelValues()
			=> new int[] { 1, 2, 3 };
	}
}
