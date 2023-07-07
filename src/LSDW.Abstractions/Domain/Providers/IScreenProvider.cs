using GTA.UI;

namespace LSDW.Abstractions.Domain.Providers;

/// <summary>
/// The screen provider interface.
/// </summary>
public interface IScreenProvider
{
	/// <summary>
	/// The base width of the screen used for all UI Calculations, unless ScaledDraw is used.
	/// </summary>
	float Width { get; }

	/// <summary>
	/// The base height of the screen used for all UI Calculations.
	/// </summary>
	float Height { get; }

	/// <summary>
	/// Gets the actual screen resolution the game is being rendered at.
	/// </summary>
	Size Resolution { get; }

	/// <summary>
	/// Gets the current screen aspect ratio.
	/// </summary>
	float AspectRatio { get; }

	/// <summary>
	/// Gets the screen width scaled against a 720pixel height base.
	/// </summary>
	float ScaledWidth { get; }

	/// <summary>
	/// Gets a value indicating whether the screen is faded in.
	/// </summary>
	/// <remarks>
	/// <see langword="true"/> if the screen is faded in; otherwise, <see langword="false"/>.
	/// </remarks>
	bool IsFadedIn { get; }

	/// <summary>
	/// Gets a value indicating whether the screen is faded out.
	/// </summary>
	/// <remarks>
	/// <see langword="true"/> if the screen is faded out; otherwise, <see langword="false"/>.
	/// </remarks>
	bool IsFadedOut { get; }

	/// <summary>
	/// Gets a value indicating whether the screen is fading in.
	/// </summary>
	/// <remarks>
	/// <see langword="true"/> if the screen is fading in; otherwise, <see langword="false"/>.
	/// </remarks>
	bool IsFadingIn { get; }

	/// <summary>
	/// Gets a value indicating whether the screen is fading out.
	/// </summary>
	/// <remarks>
	/// <see langword="true"/> if the screen is fading out; otherwise, <see langword="false"/>.
	/// </remarks>
	bool IsFadingOut { get; }

	/// <summary>
	/// Gets a value indicating whether screen kill effects are enabled.
	/// </summary>
	/// <remarks>
	/// <see langword="true"/> if screen kill effects are enabled; otherwise, <see langword="false"/>.
	/// </remarks>
	bool AreScreenKillEffectsEnabled { get; }

	/// <summary>
	/// Gets a value indicating whether a help message is currently displayed.
	/// </summary>
	bool IsHelpTextDisplayed { get; }

	/// <summary>
	/// Fades the screen in over a specific time, useful for transitioning.
	/// </summary>
	/// <param name="time">The time in milliseconds for the fade in to take.</param>
	void FadeIn(int time);

	/// <summary>
	/// Fades the screen out over a specific time, useful for transitioning.
	/// </summary>
	/// <param name="time">The time in milliseconds for the fade out to take.</param>
	void FadeOut(int time);

	/// <summary>
	/// Gets a value indicating whether the specific screen effect is running.
	/// </summary>
	/// <param name="effectName">The <see cref="ScreenEffect"/> to check.</param>
	/// <returns>
	/// <see langword="true"/> if the screen effect is active; otherwise, <see langword="false"/>.
	/// </returns>
	bool IsEffectActive(ScreenEffect effectName);

	/// <summary>
	/// Starts applying the specified effect to the screen.
	/// </summary>
	/// <param name="effectName">The <see cref="ScreenEffect"/> to start playing.</param>
	/// <param name="duration">The duration of the effect in milliseconds or zero to use the default length.</param>
	/// <param name="looped">
	/// If <see langword="true"/> the effect won't stop until <see cref="StopEffect(ScreenEffect)"/> is called.
	/// </param>
	void StartEffect(ScreenEffect effectName, int duration = 0, bool looped = false);

	/// <summary>
	/// Stops applying the specified effect to the screen.
	/// </summary>
	/// <param name="effectName">The <see cref="ScreenEffect"/> to stop playing.</param>
	void StopEffect(ScreenEffect effectName);

	/// <summary>
	/// Stops all currently running effects.
	/// </summary>
	void StopEffects();
}
