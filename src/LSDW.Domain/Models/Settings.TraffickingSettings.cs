using LSDW.Abstractions.Helpers;
using static LSDW.Abstractions.Domain.Models.ISettings;

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
			DiscoverDealer = new(true);
			BustChance = new(0.1f);
			WantedLevel = new(2);
		}

		/// <summary>
		/// The singleton instance of the trafficking settings.
		/// </summary>
		public static TraffickingSettings Instance
			=> _settings.Value;

		/// <inheritdoc/>
		public BindableProperty<bool> DiscoverDealer { get; set; }
		/// <inheritdoc/>
		public BindableProperty<float> BustChance { get; set; }
		/// <inheritdoc/>
		public BindableProperty<int> WantedLevel { get; set; }

		/// <inheritdoc/>
		public float[] GetBustChanceValues()
			=> new float[] { 0.05f, 0.1f, 0.15f, 0.2f, 0.25f, 0.3f, 0.35f, 0.4f, 0.45f, 0.5f };

		/// <inheritdoc/>
		public int[] GetWantedLevelValues()
			=> new int[] { 1, 2, 3 };
	}
}
