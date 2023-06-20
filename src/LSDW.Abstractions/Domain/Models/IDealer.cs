﻿using GTA;

namespace LSDW.Abstractions.Domain.Models;

/// <summary>
/// The dealer interface.
/// </summary>
public interface IDealer : IPedestrian
{
	/// <summary>
	/// The dealer is gone until this date time.
	/// </summary>
	DateTime? ClosedUntil { get; }

	/// <summary>
	/// When does only the inventory prices change the next time?
	/// </summary>
	DateTime NextPriceChange { get; }

	/// <summary>
	/// When does the whole inventory changes itself next time?
	/// </summary>
	DateTime NextInventoryChange { get; }

	/// <summary>
	/// Has the dealer already been discovered?
	/// </summary>
	bool Discovered { get; }

	/// <summary>
	/// Is the blip created?
	/// </summary>
	bool BlipCreated { get; }

	/// <summary>
	/// The dealer inventory.
	/// </summary>
	IInventory Inventory { get; }

	/// <summary>
	/// Creates the blip on the map.
	/// </summary>
	/// <param name="sprite"></param>
	/// <param name="color"></param>
	void CreateBlip(BlipSprite sprite = BlipSprite.Drugs, BlipColor color = BlipColor.White);

	/// <summary>
	/// Deletes the blip on the map.
	/// </summary>
	void DeleteBlip();

	/// <summary>
	/// Set the closed or not state.
	/// </summary>
	/// <param name="value">The value for closed, <see langword="null"/> for open again.</param>
	void SetClosed(DateTime? value);

	/// <summary>
	/// Sets if the dealer is discovered or not, maybe good for rediscovering too.
	/// </summary>
	/// <param name="value"><see langword="true"/> or <see langword="false"/></param>
	void SetDiscovered(bool value);

	/// <summary>
	/// Sets the next price change date time.
	/// </summary>
	/// <param name="value">The date time to set.</param>
	void SetNextPriceChange(DateTime value);

	/// <summary>
	/// Sets the next inventory change date time. 
	/// </summary>
	/// <param name="value">The date time to set.</param>
	void SetNextInventoryChange(DateTime value);
}
