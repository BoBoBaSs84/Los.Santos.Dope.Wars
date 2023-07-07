using GTA.UI;
using LSDW.Abstractions.Domain.Providers;

namespace LSDW.Domain.Providers;

/// <summary>
/// The screen provider class.
/// </summary>
/// <remarks>
/// Wrapper for the <see cref="Screen"/> methods and properties.
/// </remarks>
internal sealed class ScreenProvider : IScreenProvider
{
	/// <summary>
	/// The singleton instance of the screen provider.
	/// </summary>
	internal static readonly ScreenProvider Instance = new();

	public float Width => Screen.Width;
	public float Height => Screen.Height;
	public Size Resolution => Screen.Resolution;
	public float AspectRatio => Screen.AspectRatio;
	public float ScaledWidth => Screen.ScaledWidth;
	public bool IsFadedIn => Screen.IsFadedIn;
	public bool IsFadedOut => Screen.IsFadedOut;
	public bool IsFadingIn => Screen.IsFadingIn;
	public bool IsFadingOut => Screen.IsFadingOut;
	public bool AreScreenKillEffectsEnabled => Screen.AreScreenKillEffectsEnabled;
	public bool IsHelpTextDisplayed => Screen.IsHelpTextDisplayed;

	public void FadeIn(int time)
		=> Screen.FadeIn(time);

	public void FadeOut(int time)
		=> Screen.FadeOut(time);

	public bool IsEffectActive(ScreenEffect effectName)
		=> Screen.IsEffectActive(effectName);

	public void StartEffect(ScreenEffect effectName, int duration = 0, bool looped = false)
		=> Screen.StartEffect(effectName, duration, looped);

	public void StopEffect(ScreenEffect effectName)
		=> Screen.StopEffect(effectName);

	public void StopEffects()
		=> Screen.StopEffects();
}
