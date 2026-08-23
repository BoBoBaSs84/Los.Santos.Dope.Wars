namespace LSDW.Helpers;

/// <summary>
/// Represents a helper class for random number generation and related functionality.
/// </summary>
internal static class RandomHelper
{
  private const int MinValue = 0;
  private const int MaxValue = 100;

  /// <summary>
  /// The random number generator used for all randomised behaviour.
  /// </summary>
  internal static readonly Random Random = new();

  /// <summary>
  /// Returns a percent roll in <c>0..99</c>, to compare against a <c>0..100</c> chance setting.
  /// </summary>
  internal static int NextPercent(this Random random)
    => random.Next(MinValue, MaxValue);
}
