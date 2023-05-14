﻿using LSDW.Classes;
using LSDW.Interfaces.Classes;

namespace LSDW.Factories;

/// <summary>
/// The inventory factory class.
/// </summary>
public static class InventoryFactory
{
	/// <summary>
	/// Should create a empty player inventory instance.
	/// </summary>
	public static IInventory CreateEmptyPlayerInventory()
		=> new PlayerInventory(new List<IDrug>());

	/// <summary>
	/// Should create a player inventory instance.
	/// </summary>
	/// <param name="drugs">The drugs to add.</param>
	public static IInventory CreatePlayerInventory(List<IDrug> drugs)
		=> new PlayerInventory(drugs);
}