using LSDW.Application.Abstractions.Application.Services;
using LSDW.Application.Abstractions.Infrastructure.Services;
using LSDW.Domain.Events.System;
using LSDW.Domain.Models;
using LSDW.Presentation.Menus.Base;

namespace LSDW.Presentation.Menus;

/// <summary>
/// Represents the main modification menu that can be toggled with a specific key press.
/// </summary>
public sealed class ModificationMenu : BaseMenu
{
	private readonly IEventService _eventService;
	private readonly ISettingsService _settingsService;

	/// <summary>
	/// Initializes a new instance of the <see cref="ModificationMenu"/> class.
	/// </summary>
	/// <param name="eventService">The event service instance to be used by the modification menu.</param>
	/// <param name="settingsService">The settings service instance to be used by the modification menu.</param>
	public ModificationMenu(IEventService eventService, ISettingsService settingsService) : base("Main", "Mod Menu", "The fancy mod menu.")
	{
		_eventService = eventService;
		_settingsService = settingsService;

		RegisterEvents();
		RegisterMenuItems(_settingsService.Current);
	}

	private void RegisterMenuItems(Settings settings)
	{
		GeneralSettingsMenu generalSettingsMenu = new(settings.General);
		AddMenu(generalSettingsMenu);
		DealerSettingsMenu dealerSettingsMenu = new(settings.Dealer);
		AddMenu(dealerSettingsMenu);
		MarketSettingsMenu marketSettingsMenu = new(settings.Market);
		AddMenu(marketSettingsMenu);
		PlayerSettingsMenu playerSettingsMenu = new(settings.Player);
		AddMenu(playerSettingsMenu);
		TraffickingSettingsMenu traffickingSettingsMenu = new(settings.Trafficking);
		AddMenu(traffickingSettingsMenu);
	}

	private void RegisterEvents()
	{
		_eventService.Subscribe<AbortTriggered>(OnAbortTriggered);
		_eventService.Subscribe<KeyReleased>(OnKeyReleased);
		_eventService.Subscribe<TickTriggered>(OnTickTriggered);

		Closing += (s, e) => _settingsService.Save();
	}

	private void OnAbortTriggered(AbortTriggered triggered)
		=> throw new NotImplementedException();

	private void OnKeyReleased(KeyReleased @event)
	{
		if (@event.Keys == Keys.F10)
			Visible = !Visible;
	}

	private void OnTickTriggered(TickTriggered @event)
		=> MenuPool.Process();
}
