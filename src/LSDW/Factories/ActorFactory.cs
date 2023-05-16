﻿using GTA.Math;
using LSDW.Classes.Actors;
using LSDW.Core.Interfaces.Classes;
using LSDW.Interfaces.Actors;

namespace LSDW.Factories;

/// <summary>
/// The actor factory class.
/// </summary>
public static class ActorFactory
{
	/// <summary>
	/// Creates a new dealer instance.
	/// </summary>
	/// <param name="position">The position of the dealer.</param>
	public static IDealer CreateDealer(Vector3 position)
		=> new Dealer(position);

	/// <summary>
	/// Creates a new dealer instance.
	/// </summary>
	/// <param name="position">The position of the dealer.</param>
	/// <param name="closedUntil">The dealer is gone until this date time.</param>
	/// <param name="discovered">Has the dealer already been discovered?</param>
	/// <param name="inventory">The dealer inventory.</param>
	/// <param name="name">The name of the dealer.</param>
	public static IDealer CreateDealer(Vector3 position, DateTime? closedUntil, bool discovered, IInventory inventory, string name)
		=> new Dealer(position, closedUntil, discovered, inventory, name);
}
