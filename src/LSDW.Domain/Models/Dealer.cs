using GTA;
using GTA.Math;
using LSDW.Abstractions.Domain.Models;
using LSDW.Domain.Factories;
using LSDW.Domain.Models.Base;
using DealerSettings = LSDW.Domain.Models.Settings.Dealer;

namespace LSDW.Domain.Models;

/// <summary>
/// The dealer actor class.
/// </summary>
/// <remarks>
/// Inherits from the <see cref="Pedestrian"/> class and
/// implements the members of the <see cref="IDealer"/> interface.
/// </remarks>
internal sealed class Dealer : Pedestrian, IDealer
{
	private Blip? blip;

	/// <summary>
	/// Initializes a instance of the dealer class.
	/// </summary>
	/// <param name="position">The position of the dealer.</param>
	/// <param name="pedHash">The ped hash of the dealer.</param>
	internal Dealer(Vector3 position, PedHash pedHash) : base(position, pedHash)
	{
		Discovered = false;
		Inventory = DomainFactory.CreateInventory();

		Inventory.PropertyChanged += OnPropertyChanged;
	}

	/// <summary>
	/// Initializes a instance of the dealer class.
	/// </summary>
	/// <param name="position">The position of the dealer.</param>
	/// <param name="pedHash">The ped hash of the dealer.</param>
	/// <param name="name">The name of the dealer.</param>
	/// <param name="closedUntil">The dealer is gone until this date time.</param>
	/// <param name="discovered">Has the dealer already been discovered?</param>
	/// <param name="inventory">The dealer inventory.</param>
	/// <param name="lastRefresh">When was the inventory prices the last time refreshed?</param>
	/// <param name="lastRestock">When was the inventory the last time restocked?</param>
	internal Dealer(Vector3 position, PedHash pedHash, string name, DateTime? closedUntil, bool discovered, IInventory inventory, DateTime lastRefresh, DateTime lastRestock) : base(position, pedHash, name)
	{
		ClosedUntil = closedUntil;
		Discovered = discovered;
		Inventory = inventory;
		NextPriceChange = lastRefresh;
		NextInventoryChange = lastRestock;

		Inventory.PropertyChanged += OnPropertyChanged;
	}

	public DateTime? ClosedUntil { get; private set; }
	public bool Discovered { get; private set; }
	public bool IsBlipCreated { get; private set; }
	public IInventory Inventory { get; }
	public DateTime NextPriceChange { get; private set; }
	public DateTime NextInventoryChange { get; private set; }

	public override void Create(float healthValue = 100)
	{
		if (IsCreated || ClosedUntil.HasValue)
			return;

		base.Create(healthValue);

		if (DealerSettings.HasWeapons)
			GiveWeapon(WeaponHash.CombatShotgun, 100);

		if (DealerSettings.HasArmor)
			GiveArmor(150f);
	}

	public void CreateBlip(BlipSprite sprite = BlipSprite.Drugs, BlipColor color = BlipColor.White)
	{
		if (blip is not null || ClosedUntil.HasValue)
			return;

		blip = World.CreateBlip(Position);
		blip.Sprite = sprite;
		blip.Scale = 0.75f;
		blip.Color = color;
		blip.Name = Name;

		SetDiscovered(true);
	}

	public override void Delete()
	{
		if (IsBlipCreated)
			DeleteBlip();

		if (IsCreated)
			return;

		Inventory.PropertyChanged -= OnPropertyChanged;
		Inventory.Clear();

		base.Delete();
	}

	public void DeleteBlip()
	{
		if (blip is null)
			return;

		blip.Delete();
	}

	public override void Flee()
	{
		if (!IsCreated)
			return;

		DeleteBlip();

		base.Flee();
	}

	public void SetClosed(DateTime? value)
		=> ClosedUntil = value;

	public void SetDiscovered(bool value)
		=> Discovered = value;

	public void SetNextPriceChange(DateTime value)
		=> NextPriceChange = value;

	public void SetNextInventoryChange(DateTime value)
		=> NextInventoryChange = value;

	private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
	{
		if (!args.PropertyName.Equals(Inventory.Money) || !IsCreated)
			return;

		SetMoney(Inventory.Money);
	}
}
