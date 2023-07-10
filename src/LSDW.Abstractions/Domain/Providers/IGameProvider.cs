using GTA;
using GTAControl = GTA.Control;

namespace LSDW.Abstractions.Domain.Providers;

/// <summary>
/// The game provider interface.
/// </summary>
public interface IGameProvider
{
	/// <summary>
	/// Gets the current game language.
	/// </summary>
	Language Language { get; }

	/// <summary>
	/// Gets the version of the game.
	/// </summary>
	GameVersion Version { get; }

	/// <summary>
	/// Gets the measurement system the game uses to display.
	/// </summary>
	MeasurementSystem MeasurementSystem { get; }

	/// <summary>
	/// Gets how many milliseconds the game has been open in this session.
	/// </summary>
	int GameTime { get; }

	/// <summary>
	/// Gets or Sets the time scale of the game.
	/// </summary>
	/// <remarks>
	/// The time scale, only accepts values in range 0.0f to 1.0f.
	/// </remarks>
	float TimeScale { get; set; }

	/// <summary>
	/// Gets or sets the maximum wanted level a <see cref="Game.Player"/> can receive.
	/// </summary>
	/// <remarks>
	/// The maximum wanted level, only accepts values 0 to 5.
	/// </remarks>
	int MaxWantedLevel { get; set; }

	/// <summary>
	/// Gets or sets the current radio station.
	/// </summary>
	RadioStation RadioStation { get; set; }

	/// <summary>
	/// Gets or sets a value informing the engine if a mission is in progress.
	/// </summary>
	bool IsMissionActive { get; set; }

	/// <summary>
	/// Gets or sets a value informing the engine if a random event is in progress.
	/// </summary>
	bool IsRandomEventActive { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether the pause menu is active.
	/// </summary>
	bool IsPaused { get; set; }

	/// <summary>
	/// Gets a value indicating whether there is a loading screen being displayed.
	/// </summary>
	bool IsLoading { get; }

	/// <summary>
	/// Performs an automatic game save.
	/// </summary>
	void DoAutoSave();

	/// <summary>
	/// Creates an input box for the user to input text using the keyboard.
	/// </summary>
	/// <param name="defaultText">The default text.</param>
	/// <returns>
	/// The <see cref="string"/> of what the user entered or <see cref="string.Empty"/> if the user canceled.
	/// </returns>
	string GetUserInput(string defaultText = "");

	/// <summary>
	/// Creates an input box for the user to input text using the keyboard.
	/// </summary>
	/// <param name="windowTitle">The title of the input box window.</param>
	/// <param name="defaultText">The maximum length of text input allowed.</param>
	/// <param name="maxLength">The default text.</param>
	/// <returns>
	/// The <see cref="string"/> of what the user entered or <see cref="string.Empty"/> if the user canceled.
	/// </returns>
	string GetUserInput(WindowTitle windowTitle, string defaultText, int maxLength);

	/// <summary>
	/// Gets whether a <see cref="GTAControl"/> is currently pressed.
	/// </summary>
	/// <param name="control">The <see cref="GTAControl"/> to check.</param>
	/// <returns>
	/// <see langword="true"/> if the <see cref="GTAControl"/> is pressed; otherwise, <see langword="false"/>.
	/// </returns>
	bool IsControlPressed(GTAControl control);

	/// <summary>
	/// Gets whether a <see cref="GTAControl"/> was just pressed this frame.
	/// </summary>
	/// <param name="control">The <see cref="GTAControl"/> to check.</param>
	/// <returns>
	/// <see langword="true"/> if the <see cref="GTAControl"/> was just pressed this frame; otherwise, <see langword="false"/>.
	/// </returns>
	bool IsControlJustPressed(GTAControl control);

	/// <summary>
	/// Gets whether a <see cref="GTAControl"/> was just released this frame.
	/// </summary>
	/// <param name="control">The <see cref="GTAControl"/> to check.</param>
	/// <returns>
	/// <see langword="true"/> if the <see cref="GTAControl"/> was just released this frame; otherwise, <see langword="false"/>.
	/// </returns>
	bool IsControlJustReleased(GTAControl control);
}
