using LSDW.Application.Abstractions.Application.Providers;

using GTA.Math;
using GTA.UI;

using Screen = GTA.UI.Screen;

namespace LSDW.Application.Providers;

/// <summary>
/// Represents the implementation of the <see cref="IScreenProvider"/> interface.
/// </summary>
internal sealed class ScreenProvider : IScreenProvider
{
	public Size Resolution
	{
		get => Screen.Resolution;
	}
	public float AspectRatio
	{
		get => Screen.AspectRatio;
	}
	public float ScaledWidth
	{
		get => Screen.ScaledWidth;
	}
	public bool IsFadedIn
	{
		get => Screen.IsFadedIn;
	}
	public bool IsFadedOut
	{
		get => Screen.IsFadedOut;
	}
	public bool IsFadingIn
	{
		get => Screen.IsFadingIn;
	}
	public bool IsFadingOut
	{
		get => Screen.IsFadingOut;
	}
	public bool AreScreenKillEffectsEnabled
	{
		get => Screen.AreScreenKillEffectsEnabled;
	}
	public bool IsHelpTextDisplayed
	{
		get => Screen.IsHelpTextDisplayed;
	}
	public void FadeIn(int time) => Screen.FadeIn(time);

	public void FadeOut(int time) => Screen.FadeOut(time);

	public bool IsEffectActive(ScreenEffect effectName) => Screen.IsEffectActive(effectName);

	public void StartEffect(ScreenEffect effectName, int duration, bool looped) => Screen.StartEffect(effectName, duration, looped);

	public void StopEffect(ScreenEffect effectName) => Screen.StopEffect(effectName);

	public void StopEffects() => Screen.StopEffects();

	public void ShowSubtitle(string message, int duration) => Screen.ShowSubtitle(message, duration);

	public void ShowSubtitle(string message, int duration, bool drawImmediately) => Screen.ShowSubtitle(message, duration, drawImmediately);

	public void ShowHelpTextThisFrame(string helpText) => Screen.ShowHelpTextThisFrame(helpText);

	public void ShowHelpTextThisFrame(string helpText, bool beep) => Screen.ShowHelpTextThisFrame(helpText, beep);

	public void ShowHelpText(string helpText, int duration, bool beep, bool looped) => Screen.ShowHelpText(helpText, duration, beep, looped);

	public PointF WorldToScreen(Vector3 position, bool scaleWidth) => Screen.WorldToScreen(position, scaleWidth);

}
