using GTA;

using Button = GTA.Button;
using Control = GTA.Control;

namespace LSDW.Application.Abstractions.Application.Providers;

/// <summary>
/// Represents an abstraction contract for the <see cref="Game"/> class.
/// </summary>
public interface IGameProvider
{
	/// <inheritdoc cref="Game.Player"/>
	Player Player { get; }

	/// <inheritdoc cref="Game.PlayerBlip"/>
	Blip PlayerBlip { get; }

	/// <inheritdoc cref="Game.NorthBlip"/>
	Blip NorthBlip { get; }

	/// <inheritdoc cref="Game.Language"/>
	Language Language { get; }

	/// <inheritdoc cref="Game.Version"/>
	GameVersion Version { get; }

	/// <inheritdoc cref="Game.MeasurementSystem"/>
	MeasurementSystem MeasurementSystem { get; }

	/// <inheritdoc cref="Game.GameTime"/>
	int GameTime { get; }

	/// <inheritdoc cref="Game.TimeScale"/>
	float TimeScale { get; set; }

	/// <inheritdoc cref="Game.FrameCount"/>
	int FrameCount { get; }

	/// <inheritdoc cref="Game.FPS"/>
	float FPS { get; }

	/// <inheritdoc cref="Game.LastFrameTime"/>
	float LastFrameTime { get; }

	/// <inheritdoc cref="Game.MaxWantedLevel"/>
	int MaxWantedLevel { get; set; }

	/// <inheritdoc cref="Game.RadioStation"/>
	RadioStation RadioStation { get; set; }

	/// <inheritdoc cref="Game.IsNightVisionActive"/>
	bool IsNightVisionActive { get; set; }

	/// <inheritdoc cref="Game.IsThermalVisionActive"/>
	bool IsThermalVisionActive { get; set; }

	/// <inheritdoc cref="Game.IsMissionActive"/>
	bool IsMissionActive { get; set; }

	/// <inheritdoc cref="Game.IsRandomEventActive"/>
	bool IsRandomEventActive { get; set; }

	/// <inheritdoc cref="Game.IsCutsceneActive"/>
	bool IsCutsceneActive { get; }

	/// <inheritdoc cref="Game.IsWaypointActive"/>
	bool IsWaypointActive { get; }

	/// <inheritdoc cref="Game.IsPaused"/>
	bool IsPaused { get; set; }

	/// <inheritdoc cref="Game.IsLoading"/>
	bool IsLoading { get; }

	/// <inheritdoc cref="Game.LastInputMethod"/>
	InputMethod LastInputMethod { get; }

	/// <inheritdoc cref="Game.PlayerTargetingMode"/>
	PlayerTargetingMode PlayerTargetingMode { get; }

	/// <inheritdoc cref="Game.IsVibrationEnabled"/>
	bool IsVibrationEnabled { get; }

	/// <inheritdoc cref="Game.DoAutoSave()"/>
	void DoAutoSave();

	/// <inheritdoc cref="Game.ShowSaveMenu()"/>
	void ShowSaveMenu();

	/// <inheritdoc cref="Game.Pause(bool)"/>
	void Pause(bool value);

	/// <inheritdoc cref="Game.GetUserInput(string)"/>
	string GetUserInput(string defaultText);

	/// <inheritdoc cref="Game.GetUserInput(WindowTitle, string, int)"/>
	string GetUserInput(WindowTitle windowTitle, string defaultText, int maxLength);

	/// <inheritdoc cref="Game.WasCheatStringJustEntered(string)"/>
	bool WasCheatStringJustEntered(string cheat);

	/// <inheritdoc cref="Game.WasButtonCombinationJustEntered(Button[])"/>
	bool WasButtonCombinationJustEntered(Button[] buttons);

	/// <inheritdoc cref="Game.GetControlValue(Control)"/>
	int GetControlValue(Control control);

	/// <inheritdoc cref="Game.GetControlValueNormalized(Control)"/>
	float GetControlValueNormalized(Control control);

	/// <inheritdoc cref="Game.GetDisabledControlValueNormalized(Control)"/>
	float GetDisabledControlValueNormalized(Control control);

	/// <inheritdoc cref="Game.SetControlValueNormalized(Control, float)"/>
	void SetControlValueNormalized(Control control, float value);

	/// <inheritdoc cref="Game.IsKeyPressed(Keys)"/>
	bool IsKeyPressed(Keys key);

	/// <inheritdoc cref="Game.IsControlPressed(Control)"/>
	bool IsControlPressed(Control control);

	/// <inheritdoc cref="Game.IsControlJustPressed(Control)"/>
	bool IsControlJustPressed(Control control);

	/// <inheritdoc cref="Game.IsControlJustReleased(Control)"/>
	bool IsControlJustReleased(Control control);

	/// <inheritdoc cref="Game.IsEnabledControlPressed(Control)"/>
	bool IsEnabledControlPressed(Control control);

	/// <inheritdoc cref="Game.IsEnabledControlJustPressed(Control)"/>
	bool IsEnabledControlJustPressed(Control control);

	/// <inheritdoc cref="Game.IsEnabledControlJustReleased(Control)"/>
	bool IsEnabledControlJustReleased(Control control);

	/// <inheritdoc cref="Game.IsControlEnabled(Control)"/>
	bool IsControlEnabled(Control control);

	/// <inheritdoc cref="Game.EnableControlThisFrame(Control)"/>
	void EnableControlThisFrame(Control control);

	/// <inheritdoc cref="Game.DisableControlThisFrame(Control)"/>
	void DisableControlThisFrame(Control control);

	/// <inheritdoc cref="Game.EnableAllControlsThisFrame()"/>
	void EnableAllControlsThisFrame();

	/// <inheritdoc cref="Game.DisableAllControlsThisFrame()"/>
	void DisableAllControlsThisFrame();

	/// <inheritdoc cref="Game.GenerateHash(string)"/>
	int GenerateHash(string input);

	/// <inheritdoc cref="Game.GetLocalizedString(string)"/>
	string GetLocalizedString(string entry);

	/// <inheritdoc cref="Game.GetLocalizedString(int)"/>
	string GetLocalizedString(int entryLabelHash);

	/// <inheritdoc cref="Game.GetProfileSetting(int)"/>
	int GetProfileSetting(int index);

	/// <inheritdoc cref="Game.FindPattern(string, IntPtr)"/>
	IntPtr FindPattern(string pattern, IntPtr startAddress);

	/// <inheritdoc cref="Game.FindPattern(string, string, IntPtr)"/>
	IntPtr FindPattern(string pattern, string mask, IntPtr startAddress);
}
