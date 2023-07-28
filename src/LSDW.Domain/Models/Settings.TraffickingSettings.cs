using LSDW.Domain.Models.Base;
using static LSDW.Abstractions.Domain.Models.ISettings;

namespace LSDW.Domain.Models;

internal sealed partial class Settings
{
	/// <summary>
	/// The trafficking settings class.
	/// </summary>

	internal sealed class TraffickingSettings : NotificationBase, ITraffickingSettings
	{
		private static readonly Lazy<TraffickingSettings> _settings = new(() => new());
		private bool discoverDealer;
		private float bustChance;
		private int wantedLevel;

		/// <summary>
		/// Initializes a instance of the trafficking settings class.
		/// </summary>
		private TraffickingSettings()
		{
			discoverDealer = true;
			bustChance = 0.1f;
			wantedLevel = 2;
		}

		/// <summary>
		/// The singleton instance of the trafficking settings.
		/// </summary>
		public static TraffickingSettings Instance
			=> _settings.Value;

		/// <inheritdoc/>
		public bool DiscoverDealer
		{
			get => discoverDealer;
			set => SetProperty(ref discoverDealer, value);
		}

		/// <inheritdoc/>
		public float BustChance
		{
			get => bustChance;
			set => SetProperty(ref bustChance, value);
		}

		/// <inheritdoc/>
		public int WantedLevel
		{
			get => wantedLevel;
			set => SetProperty(ref wantedLevel, value);
		}

		/// <inheritdoc/>
		public float[] GetBustChanceValues()
			=> new float[] { 0.05f, 0.1f, 0.15f, 0.2f, 0.25f, 0.3f, 0.35f, 0.4f, 0.45f, 0.5f };

		/// <inheritdoc/>
		public int[] GetWantedLevelValues()
			=> new int[] { 1, 2, 3 };
	}
}
