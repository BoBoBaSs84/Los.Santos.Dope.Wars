using GTA;
using LSDW.Abstractions.Domain.Providers;

namespace LSDW.Domain.Providers;

/// <summary>
/// The player provider class.
/// </summary>
/// <remarks>
/// Wrapper for the <see cref="Game"/> methods and properties.
/// </remarks>
internal class GameProvider : IGameProvider
{
	/// <summary>
	/// The singleton instance of the game provider.
	/// </summary>
	internal static readonly GameProvider Instance = new();

	public Language Language
		=> Game.Language;

	public GameVersion Version
		=> Game.Version;

	public MeasurementSystem MeasurementSystem
		=> Game.MeasurementSystem;

	public int GameTime
		=> Game.GameTime;

	public float TimeScale
	{
		get => Game.TimeScale;
		set => Game.TimeScale = value;
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

	public bool IsPaused
	{
		get => Game.IsPaused;
		set => Game.IsPaused = value;
	}

	public bool IsLoading
		=> Game.IsLoading;

	public void DoAutoSave()
		=> Game.DoAutoSave();

	public string GetUserInput(string defaultText = "")
		=> Game.GetUserInput(defaultText);

	public string GetUserInput(WindowTitle windowTitle, string defaultText, int maxLength)
		=> Game.GetUserInput(windowTitle, defaultText, maxLength);

	public bool IsControlJustPressed(Control control)
		=> Game.IsControlJustPressed(control);

	public bool IsControlJustReleased(Control control)
		=> Game.IsControlJustReleased(control);

	public bool IsControlPressed(Control control)
		=> Game.IsControlPressed(control);
}
