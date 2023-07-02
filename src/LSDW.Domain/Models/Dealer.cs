using GTA;
using GTA.Math;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Domain.Factories;
using LSDW.Domain.Models.Base;
using DealerSettings = LSDW.Abstractions.Models.Settings.Dealer;

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

	public DateTime? ClosedUntil { get; set; }
	public bool Closed => ClosedUntil.HasValue;
	public bool Discovered { get; set; }
	public bool BlipCreated => blip is not null;
	public IInventory Inventory { get; }
	public DateTime NextPriceChange { get; set; }
	public DateTime NextInventoryChange { get; set; }

	public override void Attack(Ped ped)
	{
		DeleteBlip();
		base.Attack(ped);
	}

	public override void Create(IWorldProvider worldProvider, int health = 100)
	{
		base.Create(worldProvider, health);

		if (DealerSettings.HasWeapons)
			GiveWeapon(WeaponHash.Pistol, 100);

		if (DealerSettings.HasArmor)
			GiveArmor(100);
	}

	public void CreateBlip(IWorldProvider worldProvider, BlipSprite sprite = BlipSprite.Drugs, BlipColor color = BlipColor.White)
	{
		if (BlipCreated || Closed)
			return;

		blip = worldProvider.CreateBlip(SpawnPosition);
		blip.Sprite = sprite;
		blip.Scale = 0.75f;
		blip.Color = color;
		blip.IsShortRange = true;
	}

	public void DeleteBlip()
		=> blip?.Delete();

	private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
	{
		if (!args.PropertyName.Equals(Inventory.Money) || !Created)
			return;

		SetMoney(Inventory.Money);
	}
}
