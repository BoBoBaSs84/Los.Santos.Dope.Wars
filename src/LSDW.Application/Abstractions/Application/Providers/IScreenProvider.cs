using GTA.Math;
using GTA.UI;

using Screen = GTA.UI.Screen;

namespace LSDW.Application.Abstractions.Application.Providers;

/// <summary>
/// Represents an abstraction contract for the <see cref="Screen"/> class.
/// </summary>
public interface IScreenProvider
{
	/// <inheritdoc cref="Screen.Resolution"/>
	Size Resolution { get; }

	/// <inheritdoc cref="Screen.AspectRatio"/>
	float AspectRatio { get; }

	/// <inheritdoc cref="Screen.ScaledWidth"/>
	float ScaledWidth { get; }

	/// <inheritdoc cref="Screen.IsFadedIn"/>
	bool IsFadedIn { get; }

	/// <inheritdoc cref="Screen.IsFadedOut"/>
	bool IsFadedOut { get; }

	/// <inheritdoc cref="Screen.IsFadingIn"/>
	bool IsFadingIn { get; }

	/// <inheritdoc cref="Screen.IsFadingOut"/>
	bool IsFadingOut { get; }

	/// <inheritdoc cref="Screen.AreScreenKillEffectsEnabled"/>
	bool AreScreenKillEffectsEnabled { get; }

	/// <inheritdoc cref="Screen.IsHelpTextDisplayed"/>
	bool IsHelpTextDisplayed { get; }

	/// <inheritdoc cref="Screen.FadeIn(int)"/>
	void FadeIn(int time);

	/// <inheritdoc cref="Screen.FadeOut(int)"/>
	void FadeOut(int time);

	/// <inheritdoc cref="Screen.IsEffectActive(ScreenEffect)"/>
	bool IsEffectActive(ScreenEffect effectName);

	/// <inheritdoc cref="Screen.StartEffect(ScreenEffect, int, bool)"/>
	void StartEffect(ScreenEffect effectName, int duration, bool looped);

	/// <inheritdoc cref="Screen.StopEffect(ScreenEffect)"/>
	void StopEffect(ScreenEffect effectName);

	/// <inheritdoc cref="Screen.StopEffects()"/>
	void StopEffects();

	/// <inheritdoc cref="Screen.ShowSubtitle(string, int)"/>
	void ShowSubtitle(string message, int duration);

	/// <inheritdoc cref="Screen.ShowSubtitle(string, int, bool)"/>
	void ShowSubtitle(string message, int duration, bool drawImmediately);

	/// <inheritdoc cref="Screen.ShowHelpTextThisFrame(string)"/>
	void ShowHelpTextThisFrame(string helpText);

	/// <inheritdoc cref="Screen.ShowHelpTextThisFrame(string, bool)"/>
	void ShowHelpTextThisFrame(string helpText, bool beep);

	/// <inheritdoc cref="Screen.ShowHelpText(string, int, bool, bool)"/>
	void ShowHelpText(string helpText, int duration, bool beep, bool looped);

	/// <inheritdoc cref="Screen.WorldToScreen(Vector3, bool)"/>
	PointF WorldToScreen(Vector3 position, bool scaleWidth);
}
