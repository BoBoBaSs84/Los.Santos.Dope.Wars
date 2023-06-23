﻿using GTA;
using LSDW.Abstractions.Domain.Providers;

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
	/// Is the dealer closed for business?
	/// </summary>
	bool Closed { get; }

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
	/// Sets <see cref="Closed"/> to <see langword="true"/> and sets the <see cref="ClosedUntil"/> value.
	/// </summary>
	/// <param name="timeProvider">The time provider instance to use.</param>
	void SetClosed(ITimeProvider timeProvider);

	/// <summary>
	/// Sets if the dealer is discovered or not, maybe good for rediscovering too.
	/// </summary>
	/// <param name="value"><see langword="true"/> or <see langword="false"/></param>
	void SetDiscovered(bool value);

	/// <summary>
	/// Sets <see cref="Closed"/> to <see langword="false"/> and unsets the <see cref="ClosedUntil"/> value.
	/// </summary>
	void SetOpen();

	/// <summary>
	/// Sets the next price change date time.
	/// </summary>
	/// <param name="timeProvider">The time provider instance to use.</param>
	void SetNextPriceChange(ITimeProvider timeProvider);

	/// <summary>
	/// Sets the next inventory change date time. 
	/// </summary>
	/// <param name="timeProvider">The time provider instance to use.</param>
	void SetNextInventoryChange(ITimeProvider timeProvider);
}
