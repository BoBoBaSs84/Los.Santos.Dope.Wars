using LSDW.Application.Abstractions.Application.Providers;

using GTA;

using Button = GTA.Button;
using Control = GTA.Control;

namespace LSDW.Application.Providers;

/// <summary>
/// Represents the implementation of the <see cref="IGameProvider"/> interface.
/// </summary>
internal sealed class GameProvider : IGameProvider
{
	public Player Player
	{
		get => Game.Player;
	}
	public Blip PlayerBlip
	{
		get => Game.PlayerBlip;
	}
	public Blip NorthBlip
	{
		get => Game.NorthBlip;
	}
	public Language Language
	{
		get => Game.Language;
	}
	public GameVersion Version
	{
		get => Game.Version;
	}
	public MeasurementSystem MeasurementSystem
	{
		get => Game.MeasurementSystem;
	}
	public int GameTime
	{
		get => Game.GameTime;
	}
	public float TimeScale
	{
		get => Game.TimeScale;
		set => Game.TimeScale = value;
	}
	public int FrameCount
	{
		get => Game.FrameCount;
	}
	public float FPS
	{
		get => Game.FPS;
	}
	public float LastFrameTime
	{
		get => Game.LastFrameTime;
	}
	public int MaxWantedLevel
	{
		get => Game.MaxWantedLevel;
		set => Game.MaxWantedLevel = value;
	}
	public RadioStation RadioStation
	{
		get => Game.RadioStation;
		set => Game.RadioStation = value;
	}
	public bool IsNightVisionActive
	{
		get => Game.IsNightVisionActive;
		set => Game.IsNightVisionActive = value;
	}
	public bool IsThermalVisionActive
	{
		get => Game.IsThermalVisionActive;
		set => Game.IsThermalVisionActive = value;
	}
	public bool IsMissionActive
	{
		get => Game.IsMissionActive;
		set => Game.IsMissionActive = value;
	}
	public bool IsRandomEventActive
	{
		get => Game.IsRandomEventActive;
		set => Game.IsRandomEventActive = value;
	}
	public bool IsCutsceneActive
	{
		get => Game.IsCutsceneActive;
	}
	public bool IsWaypointActive
	{
		get => Game.IsWaypointActive;
	}
	public bool IsPaused
	{
		get => Game.IsPaused;
		set => Game.IsPaused = value;
	}
	public bool IsLoading
	{
		get => Game.IsLoading;
	}
	public InputMethod LastInputMethod
	{
		get => Game.LastInputMethod;
	}
	public PlayerTargetingMode PlayerTargetingMode
	{
		get => Game.PlayerTargetingMode;
	}
	public bool IsVibrationEnabled
	{
		get => Game.IsVibrationEnabled;
	}
	public void DoAutoSave() => Game.DoAutoSave();

	public void ShowSaveMenu() => Game.ShowSaveMenu();

	public void Pause(bool value) => Game.Pause(value);

	public string GetUserInput(string defaultText) => Game.GetUserInput(defaultText);

	public string GetUserInput(WindowTitle windowTitle, string defaultText, int maxLength) => Game.GetUserInput(windowTitle, defaultText, maxLength);

	public bool WasCheatStringJustEntered(string cheat) => Game.WasCheatStringJustEntered(cheat);

	public bool WasButtonCombinationJustEntered(Button[] buttons) => Game.WasButtonCombinationJustEntered(buttons);

	public int GetControlValue(Control control) => Game.GetControlValue(control);

	public float GetControlValueNormalized(Control control) => Game.GetControlValueNormalized(control);

	public float GetDisabledControlValueNormalized(Control control) => Game.GetDisabledControlValueNormalized(control);

	public void SetControlValueNormalized(Control control, float value) => Game.SetControlValueNormalized(control, value);

	public bool IsKeyPressed(Keys key) => Game.IsKeyPressed(key);

	public bool IsControlPressed(Control control) => Game.IsControlPressed(control);

	public bool IsControlJustPressed(Control control) => Game.IsControlJustPressed(control);

	public bool IsControlJustReleased(Control control) => Game.IsControlJustReleased(control);

	public bool IsEnabledControlPressed(Control control) => Game.IsEnabledControlPressed(control);

	public bool IsEnabledControlJustPressed(Control control) => Game.IsEnabledControlJustPressed(control);

	public bool IsEnabledControlJustReleased(Control control) => Game.IsEnabledControlJustReleased(control);

	public bool IsControlEnabled(Control control) => Game.IsControlEnabled(control);

	public void EnableControlThisFrame(Control control) => Game.EnableControlThisFrame(control);

	public void DisableControlThisFrame(Control control) => Game.DisableControlThisFrame(control);

	public void EnableAllControlsThisFrame() => Game.EnableAllControlsThisFrame();

	public void DisableAllControlsThisFrame() => Game.DisableAllControlsThisFrame();

	public int GenerateHash(string input) => Game.GenerateHash(input);

	public string GetLocalizedString(string entry) => Game.GetLocalizedString(entry);

	public string GetLocalizedString(int entryLabelHash) => Game.GetLocalizedString(entryLabelHash);

	public int GetProfileSetting(int index) => Game.GetProfileSetting(index);

	public IntPtr FindPattern(string pattern, IntPtr startAddress) => Game.FindPattern(pattern, startAddress);

	public IntPtr FindPattern(string pattern, string mask, IntPtr startAddress) => Game.FindPattern(pattern, mask, startAddress);

}
